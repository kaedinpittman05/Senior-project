using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;




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