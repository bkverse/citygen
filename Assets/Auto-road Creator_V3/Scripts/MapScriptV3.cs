using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapScriptV3 : MonoBehaviour
{
    public List<List<GameObject>> Parts = new List<List<GameObject>>();
    public List<List<string>> partsnames = new List<List<string>>();
    public List<List<bool>> partsExist = new List<List<bool>>();
    Texture2D empty, OneLaneStraight13, OneLaneStraight24, OneLaneTurn12, OneLaneTurn23, OneLaneTurn34, OneLaneTurn41, OneLaneThreeWay123, OneLaneThreeWay234, OneLaneThreeWay341, OneLaneThreeWay412, OneLanefourway, onelanebridge, onelanebridge2;
    Texture2D TwoLaneStraight13, TwoLaneStraight24, TwoLaneTurn12, TwoLaneTurn23, TwoLaneTurn34, TwoLaneTurn41, TwoLaneThreeWay123, TwoLaneThreeWay234, TwoLaneThreeWay341, TwoLaneThreeWay412, TwoLanefourway, Twolanebridge, Twolanebridge2;
    Texture2D ThreeLaneStraight13, ThreeLaneStraight24, ThreeLaneTurn12, ThreeLaneTurn23, ThreeLaneTurn34, ThreeLaneTurn41, ThreeLaneThreeWay123, ThreeLaneThreeWay234, ThreeLaneThreeWay341, ThreeLaneThreeWay412, ThreeLanefourway, Threelanebridge, Threelanebridge2;
    Texture2D SpecialBuildingPlace;
    bool up, dowm, left, right, ShowBoundries;
    int ColumnS, Rows;
    [HideInInspector]
    [SerializeField]
    Terrain ter;
    public MapScriptV3()
    {
        resetParts(0, 0);
        resetpartexist(0, 0);
        resetnames(0, 0);

    }
    public Terrain GetTerrain()
    {
        return ter;
    }
    public void SetTerrain(Terrain terr)
    {
        ter = terr;
    }
    public bool Getpartexist(int i, int j)
    {
        try
        { return partsExist[i][j]; }
        catch (Exception e)
        {
            Debug.Log(e); 
            return false; 
        }
    }
    public void setpartexist(int i, int j, bool exist)
    {
        partsExist[i][j] = exist;
    }
    public string getpartname(int i, int j)
    {
        return partsnames[i][j];

    }
    public void Setpartname(int i, int j, string newname)
    {
        partsnames[i][j] = newname;
    }
    public GameObject getpart(int i, int j)
    {
        return Parts[i][j];
    }
    public void setpart(int i, int j, GameObject G)
    {
        if (G == null)
        {
        }
        else
        {
            Parts[i][j] = G;
        }
    }
    public void Destroypart(int i, int j)
    {
        DestroyImmediate(Parts[i][j]);
    }

    public void createPart(int i, int j, Terrain te)
    {
        Parts[i][j] = Instantiate(Resources.Load(partsnames[i][j])) as GameObject;
        Parts[i][j].AddComponent<PartScriptV3>().Set("" + partsnames[i][j], i, j);
        Parts[i][j].name = partsnames[i][j];
        // if (Parts[i][j].name.Contains('straight'))
        // {
        //     Parts[i][j].tag = "straightRoad";
        // }
        Parts[i][j].transform.parent = transform;
        Parts[i][j].transform.localPosition = new Vector3(50 * i, 0, -50 * j);
        if (partsnames[i][j] == "SpecialBuildingPlace")
        {
            Parts[i][j].transform.localScale = new Vector3(50, 50, 50);
        }
    }

    public void ResetTheMap(int CC, int RR)
    {
        resetParts(CC, RR);
        resetnames(CC, RR);
        resetpartexist(CC, RR);
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<PartScriptV3>() != null)
                {
                    int x = transform.GetChild(i).GetComponent<PartScriptV3>().GetI();
                    int y = transform.GetChild(i).GetComponent<PartScriptV3>().GetJ();
                    setpart(x, y, transform.GetChild(i).gameObject);
                    partsExist[x][y] = true;
                    Setpartname(x, y, transform.GetChild(i).GetComponent<PartScriptV3>().GetName());
                    // Parts[x][y].GetComponent<MeshCollider>().sharedMesh = Parts[x][y].GetComponent<MeshFilter>().sharedMesh;
                    if (ter == null)
                    {
                        Parts[x][y].transform.localPosition = new Vector3(Parts[x][y].transform.localPosition.x, 0, Parts[x][y].transform.localPosition.z);
                    }
                }
            }
        }
    }
    void resetParts(int CC, int RR)
    {
        Parts = new List<List<GameObject>>();
        for (int i = 0; i < CC; i++)
        {
            Parts.Add(new List<GameObject>());
            for (int j = 0; j < RR; j++)
            {
                Parts[i].Add(null);
            }
        }
    }
    void resetnames(int CC, int RR)
    {
        partsnames = new List<List<string>>();
        for (int i = 0; i < CC; i++)
        {
            partsnames.Add(new List<string>());
            for (int j = 0; j < RR; j++)
            {
                partsnames[i].Add("");
            }
        }
    }
    void resetpartexist(int CC, int RR)
    {
        partsExist = new List<List<bool>>();
        for (int i = 0; i < CC; i++)
        {
            partsExist.Add(new List<bool>());
            for (int j = 0; j < RR; j++)
            {
                partsExist[i].Add(false);
            }
        }
    }

    public void updateColumns(int CC, int RR)
    {
        AdjustArrays(CC, ColumnS, RR, Rows);
        ColumnS = CC;
        Rows = RR;
    }
    void AdjustArrays(int newcolumns, int OldColumns, int NewRows, int oldRows)
    {
        for (int i = 0; i < newcolumns; i++)
        {
            if (i >= OldColumns)
            {
                Parts.Add(new List<GameObject>());
                partsnames.Add(new List<string>());
                partsExist.Add(new List<bool>());
            }
            for (int j = 0; j < NewRows; j++)
            {
                if (j >= oldRows || i >= OldColumns)
                {
                    Parts[i].Add(null);
                    partsnames[i].Add("");
                    partsExist[i].Add(new bool());
                }
            }
        }
    }
    public void AddRowAtFirst(int columncount)
    {

        for (int i = 0; i < columncount; i++)
        {
            Parts[i].Insert(0, null);
            partsnames[i].Insert(0, "");
            partsExist[i].Insert(0, new bool());
        }

        for (int i = 0; i < Parts.Count; i++)
        {
            for (int j = 0; j < Parts[i].Count; j++)
            {
                try
                {
                    Parts[i][j].transform.Translate(0, 0, -50, Space.World);
                    Parts[i][j].GetComponent<PartScriptV3>().setIJ(i, j);
                }
                catch (Exception e) 
                {   Debug.Log(e);}
            }
        }
    }
    public void AddAColumnAtFirst(int ColumnCount)
    {

        Parts.Insert(0, new List<GameObject>());
        partsnames.Insert(0, new List<string>());
        partsExist.Insert(0, new List<bool>());
        for (int i = 0; i < ColumnCount; i++)
        {
            Parts[0].Add(null);
            partsnames[0].Add("");
            partsExist[0].Add(new bool());
        }
        for (int i = 0; i < Parts.Count; i++)
        {
            for (int j = 0; j < Parts[i].Count; j++)
            {
                try
                {
                    Parts[i][j].transform.Translate(50, 0, 0, Space.World);
                    Parts[i][j].GetComponent<PartScriptV3>().setIJ(i, j);
                }
                catch (Exception e) 
                { Debug.Log(e); }
            }
        }
    }
}