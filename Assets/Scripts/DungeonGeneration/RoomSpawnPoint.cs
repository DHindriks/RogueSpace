using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{
    DungeonGenerator Generator;

    List<Transform> SpawnPoints = new List<Transform>();
    [Space(40)]
    public List<GameObject> Interiors; //possible interiors the rooms can contain

    void Awake()
    {
        Generator = GetComponentInParent<DungeonGenerator>();
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.tag == "Floor")
            {
                renderer.material = Generator.Data.FloorMat;
            }else
            {
                renderer.material = Generator.Data.WallMat;
            }
        }
        Invoke("SpawnTile", Random.Range(0.1f, 0.4f));
    }

    void SpawnTile()
    {
        for (int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            SpawnPoints.Add(transform.GetChild(0).GetChild(i));

            Transform CurrentSpawnPoint = transform.GetChild(0).GetChild(i);


            //detects overlap
            Vector3 overlapTestBoxScale = new Vector3(20, 10, 20);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(CurrentSpawnPoint.position, overlapTestBoxScale, collidersInsideOverlapBox);


            if (numberOfCollidersFound != 0 && collidersInsideOverlapBox[0].transform.root != transform.root) //if overlap, spawn hardwall.
            {
                GameObject NewTile = Instantiate(Generator.HardWall, transform.parent);

                NewTile.transform.position = transform.GetChild(0).GetChild(i).position;
                NewTile.transform.LookAt(transform, Vector3.up);
            }else if (Generator.CurrentLength <= Generator.PickedLength && numberOfCollidersFound == 0) //if more tiles need to be spawned, no overlap.
            {
                //calculate if next room will be a dead end, based on amount of connecting points(max 75% chance min 0% chance)
                float Randomnmbr = Random.Range(0f, 1f);
                float Chance = 1f / (transform.GetChild(0).childCount - i);
                //spawn random dead end.
                if (Randomnmbr > Chance)
                {
                    GameObject NewTile = Instantiate(Generator.Deadend, transform.parent);
                    NewTile.transform.position = transform.GetChild(0).GetChild(i).position;
                    NewTile.transform.LookAt(transform, Vector3.up);
                    Generator.CurrentLength++;
                }
                else //spawn normal 
                {
                    GameObject NewTile = Instantiate(Generator.Tiles[Random.Range(0, Generator.Tiles.Count)], transform.parent);
                    NewTile.transform.position = transform.GetChild(0).GetChild(i).position;
                    NewTile.transform.LookAt(transform, Vector3.up);
                    Generator.CurrentLength++;
                }

            }else if (numberOfCollidersFound == 0) //no overlap and no more tiles to be spawned.
            {
                GameObject NewTile = Instantiate(Generator.Deadend, transform.parent);

                NewTile.transform.position = transform.GetChild(0).GetChild(i).position;
                NewTile.transform.LookAt(transform, Vector3.up);
                Generator.CurrentLength++;
            }

        }

        SpawnInterior();

    }

    void SpawnInterior()
    {
        if (Interiors.Count > 0)
        {
            GameObject Interior = Instantiate(Interiors[Random.Range(0, Interiors.Count)], transform);
            Interior.transform.position = transform.position;
        }
    }

}

