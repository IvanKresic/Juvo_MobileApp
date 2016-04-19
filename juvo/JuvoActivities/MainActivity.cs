using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using juvo.JuvoClasses;
using System.Net;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace juvo.JuvoActivities
{
    [Activity(Label = "Juvo Home Friend", MainLauncher = true, Icon = "@drawable/juvo")]
    public class MainActivity : Activity
    {

        public static MainActivity instance;
        const string applicationURL = @"https://juvo.azurewebsites.net";
        private static bool auth = false;

        protected override void OnCreate(Bundle bundle)
        {
            instance = this;

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LogInLayout);


            // Get our button from the layout resource,
            // and attach an event to it
            Button signInButton = FindViewById<Button>(Resource.Id.email_sign_in_button);
            AutoCompleteTextView emailView = FindViewById<AutoCompleteTextView>(Resource.Id.email);
            AutoCompleteTextView passwd = FindViewById<AutoCompleteTextView>(Resource.Id.passwd);

            signInButton.Click += delegate {
                JuvoClasses.Constants.tag = emailView.Text;
                try
                {
                    var temp = Authenticate(emailView.Text, passwd.Text);                    
                }
                catch
                {

                }
            };

        }

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(this, JuvoClasses.Constants.SenderID);
        }

        private async Task Authenticate(string usr, string pass)
        {
            var myClient = new MobileServiceClient(applicationURL);
            var arguments = new Dictionary<string, string>
            {
                { "email", usr }, {"password", pass }
            };
            var calcResult = await myClient.InvokeApiAsync<LogInResponse>("/api/Auth", HttpMethod.Post, arguments);
            if(calcResult != null)
            {
                RegisterWithGCM();
                var intent = new Intent(this, typeof(DangerEventsActivity));
                StartActivity(intent);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Log In Failed!", ToastLength.Long).Show();
            }

            Log.Info("TAG", "Token:"+ calcResult.Token);
            Log.Info("TAG", "User_Id:" + calcResult.UserId);
            Log.Info("TAG", "Email:" + calcResult.Email);

        }

    }
}