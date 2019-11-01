using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretFiringSystem : MonoBehaviour 
{
    public GameObject missile;
    public Transform missileSpawn;

    public void Shoot(GameObject target)
    {
        if (target)
        {
            GameObject m = Instantiate(missile, missileSpawn.transform.position, missileSpawn.transform.rotation) as GameObject;
            m.GetComponent<HomingMissile>().target = target.transform;
        }
        else
        {
           return;
        }
    }
}
