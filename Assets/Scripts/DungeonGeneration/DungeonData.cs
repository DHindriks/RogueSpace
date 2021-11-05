using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : MonoBehaviour
{

    public List<GameObject> exits;

    public List<GameObject> GeneratedDoors;

    public List<MeshRenderer> OuterColors;

    [HideInInspector]
    public Material WallMat;
    [HideInInspector]
    public Material FloorMat;

    void Start()
    {
        foreach(MeshRenderer renderer in OuterColors)
        {
            renderer.material = FloorMat;
        }
    }
}
