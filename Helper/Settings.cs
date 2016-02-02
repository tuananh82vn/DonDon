using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace DonDon
{
	public static class Settings
	{
		private const string RestaurantIdKey = "RestaurantIdKey";
		private static readonly int RestaurantIdKeyDefault = 0;

		private const string UserIdKey = "UserIdKey";
		private static readonly int UserIdDefault = 0;

		private const string InstanceURLKey = "InstanceURLKey";
//		private static readonly string InstanceURL_Default = "http://dondon.softwarestaging.com.au";
		private static readonly string InstanceURL_Default = "http://172.28.1.53:49713";

		private const string UserNameKey = "UserNameKey";
		private static readonly string UserNameKey_Default = string.Empty;

		private const string PasswordKey = "PasswordKey";
		private static readonly string PasswordKey_Default = string.Empty;

		private const string RememberMeKey = "RememberMeKey";
		private static readonly bool RememberMeKey_Default = false;

		private const string FullnameKey = "FullnameKey";
		private static readonly string FullnameKey_Default = string.Empty;

		private const string CKStaffKey = "CKStaffKey";
		private static readonly bool CKStaffKey_Default = false;

		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static string InstanceURL
		{
			get { return AppSettings.GetValueOrDefault(InstanceURLKey, InstanceURL_Default); }
			set { AppSettings.AddOrUpdateValue(InstanceURLKey, value); }
		}

		public static int RestaurantId
		{
			get { return AppSettings.GetValueOrDefault(RestaurantIdKey, RestaurantIdKeyDefault); }
			set { AppSettings.AddOrUpdateValue(RestaurantIdKey, value); }
		}

		public static int UserId
		{
			get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault); }
			set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
		}

		public static string Username
		{
			get { return AppSettings.GetValueOrDefault(UserNameKey, UserNameKey_Default); }
			set { AppSettings.AddOrUpdateValue(UserNameKey, value); }
		}

		public static string Password
		{
			get { return AppSettings.GetValueOrDefault(PasswordKey, PasswordKey_Default); }
			set { AppSettings.AddOrUpdateValue(PasswordKey, value); }
		}

		public static bool RememberMe
		{
			get { return AppSettings.GetValueOrDefault(RememberMeKey, RememberMeKey_Default); }
			set { AppSettings.AddOrUpdateValue(RememberMeKey, value); }
		}

		public static string Fullname
		{
			get { return AppSettings.GetValueOrDefault(FullnameKey, FullnameKey_Default); }
			set { AppSettings.AddOrUpdateValue(FullnameKey, value); }
		}

		public static bool CKStaff
		{
			get { return AppSettings.GetValueOrDefault(CKStaffKey, CKStaffKey_Default); }
			set { AppSettings.AddOrUpdateValue(CKStaffKey, value); }
		}
	}
}

