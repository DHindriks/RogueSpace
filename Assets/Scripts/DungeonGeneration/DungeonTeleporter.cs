using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleporter : MonoBehaviour
{

    DungeonData Data;

    // Start is called before the first frame update
    void Start()
    {
        Data = transform.root.GetComponent<DungeonData>();
        Data.GeneratedDoors.Add(gameObject);
        if (Data.GeneratedDoors.Count <= Data.exits.Count)
        {
            GameObject PickedTp = Data.exits[Data.GeneratedDoors.Count - 1];
            PickedTp.GetComponent<Teleporter>().TpTo = transform.GetChild(0).gameObject;
            GetComponent<Teleporter>().TpTo = PickedTp.transform.GetChild(0).gameObject;
        } 
    }
}
