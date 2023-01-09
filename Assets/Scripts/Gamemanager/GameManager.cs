using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    #region Header DUNGEON LEVELS

    [Space(10)]
    [Header("DUNGEON LEVEL")]

    #endregion Header DUNGEON LEVELS

    #region Tooltip

    [Tooltip("Populate with he dungeon level scriptable objects")]
    #endregion Tooltip

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip

    [Tooltip("Populate with the startign dungeon level for testing , first level = 0")]

    #endregion Tooltip

    [SerializeField] private int currentDungeonLevelListIndex = 0;
    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private InstantiatedRoom bossRoom;

    protected override void Awake()
    {
        // Call base class
        base.Awake();

        // Set player details - saved in current player scriptable object from the main menu
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        // Instantiate player
        InstantiatePlayer();

    }

    private void OnEnable()
    {
        // Subscribe to room changed event.
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

    }


    private void OnDisable()
    {
        // Unsubscribe from room changed event
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        

    }

    /// <summary>
    /// Handle room changed event
    /// </summary>
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    


    /// <summary>
    /// Create player in scene at position
    /// </summary>
    private void InstantiatePlayer()
    {
        // Instantiate player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        // Initialize Player
        player = playerGameObject.GetComponent<Player>();

        player.Initialize(playerDetails);

    }




    // Start is called before the first frame update
    private void Start()
    {
        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleGameState();


        // For testing
        ///if (Input.GetKeyDown(KeyCode.R)) 
        ///{
            //gameState = GameState.gameStarted;
        //}
    }

    /// <summary>
    /// Handle game state
    /// </summary>
    
    private void HandleGameState()
    {
        // Handle game state
        switch (gameState)
        {

            case GameState.gameStarted:

                // Play first level
                PlayDungeonLevel(currentDungeonLevelListIndex);

                gameState = GameState.playingLevel;
                break;


            // While engaging enemies handle the escape key for the pause menu
            case GameState.engagingEnemies:

                

                break;

        }
    }

    /// <summary>
    /// Set the current room the player in in
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;

        //// Debug
        //Debug.Log(room.prefab.name.ToString());
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        //Build dungeon for level
        bool dungeonBuiltSucessfully = DungeonBulider.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.Log("Couldn't build dungeno from specified rooms and node graphs");
        }

        // Set player roughly mid-room
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // Get nearest spawn point in room nearest to player
        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

    }

    /// <summary>
    /// Room enemies defated - test if all dungeon rooms have been cleared of enemies - if so load
    /// next dungeon game level
    /// </summary>
    private void RoomEnemiesDefeated()
    {
        // Initialise dungeon as being cleared - but then test each room
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        // Loop through all dungeon rooms to see if cleared of enemies
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBulider.Instance.dungeonBuliderRoomDictionary)
        {
            // skip boss room for time being
            if (keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            // check if other rooms have been cleared of enemies
            if (!keyValuePair.Value.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        // Set game state
        // If dungeon level completly cleared (i.e. dungeon cleared apart from boss and there is no boss room OR dungeon cleared apart from boss and boss room is also cleared)
        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            // Are there more dungeon levels then
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else
            {
                gameState = GameState.gameWon;
            }
        }
    }




        /// <summary>
        /// Get the current room the player is in
        /// </summary>
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    /// <summary>
    /// Get the current dungeon level
    /// </summary>
    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif

    #endregion Validation

}
