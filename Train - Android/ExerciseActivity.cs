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
using Android.Support.V7.App;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Train___Android.Database;

namespace Train___Android
{
    interface IExercise
    {
        void ReturnExercise(Exercise value);
    }

    [Activity(Label = "")]
    public class ExerciseActivity : AppCompatActivity, IExercise
    {
        Exercise exercise;
        IMenu menu;
        private int exerciseId = 0;
        private bool cardView;
        Intent intent;

        public void ReturnExercise(Exercise value)  //way how to recieve back data from fragment + to update database
        {
            exercise = value;
            if (cardView) //user edited old exercise
            {
                exercise.Id = exerciseId;
                MyDatabase.UpdateExercise(exercise);
                Toast.MakeText(this, "Exercise was updated", ToastLength.Short).Show();
                StartCardView();
            }
            else//user just created new exercise
            {
                MyDatabase.InsertExercise(exercise);
                Toast.MakeText(this, "Exercise was created", ToastLength.Short).Show();
                Finish(); //back to list of exercises
            }
            

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_exercise);


            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            
            cardView = Intent.GetBooleanExtra("cardView", true);

            intent = new Intent(this, typeof(ExerciseEditFragment));
            if (cardView)
            {
                exerciseId = Intent.GetIntExtra("exerciseId", 0);   //0 is there just because there is required default value
                exercise = MyDatabase.GetExercise(exerciseId);
                
                StartCardView();
            }
            else //this branch means that user is creating new exercise
            {
                ShowOverflowMenu(false); //hiding menu
                var trans = SupportFragmentManager.BeginTransaction();
                trans.Add(Resource.Id.fragmentContainer_activityExercise, new ExerciseEditFragment(), "ExerciseEditFragment");

                intent.PutExtra("newExercise", true);
                trans.Commit();
            }
        }

        private void StartCardView()
        {
            SaveExerciseProperties();
            ShowOverflowMenu(true); //showing menu
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer_activityExercise, new ExerciseCardViewFragment(), "ExerciseCardViewFragment");
            trans.Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.EditOrDeleteMenu, menu);
            this.menu = menu;
            return true;
        }

        private void SaveExerciseProperties()
        {
            intent.PutExtra("name", exercise.Name);
            intent.PutExtra("description", exercise.Description);
            intent.PutExtra("time", exercise.Time);
            intent.PutExtra("difficulty", exercise.Difficulty);
            intent.PutExtra("place", exercise.Place);
        }

        public void ShowOverflowMenu(bool showMenu)
        {
            if (menu == null)
                return;
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_group, showMenu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.item_edit:
                    var trans = SupportFragmentManager.BeginTransaction();
                    trans.Add(Resource.Id.fragmentContainer_activityExercise, new ExerciseEditFragment(), "ExerciseEditFragment");
                    intent.PutExtra("newExercise", false);
                    //add current Exercise fields
                    SaveExerciseProperties();
                    ShowOverflowMenu(false); //hiding menu
                    trans.Commit();
                    return true;
                case Resource.Id.item_delete:
                    MyDatabase.DeleteExercise(exerciseId);
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}