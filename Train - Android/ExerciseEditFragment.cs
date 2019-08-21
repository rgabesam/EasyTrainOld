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
    public class ExerciseEditFragment : Fragment
    {
        Button doneButton;
        EditText nameEditText;
        EditText descriptionEditText;
        EditText timeEditText;
        EditText difficultyEditText;
        EditText placeEditText;
               
        private IExerciseActivity homeActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_group, false);
            base.OnPrepareOptionsMenu(menu);
        }
        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ExerciseEdit, container, false);

            nameEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_name);
            descriptionEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_description);
            timeEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_time);
            difficultyEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_difficulty);
            placeEditText = view.FindViewById<EditText>(Resource.Id.exerciseEdit_place);

            doneButton = view.FindViewById<Button>(Resource.Id.exerciseEdit_doneButton);
            doneButton.Click += ExerciseDone;//button event handler
            bool newExercise = Arguments.GetBoolean("newExercise");

            if (!newExercise)
            {
                //set all edittexts with current values
                nameEditText.Text = Arguments.GetString("name");
                descriptionEditText.Text = Arguments.GetString("description");
                timeEditText.Text = Arguments.GetInt("time").ToString();
                difficultyEditText.Text = Arguments.GetByte("difficulty").ToString();
                placeEditText.Text = Arguments.GetString("place");
            }
            
            //homeActivity.ShowOverflowMenu(false);
            
            return view;
        }

        private void ExerciseDone(object sender, EventArgs e)
        {
            int a = 0;
            if((nameEditText.Text).Replace(" ", "").Length == 0         //all fields must be set
                || (descriptionEditText.Text).Replace(" ", "").Length == 0
                || (timeEditText.Text).Replace(" ", "").Length == 0
                || (difficultyEditText.Text).Replace(" ", "").Length == 0
                || (placeEditText.Text).Replace(" ", "").Length == 0)
            {
                Toast.MakeText(Activity, "All fields must be set", ToastLength.Short).Show();
                return;
            }
            if((!Int32.TryParse(timeEditText.Text, out a) || a < 1 ) && timeEditText.Text.Length != 0)
            {
                Toast.MakeText(Activity, "Time must be decimal number greater than 1", ToastLength.Short).Show();
                return;
            }
            if((!Int32.TryParse(difficultyEditText.Text, out a) || a < 1 || a > 10 ) && difficultyEditText.Text.Length != 0)
            {
                Toast.MakeText(Activity, "Difficulty must be decimal number between 1 and 10", ToastLength.Short).Show();
                return;
            }
            SendExercise();
            
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

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);
            homeActivity = (IExerciseActivity) context;
        }
    }
}