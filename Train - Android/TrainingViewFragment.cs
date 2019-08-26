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
    public class TrainingViewFragment : Fragment
    {
        TextView nameTextView;
        TextView descriptionTextView;
        List<Exercise> ownedExercises;
        ListView listView;
        ScrollView scrollView;
        Button switchViewButton;
        private bool exercisesDisplayed = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_exercise_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_training_group, true);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_plan_group, false);
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.training_view_fragment, container, false);

            nameTextView = view.FindViewById<TextView>(Resource.Id.training_CardView_name);
            descriptionTextView = view.FindViewById<TextView>(Resource.Id.training_CardView_description);
            listView = view.FindViewById<ListView>(Resource.Id.exercises_in_training_list);
            scrollView = view.FindViewById<ScrollView>(Resource.Id.training_view_scrollView);
            switchViewButton = view.FindViewById<Button>(Resource.Id.training_view_switch_button);
            switchViewButton.Click += SwitchTrainingView;

            int itemId = Arguments.GetInt("itemId");
            var item = MyDatabase.GetTraining(itemId);

            nameTextView.Text = item.Name;
            descriptionTextView.Text = item.Description;

            UpdateListItems();

            
            return view;
        }

        

        private void SwitchTrainingView(object sender, EventArgs e)
        {
            if (!exercisesDisplayed) //properties of training are displayed
            {
                exercisesDisplayed = true;
                switchViewButton.Text = "Switch back to properties";
                scrollView.Visibility = ViewStates.Gone;
                listView.Visibility = ViewStates.Visible;
            }
            else
            {
                exercisesDisplayed = false;
                switchViewButton.Text = "Switch to owned exercises";
                listView.Visibility = ViewStates.Gone;
                scrollView.Visibility = ViewStates.Visible;
            }
        }

        private void UpdateListItems()
        {
            ownedExercises = MyDatabase.GetAllExercises();//not right implementation yet
            listView.Adapter = new TrainingScreenAdapter<Exercise>(Activity, ownedExercises);
            listView.ItemClick += OnListExerciseClick;
        }

        private void OnListExerciseClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}