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
        Intent intent;
        mode viewMode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_editOrView);

            //setting toolbar
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            viewMode = Enum.Parse<mode>(Intent.GetStringExtra("viewMode"));
            cardView = Intent.GetBooleanExtra("cardView", true);

            intent = new Intent(this, typeof(EditFragment));
            if (cardView)
            {
                itemId = Intent.GetIntExtra("itemId", 0);   //0 is there just because there is required default value
                exercise = MyDatabase.GetExercise(itemId);
                
                StartCardView();
            }
            else //this branch means that user is creating new exercise/training/plan
            {
                StartEditView(true);
            }
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
        //public void ShowOverflowMenu(bool showMenu)
        //{
        //    if (menu == null)
        //        return;
        //    menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_group, showMenu);
        //}

        #endregion

        private void StartCardView()
        {
            Bundle args = new Bundle();
            SaveItemProperties(args);
            //ShowOverflowMenu(true); //showing menu
            var trans = SupportFragmentManager.BeginTransaction();
            var nextFragment = new ExerciseCardViewFragment();
            nextFragment.Arguments = args;
            trans.Add(Resource.Id.fragmentContainer_activityExercise, nextFragment, "ExerciseCardViewFragment");
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
            {
                SaveItemProperties(args);
            }

            nextFragment.Arguments = args;
            trans.Add(Resource.Id.fragmentContainer_activityExercise, nextFragment, "ExerciseEditFragment");
            trans.Commit();
        }

        private void SaveItemProperties(Bundle args)
        {
            //Bundle args = new Bundle();
            args.PutString("name", exercise.Name);
            args.PutString("description", exercise.Description);
            if(viewMode == mode.exercise)
            {
                args.PutInt("time", exercise.Time);
                args.PutByte("difficulty", (sbyte)exercise.Difficulty);     //only way is using sbyte, but htat doesn't matter because difficulty is between 1 and 10
                args.PutString("place", exercise.Place);
            }
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
                case Resource.Id.item_edit:
                    StartEditView(false);
                    return true;
                case Resource.Id.item_delete:
                    MyDatabase.DeleteExercise(itemId);
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