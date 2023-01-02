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

    [HideInInspector] public GameState gameState;


    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameState.gameStarted;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleGameState();


        // For testing
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            gameState = GameState.gameStarted;
        }
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

        }
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        //Build dungeon for level
        bool dungeonBuiltSucessfully = DungeonBulider.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.Log("Couldn't build dungeno from specified rooms and node graphs");
        }
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
