using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor.SceneManagement;
using System.IO;

public class MyMenu : EditorWindow
{
    // GenRoad genRoadScript;
    // GenHouse genHouseScript;
    // ResetAll resetAllScript;
    GameObject GenRoadObject;
    GameObject GenHouseObject;
    GameObject ResetAllObject;
    [MenuItem("AutoGen/Generator")]
    private static void Generator()
    {
        MyMenu window = GetWindow<MyMenu>();
        window.titleContent = new GUIContent("City Generator");
    }

    void OnGUI()
    {
        // if (GUILayout.Button("Generate Road"))
        // {
        //     GenRoadObject = GameObject.FindGameObjectWithTag("GenRoad");
        //     genRoadScript = GenRoadObject.GetComponent<GenRoad>();
        //     genRoadScript.GenerateRoad();
        // }

        // if (GUILayout.Button("Generate House"))
        // {
        //     GenHouseObject = GameObject.Find("GenHouse");
        //     genHouseScript = GenHouseObject.GetComponent<GenHouse>();
        //     genHouseScript.GenerateHouse();
        // }

        // if (GUILayout.Button("Reset"))
        // {
        //     ResetAllObject = GameObject.Find("ResetAll");
        //     // resetAllScript = ResetAllObject.GetComponent<ResetAll>();
        //     // resetAllScript.ResetAll();
        // }
    }
}