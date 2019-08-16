using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Train___Android.Resources.Fragments;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;


namespace Train___Android
{
    [Activity(Label = "TrainingActivity")]
    public class TrainingActivity : AppCompatActivity
    {
        public BottomNavigationView bottomNavigation;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_training);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);


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


    }
}