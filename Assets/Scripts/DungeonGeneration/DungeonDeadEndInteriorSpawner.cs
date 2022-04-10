using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDeadEndInteriorSpawner : MonoBehaviour
{

    public List<GameObject> Interiors; //possible interiors the rooms can contain

    DungeonGenerator Generator;
    DungeonData data;

    void Awake()
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
    }

    public void SpawnInterior(GameObject custom = null)
    {
        if (Interiors.Count > 0 || custom != null)
        {
            if (custom == null)
            {
                custom = Interiors[Random.Range(0, Interiors.Count)];
            }

            GameObject Interior = Instantiate(custom, transform);
            Interior.transform.position = transform.position;
            Interior.transform.rotation = transform.rotation;
        }
    }
}
