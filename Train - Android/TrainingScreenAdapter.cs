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
using Train___Android.Database;

namespace Train___Android
{
    class CheckableItem<T> : IDisplayable where T:IDisplayable
    {
        public T item;
        bool itemChecked = false;

        public string Name { get => item.Name; set => item.Name = value; }
        public string Description { get => item.Description; set => item.Description = value; }
        public int Id { get => item.Id; set => item.Id = value; }
    }
    class TrainingScreenAdapter<T> : BaseAdapter<T> where T: IDisplayable
    {
        List<T> items;
        Activity context;
        bool itemIsCheckable = false;

        public TrainingScreenAdapter(Activity context, List<T> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public TrainingScreenAdapter(Activity context, List<T> items, bool checkable) : base()
        {//this is for purposes when user needs to choose set of items
            this.context = context;
            this.items = items;
            itemIsCheckable = checkable;
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override T this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomListTrainingItem, null);
            view.FindViewById<TextView>(Resource.Id.item_name).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.item_description).Text = item.Description;
            if (itemIsCheckable)
            {
                var checkBox = view.FindViewById<CheckBox>(Resource.Id.item_checkbox);
                checkBox.Visibility = ViewStates.Visible;
                checkBox.Checked = false;
                checkBox.CheckedChange += ChangeCheck;
            }

            return view;
        }

        private void ChangeCheck(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            
        }
    }
}