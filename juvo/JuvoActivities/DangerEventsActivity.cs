using System;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.IO;
using juvo.JuvoClasses;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;
using Android.Util;
using Newtonsoft.Json;
using juvo.JuvoModel;
using Android.Content;

namespace juvo.JuvoActivities
{
    [Activity(Label = "History of Danger Events", Icon = "@drawable/juvo")]
    public class DangerEventsActivity : Activity
    {
        public static DangerEventsActivity instance;

        //Mobile Service Client reference
        private MobileServiceClient client;

        //Mobile Service sync table used to access data
        private IMobileServiceSyncTable<DangerEvents> historyTable;

        //Adapter to map the items list to the view
        private DangerEventsAdapter adapter;

        //EditText containing the "New ToDo" text
        private EditText textNewToDo;

        const string applicationURL = @"https://juvo.azurewebsites.net/";

        const string localDbFilename = "localstore.db";

        List<Response> response = new List<Response>();
        List<int> niz = new List<int>();
        int[] myNiz;


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            instance = this;

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Activity_To_Do);

            CurrentPlatform.Init();

            // Create the Mobile Service Client instance, using the provided
            // Mobile Service URL
            client = new MobileServiceClient(applicationURL);
            
            await InitLocalStoreAsync();

            // Get the Mobile Service sync table instance to use
            historyTable = client.GetSyncTable<DangerEvents>();

            //textNewToDo = FindViewById<EditText>(Resource.Id.textNewToDo);

            // Create an adapter to bind the items with the view
            adapter = new DangerEventsAdapter(this, Resource.Layout.Row_List_To_Do);
            var listViewToDo = FindViewById<ListView>(Resource.Id.listViewToDo);
            listViewToDo.Adapter = adapter;

            // Load the items from the Mobile Service
            OnRefreshItemsSelected();

            listViewToDo.ItemClick += listView_ItemClick;
            // Create your application here
        }


        private async Task InitLocalStoreAsync()
        {
            // new code to initialize the SQLite store
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), localDbFilename);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Response>();

            // Uses the default conflict handler, which fails on conflict
            // To use a different conflict handler, pass a parameter to InitializeAsync. For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
            await client.SyncContext.InitializeAsync(store);
        }

        //Initializes the activity menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_main, menu);
            return true;
        }


        //Select an option from the menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh)
            {
                item.SetEnabled(false);

                OnRefreshItemsSelected();

                item.SetEnabled(true);
            }
            return true;
        }

        //On list item click
        void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var incident = incidents[e.Position];
            //Log.Info("NIZ", "" + niz[e.Position]);
            foreach(int i in niz)
            {
                Log.Info("NIZ", "" + i);
            }
            var activity2 = new Intent(this, typeof(ListDangersActivity));
            activity2.PutExtra("MyData", niz[e.Position]);
            StartActivity(activity2);
        }

        private async Task SyncAsync(bool pullData = false)
        {        
           response.Clear();
            adapter.Clear();

            try
            {
                if (pullData)
                {
                    //*************httpRequest***************
                    var mClient = new HttpClient();
                    mClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Constants.token);
                    mClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

                    var calcResult = await mClient.GetAsync(new Uri("http://juvo.azurewebsites.net/api/DangerEvents1/" + Constants.User_Id));
                    var temp = await calcResult.Content.ReadAsStringAsync();
                    //*******************************

                    var res = JsonConvert.DeserializeObject<List<UsersModel>>(temp);
                    Log.Info("TAG",""+ temp);
                    Constants.JSONresponse = res;
                    foreach (DevicesModel d in res[0].devices)
                    {
                        Response response = new Response();
                             response.Name = d.Name;
                             response.Id = d.DevicesID;
                        
                        Log.Info("ID", "" + JsonConvert.SerializeObject(d));
                             niz.Add(d.DevicesID);
                             adapter.Add(response);
                        // foreach (DangerEventsModel events in d.DangerEvents)
                        // {
                        //     Response response = new Response();
                        //     response.Name = d.Name;
                        //     response.Time = events.HappenedAt;
                        //     adapter.Add(response);
                        // }
                    }
                }
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error occured!");
            }
        }

        // Called when the refresh menu option is selected
        private async void OnRefreshItemsSelected()
        {
            await SyncAsync(pullData: true); // get changes from the mobile service
            //await RefreshItemsFromTableAsync(); // refresh view using local database
        }

        //Refresh the list with the items in the local database
        private async Task RefreshItemsFromTableAsync()
        {           
            try
            {
                // Get the items that weren't marked as completed and add them in the adapter
                //ORIGINAL: var list = await historyTable.Where(item => item.HappenedAt != null).ToListAsync();
                //var list = await historyTable.ToListAsync();
                var list = await historyTable.Where(item => item.HappenedAt != " ").ToListAsync();

                adapter.Clear();

                foreach (Response current in response)
                    adapter.Add(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error reading from local database!");
            }
        }

        public async Task CheckItem(DangerEvents item)
        {
            if (client == null)
            {
                return;
            }

            // Set the item as completed and update it in the table
            //item.Complete = true;
            try
            {
                await historyTable.UpdateAsync(item); // update the new item in the local database
                await SyncAsync(); // send changes to the mobile service

               // if (item.Complete)
               //     adapter.Remove(item);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Ooooh noooo");
            }
        }

        [Java.Interop.Export()]
        public async void AddItem(View view)
        {
            if (client == null || string.IsNullOrWhiteSpace(textNewToDo.Text))
            {
                return;
            }

            // Create a new item
            var item = new DangerEvents
            {
                PrimaryID = 1,
                DeviceId = Int32.Parse(textNewToDo.Text),
                HappenedAt = DateTime.Now.ToString()
            };

            AlertDialog.Builder builder = new AlertDialog.Builder(Application.Context);
            builder.SetMessage(item.ToString());
            builder.Create();


            try
            {
                await historyTable.InsertAsync(item); // insert the new item into the local database
                await SyncAsync(); // send changes to the mobile service

               // if (!item.Complete)
               // {
                  //  adapter.Add(item);
               // }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Custom Error");
            }

            textNewToDo.Text = "";
        }

        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        
    }
}