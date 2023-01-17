
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;

public class scoreLoader : MonoBehaviour
{
   
    private string dbName = "game_database";
    IDbConnection connection;

    public Text scores;
    public Text character;
    private string current = "";
    private float startmusic = 1;
    private float startsfx = 1; 


    // Start is called before the first frame update
    void Start()
    {

        try
        {
            if (!File.Exists(Application.dataPath + "\\StreamingAssets\\" + dbName + ".db"))
            {
                SqliteConnection.CreateFile(Application.dataPath + "\\StreamingAssets\\" + dbName + ".db");
                connection = new SqliteConnection("Data Source="+ Application.dataPath + "\\StreamingAssets\\" + dbName + ".db");
                connection.Open();
                PushCommand(string.Format("CREATE TABLE IF NOT EXISTS \"Scores\" (\r\n\t\"Run\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"Time\"\tREAL,\r\n\tPRIMARY KEY(\"Run\")\r\n)"), connection);
                PushCommand(string.Format("CREATE TABLE IF NOT EXISTS \"Settings\" (\r\n\t\"Current\"\tINTEGER UNIQUE,\r\n\t\"music\"\tNUMERIC,\r\n\t\"sound\"\tNUMERIC,\r\n\tPRIMARY KEY(\"Current\")\r\n)"), connection);




                connection.Close();
            }








        }
        catch(Exception e)
        {
            scores.text = e.Message;
        }








        connection = new SqliteConnection("Data Source=" + Application.dataPath + "\\StreamingAssets\\" + dbName + ".db");
        //Opens sqlite connectiosn, reads run and time, puts them into scores

        connection.Open();

        
        PushCommand(string.Format("CREATE TABLE IF NOT EXISTS \"Scores\" (\r\n\t\"Run\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"Time\"\tREAL,\r\n\tPRIMARY KEY(\"Run\")\r\n)"), connection);
        PushCommand(string.Format("CREATE TABLE IF NOT EXISTS \"Settings\" (\r\n\t\"Current\"\tINTEGER UNIQUE,\r\n\t\"music\"\tNUMERIC,\r\n\t\"sound\"\tNUMERIC,\r\n\tPRIMARY KEY(\"Current\")\r\n)"), connection);
        scores.text = "This Worked 3";


        IDataReader dataReader = ReadSavedData();

            while (dataReader.Read())
            {

                scores.text += dataReader["Run"] + "\t\t\t" + dataReader["Time"] + "s\n";


            }

            connection.Close();
    }





    void PushCommand(string commandString, IDbConnection connection)
    {
        // Create new command
        IDbCommand command = connection.CreateCommand();
        // Add your comment text (queries)
        command.CommandText = string.Format("{0}", commandString);
        // Execute command reader - execute command
        command.ExecuteReader();
    }

    IDataReader ReadSavedData()
    {
        scores.text = "";
        
        // Create command (query)
        IDbCommand command = connection.CreateCommand();
        // Get all data in Slot = 1 from coordinates table
        command.CommandText = "SELECT * FROM Scores ORDER BY Time LIMIT 7;";
        // Execute command
        IDataReader dataReader = command.ExecuteReader();
        return dataReader;
    }

}
