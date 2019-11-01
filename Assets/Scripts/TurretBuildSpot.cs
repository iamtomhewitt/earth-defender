using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuildSpot : MonoBehaviour 
{
    public GameObject currentTurret;
    public GameObject turret;

    public Material selectionColour;
    public Material originalColour;

    [HideInInspector]
    public bool hasPlacedTurret = false;
    public bool isSelected = false;

    ShopManager shopManager;

    public int buildCost;
    public int upgradeCost;
    public int upgradeIncrement;

    void Start()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();
    }

    void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == this.GetComponent<Collider>())
                {
                    // If we click on a build spot, then turn it green to let us know that we have selected it
                    if (!isSelected)
                    {
                        if (currentTurret && currentTurret.GetComponent<TurretSettings>().level >= currentTurret.GetComponent<TurretSettings>().maxLevel)
                        {
                            MakeSelected(false, originalColour);
                        }
                        else
                        {
                            MakeSelected(true, selectionColour);
                            shopManager.ShowBuildSpotInfo();
                        }
                    }
                    // This is effectively the second press, return to our original colour
                    else
                    {
                        MakeSelected(false, originalColour);
                        shopManager.ShowBuildSpotInfo();
                    }
                }
            }
        }
    }

    public void Build()
    {
        if (!hasPlacedTurret)
        {
            GameObject.FindObjectOfType<AudioManager>().turretSpawn.Play();

            GameObject t = Instantiate(turret, transform) as GameObject;

            t.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            t.transform.localScale = new Vector3(1f, 1f, 1f);
            t.transform.localPosition = new Vector3(0f, 0f, -.9f);

            currentTurret = t;

            hasPlacedTurret = true;

            MakeSelected(false, originalColour);
        }
    }

    public void Upgrade()
    {
        GameObject.FindObjectOfType<AudioManager>().upgrade.Play();

        TurretSettings currentTurretSettings = currentTurret.GetComponent<TurretSettings>();

        // If we haven't maxed out the turret level
        if (currentTurretSettings.level < currentTurretSettings.maxLevel)
        {
            currentTurretSettings.level++;

            currentTurretSettings.bodyDecal.material = currentTurretSettings.levelColours[currentTurretSettings.level];
            currentTurretSettings.gunDecal.material  = currentTurretSettings.levelColours[currentTurretSettings.level];
            currentTurretSettings.platform.material  = currentTurretSettings.levelColours[currentTurretSettings.level];

            currentTurretSettings.ChangeRadius();
            currentTurretSettings.ChangeFireRate();

            upgradeCost += upgradeIncrement;

            MakeSelected(false, originalColour);
        }
        else
        {
            print("This turret is at its max level!");
        }
    }

    public void MakeSelected(bool isOn, Material colour)
    {
        isSelected = isOn;
        GetComponent<Renderer>().material = colour;
    }
}
