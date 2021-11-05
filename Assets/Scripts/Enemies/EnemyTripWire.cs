using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTripWire : MonoBehaviour
{
    [SerializeField]
    LaserScript Laser;

    [SerializeField]
    ParticleSystem LaserParticles;

    public void StopParticles()
    {
        LaserParticles.loop = false;
    }


    void OnDestroy()
    {
        if (Laser != null)
        {
            LaserParticles.loop = false;
            Laser.DisableLaser();
        }    
    }
}
