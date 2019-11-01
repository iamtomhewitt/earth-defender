using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSettings : MonoBehaviour 
{
    public float fireRate;
    public int level = 0;
    public int maxLevel = 5;
    public Material[] levelColours;
    public Renderer bodyDecal;
    public Renderer gunDecal;
    public Renderer platform;

    ShopManager shopManager;

    void Start()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();
    }

    public void ChangeFireRate()
    {
        if (fireRate <= 0.25f)
            fireRate = 0.25f;

        fireRate -= 0.25f;

        GetComponent<TrackingSystem>().CancelInvoke("FireAtFirstEnemy");
        GetComponent<TrackingSystem>().InvokeRepeating("FireAtFirstEnemy", 1f, fireRate);
    }

    public void ChangeRadius()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        col.radius += 5f;

        Vector3 newCentre = col.center;
        newCentre += new Vector3(0, 5f,0);
        col.center = newCentre;
    }
}
