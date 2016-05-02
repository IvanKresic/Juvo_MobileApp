using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using juvo.JuvoClasses;
using juvo.JuvoModel;
using Android.Util;

namespace juvo.JuvoActivities
{
    [Activity(Label = "History of Danger Events", Icon = "@drawable/juvo")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "DangerEventsActivity")]
    public class ListDangersActivity : Activity
    {

        public static ListDangersActivity instance;

        private ListDangersAdapter adapter;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            instance = this;

            base.OnCreate(savedInstanceState);

            int id = Intent.GetIntExtra("MyData", 0);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Activity_To_Do);

            CurrentPlatform.Init();

            //textNewToDo = FindViewById<EditText>(Resource.Id.textNewToDo);

            // Create an adapter to bind the items with the view
            adapter = new ListDangersAdapter(this, Resource.Layout.Row_List_To_Do);
            var listViewToDo = FindViewById<ListView>(Resource.Id.listViewToDo);
            listViewToDo.Adapter = adapter;


            Log.Info("Passing_Value", "ID: " + id);
            // Load the items from the Mobile Service
            Response res = new Response();
            // Create your application here
            List<DevicesModel> devices = Constants.JSONresponse[0].devices;
            foreach(DevicesModel d in devices)
            {
                if(d.DevicesID == id)
                {
                    foreach(DangerEventsModel danger in d.DangerEvents)
                    {
                        Response response = new Response();
                        response.Time = danger.HappenedAt;
                        adapter.Add(response);
                    }
                }
            }                                                                   
        }

    }
}