
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
        connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));
        connection.Open();
        
        IDataReader dataReader = ReadSavedData();

        while (dataReader.Read())
        {

            scores.text += dataReader["Run"] + "\t\t\t" + dataReader["Time"] + "\t\t" + dataReader["CharacterName"] + "\n";


        }

        connection.Close();
    }

    // Update is called once per frame
   public void dellSelect()
    {
        current = "Dell";
        character.text = "Current Character: " + current;
    }

   public void estaSelect()
    {
        current = "Esta";
        character.text = "Current Character: " + current;
    }

   public void tiffanySelect()
    {
        current = "Tiffany";
        character.text = "Current Character: " + current;
    }

   public void mycullSelect ()
    {
        current = "Mycull";
        character.text = "Current Character: " + current;
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