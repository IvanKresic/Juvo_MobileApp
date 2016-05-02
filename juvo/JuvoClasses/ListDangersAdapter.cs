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
using juvo.JuvoModel;

namespace juvo.JuvoClasses
{
    class ListDangersAdapter : BaseAdapter<Response>
    {
        Activity activity;
        int layoutResourceId;
        List<Response> items = new List<Response>();

        public ListDangersAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
        }

        //Returns the view for a specific item on the list
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
            TextView textItem;

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                textItem = row.FindViewById<TextView>(Resource.Id.checkToDoItem);
            }
            else
                textItem = row.FindViewById<TextView>(Resource.Id.checkToDoItem);

            textItem.Text = currentItem.Name + currentItem.Time;
            return row;

        }



        #region For checkItems
        public void Add(Response item)
        {
            items.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            items.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(Response item)
        {
            items.Remove(item);
            NotifyDataSetChanged();
        }
        #endregion

        #region Implementation of abstract 
        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override Response this[int position]
        {
            get
            {
                return items[position];
            }
        }
        #endregion
    }
}