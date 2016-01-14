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
		private LoginService _loginService;

		public EditText username;
		public EditText password;
		public CheckBox cb_rememberMe;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			Button button = FindViewById<Button>(Resource.Id.btLogin);
			button.Click += btloginClick;  

			username = FindViewById<EditText>(Resource.Id.tv_username);
			username.SetOnEditorActionListener (this);

			username.RequestFocus ();

			password = FindViewById<EditText>(Resource.Id.tv_password);
			password.SetOnEditorActionListener (this);

			cb_rememberMe  = FindViewById<CheckBox>(Resource.Id.cb_rememberMe);
			cb_rememberMe.SetOnEditorActionListener (this);

			var RemmemberMe = Settings.RememberMe;

			if (RemmemberMe) {
				cb_rememberMe.Checked = true;
				username.Text = Settings.Username;
				password.Text = Settings.Password;
			}

			RequestedOrientation = ScreenOrientation.SensorLandscape;


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

			_loginService = new LoginService();

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
				RunOnUiThread (() => Toast.MakeText (this, "No Connection", ToastLength.Short).Show ());
			}
		}

		private void onSuccessfulLogin(LoginObject obj)
		{
			Settings.UserId = obj.UserId;
			Settings.Token = obj.TokenNumber;
			Settings.Username = obj.UserName;
			if (cb_rememberMe.Checked) {
				Settings.RememberMe = true;
				Settings.Password = password.Text;
			} else {
				Settings.RememberMe = false;
				Settings.Password = "";
			}

//			StartActivity (new Intent (this, typeof (HomeActivity)));
			this.OverridePendingTransition(Resource.Animation.slide_in_top, Resource.Animation.slide_out_bottom);
			this.Finish();
		}

		private void onFailLogin(LoginObject obj)
		{
			RunOnUiThread (() => Toast.MakeText (this, obj.ErrorMessage, ToastLength.Short).Show ());
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Go) {
				if (!string.IsNullOrEmpty (username.Text) && !string.IsNullOrEmpty (password.Text)) {
					Login ();
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
