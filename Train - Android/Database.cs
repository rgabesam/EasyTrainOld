using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.Util;
using SQLite;


//https://docs.microsoft.com/cs-cz/xamarin/android/data-cloud/data-access/using-sqlite-orm


namespace Train___Android.Database
{
    static class MyDatabase
    {
        static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"trainings.db");
        public static SQLiteConnection connection;

        static MyDatabase()
        {
            try
            {
                connection = new SQLiteConnection(dbPath);
            }
            catch (SQLiteException e)
            {
                Log.Info("SQLiteException", e.Message);
            }
            
            CreateDb();
        }

        static public void CreateDb()
        {
            connection.CreateTable<Exercise>();
            connection.CreateTable<Training>();
            connection.CreateTable<Plan>();
            connection.CreateTable<ExerciseInTraining>();
        }

        #region Exercise methods

        static public void InsertExercise(Exercise exercise)
        {
            connection.Insert(exercise);
        }

        static public List<Exercise> GetAllExercises()
        {
            return connection.Table<Exercise>().ToList();
            
        }

        static public void UpdateExercise(Exercise exercise)
        {
            connection.Query<Exercise>($"UPDATE Exercises SET " +
                $"Name='{exercise.Name}'," +
                $"Description='{exercise.Description}'," +
                $"Time='{exercise.Time}'," +
                $"Difficulty='{exercise.Difficulty}'," +
                $"Place='{exercise.Place}' " +
                $"WHERE Id={exercise.Id}");
        }

        static public Exercise GetExercise(int id)
        {
            return connection.Get<Exercise>(id);
        }

        static public void DeleteExercise(Exercise exercise)
        {
            connection.Delete<Exercise>(exercise.Id);
        }
        static public void DeleteExercise(int id)
        {
            connection.Delete<Exercise>(id);
        }

        #endregion

        #region Training methods

        static public void InsertTraining(Training training)
        {
            connection.Insert(training);
        }

        static public List<Training> GetAllTrainings()
        {
            return connection.Table<Training>().ToList();
        }

        static public void UpdateTraining(Training training)
        {

            //there or somewhere else must be upgraded time, diff, place, because that is depentdent on exercises
            connection.Query<Training>($"UPDATE Trainings SET " +
                $"Name='{training.Name}'," +
                $"Description='{training.Description}' " +
                $"WHERE Id={training.Id}");
        }

        static public Training GetTraining(int id)
        {
            return connection.Get<Training>(id);
        }

        static public void DeleteTraining(Training training)
        {
            connection.Delete<Training>(training.Id);
        }
        static public void DeleteTraining(int id)
        {
            connection.Delete<Training>(id);
        }

        #endregion

        #region Plans methods

        static public void InsertPlan(Plan plan)
        {
            connection.Insert(plan);
        }

        static public List<Plan> GetAllPlans()
        {
            return connection.Table<Plan>().ToList();
        }

        static public void UpdatePlan(Plan plan)
        {

            //there or somewhere else must be upgraded time, diff because that is depentdent on trainings
            connection.Query<Plan>($"UPDATE Plans SET " +
                $"Name='{plan.Name}'," +
                $"Description='{plan.Description}' " +
                $"WHERE Id={plan.Id}");
        }

        static public Plan GetPlan(int id)
        {
            return connection.Get<Plan>(id);
        }

        static public void DeletePlan(Plan plan)
        {
            connection.Delete<Plan>(plan.Id);
        }
        static public void DeletePlan(int id)
        {
            connection.Delete<Plan>(id);
        }

        #endregion

        #region ExerciseInTraining

        static public void InsertExerciseToTraining(ExerciseInTraining item)
        {
            connection.Insert(item);
        }

        static public List<ExerciseInTraining> GetAllExercisesInAllTrainings()
        {
            return connection.Table<ExerciseInTraining>().ToList();
        }

        static public List<Exercise> GetAllExercisesOfTraining(int trainingId)
        {
            return connection.Query<Exercise>($"SELECT Id, Name, Description, Time, Difficulty, Place " +
                $"FROM Exercises a " +
                $"INNER JOIN" +
                    $"(SELECT * FROM ExerciseInTraining " +
                    $"WHERE TrainingId='{trainingId}') b " +
                $"ON a.Id = b.ExerciseId");
        }

        static public List<Exercise> GetAllExercisesWhichAreNotInTraining(int trainingId)
        {
            return connection.Query<Exercise>($"SELECT * FROM Exercises " +
                $"EXCEPT " +
                    $"SELECT Id, Name, Description, Time, Difficulty, Place " +
                    $"FROM Exercises a " +
                    $"INNER JOIN" +
                        $"(SELECT * FROM ExerciseInTraining " +
                        $"WHERE TrainingId='{trainingId}') b " +
                    $"ON a.Id = b.ExerciseId");
        }

        static public void DeleteExerciseInTraining(int exerciseId, int trainingId)
        {
            connection.Execute($"DELETE FROM ExerciseInTraining " +
                $"WHERE TrainingId='{trainingId}' " +
                $"AND ExerciseId='{exerciseId}'");
        }        

        #endregion
    }
}