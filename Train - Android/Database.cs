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
        }


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
    }
}