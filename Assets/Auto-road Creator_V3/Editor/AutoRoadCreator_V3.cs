#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor.SceneManagement;
using System.IO;

public class AutoRoadCreator_V3 : EditorWindow
{
    bool tempLeft, tempRight, tempUp, tempDown, InitialDraggingBool;

    [HideInInspector]
    public int columns, rows;
    Vector2 CurrentGridBox;

    [SerializeField]
    private List<List<GridBox>> GridBoxes;

    [SerializeField]
    private MapData MD;

    [HideInInspector]
    public int Lanes;
    private Vector2 offset;
    private Vector2 drag;

    private float menuBarHeight = 20f;
    private Rect menuBar;
    TextureManager TM;

    GameObject TheMap;
    bool up, dowm, left, right;
    // bool MapUpdated;

    [HideInInspector]
    public bool DeformedMap;
    Terrain ter;
    MapScriptV3 mapscript;
    // bool Addterrain;


    [MenuItem("Window/Auto Road Creator")]
    private static void OpenWindow()
    {
        AutoRoadCreator_V3 window = GetWindow<AutoRoadCreator_V3>();
        window.titleContent = new GUIContent("Auto Road Creator");
    }

    private void OnEnable()
    {
        addTag();
        columns = 20;
        rows = 20;
        TheMap = GameObject.FindGameObjectWithTag("GridMap");
        try
        {
            mapscript = TheMap.GetComponent<MapScriptV3>();
            loadMapData("mapdata" + mapscript.name);
            //ter = mapscript.GetTerrain();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        Lanes = 2;
        if (TheMap == null)
        {
            TheMap = new GameObject("MAP");
            TheMap.tag = "GridMap";
            TheMap.name = "Map#" + TheMap.GetInstanceID();
            TheMap.AddComponent<MapScriptV3>();
            mapscript = TheMap.GetComponent<MapScriptV3>();
            //Addterrain = (mapscript.GetTerrain() != null);
            //Addterrain = false;
            //ter = null;
            //mapscript.SetTerrain(null);
        }
        else
        {
            mapscript.ResetTheMap(columns, rows);
        }
        Selection.activeObject = TheMap;
        //Addterrain = (mapscript.GetTerrain() != null);

        //if (Addterrain)
        //{
        //    ter = mapscript.GetTerrain();
        //}


        TM = new TextureManager();
      create2DArrayofGridBoxes(columns, rows);
        try
        {
            Load("GridBoxes" + mapscript.name, "mapdata" + mapscript.name);
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }
    }


    private void OnSelectionChange()
    {
        if (Selection.activeObject != null)
        {
            GameObject G = Selection.activeObject as GameObject;
            if (G != null)
            {
                if (G.GetComponent<MapScriptV3>() != null)
                {
                    TheMap = G;
                    try
                    {
                        mapscript = G.GetComponent<MapScriptV3>();
                        //ter = mapscript.GetTerrain();
                        loadMapData("mapdata" + mapscript.name);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                    mapscript.ResetTheMap(columns, rows);
                    //Addterrain = (mapscript.GetTerrain() != null);
                    //if (Addterrain)
                    //{
                    //    ter = mapscript.GetTerrain();
                    //}
                    AssetDatabase.SaveAssets();
                    Focus();
                    Load("GridBoxes" + mapscript.name, "mapdata" + mapscript.name);
                }
            }
        }
    }


    private void OnGUI()
    {
        DrawGrid(30, 0.2f, Color.gray);
        DrawGrid(150, 0.6f, Color.gray);
        DrawGridBoxes();
        DrawMenuBar();
        ProcessGridBoxesEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();
        if (Lanes != 3)
        {
            EditorGUILayout.LabelField(("lanes:" + (Lanes + 1)), GUILayout.Width(50));
        }
        else
        {
            EditorGUILayout.LabelField(("Customized Pieces"), GUILayout.Width(120));
        }
        //if (Addterrain)
        //{
        //    if (GUILayout.Button(new GUIContent("remove terrain"), EditorStyles.toolbarButton, GUILayout.Width(100)))
        //    {
        //        Addterrain = false;
        //        ter = null;
        //        mapscript.SetTerrain(null);
        //        RevertFittingToTerrain();
        //    }
        //    EditorGUILayout.BeginHorizontal();
        //    EditorGUILayout.LabelField("", GUILayout.Width(10));
        //    EditorGUILayout.LabelField("terrain ", GUILayout.Width(50));
        //    try
        //    {
        //        ter = mapscript.GetTerrain();
        //    }
        //    catch (Exception e) 
        //    { 
        //        Debug.Log(e);
        //    }
        //    ter = EditorGUILayout.ObjectField(ter, typeof(Terrain), true, GUILayout.Width(150)) as Terrain;

        //    if (ter != null)
        //    {
        //        if (mapscript.GetTerrain() == null)
        //        {
        //            mapscript.SetTerrain(ter);
        //        }
        //        if (mapscript.GetTerrain() != ter)
        //        {
        //            mapscript.SetTerrain(ter);
        //        }
        //    }
        //    if (ter == null)
        //    {
        //        if (mapscript.GetTerrain() != null)
        //        {
        //            RevertFittingToTerrain();
        //        }
        //    }
        //    if (GUILayout.Button(new GUIContent("update to terrain"), EditorStyles.toolbarButton, GUILayout.Width(120)))
        //    {
        //        UpdateAllPartsToTerrain();
        //    }
        //    EditorGUILayout.EndHorizontal();
        //}
        //else
        //{
        //    if (GUILayout.Button(new GUIContent("add terrain"), EditorStyles.toolbarButton, GUILayout.Width(80)))
        //    {
        //        Addterrain = true;
        //    }


        //}


        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawGridBoxes()
    {
        if (GridBoxes != null)
        {
            for (int C = 0; C < GridBoxes.Count; C++)
            {
                for (int R = 0; R < GridBoxes[C].Count; R++)
                {
                    GridBoxes[C][R].Draw();
                }
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);

                }
                break;
            case EventType.MouseUp:
                Save("GridBoxes" + mapscript.name, "mapdata" + mapscript.name);
                CurrentGridBox.Set(-100, -100);
                break;
            case EventType.MouseDrag:
                if (e.button == 2)
                {
                    OnDrag(e.delta);
                }
                break;
            case EventType.ScrollWheel:
                if (e.button == 2)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ExpandTo(Vector2 mousePosition)
    {
        bool leftORup = false;

        if (mousePosition.y < GridBoxes[0][0].rect.y)
        {
            int iteration = Mathf.CeilToInt((GridBoxes[0][0].rect.y - mousePosition.y) / 30);

            for (int C = 0; C < iteration; C++)
            {
                rows++;
                for (int R = 0; R < columns; R++)
                {
                    GridBoxes[R].Insert(0, new GridBox(GridBoxes[R][0].rect.position - new Vector2(0, 30), TM.GetTextureByID(0), 0, C, R));
                }
                mapscript.AddRowAtFirst(columns);
                TheMap.transform.Translate(0, 0, 50, Space.World);
            }
            leftORup = true;
        }
        if (mousePosition.y > GridBoxes[columns - 1][rows - 1].rect.y + 30)
        {
            int iteration = Mathf.CeilToInt((mousePosition.y - GridBoxes[columns - 1][rows - 1].rect.y - 30) / 30);

            for (int C = 0; C < iteration; C++)
            {
                rows++;
                for (int R = 0; R < columns; R++)
                {
                    GridBoxes[R].Add(new GridBox(GridBoxes[R][rows - 2].rect.position + new Vector2(0, 30), TM.GetTextureByID(0), 0, C, R));

                }
            }
        }

        if (mousePosition.x < GridBoxes[0][0].rect.x)
        {
            int iteration = Mathf.CeilToInt((GridBoxes[0][0].rect.x - mousePosition.x) / 30);

            for (int C = 0; C < iteration; C++)
            {
                columns++;
                GridBoxes.Insert(0, new List<GridBox>());
                for (int R = 0; R < rows; R++)
                {
                    GridBoxes[0].Add(new GridBox(GridBoxes[1][R].rect.position - new Vector2(30, 0), TM.GetTextureByID(0), 0, C, R));
                }
                mapscript.AddAColumnAtFirst(rows);
                TheMap.transform.Translate(-50, 0, 0, Space.World);
            }
            leftORup = true;
        }
        if (mousePosition.x > GridBoxes[columns - 1][rows - 1].rect.x + 30)
        {
            int iteration = Mathf.CeilToInt((mousePosition.x - GridBoxes[columns - 1][rows - 1].rect.x - 30) / 30);

            for (int C = 0; C < iteration; C++)
            {
                columns++;
                GridBoxes.Add(new List<GridBox>());
                for (int R = 0; R < rows; R++)
                {
                    GridBoxes[GridBoxes.Count - 1].Add(new GridBox(GridBoxes[GridBoxes.Count - 2][R].rect.position + new Vector2(30, 0), TM.GetTextureByID(0), 0, C, R));
                }
            }
        }
        if (leftORup && ter != null)
        {
            UpdateAllPartsToTerrain();
        }

        mapscript.updateColumns(columns, rows);
        Save("GridBoxes" + mapscript.name, "mapdata" + mapscript.name);
    }


    private bool IsInsideDrawingArea(Vector2 mousePosition)
    {
        if (mousePosition.y < GridBoxes[0][0].rect.y)
        {
            return false;
        }
        if (mousePosition.y > GridBoxes[columns - 1][rows - 1].rect.y + 30)
        {
            return false;
        }
        if (mousePosition.x < GridBoxes[0][0].rect.x)
        {
            return false;
        }
        if (mousePosition.x > GridBoxes[columns - 1][rows - 1].rect.x + 30)
        {
            return false;
        }
        return true; ;

    }

    private void ProcessGridBoxesEvents(Event e)
    {
        if (GridBoxes != null)
        {
            for (int C = 0; C < GridBoxes.Count; C++)
            {
                for (int R = 0; R < GridBoxes[C].Count; R++)
                {
                    bool guiChanged = GridBoxes[C][R].ProcessEvent(e);
                    if (guiChanged)
                    {
                        bool specialCase = mapscript.getpartname(C, R) == "4_fourway" || mapscript.getpartname(C, R) == "3_fourway" || mapscript.getpartname(C, R) == "2_fourway" || mapscript.getpartname(C, R) == "1_fourway" || mapscript.getpartname(C, R) == "1_bridge1" || mapscript.getpartname(C, R) == "2_bridge1" || mapscript.getpartname(C, R) == "3_bridge1" || mapscript.getpartname(C, R) == "4_bridge1";
                        if (CurrentGridBox.x != C || CurrentGridBox.y != R)
                        {
                            CurrentGridBox.Set(C, R);
                            if (GridBoxes[C][R].HelperInt == 1)
                            {
                                GridBoxes[C][R].HelperInt = 0;
                                InitialDraggingBool = !GridBoxes[C][R].IsOn;
                            }
                            GUI.changed = true;
                            int id = 0;
                            bool isSpecialBuidling = false;
                            if (!specialCase)
                            {

                                if (Lanes != 4)
                                {
                                    isSpecialBuidling = false;
                                }
                                else
                                {
                                    isSpecialBuidling = true;
                                }
                                GridBoxes[C][R].IsOn = InitialDraggingBool;
                                id = GetID(C, R, true, false);
                                GridBoxes[C][R].UpdateTexture(TM.GetTextureByID(id), id);
                                try
                                {
                                    id = GetID(C + 1, R, false , isSpecialBuidling);
                                    GridBoxes[(C + 1)][R].UpdateTexture(TM.GetTextureByID(id), id);
                                }
                                catch (System.Exception exe) { };

                                try
                                {
                                    id = GetID(C - 1, R, false, isSpecialBuidling);
                                    GridBoxes[(C - 1)][R].UpdateTexture(TM.GetTextureByID(id), id);
                                }
                                catch (Exception exe) { };

                                try
                                {
                                    id = GetID(C, R + 1, false, isSpecialBuidling);
                                    GridBoxes[C][R + 1].UpdateTexture(TM.GetTextureByID(id), id);

                                }
                                catch (Exception exe) { };

                                try
                                {
                                    id = GetID(C, R - 1, false, isSpecialBuidling);
                                    GridBoxes[C][R - 1].UpdateTexture(TM.GetTextureByID(id), id);

                                }
                                catch (Exception exe) { };
                                //}
                                //else
                                //{
                                //    GridBoxes[C][R].IsOn = InitialDraggingBool;
                                //    id = GetID(C, R, true);
                                //    GridBoxes[C][R].UpdateTexture(TM.GetTextureByID(id), id);
                                //}
                            }

                            if (InitialDraggingBool)
                            {
                                mapscript.setpartexist(C, R, true);
                                BuildPart(C, R, true);

                                if (C > 0 && mapscript.Getpartexist(C - 1, R))
                                {
                                    BuildPart(C - 1, R, false);
                                }
                                if (C < columns && mapscript.Getpartexist(C + 1, R))
                                {
                                    BuildPart(C + 1, R, false);
                                }
                                if (R > 0 && mapscript.Getpartexist(C, R - 1))
                                {
                                    BuildPart(C, R - 1, false);
                                }
                                if (R < rows && mapscript.Getpartexist(C, R + 1))
                                {
                                    BuildPart(C, R + 1, false);
                                }
                            }
                            else
                            {
                                if (specialCase)
                                {
                                    BuildPart(C, R, true);
                                    BuildGridBoxBridgeScript(C, R);
                                }
                                else
                                {
                                    mapscript.setpartexist(C, R, false);
                                    mapscript.Setpartname(C, R, "");
                                    mapscript.Destroypart(C, R);
                                }
                                if (C > 0 && mapscript.Getpartexist(C - 1, R))
                                {
                                    BuildPart(C - 1, R, false);
                                }
                                if (C < columns && mapscript.Getpartexist(C + 1, R))
                                {
                                    BuildPart(C + 1, R, false);
                                }
                                if (R > 0 && mapscript.Getpartexist(C, R - 1))
                                {
                                    BuildPart(C, R - 1, false);
                                }
                                if (R < rows && mapscript.Getpartexist(C, R + 1))
                                {
                                    BuildPart(C, R + 1, false);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildGridBoxBridgeScript(int c, int r)
    {
        int id = 0;
        string PARTNAME;
        PARTNAME = mapscript.getpartname(c, r);

        if (PARTNAME.StartsWith("4"))
        {
            id = 400;
        }
        if (PARTNAME.StartsWith("3"))
        {
            id = 300;
        }
        if (PARTNAME.StartsWith("2"))
        {
            id = 200;
        }
        if (PARTNAME.StartsWith("1"))
        {
            id = 100;
        }
        if (PARTNAME.Contains("fourway"))
        {
            id += 11;
        }
        if (PARTNAME.Contains("bridge1"))
        {
            id += 12;
        }
        if (PARTNAME.Contains("bridge2"))
        {
            id += 13;
        }

        GridBoxes[c][r].IsOn = true;
        GridBoxes[c][r].textID = id;
        GridBoxes[c][r].UpdateTexture(TM.GetTextureByID(id), id);
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        if (!IsInsideDrawingArea(mousePosition))
        {
            genericMenu.AddItem(new GUIContent("Expand to here"), false, () => ExpandTo(mousePosition));
            genericMenu.AddSeparator("");
        }
        genericMenu.AddItem(new GUIContent("One Lane"), false, () => SetLanes(0));
        genericMenu.AddItem(new GUIContent("Two Lanes"), false, () => SetLanes(1));
        genericMenu.AddItem(new GUIContent("Three Lanes"), false, () => SetLanes(2));
        genericMenu.AddItem(new GUIContent("Special Buildings Places"), false, () => Building());
        genericMenu.AddItem(new GUIContent("Customized Pieces"), false, () => CustomPieces());
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Custom Pieces Wizard"), false, () => CustomPiecesWizard());
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("create new map"), false, () => CreateNewMap());
        genericMenu.AddItem(new GUIContent("reset"), false, () => resetMap());
        genericMenu.ShowAsContext();
    }
    private void CustomPieces()
    {
        SetLanes(3);
        if (!checkCustomPiecesAvailable())
        {
            EditorWindow wizard = GetWindow(typeof(CustomPiecesWizard), false, "", false);
        }
    }
    private void CustomPiecesWizard()
    {
        EditorWindow wizard = GetWindow(typeof(CustomPiecesWizard), false, "", false);

    }

    private bool checkCustomPiecesAvailable()
    {
        bool has1_bridge1 = Resources.Load("4_bridge1") != null;
        bool has1_bridge2 = Resources.Load("4_bridge2") != null;
        bool has1_fourway = Resources.Load("4_fourway") != null;
        bool has1_straight1 = Resources.Load("4_straight1") != null;
        bool has1_straight2 = Resources.Load("4_straight2") != null;
        bool has1_Threeway1 = Resources.Load("4_Threeway1") != null;
        bool has1_Threeway2 = Resources.Load("4_Threeway2") != null;
        bool has1_Threeway3 = Resources.Load("4_Threeway3") != null;
        bool has1_Threeway4 = Resources.Load("4_Threeway4") != null;
        bool has1_turn1 = Resources.Load("4_turn1") != null;
        bool has1_turn2 = Resources.Load("4_turn2") != null;
        bool has1_turn3 = Resources.Load("4_turn3") != null;
        bool has1_turn4 = Resources.Load("4_turn4") != null;
        if (has1_bridge1 && has1_bridge2 && has1_fourway && has1_straight1 && has1_straight2 && has1_Threeway1 && has1_Threeway2 && has1_Threeway3 && has1_Threeway4 && has1_turn1 && has1_turn2 && has1_turn3 && has1_turn4)
        { return true; }
        else
        { return false; }
    }

    private void Building()
    {
        SetLanes(4);
    }

    private void CreateNewMap()
    {
        TheMap = new GameObject("MAP");
        TheMap.tag = "GridMap";
        TheMap.name = "Map#" + TheMap.GetInstanceID();
        TheMap.AddComponent<MapScriptV3>();
        mapscript = TheMap.GetComponent<MapScriptV3>();
        // Addterrain = false;
        ter = null;
        mapscript.SetTerrain(null);
        resetMap();
        Selection.activeObject = TheMap;
    }

    private void SetLanes(int lanesssss)
    {
        Lanes = lanesssss;
    }

    private void resetMap()
    {
        RevertFittingToTerrain();
        columns = 10;
        rows = 10;

        GridBoxes = new List<List<GridBox>>();
        for (int C = 0; C < columns; C++)
        {
            GridBoxes.Add(new List<GridBox>());
            for (int R = 0; R < rows; R++)
            {
                GridBoxes[C].Add(new GridBox(new Vector2(C * 30, R * 30), TM.GetTextureByID(0), 0, C, R));
            }
        }
        mapscript.updateColumns(columns, rows);
        Save("GridBoxes" + mapscript.name, "mapdata" + mapscript.name);
        for (int i = 0; i < TheMap.transform.childCount; i++)
        {

            DestroyImmediate(TheMap.transform.GetChild(i).gameObject);
        }
        while (TheMap.transform.childCount > 0)
        {
            DestroyImmediate(TheMap.transform.GetChild(0).gameObject);
        }
        mapscript.ResetTheMap(columns, rows);
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (GridBoxes != null)
        {
            for (int C = 0; C < GridBoxes.Count; C++)
            {
                for (int R = 0; R < GridBoxes[C].Count; R++)
                {
                    GridBoxes[C][R].Drag(delta);
                }
            }
        }
        GUI.changed = true;
    }



    private void Save(String GridBoxesFileName, string MapDataFileName)
    {

        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3"))
        {
            AssetDatabase.CreateFolder("Assets", "Auto-road Creator_V3");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/Resources"))
        {
            AssetDatabase.CreateFolder("Assets/Auto-road Creator_V3", "Resources");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/Resources/MapsFiles"))
        {
            AssetDatabase.CreateFolder("Assets/Auto-road Creator_V3/Resources", "MapsFiles");
        }

        EditorUtility.SetDirty(TheMap);
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        MD.columns = columns;
        MD.rows = rows;
        MD.LastLaneUsed = Lanes;
        XMLOp.Serialize(MD, "Assets/Auto-road Creator_V3/Resources/MapsFiles/" + MapDataFileName + ".xml");
        List<GridBox> SimpleGridBoxesArray = new List<GridBox>();
        for (int C = 0; C < GridBoxes.Count; C++)
        {
            for (int R = 0; R < GridBoxes[C].Count; R++)
            {
                SimpleGridBoxesArray.Add(GridBoxes[C][R]);
            }
        }
        XMLOp.Serialize(SimpleGridBoxesArray, ("Assets/Auto-road Creator_V3/Resources/MapsFiles/" + GridBoxesFileName + ".xml"));
    }

    private void Load(string GridBoxesFileName, string MapDataFileName)
    {
        var mapdata = new MapData();
        try
        {
            mapdata = XMLOp.Deserialize<MapData>("Assets/Auto-road Creator_V3/Resources/MapsFiles/" + MapDataFileName + ".xml");
        }
        catch (Exception e) { return; }

        columns = mapdata.columns;
        rows = mapdata.rows;
        Lanes = mapdata.LastLaneUsed;
        var GridBoxesDeserialized = XMLOp.Deserialize<List<GridBox>>("Assets/Auto-road Creator_V3/Resources/MapsFiles/" + GridBoxesFileName + ".xml");
        GridBoxes = new List<List<GridBox>>();
        int GridBoxhelper = 0;
        int GridBoxescount = columns * rows;
        for (int C = 0; C < mapdata.columns; C++)
        {
            GridBoxes.Add(new List<GridBox>());
            for (int R = 0; R < mapdata.rows; R++)
            {
                GridBoxes[C].Add(new GridBox(new Vector2(C * 30, R * 30), TM.GetTextureByID(GridBoxesDeserialized[GridBoxhelper].textID), GridBoxesDeserialized[GridBoxhelper].textID, GridBoxesDeserialized[GridBoxhelper].co, GridBoxesDeserialized[GridBoxhelper].ro));
                GridBoxhelper++;
                EditorUtility.DisplayProgressBar("loading map", "", (float)(((C) * rows + R + 1f) / GridBoxescount));
            }
        }
        EditorUtility.DisplayProgressBar("loading map", "", 1f);
        EditorUtility.ClearProgressBar();

        mapscript.updateColumns(columns, rows);
    }

    void loadMapData(String MapDataFileName)
    {
        var mapdata = XMLOp.Deserialize<MapData>("Assets/Auto-road Creator_V3/Resources/MapsFiles/" + MapDataFileName + ".xml");
        columns = mapdata.columns;
        rows = mapdata.rows;
        Lanes = mapdata.LastLaneUsed;
    }

    private void create2DArrayofGridBoxes(int Col, int Ro)
    {
        if (GridBoxes == null)
        {
            GridBoxes = new List<List<GridBox>>();
            MD = new MapData();
            for (int C = 0; C < Col; C++)
            {
                GridBoxes.Add(new List<GridBox>());
                for (int R = 0; R < Ro; R++)
                {
                    GridBoxes[C].Add(new GridBox(new Vector2(C * 30, R * 30), TM.GetTextureByID(0), 0, C, R));
                }
            }
            mapscript.updateColumns(Col, Ro);
        }
    }

    void addTag()
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == "GridMap")
                {
                    return;
                }
            }

            tags.InsertArrayElementAtIndex(0);
            tags.GetArrayElementAtIndex(0).stringValue = "GridMap";
            so.ApplyModifiedProperties();
            so.Update();
        }
    }
    void BuildPart(int i, int J, bool primarypart)
    {
        mapscript.Setpartname(i, J, ReturnPartName(i, J, primarypart));
        Debug.Log(mapscript.getpartname(i, J));
        if (mapscript.getpart(i, J) != null)
        {
            Debug.Log(mapscript.getpart(i, J).name);
            if (mapscript.getpartname(i, J) != mapscript.getpart(i, J).name)
            {
                mapscript.getpartname(i, J);
                mapscript.Destroypart(i, J);
                mapscript.createPart(i, J, null);
            }
        }
        else
        {
            mapscript.createPart(i, J, null);
        }
        if (ter != null)
        {
            FitToTerrain(i, J, mapscript.getpart(i, J), ter);
            if (!DeformedMap)
            {
                DeformedMap = true;
            }
        }
        // MapUpdated = true;
        bool specialCase = mapscript.getpartname(i, J) == "4_fourway" || mapscript.getpartname(i, J) == "3_fourway" || mapscript.getpartname(i, J) == "2_fourway" || mapscript.getpartname(i, J) == "1_fourway" || mapscript.getpartname(i, J) == "1_bridge1" || mapscript.getpartname(i, J) == "2_bridge1" || mapscript.getpartname(i, J) == "3_bridge1" || mapscript.getpartname(i, J) == "4_bridge1";
        if (specialCase)
        {
            BuildGridBoxBridgeScript(i, J);
        }
    }
    string ReturnPartName(int i, int j, bool primarypart)
    {
        bool SpecialPart = false;
        if (Lanes == 4)
        {
            SpecialPart = true;
        }
        if (mapscript.Getpartexist(i, j))
        {
            if (mapscript.getpart(i, j) != null)
            {
                Debug.Log("exist part");

                if (mapscript.getpart(i, j).name != "SpecialBuildingPlace")
                {
                    Debug.Log("exist part special");
                    SpecialPart = false;
                }
            }
            
            try
            {
                if (mapscript.Getpartexist(i + 1, j))
                {
                    right = true;
                }
            }
            catch (Exception e) { right = false; };

            try
            {
                if (mapscript.Getpartexist(i - 1, j))
                {
                    left = true;
                }
            }
            catch (Exception e) { left = false; };

            try
            {
                if (mapscript.Getpartexist(i, j + 1))
                {
                    dowm = true;
                }
            }
            catch (Exception e) { dowm = false; };

            try
            {
                if (mapscript.Getpartexist(i, j - 1))
                {
                    up = true;
                }
            }
            catch (Exception e) { up = false; };
        }
        
        if (!SpecialPart)
        {
            var tempLanes = Lanes;
            if (Lanes == 4)
            {
                tempLanes = 2;
            }
            if (!up && !dowm && !left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;

                return "" + (tempLanes + 1) + "_straight1";
            }

            if (up && !dowm && !left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight1";
            }

            if (!up && dowm && !left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight1";
            }

            if (!up && !dowm && left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight2";
            }

            if (!up && !dowm && !left && right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight2";
            }

            if (up && dowm && !left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight1";
            }

            if (left && right && !up && !dowm)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_straight2";
            }

            if (up && right && !dowm && !left)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_turn2";
            }

            if (right && dowm && !left && !up)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_turn3";
            }

            if (dowm && left && !up && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_turn4";
            }

            if (left && up && !right && !dowm)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;

                return "" + (tempLanes + 1) + "_turn1";
            }

            if (up && left && right && !dowm)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_Threeway3";
            }
            if (dowm && up && right && !left)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_Threeway2";
            }

            if (dowm && left && right && !up)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_Threeway1";
            }

            if (dowm && up && left && !right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                return "" + (tempLanes + 1) + "_Threeway4";
            }

            if (dowm && up && left && right)
            {
                up = false;
                right = false;
                dowm = false;
                left = false;
                if (mapscript.getpartname(i, j) == "1_fourway")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge1";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "2_fourway")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge1";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "3_fourway")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge1";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "4_fourway")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge1";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }

                if (mapscript.getpartname(i, j) == "1_bridge1")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge2";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "2_bridge1")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge2";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "3_bridge1")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge2";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "4_bridge1")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_bridge2";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "1_bridge2")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_fourway";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "2_bridge2")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_fourway";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "3_bridge2")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_fourway";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                if (mapscript.getpartname(i, j) == "4_bridge2")
                {
                    if (primarypart)
                    {
                        return "" + (tempLanes + 1) + "_fourway";
                    }
                    else
                    {
                        return mapscript.getpartname(i, j);
                    }
                }
                return "" + (tempLanes + 1) + "_fourway";
            }
            up = false;
            right = false;
            dowm = false;
            left = false;
        }
        else
        {
            up = false;
            right = false;
            dowm = false;
            left = false;

            return "SpecialBuildingPlace";
        }
        return "Dir error";
    }

    void UpdateAllPartsToTerrain()
    {
        int GridBoxesCount = columns * rows;
        DeformedMap = true;
        if (ter != null)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (mapscript.Getpartexist(i, j))
                    {
                        FitToTerrain(i, j, mapscript.getpart(i, j), ter);
                    }
                    EditorUtility.DisplayProgressBar("Updating to terrain", "", (float)(((i) * rows + j + 1f) / GridBoxesCount));
                }
            }
            EditorUtility.DisplayProgressBar("Updating to terrain", "", 1f);
            EditorUtility.ClearProgressBar();
        }
    }

    private void FitToTerrain(int i, int j, GameObject G, Terrain ter)
    {
        GameObject GA = (GameObject)Resources.Load("" + mapscript.getpartname(i, j));
        Mesh newmesh = (Mesh)Instantiate(GA.GetComponent<MeshFilter>().sharedMesh);
        G.transform.localPosition = new Vector3(G.transform.localPosition.x, ter.SampleHeight(G.transform.position) + ter.transform.position.y - TheMap.transform.position.y, G.transform.localPosition.z);
        Vector3[] ver = newmesh.vertices;
        for (int v = 0; v < ver.Length; v++)
        {
            float coss = Mathf.Cos(G.transform.eulerAngles.y * Mathf.Deg2Rad);
            float sinn = Mathf.Sin(G.transform.eulerAngles.y * Mathf.Deg2Rad);
            float x = ver[v].x * coss + ver[v].z * sinn;
            float y = ver[v].z * coss - ver[v].x * sinn;
            ver[v] = new Vector3(ver[v].x, ver[v].y + ter.SampleHeight(G.transform.position + new Vector3(x, 0, y)) + 2f - G.transform.position.y + ter.transform.position.y, ver[v].z);
        }
        newmesh.vertices = ver;
        G.GetComponent<MeshFilter>().sharedMesh = newmesh;
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3"))
        {
            AssetDatabase.CreateFolder("Assets", "Auto-road Creator_V3");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/TerMesh"))
        {
            AssetDatabase.CreateFolder("Assets/Auto-road Creator_V3", "TerMesh");
        }
        if (!AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/TerMesh/" + mapscript.name))
        {
            AssetDatabase.CreateFolder("Assets/Auto-road Creator_V3/TerMesh", "" + mapscript.name);
        }
        AssetDatabase.CreateAsset(newmesh, "Assets/Auto-road Creator_V3/TerMesh/" + mapscript.name + "/TerMesh-" + i + "-" + j + "map#" + TheMap.GetInstanceID() + ".asset");
        mapscript.getpart(i, j).GetComponent<MeshCollider>().sharedMesh = newmesh;
    }
    void RevertFittingToTerrain()
    {
        DeformedMap = false;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (mapscript.Getpartexist(i, j))
                {
                    GameObject GA = (GameObject)Resources.Load("" + mapscript.getpartname(i, j));

                    Mesh Gamesh = (Mesh)(GA.GetComponent<MeshFilter>().sharedMesh);
                    mapscript.getpart(i, j).GetComponent<MeshFilter>().sharedMesh = Gamesh;
                    mapscript.setpart(i, j, GA);
                    mapscript.setpartexist(i, j, true);
                }
            }
        }
        mapscript.ResetTheMap(columns, rows);
        if (AssetDatabase.IsValidFolder("Assets/Auto-road Creator_V3/TerMesh/" + mapscript.name))
        {
            FileUtil.DeleteFileOrDirectory("Assets/Auto-road Creator_V3/TerMesh/" + mapscript.name);
            AssetDatabase.Refresh();
        }
        AssetDatabase.SaveAssets();
    }

    private int GetID(int C, int R, bool PRIMARY, bool isSpecialBuidling)
    {
        bool specialCase = mapscript.getpartname(C, R) == "4_fourway" || mapscript.getpartname(C, R) == "3_fourway" || mapscript.getpartname(C, R) == "2_fourway" || mapscript.getpartname(C, R) == "1_fourway" || mapscript.getpartname(C, R) == "1_bridge1" || mapscript.getpartname(C, R) == "2_bridge1" || mapscript.getpartname(C, R) == "3_bridge1" || mapscript.getpartname(C, R) == "4_bridge1";
        if (specialCase)
        {
            if (PRIMARY)
            {
                return 0;
            }
        }   
        if (!GridBoxes[C][R].IsOn)
        {
            return 0;
        }
        // Special building (in work)
        if (mapscript.getpartname(C, R) == "SpecialBuildingPlace")
        {
            return 501;
        }


        tempDown = false;
        tempUp = false;
        tempRight = false;
        tempLeft = false;
        try
        {
            if (GridBoxes[(C + 1)][R].IsOn)
            {
                tempRight = true;
            }
        }
        catch (ArgumentOutOfRangeException e) { }

        try
        {
            if (GridBoxes[(C - 1)][R].IsOn)
            {
                tempLeft = true;
            }
        }
        catch (ArgumentOutOfRangeException e) { }

        try
        {
            if (GridBoxes[C][R + 1].IsOn)
            {
                tempDown = true;
            }
        }
        catch (ArgumentOutOfRangeException e) { }
        try
        {
            if (GridBoxes[C][R - 1].IsOn)
            {
                tempUp = true;
            }
        }
        catch (ArgumentOutOfRangeException e) { }
        return GetTextureID(tempRight, tempUp, tempLeft, tempDown, isSpecialBuidling);
    }
    private int GetTextureID(bool Right, bool Up, bool Left, bool Down, bool isSpecialBuidling)
    {
        // change for building (this place) to get right one
        if (Lanes != 4)
        {
            if (Right && Up && Left && Down)
            {
                return ((Lanes + 1) * 100 + 11);
            }
            if (!Right && Up && Left && Down)
            {
                return ((Lanes + 1) * 100 + 8);
            }
            if (Right && !Up && Left && Down)
            {
                return ((Lanes + 1) * 100 + 9);
            }
            if (Right && Up && !Left && Down)
            {
                return ((Lanes + 1) * 100 + 10);
            }
            if (Right && Up && Left && !Down)
            {
                return ((Lanes + 1) * 100 + 7);
            }
            if (!Right && !Up && Left && Down)
            {
                return ((Lanes + 1) * 100 + 5);
            }
            if (!Right && Up && !Left && Down)
            {
                return ((Lanes + 1) * 100 + 2);
            }
            if (!Right && Up && Left && !Down)
            {
                return ((Lanes + 1) * 100 + 4);
            }
            if (Right && !Up && !Left && Down)
            {
                return ((Lanes + 1) * 100 + 6);
            }
            if (Right && !Up && Left && !Down)
            {
                return ((Lanes + 1) * 100 + 1);
            }
            if (Right && Up && !Left && !Down)
            {
                return ((Lanes + 1) * 100 + 3);
            }
            if (!Right && !Up && !Left && Down)
            {
                return ((Lanes + 1) * 100 + 2);
            }
            if (!Right && !Up && Left && !Down)
            {
                return ((Lanes + 1) * 100 + 1);
            }
            if (!Right && Up && !Left && !Down)
            {
                return ((Lanes + 1) * 100 + 2);
            }
            if (Right && !Up && !Left && !Down)
            {
                return ((Lanes + 1) * 100 + 1);
            }
            if (!Right && !Up && !Left && !Down)
            {
                return ((Lanes + 1) * 100 + 2);
            }
        }
        else
        {
            if (isSpecialBuidling)
            {
                if (Right && Up && Left && Down)
                {
                    return (3 * 100 + 11);
                }
                if (!Right && Up && Left && Down)
                {
                    return (3 * 100 + 8);
                }
                if (Right && !Up && Left && Down)
                {
                    return (3 * 100 + 9);
                }
                if (Right && Up && !Left && Down)
                {
                    return (3 * 100 + 10);
                }
                if (Right && Up && Left && !Down)
                {
                    return (3 * 100 + 7);
                }
                if (!Right && !Up && Left && Down)
                {
                    return (3 * 100 + 5);
                }
                if (!Right && Up && !Left && Down)
                {
                    return (3 * 100 + 2);
                }
                if (!Right && Up && Left && !Down)
                {
                    return (3 * 100 + 4);
                }
                if (Right && !Up && !Left && Down)
                {
                    return (3 * 100 + 6);
                }
                if (Right && !Up && Left && !Down)
                {
                    return (3 * 100 + 1);
                }
                if (Right && Up && !Left && !Down)
                {
                    return (3 * 100 + 3);
                }
                if (!Right && !Up && !Left && Down)
                {
                    return (3 * 100 + 2);
                }
                if (!Right && !Up && Left && !Down)
                {
                    return (3 * 100 + 1);
                }
                if (!Right && Up && !Left && !Down)
                {
                    return (3 * 100 + 2);
                }
                if (Right && !Up && !Left && !Down)
                {
                    return (3 * 100 + 1);
                }
                if (!Right && !Up && !Left && !Down)
                {
                    return (3 * 100 + 2);
                }
            }
            return ((Lanes + 1) * 100 + 1);
        }
        return 0;
    }
}