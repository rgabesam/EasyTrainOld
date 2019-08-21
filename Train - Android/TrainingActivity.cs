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
        public BottomNavigationView bottomNavigation;
        ListView listView;
        List<IDisplayable> tableItems;

        private static bool isFabOpen;
        private FloatingActionButton fab_addItem;
        private FloatingActionButton fab_filter;
        private FloatingActionButton fabMain;
        private View bgFabMenu;


        private void FillTAbleItems()
        {
            //tableItems = new List<IDisplayable>();
            for (int i = 0; i < 50; i++)
            {
                IDisplayable item = new Exercise();
                item.Name = $"Item no. {i}";
                item.Description = $"This is very long and very and extra ordinary boring description of item no.   {i}   just to have there something";
                tableItems.Add(item);
            }
        }

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
                var intent = new Intent(this, typeof(ExerciseActivity));
                intent.PutExtra("cardView", false);
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
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            

            //for (int i = 0; i < 20; i++)
            //{
            //    Exercise exr = new Exercise();
            //    exr.Name = $"Exercise no. {i}";
            //    exr.Description = $"This is very long and very and extra ordinary boring description of item no.   {i}   just to have there something";
            //    MyDatabase.InsertExercise(exr);
            //}
            tableItems = new List<IDisplayable>();
            //FillTAbleItems();
            var exercises = MyDatabase.GetAllExercises();
            foreach (var exr in exercises)
            {
                tableItems.Add(exr);
            }
            listView.Adapter = new TrainingScreenAdapter(this, tableItems);
            //listView.ItemClick += OnListItemClick;  // to be defined

            //var trans = SupportFragmentManager.BeginTransaction();
            //trans.Add(Resource.Id.fragmentContainer, new FAQFragment(), "FAQFragment");
            //trans.Commit();


            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            bottomNavigation.NavigationItemSelected += (sender, e) =>
            {
                switch (e.Item.ItemId)
                {
                    case Resource.Id.bottomNav_exercises:
                        //ReplaceFragment(Resource.Id.fragmentContainer, new HomeFragment(), "HomeFragment");
                        Toast.MakeText(Application.Context, "exercise", ToastLength.Short).Show();
                        break;
                    case Resource.Id.bottomNav_trainings:
                        Toast.MakeText(Application.Context, "training", ToastLength.Short).Show();
                        //ReplaceFragment(Resource.Id.fragmentContainer, new ReportBugFragment(), "ReportBugFragment");
                        break;
                    case Resource.Id.bottomNav_plans:
                        Toast.MakeText(Application.Context, "plans", ToastLength.Short).Show();
                        //ReplaceFragment(Resource.Id.fragmentContainer, new DatabaseFragment(), "DatabaseFragment");
                        break;
                }
            };
        }

       
        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }

        private void ReplaceFragment(int targetId, Android.Support.V4.App.Fragment fragment, string tag)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(targetId, fragment, tag);
            trans.Commit();
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