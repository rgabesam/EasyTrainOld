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
               
        private IExercise homeActivity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
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
                nameEditText.Text = Arguments.GetInt("time").ToString();
                nameEditText.Text = Arguments.GetString("difficulty");
                nameEditText.Text = Arguments.GetString("place");
            }
            
            return view;
        }

        private void ExerciseDone(object sender, EventArgs e)
        {
            if((nameEditText.Text).Replace(" ", "").Length == 0 || (descriptionEditText.Text).Replace(" ", "").Length == 0)
            {
                Toast.MakeText(Activity, "Name and description mustn't be empty", ToastLength.Short).Show();
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
            homeActivity = (IExercise) context;
        }
    }
}