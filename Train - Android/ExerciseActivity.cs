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
    interface IExerciseActivity
    {
        void ReturnExercise(Exercise value);
        void ShowOverflowMenu(bool showMenu);
    }

    [Activity(Label = "", NoHistory = true)]
    public class ExerciseActivity : AppCompatActivity, IExerciseActivity
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
                StartEditView(true);
            }
        }



        private void StartCardView()
        {
            Bundle args = new Bundle();
            SaveExerciseProperties(args);
            //ShowOverflowMenu(true); //showing menu
            var trans = SupportFragmentManager.BeginTransaction();
            var nextFragment = new ExerciseCardViewFragment();
            nextFragment.Arguments = args;
            trans.Add(Resource.Id.fragmentContainer_activityExercise, nextFragment, "ExerciseCardViewFragment");
            trans.Commit();
        }

        private void StartEditView(bool newExerise)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            var nextFragment = new ExerciseEditFragment();
            Bundle args = new Bundle();
            args.PutBoolean("newExercise", newExerise);

            if (!newExerise)
            {
                SaveExerciseProperties(args);
            }

            nextFragment.Arguments = args;
            trans.Add(Resource.Id.fragmentContainer_activityExercise, nextFragment, "ExerciseEditFragment");
            trans.Commit();
        }

        private void SaveExerciseProperties(Bundle args)
        {
            //Bundle args = new Bundle();
            args.PutString("name", exercise.Name);
            args.PutString("description", exercise.Description);
            args.PutInt("time", exercise.Time);       
            args.PutByte("difficulty", (sbyte)exercise.Difficulty);     //only way is using sbyte, but htat doesn't matter because difficulty is between 1 and 10
            args.PutString("place", exercise.Place);
            //return args;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.EditOrDeleteMenu, menu);
            this.menu = menu;
            return true;
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
                    StartEditView(false);
                    return true;
                case Resource.Id.item_delete:
                    MyDatabase.DeleteExercise(exerciseId);
                    Finish();
                    return true;
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}