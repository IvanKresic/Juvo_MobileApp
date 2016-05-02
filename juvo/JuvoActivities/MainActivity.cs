using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace juvo.JuvoActivities
{
    [Activity(Label = "Juvo Home Friend", MainLauncher = true, Icon = "@drawable/juvo")]
    
    public class MainActivity : Activity
    {
        public static MainActivity instance;
        const string applicationURL = @"https://juvo.azurewebsites.net/";

        protected override void OnCreate(Bundle bundle)
        {
            instance = this;
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LogInLayout);

            Button signInButton = FindViewById<Button>(Resource.Id.email_sign_in_button);
            AutoCompleteTextView emailView = FindViewById<AutoCompleteTextView>(Resource.Id.email);
            AutoCompleteTextView passwd = FindViewById<AutoCompleteTextView>(Resource.Id.passwd);

            signInButton.Click += delegate {                              
                try
                {
                    if(!Patterns.EmailAddress.Matcher(emailView.Text).Matches())
                    {
                        Toast.MakeText(this, "Invalid email!", ToastLength.Long).Show();
                        emailView.Text = "";
                        passwd.Text = "";
                    }
                    else
                    {
                        JuvoClasses.Constants.tag = emailView.Text;
                        JuvoClasses.Constants.Username = emailView.Text;
                        JuvoClasses.Constants.Password = passwd.Text;
                        var intent = new Intent(this, typeof(LogInActivity));
                        StartActivity(intent);
                        Finish();
                    }
                }
                catch
                {

                }
            };
        }        
    }
}