
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Diagnostics;

public class highscoreSaver: MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    private string dbName = "game_database";
    IDbConnection connection;



    // Start is called before the first frame update
    void Start()
    {
        connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));
    }

    // Update is called once per frame
    void Update()
    {
       
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
        // Create command (query)
        IDbCommand command = connection.CreateCommand();
        // Get all data in Slot = 1 from coordinates table
        command.CommandText = "SELECT * FROM Coordinates WHERE Slot = 1;";
        // Execute command
        IDataReader dataReader = command.ExecuteReader();
        return dataReader;
    }

}
