using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static SimpleVisualizer;
using Random=UnityEngine.Random;

	public class Visualizer : MonoBehaviour
	{
        public LSystemGenerator lsystem;
        List<Vector3> positions = new List<Vector3>();
        public RoadHelper roadHelper;
        public StructureHelper structureHelper;

        private int MaxLength = 50;
        private int MinLength = 10;

        // private int Length = 50;
        // private float angle = 90;

        // public int Length
        // {
        //     get
        //     {
        //         if (length > 0)
        //         {
        //             return length;
        //         }
        //         else
        //         {
        //             return 1;
        //         }
        //     }
        //     set => length = value;
        // }

        // public void CreateRoad(Vector3 position)
        // {
        //     // float dist = 8;
        //     // var sequence = "[F]--F";
        //     // Vector3 direction = new Vector3(0,0,50); 
            
        //     // VisualizeSequence(sequence, position, direction, dist);
        //     // for (int i = 0; i < clickPositionManager.points.Length - 1; i++)
        //     // {
        //     //     point1 = clickPositionManager.points[i].transform.position;
        //     //     point2 = clickPositionManager.points[i + 1].transform.position;
        //     //     point1.x = (float)(Mathf.Round(point1.x * 100) / 100.0);
        //     //     point1.z = (float)(Mathf.Round(point1.z * 100) / 100.0);
        //     //     point2.x = (float)(Mathf.Round(point2.x * 100) / 100.0);
        //     //     point2.z = (float)(Mathf.Round(point2.z * 100) / 100.0);
        //     //     point1.y = 0;
        //     //     point2.y = 0;
        //     //     pointAvg = (point1 + point2) / 2;
        //     //     pointAvg.x = (float)(Mathf.Round(pointAvg.x * 100) / 100.0);
        //     //     pointAvg.z = (float)(Mathf.Round(pointAvg.z * 100) / 100.0);
        //     //     direction = point2 - pointAvg;
        //     //     direction = Vector3.Normalize(direction);
        //     //     dist = Vector3.Distance(point2, pointAvg);
        //     //     if (i == 0)
        //     //     {
        //     //         VisualizeSequence(sequence, pointAvg, direction, dist);
        //     //         MainDirection = direction;
        //     //     }
        //     //     if (i > 0)
        //     //     {
        //     //         MainDirection = Vector3.Normalize(MainDirection);
        //     //         Vector3 temp = new Vector3(0, 1, 0);
        //     //         Vector3 CrossDirect = Vector3.Cross(MainDirection, temp);
        //     //         float distanceX = (((point2.x - point1.x) / CrossDirect.x - (point2.z - point1.z) / CrossDirect.z) / (MainDirection.x / CrossDirect.x - MainDirection.z / CrossDirect.z));
        //     //         float distanceY = (((point2.x - point1.x) / MainDirection.x - (point2.z - point1.z) / MainDirection.z) / (CrossDirect.x / MainDirection.x - CrossDirect.z / MainDirection.z));
        //     //         Vector3 point3 = point1 + distanceY * CrossDirect;
        //     //         Vector3 point4 = point3 + distanceX * MainDirection;
        //     //         point3.x = (float)(Mathf.Round(point3.x * 100) / 100.0);
        //     //         point3.z = (float)(Mathf.Round(point3.z * 100) / 100.0);
        //     //         direction = point1 - point3;
        //     //         direction = Vector3.Normalize(direction);
        //     //         dist = (float)(Mathf.Round(Vector3.Distance(point1, point3) * 100) / 100.0);
        //     //         VisualizeSequence(sequence, point1, direction, dist);
        //     //     }
        //     // }


        //     //// position, object and direction of main road dictionary
        //     // Dictionary<Vector3, Tuple<GameObject, Vector3>> mainRoad = roadHelper.GetRoads();
        //     // List<int> betweenBrach = new List<int>(0);
        //     // for (int j = 0; j < 3; j++)
        //     // {
        //     //     var branch = UnityEngine.Random.Range(0, mainRoad.Count);
        //     //     bool tooClose = false;
        //     //     for (int k = 0; k < betweenBrach.Count; k++)
        //     //     {
        //     //         if (Mathf.Abs(betweenBrach[k] - branch) < 3)
        //     //         {
        //     //             tooClose = true;
        //     //             continue;
        //     //         }
        //     //     }
        //     //     betweenBrach.Add(branch);
        //     //     if (tooClose)
        //     //     {
        //     //         tooClose = false;
        //     //         continue;
        //     //     }
        //     //     dist = 8;
        //     //     var branchDist = UnityEngine.Random.Range(dist / 2, dist);
        //     //     var branchElement = mainRoad.ElementAt(branch);
        //     //     Vector3 branchDirect = mainRoad[branchElement.Key].Item2;
        //     //     Vector3 branchPosition = branchElement.Key;
        //     //     Vector3 temp = new Vector3(0, 1, 0);
        //     //     branchDirect = Vector3.Cross(branchDirect, temp);
        //     //     sequence = lsystem.GenerateSentence();
        //     //     VisualizeSequence(sequence, branchPosition, branchDirect, branchDist);
        //     // }
        // }

        // private void VisualizeSequence(string sequence, Vector3 currentPosition, Vector3 direction, float dist)
        // {
        //     Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            

        //     Vector3 tempPosition = Vector3.zero;

        //     positions.Add(currentPosition);

        //     foreach (var letter in sequence)
        //     {
        //         EncodingLetters encoding = (EncodingLetters)letter;
        //         switch (encoding)
        //         {
        //             case EncodingLetters.save:
        //                 savePoints.Push(new AgentParameters
        //                 {
        //                     position = currentPosition,
        //                     direction = direction,
        //                     length = (int)dist
        //                 });
        //                 break;
        //             case EncodingLetters.load:
        //                 if (savePoints.Count > 0)
        //                 {
        //                     var agentParameter = savePoints.Pop();
        //                     currentPosition = agentParameter.position;
        //                     direction = agentParameter.direction;
        //                     Length = agentParameter.length;
        //                 }
        //                 else
        //                 {
        //                     throw new System.Exception("Dont have saved point in our stack");
        //                 }
        //                 break;
        //             case EncodingLetters.draw:
        //                 tempPosition = currentPosition;
        //                 currentPosition += direction * length;
        //                 roadHelper.PlaceStreetPositions(tempPosition, direction, length);
        //                 Length -= 2;
        //                 positions.Add(currentPosition);
        //                 break;
        //             case EncodingLetters.turnRight:
        //                 direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
        //                 break;
        //             case EncodingLetters.turnLeft:
        //                 direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        // }

        public void CreateRoad2(Vector3 position, Vector3 direction, int MapSize)
        {
            MaxLength = MapSize;
            MinLength = MapSize/10;
            int roadLength = MaxLength;
            roadHelper.PlaceStreetPositions(position, direction, roadLength);
            int keepGenerating = 0;
            int roadCount = 0;
            int MaxRoad = Mathf.FloorToInt(MapSize/2);
            while ( keepGenerating != 1 && roadCount < MaxRoad) {
                roadLength = Random.Range(MinLength, MaxLength);
                GameObject[] GenPoint = GameObject.FindGameObjectsWithTag("GenRoad");
                var element = GenPoint[Random.Range(0, GenPoint.Length-1)];
                roadHelper.CreateBranchRoad(element, roadLength);
                keepGenerating = Random.Range(1, MapSize);
                roadCount++;
            }
        }
        
        public void fixRoad()
        {
            roadHelper.FixRoad();
        }

        public void AddHouse(int MapSize)
        {
            structureHelper.PlaceStructuresAroundRoad(roadHelper.GetRoads());
            // structureHelper.PlaceRandomStructuresInside(MapSize, roadHelper.GetRoadPositions());
        }
    }


