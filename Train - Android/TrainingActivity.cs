using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Train___Android.Database;
using Train___Android.Resources.Fragments;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;


namespace Train___Android
{
    [Activity(Label = "")]
    public class TrainingActivity : AppCompatActivity
    {
        public enum mode {exercise = 1, training = 2, plan = 3};
        public BottomNavigationView bottomNavigation;
        ListView listView;
        List<Exercise> exercises;
        List<Training> trainings;
        List<Plan> plans;
        private mode _viewMode = mode.exercise;
        public mode ViewMode
        {
            get { return _viewMode; }
            private set
            {
                if (value != _viewMode)//if is value changing
                {
                    //when mode is changing listview before subscribing to new event handler needs to unsubscribe the old one
                    //if(_viewMode == mode.exercise)
                    //    listView.ItemClick -= OnListExerciseClick;
                    //else if (_viewMode == mode.training)
                    //    listView.ItemClick -= OnListTrainingClick;
                    //else if (_viewMode == mode.plan)
                    //    listView.ItemClick -= OnListPlanClick;

                    _viewMode = value;
                    UpdateListItems();
                }
            }
        }
        //mode can be only 1 or 2 or 3, each represents if user is looking at exercises=1 or trainings=2 or plans=3
        private static bool isFabOpen;
        private FloatingActionButton fab_addItem;
        private FloatingActionButton fab_filter;
        private FloatingActionButton fabMain;
        private View bgFabMenu;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_training);

            #region fab menu

            fab_addItem = FindViewById<FloatingActionButton>(Resource.Id.fab_add_trainingActivity);
            fab_filter = FindViewById<FloatingActionButton>(Resource.Id.fab_filters_trainingActivity);
            fabMain = FindViewById<FloatingActionButton>(Resource.Id.fab_menu_trainingActivity);
            bgFabMenu = FindViewById<View>(Resource.Id.bg_fab_menu_trainingActivity);

            fabMain.Click += (o, e) =>
            {
                if (!isFabOpen)
                    ShowFabMenu();
                else
                    CloseFabMenu();
            };

            fab_addItem.Click += (o, e) =>
            {
                CloseFabMenu();
                var intent = new Intent(this, typeof(EditOrViewActivity));
                intent.PutExtra("cardView", false);
                intent.PutExtra("viewMode", ViewMode.ToString()); //because there is no method to send enum, in java it is possible because there is enum ISerializable
                StartActivity(intent);
            };

            fab_filter.Click += (o, e) =>
            {
                CloseFabMenu();
                Toast.MakeText(this, "NO FILTERS MOROON!", ToastLength.Short).Show();
            };

            bgFabMenu.Click += (o, e) => CloseFabMenu();
            #endregion

            listView = FindViewById<ListView>(Resource.Id.trainings_list);
            listView.ItemClick += OnListItemClick;

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            //var trans = SupportFragmentManager.BeginTransaction();
            //trans.Add(Resource.Id.fragmentContainer, new FAQFragment(), "FAQFragment");
            //trans.Commit();

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += OnBottomNavigationItemClick;
        }

        protected override void OnResume()
        {
            UpdateListItems();
            base.OnResume();
        }

        private void OnBottomNavigationItemClick(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.bottomNav_exercises:
                    ViewMode = mode.exercise;
                    //Toast.MakeText(Application.Context, "exercise", ToastLength.Short).Show();
                    break;
                case Resource.Id.bottomNav_trainings:
                    //Toast.MakeText(Application.Context, "training", ToastLength.Short).Show();
                    ViewMode = mode.training;
                    break;
                case Resource.Id.bottomNav_plans:
                    //Toast.MakeText(Application.Context, "plans", ToastLength.Short).Show();
                    ViewMode = mode.plan;
                    break;
            }
        }

        private void UpdateListItems()//dependent on viewMode
        {
            switch (ViewMode)
            {
                case mode.exercise:
                    exercises = MyDatabase.GetAllExercises();
                    listView.Adapter = new TrainingScreenAdapter<Exercise>(this, exercises);
                    break;
                case mode.training:
                    trainings = MyDatabase.GetAllTrainings();
                    listView.Adapter = new TrainingScreenAdapter<Training>(this, trainings);
                    break;
                case mode.plan:
                    plans = MyDatabase.GetAllPlans();
                    listView.Adapter = new TrainingScreenAdapter<Plan>(this, plans);
                    break;
            }
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = null;
            switch (ViewMode)
            {
                case mode.exercise:
                    intent = new Intent(this, typeof(EditOrViewActivity));
                    intent.PutExtra("itemId", exercises[e.Position].Id);
                    break;
                case mode.training:
                    intent = new Intent(this, typeof(EditOrViewActivity));
                    intent.PutExtra("itemId", trainings[e.Position].Id);
                    break;
                case mode.plan:
                    //var intent = new Intent(this, typeof(EditOrViewActivity));
                    break;
            }
            intent.PutExtra("viewMode", ViewMode.ToString());
            intent.PutExtra("cardView", true);
            StartActivity(intent);
        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }

       

        private void CloseFabMenu()
        {
            isFabOpen = false;

            fabMain.Animate().Rotation(0f);
            bgFabMenu.Animate().Alpha(0f);
            fab_addItem.Animate()
                .TranslationY(0f)
                .Rotation(90f);
            fab_filter.Animate()
                .TranslationY(0f)
                .Rotation(90f).SetListener(new FabAnimatorListener(bgFabMenu, fab_filter, fab_addItem));
        }

        private void ShowFabMenu()
        {
            isFabOpen = true;
            fab_addItem.Visibility = ViewStates.Visible;
            fab_filter.Visibility = ViewStates.Visible;
            bgFabMenu.Visibility = ViewStates.Visible;

            fabMain.Animate().Rotation(135f);
            bgFabMenu.Animate().Alpha(1f);
            fab_addItem.Animate()
                .TranslationY(-Resources.GetDimension(Resource.Dimension.standard_100))
                .Rotation(0f);
            fab_filter.Animate()
                .TranslationY(-Resources.GetDimension(Resource.Dimension.standard_55))
                .Rotation(0f);
        }

        private class FabAnimatorListener : Java.Lang.Object, Animator.IAnimatorListener
        {
            View[] viewsToHide;

            public FabAnimatorListener(params View[] viewsToHide)
            {
                this.viewsToHide = viewsToHide;
            }

            public void OnAnimationCancel(Animator animation)
            {
            }

            public void OnAnimationEnd(Animator animation)
            {
                if (!isFabOpen)
                    foreach (var view in viewsToHide)
                        view.Visibility = ViewStates.Gone;
            }

            public void OnAnimationRepeat(Animator animation)
            {
            }

            public void OnAnimationStart(Animator animation)
            {
            }
        }


    }
}