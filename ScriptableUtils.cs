using System;
using System.Linq;
using MoralisUnity;
using MoralisUnity.Platform.Queries;
using UnityEngine;

public class ScriptableUtils {
  
  public static PlantObject GetPlant(string _plantName){
    var plantsObject = Resources.LoadAll("Plant", typeof(PlantObject)).Cast<PlantObject>().ToArray();
    return Array.Find<PlantObject>(plantsObject, obj => obj.plantName == _plantName) ;
  }
}