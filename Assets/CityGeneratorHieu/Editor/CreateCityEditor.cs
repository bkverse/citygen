using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateCityEditor : Editor
{
    public GameObject CityGenerator;
    [MenuItem("Tools/HieuCityGen",false,0)]
    public static void NewCity()
    {
        GameObject newCity = Instantiate(Resources.Load("HieuCityGen", typeof(GameObject))) as GameObject;
        newCity.name = "HieuCityGen";
    }
}
