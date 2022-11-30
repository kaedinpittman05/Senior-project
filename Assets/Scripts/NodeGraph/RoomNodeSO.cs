using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Editor Code

    // the following code should only be run in the Unity Editor
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;

    // Initalise node
    public void Initalise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        // Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    // draw node with the nodestyle
    public void Draw(GUIStyle nodeStyle)
    {
        // Draw Node Box Using Begin Area
        GUILayout.BeginArea(rect, nodeStyle);

        // start Region to Detect Poup selection changes
        EditorGUI.BeginChangeCheck();

        // Display a popup using the RoomNodeType name values that can be selcted from (default to the currently set roomNodeType)

        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

        roomNodeType = roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }

    // Populate a string array with the room node types to display that can be selected
    
    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }

        return roomArray;
    }


    // process events for the node
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // process nouse down event
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            // Process Mouse up Events
            case EventType.MouseUp:
                ProcessMouseUpEvent;
                break;
            // process mouse drag event
            case EventType.MouseDrag:
                ProcessMouseDragEvent;
                break;

            defualt:
                break;
        }
    }

    // Process mouse down events
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // left click down
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
    }

    // process left click down event
    private void ProcessLeftClickDownEvent()
    {
        selection.activeObject = this;

        // Toggle node selection
        if (isSelected == true)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }
    }

#endif

    #endregion Editor code






}