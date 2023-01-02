using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


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

            // else if the room type isn't an entrance
            else
            {
                // Else get parent room for node
                Room parentRoom = dungeonBuliderRoomDictionary[roomNode.parentRoomNodeIDList[0]];

                // see if room can be placed without overlaps
                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }

        }

        return noRoomOverlaps;
    }

    /// attempt to place the room node in the dungeon if  room can be placed return the room, else return null
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        // initialise and assume overlap until proven otherwise
        bool roomOverlaps = true;

        // Do While Room Overlaps - try to place against all available doorways of teh parent until
        // teh room is successfully placed without overlap
        while (roomOverlaps)
        {
            // Select random unconnected available doorway for Parent
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvailableDoorways(parentRoom.doorWayList).ToList();

            if (unconnectedAvailableParentDoorways.Count == 0)
            {
                // If no more doorways to try then overlap failure.
                return false; // room overlaps
            }

            Doorway doorwayParent = unconnectedAvailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorways.Count)];

            // Get a random room template for room node that is consistent with the parent door orientation
            RoomTemplateSO roomtemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);

            // Create a room
            Room room = CreateRoomFromRoomTemplate(roomtemplate, roomNode);

            // Place the room - returns true if the room doesn't overlap
            if (PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                // if room dosen't overlap then set to false to exit loop
                roomOverlaps = false;

                //  Mark room as positioned
                room.isPositioned = true;

                // add room to dictionary
                dungeonBuliderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }

        return true; // no room overlaps
    }

    /// Get random room template for room node taking into account the parent doorway orientation
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomtemplate = null;

        // if room node is a corridor then select random correct corridor room template based on
        // aren doorway orientation
        if (roomNode.roomNodeType.isCorridor)
        {
            switch (doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;


                case Orientation.east:
                case Orientation.west:
                    roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;


                case Orientation.none:
                    break;

                default:
                    break;
            }
        }
        // Else select random room template
        else
        {
            roomtemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }


        return roomtemplate;
    }

    /// place the room - returns true if the room doesn't overlap, false otherwise
    private bool PlaceTheRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        // Get current room doorway position
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);

        // Return if no doorway in room opposite to parent doorway
        if (doorway == null)
        {
            // just mark the parent doorway as unavailable so we don't try and connect it again
            doorwayParent.isUnavailable = true;

            return false;
        }

        // Calculate 'world' grid parent doorway positon
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;

        // Calculare adjustment position offset based on room doorway position that we are trying to connect (e.g if this doorway is west then we need
        // to add (1,0) to the east parent doorway)

        switch (doorway.orientation)
        {
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;

            case Orientation.east:
                adjustment = new Vector2Int(-1, 0);
                break;

            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;

            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;

            defualt:
                break;
        }

        // Calculate room lower bounds and upper bounds based on positioning to align with parent doorway
        room.lowerBounds = parentDoorwayPosition + adjustment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);

        if (overlappingRoom == null)
        {
            // mark doorways as connected & unavailable
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;

            doorway.isConnected = true;
            doorway.isUnavailable = true;

            // return true to show rooms have been connected with no overlap
            return true;
        }
        else
        {
            // Just mark the parent doorway as unavailable so we dont try and connect it again
            doorwayParent.isUnavailable = true;

            return false;
        }

    }

    /// Get the doorway from the doorway list that has the opposite orientation to doorway
    private Doorway GetOppositeDoorway(Doorway parentDoorway, List<Doorway> doorwayList)
    {

        foreach (Doorway doorwayToCheck in doorwayList)
        {
            if(parentDoorway.orientation == Orientation.east && doorwayToCheck.orientation == Orientation.west)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.west && doorwayToCheck.orientation == Orientation.east)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.north && doorwayToCheck.orientation == Orientation.south)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.south && doorwayToCheck.orientation == Orientation.north)
            {
                return doorwayToCheck;
            }
        }
        return null;
    }

    // Chek for rooms that overlap the upper and lower bounds parameters, and if there are overlapping rooms then return room else return null
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        //iterate througn all rooms
        foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuliderRoomDictionary)
        {
            Room room = keyvaluepair.Value;

            // skip if same room as room to test or room hasn't been positioned
            if (room.id == roomToTest.id || !room.isPositioned)
                continue;

            // If room overlaps
            if (IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }


        // Return
        return null;
    }

    // Check if 2 rooms overlap each other - return true if they overlap or false if they dont overlap
    private bool IsOverLappingRoom(Room room1, Room room2)
    {
        bool isOverlappingX = IsOverLappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);

        bool isOverlappingY = IsOverLappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);

        if (isOverlappingX && isOverlappingY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Check if interval 1 overlaps interval 2 this method is used by the IsOverLappingRoom method
    private bool IsOverLappingInterval(int imin1, int imax1, int imin2, int imax2)
    {
        if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// Get a random room template from the roomtemplate list that matches the roomType and return it
    /// (retun null if no matching room templates found).
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
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

    /// Get unconnected doorways
    private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
    {
        // Loop through doorway list
        foreach (Doorway doorway in roomDoorwayList)
        {
           if(!doorway.isConnected && !doorway.isUnavailable)
                yield return doorway;
        }
    }

    /// Create room based on room template and layout Node, and return the created room
    private Room CreateRoomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        // Initialise room from template
        Room room = new Room();

        room.templateID = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPositionArray;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;
        room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwayList);

        // Set parent ID for room
        if (roomNode.parentRoomNodeIDList.Count == 0) // Entrance
        {
            room.parentRoomID = "";
            room.isPreviouslyVisited = true;

            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomID = roomNode.parentRoomNodeIDList[0];
        }

        return room;
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

    /// Create deep copy of string list
    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();

        foreach (string stringValue in oldStringList) 
        { 
            newStringList.Add(stringValue);
        }

        return newStringList;
    }

    // Instantiate the dungeon room gameobjects from the prefabs
    private void InstantiateRoomGameObjects()
    {
        // Iterate through all dungeon rooms.
        foreach (KeyValuePair<string, Room> keyvaluepair in dungeonBuliderRoomDictionary)
        {
            Room room = keyvaluepair.Value;

            // Calculate room position (remember the room instantiatation position needs to be adjusted by the room template lower bounds)
            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);

            // Instantiate room
            GameObject roomGameobject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);

            // Get instantiated room component from instantiated prefab.
            InstantiatedRoom instantiatedRoom = roomGameobject.GetComponentInChildren<InstantiatedRoom>();

            instantiatedRoom.room = room;

            // Initialise The Instantiated Room
            instantiatedRoom.Initialise(roomGameobject);

            // Save gameobject reference.
            room.instantiatedRoom = instantiatedRoom;
        }
    }

    // get a room template by room templte ID, returns nul id ID doesnt exist
    public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
    {
        if (roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
        else
        {
            return null;
        }
    }

    // Get room by roomID if no room exists with that id return null
    public Room GetRoomByRoomID(string roomID)
    {
        if (dungeonBuliderRoomDictionary.TryGetValue(roomID, out Room room))
        {
            return room;
        }
        else
        {
            return null;
        }
    }

    /// Create deep copy of doorway list
    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
    {
        List<Doorway> newDoorwayList = new List<Doorway>();

        foreach (Doorway doorway in oldDoorwayList)
        {
            Doorway newDoorway = new Doorway();

            newDoorway.position = doorway.position;
            newDoorway.orientation = doorway.orientation;
            newDoorway.doorPrefab = doorway.doorPrefab;
            newDoorway.isConnected = doorway.isConnected;
            newDoorway.isUnavailable = doorway.isUnavailable;
            newDoorway.doorwayStartCopyPosition = doorway.doorwayStartCopyPosition;
            newDoorway.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;
            newDoorway.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;

            newDoorwayList.Add(newDoorway);
        }

        return newDoorwayList;
    }

    /// Clear dungeon room game objects and dungeon room dictionary
    private void ClearDungeon()
    {
        // Destroy instantiated dungeon gameobjects and clear dungeon manager room dictionary
        if (dungeonBuliderRoomDictionary.Count > 0)
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
