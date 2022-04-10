using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<GameObject> Tiles;
    [Space(20)]

    public List<GameObject> StartTiles; //the first tiles aren't connected to anything, so they will have room spawn points on all (possible) sides.
    [Space(15)]
    public GameObject Deadend; //the dead end is a one-way room, used randomly in the dungeon and to close it off when its max rooms is reached.
    public GameObject HardWall; //the hardwall is only a wall, used to close rooms off when there is no space for a full room next to it.
    public GameObject ExitInterior; //special interior, the places where the player enters/exits the dungeons.
    [Space(20)]
    public int MinLength;
    public int MaxLength;
    [Space(20)]
    [SerializeField]
    Material FloorMatBase;
    [SerializeField]
    Material WallMatBase;

    [SerializeField]
    List<Color> DungeonColors;

    public List<GameObject> CurrentDungeonTiles;
    public List<GameObject> CurrentDungeonDeadEnds;

    [HideInInspector]
    public int PickedLength;
    [HideInInspector]
    public int CurrentLength;
    [HideInInspector]
    public DungeonData Data;
    [HideInInspector]
    public System.Random seed;

    [ContextMenu("Reset Dungeon")]
    void ResetDungeon()
    {
        foreach(Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }
        Data.GeneratedDoors.Clear();
        CurrentLength = 0;
        Awake();
    }

    void Awake()
    {
        //TODO: link to world generator, dungeons should be tied to seed
        PickedLength = Random.Range(MinLength, MaxLength + 1);

        seed = GameManager.instance.WorldGenerator.Seed;

        Data = GetComponentInParent<DungeonData>();
        Color ColorTop = DungeonColors[seed.Next(0, DungeonColors.Count)];
        Color ColorBot = ColorTop * 0.05f;

        Data.WallMat = Instantiate(WallMatBase);
        Data.WallMat.SetColor("BottomColor", ColorBot);
        Data.WallMat.SetColor("TopColor", ColorTop);

        Data.FloorMat = Instantiate(FloorMatBase);
        Data.FloorMat.SetColor("BottomColor", ColorTop);
        Data.FloorMat.SetColor("TopColor", ColorBot);

        Data.Init();

        GameObject FirstTile = Instantiate(StartTiles[seed.Next(0, StartTiles.Count)], transform);
        CurrentDungeonTiles.Add(FirstTile);
        FirstTile.name += "(Origin)";
    }

    //rooms that spawn at or above the maxrooms count will call this 
    public void CheckRoomCount()
    {
        CancelInvoke("PopulateDungeon");
        Invoke("PopulateDungeon", 1);
    }


    void PopulateDungeon()
    {
        int ExitsNeeded = Data.exits.Count;
        int CurrentExits = 0;

        //generates all possible exits in dead end rooms
        for(int i = 0; i <= ExitsNeeded - CurrentExits; i++)
        {
            if (CurrentExits >= ExitsNeeded || CurrentDungeonDeadEnds.Count == 0)
            {
                break;
            }
            int SelectedDeadEnd = Random.Range(0, CurrentDungeonDeadEnds.Count);
            CurrentDungeonDeadEnds[SelectedDeadEnd].GetComponent<DungeonDeadEndInteriorSpawner>().SpawnInterior(ExitInterior);
            CurrentDungeonDeadEnds.RemoveAt(SelectedDeadEnd);

            CurrentExits++;
        }

        //generates remaining exits in normal rooms
        if (CurrentExits < ExitsNeeded)
        {
            for (int i = 0; i <= ExitsNeeded - CurrentExits; i++)
            {
                if (CurrentExits >= ExitsNeeded || CurrentDungeonTiles.Count == 0)
                {
                    break;
                }
                int SelectedRoom = Random.Range(0, CurrentDungeonTiles.Count);
                CurrentDungeonTiles[SelectedRoom].GetComponent<RoomSpawnPoint>().SpawnInterior(ExitInterior);
                CurrentDungeonTiles.RemoveAt(SelectedRoom);

                CurrentExits++;
            }
        }

        foreach (GameObject room in CurrentDungeonDeadEnds)
        {
            if (room.GetComponent<DungeonDeadEndInteriorSpawner>())
            {
                room.GetComponent<DungeonDeadEndInteriorSpawner>().SpawnInterior();
            }
            else
            {
                Debug.LogWarning("Spawned DeadEnd " + room.name + " does not contain DungeonDeadEndInteriorSpawner class, is this intentional?");
            }
        }

        foreach (GameObject room in CurrentDungeonTiles)
        {
            if(room.GetComponent<RoomSpawnPoint>())
            {
                room.GetComponent<RoomSpawnPoint>().SpawnInterior();
            }
            else
            {
                Debug.LogWarning("Spawned room " + room.name + " does not contain RoomSpawnPoint class, is this intentional?");
            }
        }

    }

}
