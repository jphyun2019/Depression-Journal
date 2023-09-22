using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;

public class sqlTest : MonoBehaviour
{

    // Controller main script
    public controlScript Controller;


    // Called at startup to connect to tables
    public void sqlStart()
    {
        // Connects to the table
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand dbcmd;
        IDataReader reader;

        // Creates an empty table if it does not exist
        dbcmd = dbcon.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS maintable (date INTEGER,val FLOAT,notes varchar(255),dateSince2022 INTEGER)";

        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();
        Debug.Log("connection success");



        dbcon.Close();
    }


    // Adds a row to the database
    public void add(DateTime date, float val, string notes)
    {

        // Connects to the table
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();


        IDbCommand cmnd = dbcon.CreateCommand();

        // Delete any entries sharing the same date value, there can only be one entry per day
        // Acts as Update at the same time
        int daysSince = (date - (new DateTime(2022, 1, 1).Date)).Days;
        cmnd.CommandText = $"DELETE FROM maintable WHERE dateSince2022 = {daysSince}";
        Debug.Log("adding: " + date + " day: " + daysSince + " value: " + val);
        cmnd.ExecuteNonQuery();


        // Inserts the entry into the database


        bool isDaylightSaving = date.IsDaylightSavingTime();
        
        cmnd.CommandText = $"INSERT INTO maintable (date, val, notes, dateSince2022) VALUES ({((DateTimeOffset)date).ToUnixTimeSeconds() - (isDaylightSaving? 14400 : 18000)}, {val}, '{notes}', {daysSince})";
        Debug.Log("unix: " + (((DateTimeOffset)date).ToUnixTimeSeconds() - (isDaylightSaving ? 14400 : 18000)));
        cmnd.ExecuteNonQuery();
        dbcon.Close();


    }

    // Adds a sample data entry
    public void addSam()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();


        IDbCommand cmnd = dbcon.CreateCommand();

        DateTime date = DateTime.Now;

        float val = 5f;
        string notes = "nothin much";
        string additem = $"INSERT INTO maintable (date, val, notes, dateSince2022) VALUES ({((DateTimeOffset)date).ToUnixTimeSeconds() - 14400}, {val}, '{notes}', {(date - (new DateTime(2022, 1, 1).Date)).Days})";
        cmnd.CommandText = additem;
        cmnd.ExecuteNonQuery();
        dbcon.Close();
    }


    // Reads from the database
    public void read()
    {
        // Connects to the table
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDataReader reader;

        IDbCommand cmnd = dbcon.CreateCommand();

        // Delete all blank dates
        cmnd.CommandText = $"DELETE FROM maintable WHERE val = 0";
        cmnd.ExecuteNonQuery();

        IDbCommand cmnd_read = dbcon.CreateCommand();

        // Reads all rows into a reader
        string query = "SELECT * FROM maintable";

        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        // Clears the Container List of entries
        Controller.entries.Clear();

        // For each row
        while (reader.Read())
        {
            // Creates a a new Entry Object
            Entry newEntry = new Entry(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(reader[0].ToString())), float.Parse(reader[1].ToString()), reader[2].ToString());
            // Adds it to the Container List
            Controller.entries.Add(newEntry);
        }
        dbcon.Close();
    }


    // Deletes an entry
    public void delete(DateTime date)
    {

        // Connects to the table
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();        

        IDbCommand cmnd = dbcon.CreateCommand();


        // Deletes from where the date matches
        int daysSince = (date - (new DateTime(2022, 1, 1).Date)).Days;
        cmnd.CommandText = $"DELETE FROM maintable WHERE dateSince2022 = {daysSince}";
        cmnd.ExecuteNonQuery();

        read();
        Debug.Log("Delete: " + date + " days: " + daysSince);

        read();


        dbcon.Close();

    }


}
