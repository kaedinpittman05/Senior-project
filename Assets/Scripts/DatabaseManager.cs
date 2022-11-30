
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Diagnostics;

public class DatabaseManager : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            connection.Open();

            PushCommand(string.Format("UPDATE Coordinates SET XAxis = {0}, YAxis = {1} , ZAxis = {2} WHERE Slot = 1;", playerTransform.position.x, playerTransform.position.y, playerTransform.position.z), connection);
            UnityEngine.Debug.Log("Saved");
           

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Open database
            connection.Open();

            // Read X , Y , Z Axis
            IDataReader dataReader = ReadSavedData();

            // Separate Float Data and assign to player position
            while (dataReader.Read())
            {
                // Assigning saved position
                playerTransform.position = new Vector3(dataReader.GetFloat(1), dataReader.GetFloat(2), dataReader.GetFloat(3));
            }

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
        // Create command (query)
        IDbCommand command = connection.CreateCommand();
        // Get all data in Slot = 1 from coordinates table
        command.CommandText = "SELECT * FROM Coordinates WHERE Slot = 1;";
        // Execute command
        IDataReader dataReader = command.ExecuteReader();
        return dataReader;
    }

}
