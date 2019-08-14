using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;

namespace Train___Android
{
    //https://www.c-sharpcorner.com/article/xamarin-android-create-left-drawer-layout/
    //https://www.c-sharpcorner.com/article/xamarin-android-create-android-navigation-drawer-layout-using-support-design/
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

            

        }

        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            navigationView.InflateMenu(Resource.Menu.menu); //Navigation Drawer Layout Menu Creation  
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}