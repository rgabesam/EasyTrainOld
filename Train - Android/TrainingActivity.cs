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


    }
}