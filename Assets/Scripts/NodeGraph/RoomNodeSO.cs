using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeScriptableObject : MonoBehaviour
{
    [HideInInspector] public string id:
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSo roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
}
