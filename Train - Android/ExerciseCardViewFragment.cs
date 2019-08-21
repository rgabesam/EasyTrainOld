using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Train___Android
{
    public class ExerciseCardViewFragment : Fragment
    {
        TextView nameTextView;
        TextView descriptionTextView;
        TextView timeTextView;
        TextView difficultyTextView;
        TextView placeTextView;

        private IExerciseActivity homeActivity;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_group, true);
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ExerciseCardView, container, false);

            nameTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_name);
            descriptionTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_description);
            timeTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_time);
            difficultyTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_difficulty);
            placeTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_place);

            nameTextView.Text = Arguments.GetString("name");
            descriptionTextView.Text = Arguments.GetString("description");
            timeTextView.Text = Arguments.GetInt("time").ToString();
            difficultyTextView.Text = Arguments.GetByte("difficulty").ToString();
            placeTextView.Text = Arguments.GetString("place");
            
            //homeActivity.ShowOverflowMenu(true);

            return view;
        }

        

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            homeActivity = (IExerciseActivity)context;
        }
    }
}