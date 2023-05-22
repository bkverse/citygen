using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor 
{

    public int __GridX;
    public int __GridZ;
    private BuildingDataType building;
    private BuildingDataType nature;
    public void OnEnable()
    {
        __GridX = PlayerPrefs.GetInt("__GridX");
        __GridZ = PlayerPrefs.GetInt("__GridZ");
    }   

    public override void OnInspectorGUI() 
    {
        CityGenerator myScript = (CityGenerator)target;
        GUI.backgroundColor = Color.white;
        EditorStyles.label.wordWrap = true;
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        // #map Grid Sizes
        GUILayout.BeginVertical("City Grids", "window");
        EditorGUILayout.LabelField("Select the base size of your city" + "\n" + "Information about your city can be found below in the city description");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid X: ");
        __GridX = (int)EditorGUILayout.Slider(__GridX, 1, 9);
        myScript.gridX = __GridX;

        if (__GridX % 2 == 0)
        {
            __GridX = __GridX-1;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Z: ");
        __GridZ = (int)EditorGUILayout.Slider(__GridZ, 1, 9);
        myScript.gridZ = __GridZ;
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        PlayerPrefs.SetInt("__GridX", __GridX);
        PlayerPrefs.SetInt("__GridZ", __GridZ);

        GUILayout.EndVertical();
        GUILayout.Space(20);
        // #end map
        
        // #map RoadPrefabs
        GUILayout.BeginVertical("Roads", "window");

        EditorGUILayout.LabelField("Put your roads into here");
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("roadStraight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("roadCorner"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("road3way"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("road4way"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("trafficLight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lightRoad"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("treeRoad"));
        serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();
        GUILayout.Space(20);

        //  #map buldings
        GUILayout.BeginVertical("Buildings", "window");
        EditorGUILayout.LabelField("Put your buildings into here");
        serializedObject.Update();
        if (myScript.buildingPrefabs.Count >= 1)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("buildingPrefabs"), true);
        }
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("+", GUILayout.Height(25)))
        {
            if (myScript.buildingPrefabs.Count < 10)
            {
                myScript.buildingPrefabs.Add(building);
            }

        }
        GUILayout.Space(20);
        if (GUILayout.Button("-", GUILayout.Height(25)))
        {
            if (myScript.buildingPrefabs.Count > 0 )
            {
                myScript.buildingPrefabs.RemoveAt(myScript.buildingPrefabs.Count -1);
            }
        }
        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();

        GUILayout.EndVertical();
        
        //  #map natures
        GUILayout.BeginVertical("Natures", "window");
        EditorGUILayout.LabelField("Put your natures into here");
        serializedObject.Update();
        if (myScript.naturePrefabs.Count >= 1)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("naturePrefabs"), true);
        }
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("+", GUILayout.Height(25)))
        {
            if (myScript.naturePrefabs.Count < 10)
            {
                myScript.naturePrefabs.Add(nature);
            }

        }
        GUILayout.Space(20);
        if (GUILayout.Button("-", GUILayout.Height(25)))
        {
            if (myScript.naturePrefabs.Count > 0 )
            {
                myScript.naturePrefabs.RemoveAt(myScript.naturePrefabs.Count -1);
            }
        }
        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
        GUILayout.EndVertical();

        //  #Buttons
        EditorGUILayout.LabelField("Generate");
        if (GUILayout.Button("Generate Your City"))
        {
            // generate your city
            myScript.clearAll();
            myScript.generateCity();
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Remove");
        if (GUILayout.Button("Clear"))
        {
            myScript.clearAll();
        }
    }
}
