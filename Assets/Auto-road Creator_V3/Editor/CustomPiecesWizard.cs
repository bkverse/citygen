using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomPiecesWizard : EditorWindow
{
    string ErrorString;
    bool Completed, errored;
    GUIStyle RedColored;
    GameObject straight, turn, threeway, fourway, bridge;

    private static void OpenWindow()
    {
        CustomPiecesWizard window = GetWindow<CustomPiecesWizard>();
        window.titleContent = new GUIContent("Custom Pieces Wizard");
        window.maxSize = new Vector2(250, 200);
        window.minSize = new Vector2(250, 200);

    }
    private void OnEnable()
    {
        errored = false;
        RedColored = new GUIStyle();
        RedColored.normal.textColor = Color.red;
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Straight", GUILayout.Width(80));
        straight = EditorGUILayout.ObjectField(straight, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Turn", GUILayout.Width(80));
        turn = EditorGUILayout.ObjectField(turn, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("threeway", GUILayout.Width(80));
        threeway = EditorGUILayout.ObjectField(threeway, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("fourway", GUILayout.Width(80));
        fourway = EditorGUILayout.ObjectField(fourway, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("bridge", GUILayout.Width(80));
        bridge = EditorGUILayout.ObjectField(bridge, typeof(GameObject), true, GUILayout.Width(150)) as GameObject;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.LabelField("", GUILayout.Height(10));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(20));
        if (GUILayout.Button("Create custom pieces", GUILayout.Width(200), GUILayout.Height(50)))
        {
            CreateCustomizedAssets();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (errored)
        {
            EditorGUILayout.LabelField("      Error:incomplete Pieces", RedColored, GUILayout.Width(100));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void CreateCustomizedAssets()
    {
        Completed = straight != null && turn != null && threeway != null && fourway != null && bridge != null;
        if (Completed)
        {
            errored = false;
            CreateCustomPrefab();
            EditorWindow.GetWindow<CustomPiecesWizard>().Close();
        }
        else
        {
            errored = true;
        }
    }
    private void CreateCustomPrefab()
    {

        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3"))
        {
            AssetDatabase.CreateFolder("Assets", "Auto-road Creator_V3");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/Resources"))
        {
            AssetDatabase.CreateFolder("Assets/Auto-road Creator_V3", "Resources");
        }
      

        if (Resources.Load("4_bridge1") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_bridge1.prefab");

        }
        if (Resources.Load("4_bridge2") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_bridge2.prefab");
        }
        if (Resources.Load("4_fourway") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_fourway.prefab");
        }
        if (Resources.Load("4_straight1") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_straight1.prefab");
        }
        if (Resources.Load("4_straight2") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_straight2.prefab");
        }
        if (Resources.Load("4_Threeway1") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_Threeway1.prefab");
        }
        if (Resources.Load("4_Threeway2") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_Threeway2.prefab");
        }
        if (Resources.Load("4_Threeway3") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_Threeway3.prefab");
        }
        if (Resources.Load("4_Threeway4") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_Threeway4.prefab");
        }
        if (Resources.Load("4_turn1") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_turn1.prefab");
        }
        if (Resources.Load("4_turn2") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_turn2.prefab");
        }
        if (Resources.Load("4_turn3") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_turn3.prefab");
        }
        if (Resources.Load("4_turn4") != null)
        {
            AssetDatabase.DeleteAsset("Assets/Auto-road Creator_V3/Resources/4_turn4.prefab");
        }


        if (Resources.Load("4_bridge1") == null)
        {
            GameObject bridge1 = bridge;
            bridge1.transform.eulerAngles = new Vector3(0, 90, 0);
            //AddRigidbodyAndMeshcollider(bridge1);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_bridge1.prefab", bridge1);
        }
        if (Resources.Load("4_bridge2") == null)
        {
            GameObject bridge2 = bridge;
            bridge2.transform.eulerAngles = new Vector3(0, 0, 0);
            //AddRigidbodyAndMeshcollider(bridge2);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_bridge2.prefab", bridge2);
        }
        if (Resources.Load("4_fourway") == null)
        {
            GameObject fourway1 = fourway;
            //AddRigidbodyAndMeshcollider(fourway1);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_fourway.prefab", fourway1);
        }

        if (Resources.Load("4_straight2") == null)
        {
            GameObject straight2 = straight;
            straight2.transform.eulerAngles = new Vector3(0, 90, 0);
            //AddRigidbodyAndMeshcollider(straight2);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_straight2.prefab", straight2);
        }
        if (Resources.Load("4_straight1") == null)
        {
            GameObject straight1 = straight;
            straight1.transform.eulerAngles = new Vector3(0, 0, 0);
            //AddRigidbodyAndMeshcollider(straight1);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_straight1.prefab", straight1);
        }

        if (Resources.Load("4_Threeway1") == null)
        {
            GameObject Threeway1 = threeway;
            Threeway1.transform.eulerAngles = new Vector3(0, 270, 0);
            //AddRigidbodyAndMeshcollider(Threeway1);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_Threeway1.prefab", Threeway1);
        }
        if (Resources.Load("4_Threeway2") == null)
        {
            GameObject Threeway2 = threeway;
            Threeway2.transform.eulerAngles = new Vector3(0, 180, 0);
            //AddRigidbodyAndMeshcollider(Threeway2);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_Threeway2.prefab", Threeway2);
        }
        if (Resources.Load("4_Threeway3") == null)
        {
            GameObject Threeway3 = threeway;
            Threeway3.transform.eulerAngles = new Vector3(0, 90, 0);
            //AddRigidbodyAndMeshcollider(Threeway3);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_Threeway3.prefab", Threeway3);
        }
        if (Resources.Load("4_Threeway4") == null)
        {
            GameObject Threeway4 = threeway;
            Threeway4.transform.eulerAngles = new Vector3(0, 0, 0);
            //AddRigidbodyAndMeshcollider(Threeway4);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_Threeway4.prefab", Threeway4);
        }

        if (Resources.Load("4_turn2") == null)
        {
            GameObject turn2 = turn;
            turn2.transform.eulerAngles = new Vector3(0, 90, 0);
            //AddRigidbodyAndMeshcollider(turn2);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_turn2.prefab", turn2);
        }
        if (Resources.Load("4_turn3") == null)
        {
            GameObject turn3 = turn;
            turn3.transform.eulerAngles = new Vector3(0, 180, 0);
            //AddRigidbodyAndMeshcollider(turn3);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_turn3.prefab", turn3);
        }
        if (Resources.Load("4_turn4") == null)
        {
            GameObject turn4 = turn;
            turn4.transform.eulerAngles = new Vector3(0, 270, 0);
            //AddRigidbodyAndMeshcollider(turn4);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_turn4.prefab", turn4);
        }
        if (Resources.Load("4_turn1") == null)
        {
            GameObject turn1 = turn;
            turn1.transform.eulerAngles = new Vector3(0, 0, 0);
            //AddRigidbodyAndMeshcollider(turn1);
            PrefabUtility.CreatePrefab("Assets/Auto-road Creator_V3/Resources/4_turn1.prefab", turn1);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    //private void AddRigidbodyAndMeshcollider(GameObject G)
    //{
    //    if (G.GetComponent<MeshCollider>() == null)
    //    {
    //        G.AddComponent<MeshCollider>();
    //    }
    //    if (G.GetComponent<Rigidbody>() == null)
    //    {
    //        Rigidbody r = G.AddComponent<Rigidbody>();
    //        r.isKinematic = true;
    //    }
    //}

}
