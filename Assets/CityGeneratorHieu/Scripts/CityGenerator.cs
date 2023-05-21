using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    [Range(1, 9)]
    public int gridX = 5;
    [Range(1, 9)]
    public int gridZ = 5;
    public int MapSize = 50;

    public GameObject roadStraight, roadCorner, road3way, road4way, trafficLight, lightRoad, treeRoad;
    public List<BuildingDataType> buildingPrefabs = new List<BuildingDataType>();
    public List<GameObject> naturePrefabs = new List<GameObject>();

    Visualizer visualizerScript;
    RoadHelper roadHelperScript;
    StructureHelper structureHelperScript;
    private int distanceBetween = 50;
    private int distance = 2;
    // private int randomRange = 5;
    public void generateCity()
    {
        setRoads();
        setBuildingsAndNatures();
        setMap();
        GenerateRoad();
        GenerateAssetRoad();
        GenerateHouse();
        
    }

    private void setMap()
    {
        GameObject roadHelperObject = GameObject.Find("RoadHelper2");
        roadHelperScript = roadHelperObject.GetComponent<RoadHelper>();
        roadHelperScript.setBeginMap(MapSize);
    }

    private void setRoads()
    {
        GameObject roadHelperObject = GameObject.Find("RoadHelper2");
        roadHelperScript = roadHelperObject.GetComponent<RoadHelper>();
        if (roadStraight)
        {
            roadHelperScript.roadStraight = roadStraight;
            roadHelperScript.roadEnd = roadStraight;
        }
        if (roadCorner)
        {
            roadHelperScript.roadCorner = roadCorner;
        }
        if (road3way)
        {
            roadHelperScript.road3way = road3way;
        }
        if (road4way)
        {
            roadHelperScript.road4way = road4way;
        }
        if (trafficLight)
        {
            roadHelperScript.trafficLight = trafficLight;
        }
        if (lightRoad)
        {
            roadHelperScript.lightRoad = lightRoad;
        }
        if (treeRoad)
        {
            roadHelperScript.treeRoad = treeRoad;
        }
    }

    private void setBuildingsAndNatures()
    {
        GameObject structureHelperObject = GameObject.Find("StructureHelper2");
        structureHelperScript = structureHelperObject.GetComponent<StructureHelper>();
        if (buildingPrefabs.Count >= 1)
        {
            BuildingDataType[] arrayOfBuildingPrefabs = buildingPrefabs.ToArray();
            structureHelperScript.buildingTypes = arrayOfBuildingPrefabs;
        }
        if (naturePrefabs.Count >= 1)
        {
            GameObject[] arrayOfNaturePrefabs = naturePrefabs.ToArray();
            structureHelperScript.naturePrefabs = arrayOfNaturePrefabs;
        }
    }

    private void GenerateRoad()
    {
        GameObject[] StraightRoad = GameObject.FindGameObjectsWithTag("StraightRoad");
        if (StraightRoad.Length > 0)
        {
            int MinDistanceRandom = distance * distanceBetween;
            GameObject visualizerObject = GameObject.Find("Visualizer2");
            visualizerScript = visualizerObject.GetComponent<Visualizer>();
            // visualizerScript.CreateRoad(StraightRoad[2].transform.position);
            // visualizerScript.fixRoad();
            // foreach (GameObject startPoint in StraightRoad)
            // {

            //     float xStartPos = startPoint.transform.position.x;
            //     float zStartPos = startPoint.transform.position.z;
            //     Vector3 pos = startPoint.transform.position;
            //     Collider[] colliders = Physics.OverlapSphere(pos, 40);
            //     var flag = false;
            //     foreach (Collider collider in colliders)
            //     {
            //         if (collider.gameObject.name.Contains("turn"))
            //         {
            //             flag = true;
            //             break;
            //         }
            //     }
            //     if (flag)
            //     {
            //         continue;
            //     }
            //     if (Random.Range(1, randomRange) == 1)
            //     {
            //         bool rotate = false;
            //         if (startPoint.name.Contains("straight1"))
            //         {
            //             rotate = true;
            //         }
            //         // visualizerScript.CreateRoad(startPoint.transform.position, rotate);
            //         // DestroyImmediate(startPoint);
            //     }
            // }
        }
        else
        {
            GameObject visualizerObject = GameObject.Find("Visualizer2");
            visualizerScript = visualizerObject.GetComponent<Visualizer>();
            var direction  = new Vector3(-50,0,0);
            visualizerScript.CreateRoad2(new Vector3(Mathf.FloorToInt(MapSize/2)*50,0,Mathf.FloorToInt(MapSize/2)*50), direction, MapSize);
            visualizerScript.fixRoad();
        }
    }

    private void GenerateAssetRoad()
    {
        GameObject roadHelperObject = GameObject.Find("RoadHelper2");
        roadHelperScript = roadHelperObject.GetComponent<RoadHelper>();
        roadHelperScript.GenerateAssetRoad();
    }

    private void GenerateHouse()
    {
        GameObject visualizerObject = GameObject.Find("Visualizer2");
        visualizerScript = visualizerObject.GetComponent<Visualizer>();
        visualizerScript.AddHouse(MapSize);
    }

    public void clearAll()
    {
        Debug.Log("Clearing");
        GameObject roadHelperObject = GameObject.Find("RoadHelper2");
        roadHelperScript = roadHelperObject.GetComponent<RoadHelper>();
        GameObject structureHelperObject = GameObject.Find("StructureHelper2");
        structureHelperScript = structureHelperObject.GetComponent<StructureHelper>();
        roadHelperScript.clearAll();
        structureHelperScript.clearAll();
    }

    


}
