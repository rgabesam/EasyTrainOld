using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;


namespace Train___Android.Database
{

    interface IDisplayable
    {
        string Name { get; set; }
        string Description { get; set; }
        int Id { get; set; }
    }

    [Table("Exercises")]
    public class Exercise : IDisplayable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }    //minimal time necessary to complete the exercise
        public byte Difficulty { get; set; }    //from 1 to 10, where 10 is the hardest
        //tags tags are stored in separate table so it has its own class  -  allows to group exercise by users own "attributes"
        public string Place { get; set; }

        

    }

    [Table("Trainings")]
    public class Training : IDisplayable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }    //sum of exercises times ~ in minutes
        public byte Difficulty { get; set; }    //from 1 to 10, where 10 is the hardest
        public string Place { get; set; }

        

    }

    [Table("Plans")]
    public class Plan : IDisplayable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }    //number of days the training plan least
        public byte Difficulty { get; set; }    //from 1 to 10, where 10 is the hardest

        
    }

    [Table("Tags")]
    class Tag
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Table("TrainingStatistics")]
    class TrainingStatistic
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TrainingId { get; set; }
    }


    //next classes and tables as well are to hold relations, 
    //composite key is not supported

    [Table("ExerciseTags")]
    class ExerciseTag
    {
        public int ExerciseId { get; set; }
        public int TagId { get; set; }
    }

    [Table("TrainingTags")]
    class TrainingTag
    {
        public int TrainingId { get; set; }
        public int TagId { get; set; }
    }

    [Table("ExerciseInTraining")]
    class ExerciseInTraining
    {
        public int ExerciseId { get; set; }
        public int TrainingId { get; set; }
    }

    [Table("TrainingInPlan")]
    class TrainingInPlan
    {
        public int TrainingId { get; set; }
        public int PlanId { get; set; }
    }

    
}