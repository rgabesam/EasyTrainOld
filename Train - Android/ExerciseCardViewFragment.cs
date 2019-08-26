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
using Train___Android.Database;

namespace Train___Android
{
    public class ExerciseCardViewFragment : Fragment
    {
        TextView nameTextView;
        TextView descriptionTextView;
        TextView timeTextView;
        TextView difficultyTextView;
        TextView placeTextView;

        private IEditActivity homeActivity;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_exercise_group, true);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_training_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_plan_group, false);
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.exercise_view_fragment, container, false);

            nameTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_name);
            descriptionTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_description);
            timeTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_time);
            difficultyTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_difficulty);
            placeTextView = view.FindViewById<TextView>(Resource.Id.exerciseCardView_place);

            int itemId = Arguments.GetInt("itemId");
            var item = MyDatabase.GetExercise(itemId);

            nameTextView.Text = item.Name;
            descriptionTextView.Text = item.Description;
            timeTextView.Text = item.Time.ToString();
            difficultyTextView.Text = item.Difficulty.ToString();
            placeTextView.Text = item.Place;
            
            //homeActivity.ShowOverflowMenu(true);

            return view;
        }

        

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            homeActivity = (IEditActivity)context;
        }
    }
}