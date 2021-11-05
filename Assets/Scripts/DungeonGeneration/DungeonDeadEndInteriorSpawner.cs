using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDeadEndInteriorSpawner : MonoBehaviour
{

    public List<GameObject> Interiors; //possible interiors the rooms can contain

    DungeonGenerator Generator;
    DungeonData data;

    void Start()
    {
        Generator = GetComponentInParent<DungeonGenerator>();
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.tag == "Floor")
            {
                renderer.material = Generator.Data.FloorMat;
            }
            else
            {
                renderer.material = Generator.Data.WallMat;
            }
        }
        data = transform.root.GetComponent<DungeonData>();

        //if not enough exits, generate exit;
        if(data.GeneratedDoors.Count < data.exits.Count && (Generator.CurrentLength / Generator.MaxLength) > (data.GeneratedDoors.Count / data.exits.Count))
        {
            GameObject Interior = Instantiate(Generator.ExitInterior, transform);
            Interior.transform.position = transform.position;
            Interior.transform.rotation = transform.rotation;
        }
        else if (Interiors.Count > 0)
        {
            GameObject Interior = Instantiate(Interiors[Random.Range(0, Interiors.Count)], transform);
            Interior.transform.position = transform.position;
        }
    }
}
