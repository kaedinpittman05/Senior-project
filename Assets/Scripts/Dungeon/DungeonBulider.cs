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
    private bool dungeonBulidSuccessful;

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
        roomTemplateList = currentDungeonLevel.roomTemplateList;

        // load the scriptable object room templates into the dictionary
        LoadRoomNodeTemplatesIntoDictionary();

        dungeonBulidSuccessful = false;
        int dungeonBulidAttempts = 0;

        while (!dungeonBulidSuccessful && dungeonBulidAttempts < Setting.maxDungeonBuildAttempts)
        {
            dungeonBulidAttempts++;

            // Select a random room node graph from the list
            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBulidSuccessful = false;

            // Loop until dungeon successfully built or more than max attempts for node graph
            while (!dungeonBulidSuccessful && dungeonRebuildAttemptsForNodeGraph <= Setting.maxDungeonRebuildAttemptsForRoomGraph)
            {
                // Clear dungeon room game objects and dungeon room dictionary
                ClearDungeon();

                dungeonRebuildAttemptsForNodeGraph++;

                // attempt To Build a random dungeon for the selected room node graph
                dungeonBulidSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }

            if (dungeonBulidSuccessful)
            {
                // Instantiate Room Game objects
                InstantiateRoomGameObjects();
            }
        }
        return dungeonBulidSuccessful;
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

    ///Attempt to randomly build the dungeon for the specified room nodegraph. Returns true if a
    /// successful random layout was generated, else returns false if a problem was encountered and
    /// another attempt is required
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        // Create open room node queue
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        // Add Entrance Node To Room Node Queue From Room Node Graph
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(ConfigXmlDocument => ConfigXmlDocument.isEntrance));

        if (entranceNode != null)
        {
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            Debug.Log("No Entrance Node");
            return false; // Dungeon Not Built
        }

        //Start with no room overlaps
        bool noRoomOverlaps = true;

        // Process open room nodes queue
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);

        //If all the room nodes have been processed and there hasn't been a room overlap then return true
        if (openRoomNodeQueue.Count == 0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// Process rooms in teh open room node queue, returning true if there are no room overlaps
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomOverlaps)
    {
        // While room nodes in open room node queue & no room overlaps detected
        while (openRoomNodeQueue.Count > 0 && noRoomOverlaps == true)
        {
            // Get next room node from open room node queue
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            // Add child nodes to queue from room node graph (with links to this parent room)
            foreach (RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            // if the room is the entrance mark as positioned and add to room dictioinary
            if (roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);


                Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

                room.isPositioned = true;

                // add room to room dictionary
                dungeonBuliderRoomDictionary.Add(room.id, room);
            }
        }
    }

    /// Get a random room template from the roomtemplate list that matches the roomType and return it
    /// (retun null if no matching room templates found).
    private RoomTemplateSO GetRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();

        // Loop through room template list
        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            // Add matching room templates
            if (roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }

        // Return null if list is zero
        if (matchingRoomTemplateList.Count == 0)
            return null;

        // Select random room template from list and retun
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];


    }

    /// Create room based on room template and layout Node, and return the created room
    private Room CreatedRoomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        // Initialise room from template
        Room room = new Room();

        room.templateID = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds;
    }



    ///Select a random room node graph from the list of room node graphs
    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if(roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];

        }
        else
        {
            Debug.Log("No room node graphs in list");
            return null;
        }
    }

    /// Clear dungeon room game objects and dungeon room dictionary
    private void ClearDungeon()
    {
        //Destroy instantated dungeon game objects and clear dungeon manager room dictionary
        if(dungeonBuliderRoomDictionary.Count > 0)
        {
            foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuliderRoomDictionary)
            {
                Room room = keyvaluepair.Value;

                if (room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }
            }
            dungeonBuliderRoomDictionary.Clear();
        }
    }






}
