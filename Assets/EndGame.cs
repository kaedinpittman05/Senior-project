using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    private string dbName = "game_database";
    IDbConnection connection;
    [SerializeField] Text countdownText;
    

    private int time;

    // Start is called before the first frame update
    void Start()
    {
        PushCommand(string.Format("CREATE TABLE IF NOT EXISTS \"Scores\" (\r\n\t\"Run\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"Time\"\tREAL,\r\n\tPRIMARY KEY(\"Run\")\r\n)"), connection);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if Boss is alive, if not saves scores and move to Credits
        if (GameObject.FindGameObjectWithTag("Boss") == null)
        {
            time = int.Parse(countdownText.text);
            connection = new SqliteConnection(string.Format("URI=file:Assets/Streaming Assets/{0}.db", dbName));

            connection.Open();

            PushCommand(string.Format("INSERT INTO Scores (time) Values ({0});", time), connection);

            SceneManager.LoadScene("Credits");
            FindObjectOfType<AudioManager>().StopPlaying("BattleTheme");
        }
    }

    //Pushs command to database
    void PushCommand(string commandString, IDbConnection connection)
    {
        // Create new command
        IDbCommand command = connection.CreateCommand();
        // Add your comment text (queries)
        command.CommandText = string.Format("{0}", commandString);
        // Execute command reader - execute command
        command.ExecuteReader();
    }

}
