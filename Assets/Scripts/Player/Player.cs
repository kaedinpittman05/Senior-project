using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Will be implamented at a later time
/// </summary>


public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;


    /// <summary>
    /// Initialize the player
    /// </summary>
    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
}