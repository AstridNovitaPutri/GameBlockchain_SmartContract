using System.Collections;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    public GameObject plantItem;
    public bool isActive = false;
    public Transform listP;

    private void Awake()
    {
        var loadPlants = Resources.LoadAll("Plant", typeof(PlantObject));
        foreach (var plant in loadPlants)
        {
            PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            newPlant.plant = (PlantObject)plant;
        }
    }

    public void active()
    {
        Vector3 start = new Vector3(0, 0, 0);
        Vector3 target = new Vector3(-914, 0, 0);
        if (isActive)
        {
            StartCoroutine(Move(start, target));
        }
        else
        {
            StartCoroutine(Move(target, start));
        }
        isActive = !isActive;
    }

    IEnumerator Move(Vector3 start, Vector3 target)
    {
        float time = 0.0f;

        while (time < 0.5)
        {
            time += Time.deltaTime;
            listP.localPosition = Vector3.Lerp(start, target, time / 0.5f);
            yield return null;
        }
    }
}
