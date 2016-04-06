using System;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Android.Content.PM;
using Android.Graphics;
using Android.Views.Animations;

namespace DonDon
{
	[Activity(Label = "Link-OM", Icon = "@drawable/icon")]
	public class LoginActivity : Activity, TextView.IOnEditorActionListener
	{
		private LoginController _loginService;

		public EditText username;
		public EditText password;
		public CheckBox cb_rememberMe;

		public ProgressDialog progress;
		public LinearLayout linearLayout4;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			RequestedOrientation = ScreenOrientation.SensorPortrait;


			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			Button buttonForgot = FindViewById<Button>(Resource.Id.btForgot);
			buttonForgot.Click += btForgotClick; 

			username = FindViewById<EditText>(Resource.Id.tv_username);
			username.SetOnEditorActionListener (this);

			password = FindViewById<EditText>(Resource.Id.tv_password);
			password.SetOnEditorActionListener (this);

			cb_rememberMe  = FindViewById<CheckBox>(Resource.Id.cb_rememberMe);
			cb_rememberMe.SetOnEditorActionListener (this);

			var RemmemberMe = Settings.RememberMe;

			if (RemmemberMe) {
				cb_rememberMe.Checked = true;
				var temp = Settings.Username;
				username.Text = temp;
				password.Text = Settings.Password;
			}

			var metrics = Resources.DisplayMetrics;
			var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

			if (heightInDp > 1024) {
				linearLayout4  = FindViewById<LinearLayout>(Resource.Id.linearLayout4);
				ViewGroup.MarginLayoutParams  ll = (ViewGroup.MarginLayoutParams)linearLayout4.LayoutParameters;
				ll.TopMargin = 55;
			}
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/Resources.DisplayMetrics.Density);
			return dp;
		}

		public override bool DispatchTouchEvent(MotionEvent event1) {
			if (event1.Action == MotionEventActions.Down) {
				View v = this.CurrentFocus;
				if ( v is EditText) {
					Rect outRect = new Rect();
					v.GetGlobalVisibleRect(outRect);
					if (!outRect.Contains((int)event1.GetX(), (int)event1.GetY())) {
						v.ClearFocus();
						InputMethodManager imm = (InputMethodManager) GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(v.WindowToken, 0);
					}
				}
			}
			return base.DispatchTouchEvent( event1 );
		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		public void btForgotClick(object sender, EventArgs e)
		{
			var uri = Android.Net.Uri.Parse ("http://dondon.softwarestaging.com.au/user/forgotpassword");
			var intent = new Intent (Intent.ActionView, uri);
			StartActivity (intent);
		}

		public void btloginClick(object sender, EventArgs e)
		{

			if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {

				var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);

				imm.HideSoftInputFromWindow (username.WindowToken, HideSoftInputFlags.NotAlways);

				imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);

				ThreadPool.QueueUserWorkItem (o => Login ());

			} 
			else 
			{
				RunOnUiThread (() => Toast.MakeText (this, "Please enter username and password", ToastLength.Short).Show ());
			}
		}

		private void Login(){

			RunOnUiThread (() => progress = new ProgressDialog (this,Resource.Style.StyledDialog));
			RunOnUiThread (() => progress.Indeterminate = true);
			RunOnUiThread (() => progress.SetMessage("Please wait..."));
			RunOnUiThread (() => progress.SetCancelable (true));
			RunOnUiThread (() => progress.Show ());


			_loginService = new LoginController();

			LoginObject obj = _loginService.Login (username.Text, password.Text);

			if (obj != null) 
			{
				if (obj.Success)
					onSuccessfulLogin (obj);
				else
					onFailLogin (obj);
			} 
			else 
			{
				RunOnUiThread (() => progress.Dismiss ());

				RunOnUiThread (() => Toast.MakeText (this, "No Connection", ToastLength.Short).Show ());
			}
		}

		private void onSuccessfulLogin(LoginObject obj)
		{
			Settings.UserId = obj.UserId;
			Settings.Username = obj.UserName;
			Settings.Fullname = obj.Fullname;
			Settings.CKStaff = obj.CKStaff;

			if (cb_rememberMe.Checked) {
				Settings.RememberMe = true;
				Settings.Password = password.Text;
			} else {
				Settings.RememberMe = false;
				Settings.Password = "";
			}

			RunOnUiThread (() => progress.Dismiss ());

			StartActivity(typeof(HomeActivity));

			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);

			this.Finish();
		}

		private void onFailLogin(LoginObject obj)
		{
			RunOnUiThread (() => progress.Dismiss ());

			RunOnUiThread (() => Toast.MakeText (this, obj.ErrorMessage, ToastLength.Short).Show ());
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Go) {
				if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {
					ThreadPool.QueueUserWorkItem (o => Login ());
				} else if (string.IsNullOrEmpty (username.Text)) {
					username.RequestFocus ();
				} else {
					password.RequestFocus ();
				}
				return true;
			} else if (actionId == ImeAction.Next) {
				if (!string.IsNullOrEmpty (username.Text)) {
					password.RequestFocus ();
				}
				return true;
			}
			return false;
		}


	}
}
