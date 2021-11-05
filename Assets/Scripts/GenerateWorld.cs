using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    [SerializeField]
    bool AutoSeed; //if true, will generate a random seed on Start

    [SerializeField]
    int MapSize;

    [SerializeField]
    int LevelSeed; 

    [SerializeField]
    List<GameObject> LargeStructures = new List<GameObject>();

    [SerializeField]
    List<GameObject> MediumStructures = new List<GameObject>();

    [SerializeField]
    List<GameObject> SmallStructures = new List<GameObject>();

    [SerializeField]
    float[] StartWeights;

    [SerializeField]
    float[] EndWeights;

    [SerializeField]
    int overlapTestBoxSize;

    public System.Random Seed;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.WorldGenerator = this;
        if (AutoSeed)
        {
            LevelSeed = Random.Range(0, int.MaxValue);
            Seed = new System.Random(LevelSeed);
        }
        else
        {
            Seed = new System.Random(LevelSeed);
        }

        SpawnStructures(60);
    }

    //picks the type(small, medium or large) the structure is going to be, based on distance from the player's spawn point(0, 0)
    List<GameObject> PickStructureType(Vector3 Location)
    {
        float CurrentWeight = 0;
        float[] Localweights = new float[StartWeights.Length]; 

        float RandomNbr = (float)Seed.NextDouble();

        for (int i = 0; i < StartWeights.Length; i++)
        {
            float LocalWeight = Mathf.Lerp(StartWeights[i], EndWeights[i], Vector3.Distance(Vector3.zero, Location) / Vector3.Distance(Vector3.zero, new Vector3(MapSize, 0, MapSize)));
            Localweights[i] = LocalWeight;

            if (CurrentWeight + LocalWeight >= (RandomNbr))
            {
                switch (i)
                {
                    case 0:
                        return SmallStructures;

                    case 1:
                        return MediumStructures;

                    case 2:
                        return LargeStructures;
                }
            }else
            {
                CurrentWeight += LocalWeight;
            }
        }

        Debug.LogError("Couldn't generate type");

        return null;
    }

    void SpawnStructures(int amount)
    {
        for (int i = 0; i < amount; i++)
        {

            Vector3 Pos = new Vector3(Seed.Next(-MapSize, MapSize), 0, Seed.Next(-MapSize, MapSize));
            Quaternion Rot = Quaternion.Euler(0, Seed.Next(0, 361), 0);

            List<GameObject> PickedList = new List<GameObject>();

            PickedList = PickStructureType(Pos);

            //detects overlap
            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(Pos, overlapTestBoxScale, collidersInsideOverlapBox, Rot);

            if (numberOfCollidersFound == 0)
            {
                GameObject Structure = Instantiate(PickedList[Seed.Next(0, PickedList.Count)]);
                Structure.transform.position = Pos;
                Structure.transform.rotation = Rot;
            }
            else
            {
                Debug.Log("collider found, skipping structure.");
            }

        }
    }

}
