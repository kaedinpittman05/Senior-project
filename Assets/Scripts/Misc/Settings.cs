using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will be implamented at a later time
/// </summary>

public static class Setting
{
    #region UNITS
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion


    #region ROOM SETTINGS

    public const int maxChildCorridors = 3; // Maz number of child coridors leading from a room. - mazimum should be 3 although this is not reccomended 
    // it can cause the dungeon building to fail since the rooms are more likely to not fit togeather;
    public const float doorUnlockDelay = 1f;


    #endregion

    #region ANIMATOR PARAMETERS

    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");
    #endregion

    #region GAMEOBJECT TAGS
    public const string playerTag = "Player";
    #endregion
}
