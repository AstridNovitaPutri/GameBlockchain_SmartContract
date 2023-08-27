using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant3D")]
public class PlantObject : ScriptableObject
{
    public string plantName;
    public GameObject[] plantStages;
    public float growTime;
    public Sprite icon;
    public int lifeExpetancy;
}
