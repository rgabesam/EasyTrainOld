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
using static Train___Android.TrainingActivity;

namespace Train___Android
{
    public class EditFragment : Fragment
    {
        mode viewMode;

        Button doneButton;
        EditText nameEditText;
        EditText descriptionEditText;
        EditText timeEditText;
        EditText difficultyEditText;
        EditText placeEditText;
               
        private IEditActivity homeActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            //bool newExercise = Arguments.GetBoolean("newExercise");
            viewMode = Enum.Parse<mode>(Arguments.GetString("viewMode"));
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_exercise_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_training_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_plan_group, false);
            base.OnPrepareOptionsMenu(menu);
        }
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.edit_fragment, container, false);

            nameEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_name);
            descriptionEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_description);
            timeEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_time);
            difficultyEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_difficulty);
            placeEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_place);

            if(viewMode != mode.exercise)
            {
                timeEditText.Visibility = ViewStates.Gone;
                difficultyEditText.Visibility = ViewStates.Gone;
                placeEditText.Visibility = ViewStates.Gone;

                view.FindViewById<TextView>(Resource.Id.exercise_view_time).Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.exercise_view_difficulty).Visibility = ViewStates.Gone;
                view.FindViewById<TextView>(Resource.Id.exercise_view_place).Visibility = ViewStates.Gone;
            }

            doneButton = view.FindViewById<Button>(Resource.Id.itemEdit_doneButton);
            doneButton.Click += NewItemDone;//button event handler

            bool newItem = Arguments.GetBoolean("newItem");


            if (!newItem)
            {
                int itemId = Arguments.GetInt("itemId");

                //set all editTexts with current values
                switch (viewMode)
                {
                    case mode.exercise:
                        Exercise exercise = MyDatabase.GetExercise(itemId);
                        nameEditText.Text = exercise.Name;
                        descriptionEditText.Text = exercise.Description;
                        timeEditText.Text = exercise.Time.ToString();
                        difficultyEditText.Text =exercise.Difficulty.ToString();
                        placeEditText.Text = exercise.Place;
                        break;
                    case mode.training:
                        Training training = MyDatabase.GetTraining(itemId);
                        nameEditText.Text = training.Name;
                        descriptionEditText.Text = training.Description;
                        break;
                    case mode.plan:
                        Plan plan = MyDatabase.GetPlan(itemId);
                        nameEditText.Text = plan.Name;
                        descriptionEditText.Text = plan.Description;
                        break;
                }                
            }
            
            
            return view;
        }

       

        private void NewItemDone(object sender, EventArgs e)
        {
            int a = 0;
            if ((nameEditText.Text).Replace(" ", "").Length == 0         //common condition for exercises, trainings, plans
                || (descriptionEditText.Text).Replace(" ", "").Length == 0)
            {
                Toast.MakeText(Activity, "All fields must be set", ToastLength.Short).Show();
                return;
            }

            switch (viewMode)
            {
                case mode.exercise:
                    if((timeEditText.Text).Replace(" ", "").Length == 0
                        || (difficultyEditText.Text).Replace(" ", "").Length == 0
                        || (placeEditText.Text).Replace(" ", "").Length == 0)
                    {
                        Toast.MakeText(Activity, "All fields must be set", ToastLength.Short).Show();
                        return;
                    }
                    if ((!Int32.TryParse(timeEditText.Text, out a) || a < 1) && timeEditText.Text.Length != 0)
                    {
                        Toast.MakeText(Activity, "Time must be decimal number greater than 1", ToastLength.Short).Show();
                        return;
                    }
                    if ((!Int32.TryParse(difficultyEditText.Text, out a) || a < 1 || a > 10) && difficultyEditText.Text.Length != 0)
                    {
                        Toast.MakeText(Activity, "Difficulty must be decimal number between 1 and 10", ToastLength.Short).Show();
                        return;
                    }
                    SendExercise();
                    break;
                case mode.training:
                    SendTraining();
                    break;
                case mode.plan:
                    SendPlan();
                    break;
            }
            
        }

        private void SendExercise()
        {
            Exercise exercise = new Exercise();
            exercise.Name = nameEditText.Text;
            exercise.Description = descriptionEditText.Text;
            exercise.Time = Int32.Parse(timeEditText.Text);
            exercise.Difficulty = Byte.Parse(difficultyEditText.Text);
            exercise.Place = placeEditText.Text;

            homeActivity.ReturnExercise(exercise);
        }

        private void SendTraining()
        {
            Training training = new Training();
            training.Name = nameEditText.Text;
            training.Description = descriptionEditText.Text;

            homeActivity.ReturnTraining(training);
        }

        private void SendPlan()
        {
            Plan plan = new Plan();
            plan.Name = nameEditText.Text;
            plan.Description = descriptionEditText.Text;

            homeActivity.ReturnPlan(plan);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            homeActivity = (IEditActivity) context;
        }
    }
}