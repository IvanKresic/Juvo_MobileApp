using System;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace juvo.JuvoClasses
{
    public class DangerEventsAdapter : BaseAdapter<DangerEvents>
    {

        Activity activity;
        int layoutResourceId;
        List<DangerEvents> items = new List<DangerEvents>();

        public DangerEventsAdapter(Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
        }

        //Returns the view for a specific item on the list
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];
            CheckBox checkBox;

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);

                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkToDoItem);

                checkBox.CheckedChange += async (sender, e) => {
                    var cbSender = sender as CheckBox;
                    if (cbSender != null && cbSender.Tag is HistoryItemWrapper && cbSender.Checked)
                    {
                        cbSender.Enabled = false;
                        if (activity is JuvoActivities.DangerEventsActivity)
                            await ((JuvoActivities.DangerEventsActivity)activity).CheckItem((cbSender.Tag as HistoryItemWrapper).HistoryItem);
                    }
                };
            }
            else
                checkBox = row.FindViewById<CheckBox>(Resource.Id.checkToDoItem);

            checkBox.Text = currentItem.HappenedAt;
            checkBox.Checked = false;
            checkBox.Enabled = true;
            checkBox.Tag = new HistoryItemWrapper(currentItem);

            return row;
        }

        public void Add(DangerEvents item)
        {
            items.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            items.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(DangerEvents item)
        {
            items.Remove(item);
            NotifyDataSetChanged();
        }


        #region Implemented Abstract of BaseAdapter
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

        public override DangerEvents this[int position]
        {
            get
            {
                return items[position];
            }
        }
        #endregion
    }
}