using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using System;

public class sqlTest : MonoBehaviour
{

    public controlScript Controller;

    public void sqlStart()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand dbcmd;
        IDataReader reader;

        dbcmd = dbcon.CreateCommand();
        string droptable =
          "DROP TABLE IF EXISTS maintable";

        dbcmd.CommandText = droptable;
        reader = dbcmd.ExecuteReader();
        Debug.Log("tabledeleted");





        dbcmd = dbcon.CreateCommand();
        string q_createTable =
          "CREATE TABLE IF NOT EXISTS maintable (date INTEGER,val FLOAT,notes varchar(255),dateSince2022 INTEGER)";

        dbcmd.CommandText = q_createTable;
        reader = dbcmd.ExecuteReader();
        Debug.Log("connection success");





        IDbCommand cmnd = dbcon.CreateCommand();


        string additem = "INSERT INTO maintable (date,val,notes,dateSince2022) VALUES (1650582659,3,'hello',110)";

        cmnd.CommandText = additem;
        cmnd.ExecuteNonQuery();

        dbcon.Close();
    }


    public void add(DateTime date, int val, string notes)
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand dbcmd;
        IDataReader reader;

        IDbCommand cmnd = dbcon.CreateCommand();

        int daysSince = (date - (new DateTime(2022, 1, 1).Date)).Days;
        cmnd.CommandText = $"DELETE FROM maintable WHERE dateSince2022 = {daysSince}";
        cmnd.ExecuteNonQuery();


        cmnd.CommandText = $"INSERT INTO maintable (date, val, notes, dateSince2022) VALUES ({((DateTimeOffset)date).ToUnixTimeSeconds() - 14400}, {val}, '{notes}', {daysSince})";
        cmnd.ExecuteNonQuery();
        dbcon.Close();


    }

    public void addSam()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand dbcmd;
        IDataReader reader;

        IDbCommand cmnd = dbcon.CreateCommand();

        DateTime date = DateTime.Now;

        float val = 5f;
        string notes = "nothin much";
        string additem = $"INSERT INTO maintable (date, val, notes, dateSince2022) VALUES ({((DateTimeOffset)date).ToUnixTimeSeconds() - 14400}, {val}, '{notes}', {(date - (new DateTime(2022, 1, 1).Date)).Days})";
        cmnd.CommandText = additem;
        cmnd.ExecuteNonQuery();
        dbcon.Close();
    }



    public void read()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand dbcmd;
        IDataReader reader;

        IDbCommand cmnd_read = dbcon.CreateCommand();

        string query = "SELECT * FROM maintable";

        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Entry newEntry = new Entry(new DateTime(1970, 1, 1).AddSeconds(Int32.Parse(reader[0].ToString())), (float)Int32.Parse(reader[1].ToString()), reader[2].ToString());
            Controller.entries.Add(newEntry);
            newEntry.printEntry();
        }
        dbcon.Close();
    }


}
