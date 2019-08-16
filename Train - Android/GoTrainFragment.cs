using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Train___Android.Resources.Fragments
{
    public class GoTrainFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.goTrain_fragment, container, false);

            return view;
        }

        //public override void OnActivityCreated(Bundle savedInstanceState)
        //{
        //    //View.FindViewById<Android.Support.Design.BottomNavigation>(Resource.Id.)

        //}
    }
}