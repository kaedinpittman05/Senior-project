
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
public class scoreLoader : MonoBehaviour
{
   
    private string dbName = "game_database";
    IDbConnection connection;

    public Text scores;
    public Text character;
    private string current = "";



    // Start is called before the first frame update
    void Start()
    {
        //Opens sqlite connectiosn, reads run and time, puts them into scores
        connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));
        connection.Open();
        
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
