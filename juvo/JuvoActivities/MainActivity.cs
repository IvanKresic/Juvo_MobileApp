using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;

namespace juvo.JuvoActivities
{
    [Activity(Label = "Juvo Home Friend", MainLauncher = true, Icon = "@drawable/juvo")]
    public class MainActivity : Activity
    {
        public static MainActivity instance;

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
                    Authenticate(emailView.Text, passwd.Text);
                }
                catch
                {

                }
                RegisterWithGCM();
                var intent = new Intent(this, typeof(DangerEventsActivity));
                StartActivity(intent);
                Finish();

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

        private void Authenticate(string usr, string pass)
        {

        }


    }
}