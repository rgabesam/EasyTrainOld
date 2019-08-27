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
using static Train___Android.TrainingActivity;
using Train___Android.Database;


namespace Train___Android
{
    public class CheckableItemListFragment : Fragment
    {
        mode viewMode; //if mode is training it means app will show exercises to add
        Button addButton;
        ListView listView;
        List<Exercise> exercises;
        //List<Training> trainings;
        int itemId;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            viewMode = Enum.Parse<mode>(Arguments.GetString("viewMode"));
        }
        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_exercise_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_training_group, false);
            menu.SetGroupVisible(Resource.Id.editOrDeleteMenu_plan_group, false);
            base.OnPrepareOptionsMenu(menu);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.checkable_item_list_fragment, container, false);
            itemId = Arguments.GetInt("itemId");

            addButton = view.FindViewById<Button>(Resource.Id.add_checked_button);
            addButton.Click += AddCheckedExercises;
            listView = view.FindViewById<ListView>(Resource.Id.checkable_item_list);

            switch (viewMode)
            {
                case mode.training:
                    exercises = MyDatabase.GetAllExercisesWhichAreNotInTraining(itemId);
                    listView.Adapter = new TrainingScreenAdapter<Exercise>(Activity, exercises, true);
                    break;
                case mode.plan:
                    break;
            }


            return view;
        }

        private void AddCheckedExercises(object sender, EventArgs e)
        {
            var checkedItems = listView.CheckedItemPositions;
            //for (int i = 0; i < checkedItems.Size(); i++)
            for (int i = 0; i < listView.Adapter.Count; i++)
            {
                //MyDatabase.InsertExerciseToTraining(new ExerciseInTraining(((TrainingScreenAdapter<Exercise>)listView.Adapter)[checkedItems.ValueAt(i)].Id, itemId));
                if(listView.item)
                    MyDatabase.InsertExerciseToTraining(new ExerciseInTraining(((TrainingScreenAdapter<Exercise>)listView.Adapter)[i].Id, itemId));
                listView.
            }

            Activity.SupportFragmentManager.PopBackStackImmediate();
        }
    }
}