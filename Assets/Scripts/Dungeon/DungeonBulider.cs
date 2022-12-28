using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Configuration;

[DisallowMultipleComponent]
public class DungeonBulider : SingletonMonoBehaviour<DungeonBulider>
{
    public Dictionary<string, Room> dungeonBuliderRoomDictionary = new Dictionary<string, Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuliderSuccessful;

    protected override void Awake()
    {
        base.Awake();

        // load the room node type list
        LoadRoomNodeTypeList();

        // set dimmed material to fully visible
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 1f);
    }

    /// Load the room node type list
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// Generate random dungeon, returns true if dungeon built, false if failed
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomtemplateList = currentDungeonLevel.roomTemplateList;

        // load the scriptable object room templates into the dictionary
        LoadRoomNodeTemplatesIntoDictionary();

        dungeonBuliderSuccessful = false;
        int dungeonBulidAttempts = 0;

        while (!dungeonBuliderSuccessful && dungeonBulidAttempts < Settings.maxDungeonBuildAttempts)
        {

        }
    }

    /// Load the room templates into the dictionary
    private void LoadRoomNodeTemplatesIntoDictionary()
    {
        //Clear room template dictionary
        roomTemplateDictionary.Clear();

        // load room template list into dictionary
        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log("Duplicate Room Template Key In " + roomTemplateList);
            }
        }
    }
}
