using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Train___Android.Database
{

    public enum Place { anywhere, outside, gym, playingField, elsewhere }
    
    static class EnumValuesHolder
    {
        static readonly string[] Places = { "Anywhere", "Outside", "Gym", "Indoor/outdoor playing field", "Elsewhere" };

    }


    public class Exercise
    {
        string Name { get; set; }
        string Description { get; set; }
        public int Time { get; set; }    //minimal time necessary to complete the exercise
        public List<string> tags;   //allows to group exercise by users own "attributes"
        public Place place { get; set; }
        public byte Difficulty { get; set; }    //from 1 to 10, where 10 is the hardest
    }

    public class Training
    {
        public List<Exercise> exercises;
        string Name { get; set; }
        string Description { get; set; }
        public int Time { get; set; }    //sum of exercises times
        public List<string> tags;   //allows to group exercise by users own "attributes"
        public HashSet<Place> places;
        public byte Difficulty { get; set; }    //from 1 to 10, where 10 is the hardest

    }

    public class TrainingPlan
    {

    }



}