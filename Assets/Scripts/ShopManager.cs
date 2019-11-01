using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour 
{
    public int cash;
    public Text cashText;

    public GameObject[] purchasablePlanets;
    int lastPurchasedPlanetIndex = 0;
    public int planetCost;
    public int planetIncrement;

    TurretBuildSpot[] buildSpots;

    public GameObject nuke;
    public int nukeCost;

    public Text buildSpotInfo;

    void Start()
    {
        FindBuildSpots();
    }


    public void FindBuildSpots()
    {
        buildSpots = GameObject.FindObjectsOfType<TurretBuildSpot>();
    }

    // Function is called via a button in Unity Editor
    public void ShowToolTip(GameObject button)
    {
        foreach (Transform child in button.transform)
        {            
            if (child.tag == "Tooltip")
            {
                child.GetComponent<Text>().enabled = true;
            }
        }
    }

    // Function is called via a button in Unity Editor
    public void HideToolTip(GameObject button)
    {
        foreach (Transform child in button.transform)
        {            
            if (child.tag == "Tooltip")
            {
                child.GetComponent<Text>().enabled = false;
            }
        }
    }


    public void PurchasePlanet()
    {
        if (lastPurchasedPlanetIndex < purchasablePlanets.Length && planetCost <= cash) // Stops us gett a null reference exception.
        {
            // Set the planet active
            purchasablePlanets[lastPurchasedPlanetIndex].SetActive(true);

            FindBuildSpots();

            lastPurchasedPlanetIndex++;
           
            ChangeCashAmount(-planetCost);

            planetCost += planetIncrement;
        }

        ShowBuildSpotInfo();
    }


    public void BuildTurrets()
    {
        for (int i = 0; i < buildSpots.Length; i++)
        {
            // If we have selected a build spot, and we have cash, and we can afford it, AND there isn't a turret already there
            if (buildSpots[i].isSelected && cash > 0 && buildSpots[i].buildCost <= cash && buildSpots[i].currentTurret == null)
            {
                ChangeCashAmount(-buildSpots[i].buildCost);
                buildSpots[i].Build();
            }
        }
    }


    public void UpgradeTurrets()
    {
        int cost = 0;

        for (int i = 0; i < buildSpots.Length; i++)
        {
            if (buildSpots[i].isSelected)
            {
                cost += buildSpots[i].upgradeCost;
            }

            // If we have money, and level isnt maxed out
            if (buildSpots[i].isSelected && cash > 0 && cost <= cash && buildSpots[i].currentTurret.GetComponent<TurretSettings>().level < buildSpots[i].currentTurret.GetComponent<TurretSettings>().maxLevel)
            {                
                buildSpots[i].Upgrade();
            }
        }

        ChangeCashAmount(-cost);
    }


    public void SelectAllBuildSpots()
    {
        for (int i = 0; i < buildSpots.Length; i++)
        {
            TurretBuildSpot turretBuildSpot = buildSpots[i].GetComponent<TurretBuildSpot>();

            if (turretBuildSpot.currentTurret && turretBuildSpot.currentTurret.GetComponent<TurretSettings>().level >= turretBuildSpot.currentTurret.GetComponent<TurretSettings>().maxLevel)
            {
                print("1 or more turrets are at their max level, so their build spot has not been selected.");
                turretBuildSpot.MakeSelected(false, turretBuildSpot.originalColour);
            }
            else
            {
                turretBuildSpot.MakeSelected(true, turretBuildSpot.selectionColour);
                ShowBuildSpotInfo();
            }
        }
    }


    public void DeselectAllBuildSpots()
    {
        for (int i = 0; i < buildSpots.Length; i++)
        {            
            buildSpots[i].GetComponent<TurretBuildSpot>().MakeSelected(false, buildSpots[i].GetComponent<TurretBuildSpot>().originalColour);
        }

        ShowBuildSpotInfo();
    }


    public void FireNuke()
    {
        if (nukeCost <= cash)
        {
            Instantiate(nuke, Vector3.zero, Quaternion.identity);
        }
    }


    public void ChangeCashAmount(int amountToAdd)
    {
        cash += amountToAdd;
        cashText.text = "$" + cash.ToString();

        ShowBuildSpotInfo();
    }


    public void ShowBuildSpotInfo()
    {
        string buildCost = "";
        string upgradeCost = "";

        int finalBuildCost = 0;
        int finalUpgradeCost = 0;

        int numberSelected = 0;

        for (int i = 0; i < buildSpots.Length; i++)
        {
            if (buildSpots[i].isSelected)
            {
                finalBuildCost +=  buildSpots[i].buildCost;
                finalUpgradeCost += buildSpots[i].upgradeCost;
                numberSelected++;
            }
        }

        buildCost = finalBuildCost.ToString();
        upgradeCost = finalUpgradeCost.ToString();

        buildSpotInfo.text = numberSelected.ToString()+" Selected"+"\nBuild Cost: $" + buildCost + "\nUpgrade Cost: $" + upgradeCost + "\nPlanet Cost: $" + planetCost;
    }
}
