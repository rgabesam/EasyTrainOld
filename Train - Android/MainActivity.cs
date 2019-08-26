using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Train___Android.Resources.Fragments;
using Android.Content;

namespace Train___Android
{
    

    //https://stackoverflow.com/questions/44611228/navigation-drawer-in-multiple-activities-in-xamarin-android ...fragments
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);

            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            SetupDrawerContent(navigationView);

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.fragmentContainer, new HomeFragment(), "HomeFragment");
            trans.Commit();


        }

        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_home:
                        {
                            ReplaceFragment(Resource.Id.fragmentContainer, new HomeFragment(), "HomeFragment");
                            Toast.MakeText(Application.Context, "Welcome home !", ToastLength.Short).Show();
                            break;
                        }
                    case Resource.Id.nav_goTrain:
                        {
                            //ReplaceFragment(Resource.Id.fragmentContainer, new GoTrainFragment(), "GoTrainFragment");
                            var nextAct = new Intent(this, typeof(TrainingActivity));
                            StartActivity(nextAct);
                            Toast.MakeText(Application.Context, "Remember: no pain no gain.", ToastLength.Short).Show();
                            break;
                        }
                    case Resource.Id.nav_import:
                        {
                            ReplaceFragment(Resource.Id.fragmentContainer, new DatabaseFragment(), "DatabaseFragment");
                            Toast.MakeText(Application.Context, "What do you have for me today?", ToastLength.Short).Show();
                            break;
                        }
                    case Resource.Id.nav_faq:
                        {
                            ReplaceFragment(Resource.Id.fragmentContainer, new FAQFragment(), "FAQFragment");
                            Toast.MakeText(Application.Context, "App is simple, no FAQ needed.", ToastLength.Short).Show();
                            break;
                        }
                    case Resource.Id.nav_bug:
                        {
                            ReplaceFragment(Resource.Id.fragmentContainer, new ReportBugFragment(), "ReportBugFragment");
                            Toast.MakeText(Application.Context, "App hasn't got any bugs.", ToastLength.Short).Show();
                            break;
                        }

                }

                drawerLayout.CloseDrawers();

                
            };
        }

       


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.menu); //Navigation Drawer Layout Menu Creation  
            return true;
        }

        private void ReplaceFragment(int targetId, Android.Support.V4.App.Fragment fragment, string tag)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Replace(targetId, fragment, tag);
            trans.Commit();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}