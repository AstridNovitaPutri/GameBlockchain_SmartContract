using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public List<PlotManager> plotChild = new List<PlotManager>();
    public PlantItem selectPlant;
    public bool isPlanting = false;
    public int plantCount = 0;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            plotChild.Add(child.gameObject.GetComponent<PlotManager>());
        }
    }

    public void SelectPlant(PlantItem newPlant)
    {
        if (selectPlant == newPlant)
        {
            Debug.Log("Deselecting " + selectPlant.plant.plantName);
            selectPlant = null;
            isPlanting = false;
        }
        else
        {
            selectPlant = newPlant;
            Debug.Log("Selecting " + selectPlant.plant.plantName);
            isPlanting = true;
        }
    }

    public void PlantAll()
    {
        if (selectPlant != null)
            for (int i = 0; i < plotChild.Count; i++)
            {
                if (!(plotChild[i].isPlanted))
                    plotChild[i].Plant(selectPlant.plant);
            }
    }

    public void PlantCount()
    {
        if (selectPlant != null)
            for (int i = 0; i < plantCount; i++)
            {
                if (!(plotChild[i].isPlanted))
                    plotChild[i].Plant(selectPlant.plant);
            }
    }

    public void HarvestAll()
    {
        bool allRipe = true;
        foreach (PlotManager pm in plotChild)
        {
            if (!(pm.isPlantRipe()))
                allRipe = false;
        }

        if (allRipe)
            transform.BroadcastMessage("Harvest");
    }
}
