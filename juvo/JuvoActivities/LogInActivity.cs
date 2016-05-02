using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using juvo.JuvoClasses;
using Gcm.Client;
using System.Net.Http;
using Android.Util;

namespace juvo.JuvoActivities
{
    [Activity(Label = "Please wait ...", Icon = "@drawable/juvo")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "MainActivity")]
    public class LogInActivity : Activity
    {

        const string applicationURL = @"https://juvo.azurewebsites.net/";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Loging_In_Layout);

            var temp = Authenticate();
        }

        private async Task Authenticate()
        {
            var myClient = new MobileServiceClient(applicationURL);
            var arguments = new Dictionary<string, string>
            {
                { "email", JuvoClasses.Constants.Username }, {"password", JuvoClasses.Constants.Password }
            };

            var calcResult = await myClient.InvokeApiAsync<LogInResponse>("/api/Auth", HttpMethod.Post, arguments);
            Toast.MakeText(this, calcResult.ToString(), ToastLength.Long).Show();
            Log.Info("tag", "Do tud sam!");
            if (calcResult != null)
            {
                Log.Info("tag", "uOŠSAO SAM OVDJE");
                JuvoClasses.Constants.token = calcResult.Token;
                JuvoClasses.Constants.User_Id = Int32.Parse(calcResult.UserId);
                Log.Info("TAG","ID: " + JuvoClasses.Constants.User_Id);
                RegisterWithGCM();
                var intent = new Intent(this, typeof(DangerEventsActivity));                
                StartActivity(intent);
                Finish();
            }
            else
            {
                Finish();
                Toast.MakeText(this, "Log In Failed!", ToastLength.Long).Show();
            }
        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            GcmClient.Register(this, JuvoClasses.Constants.SenderID);
        }
    }
}