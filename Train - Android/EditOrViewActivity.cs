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
using static Train___Android.TrainingActivity;

namespace Train___Android
{
    interface IEditActivity
    {
        void ReturnExercise(Exercise value);
        void ReturnTraining(Training value);
        void ReturnPlan(Plan value);
        //void ShowOverflowMenu(bool showMenu);
    }

    [Activity(Label = "", NoHistory = true)]
    public class EditOrViewActivity : AppCompatActivity, IEditActivity
    {
        Exercise exercise;
        Training training;
        Plan plan;
        IMenu menu;
        private int itemId = 0;
        private bool cardView;
        mode viewMode;
        //Android.Support.V4.App.FragmentTransaction trans;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_editOrView);

            //trans = SupportFragmentManager.BeginTransaction();

            //setting toolbar
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            viewMode = Enum.Parse<mode>(Intent.GetStringExtra("viewMode"));
            cardView = Intent.GetBooleanExtra("cardView", true);

            if (cardView)
            {
                itemId = Intent.GetIntExtra("itemId", -1);   //-1 is there just because there is required default value
                StartCardView();
            }
            else //this branch means that user is creating new exercise/training/plan NOT editing existing one
            {
                StartEditView(true);
            }
        }
            
        protected override void OnPause()
        {
            base.OnPause();
            
        }

        #region interface implementation

        public void ReturnExercise(Exercise value)  //way how to recieve back data from fragment + to update database
        {
            exercise = value;
            if (cardView) //user edited old exercise
            {
                exercise.Id = itemId;
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

        public void ReturnTraining(Training value)
        {
            training = value;
            if (cardView) 
            {
                training.Id = itemId;
                MyDatabase.UpdateTraining(training);
                Toast.MakeText(this, "Training was updated", ToastLength.Short).Show();
                StartCardView();
            }
            else
            {
                MyDatabase.InsertTraining(training);
                Toast.MakeText(this, "Training was created", ToastLength.Short).Show();
                Finish(); 
            }
        }

        public void ReturnPlan(Plan value)
        {
            plan = value;
            if (cardView) //user edited old exercise
            {
                plan.Id = itemId;
                MyDatabase.UpdatePlan(plan);
                Toast.MakeText(this, "Plan was updated", ToastLength.Short).Show();
                StartCardView();
            }
            else
            {
                MyDatabase.InsertPlan(plan);
                Toast.MakeText(this, "Plan was created", ToastLength.Short).Show();
                Finish(); 
            }
        }
        
        #endregion

        private void StartCardView()
        {
            Bundle args = new Bundle();
            args.PutInt("itemId", itemId);
            var trans = SupportFragmentManager.BeginTransaction();
            Android.Support.V4.App.Fragment nextFragment = null;
            switch (viewMode)
            {
                case mode.exercise:
                    nextFragment = new ExerciseCardViewFragment();
                    break;
                case mode.training:
                    nextFragment = new TrainingViewFragment();
                    break;
                case mode.plan:
                    nextFragment = new ExerciseCardViewFragment();//not implemented yet
                    break;
            }
            nextFragment.Arguments = args;
            trans.Replace(Resource.Id.fragmentContainer_activityEditOrView, nextFragment, nextFragment.GetType().ToString());
            trans.Commit();
        }

        private void StartEditView(bool newItem)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            var nextFragment = new EditFragment();
            Bundle args = new Bundle();
            args.PutBoolean("newItem", newItem);
            args.PutString("viewMode", viewMode.ToString());

            if (!newItem)
                args.PutInt("itemId", itemId);
                
            nextFragment.Arguments = args;
            trans.Replace(Resource.Id.fragmentContainer_activityEditOrView, nextFragment, "ExerciseEditFragment");

            if(!newItem)
                trans.AddToBackStack(null);

            trans.Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.EditOrDeleteMenu, menu);
            this.menu = menu;
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.exercise_edit:
                    StartEditView(false);
                    return true;
                case Resource.Id.exercise_delete:
                    MyDatabase.DeleteExercise(itemId);
                    Finish();
                    return true;
                case Resource.Id.training_edit:
                    StartEditView(false);
                    return true;
                case Resource.Id.training_delete:
                    MyDatabase.DeleteTraining(itemId);
                    Finish();
                    return true;
                case Resource.Id.plan_edit:
                    StartEditView(false);
                    return true;
                case Resource.Id.plan_delete:
                    MyDatabase.DeletePlan(itemId);
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