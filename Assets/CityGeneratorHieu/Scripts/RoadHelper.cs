using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;


	public class RoadHelper : MonoBehaviour
	{

		public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd, trafficLight, lightRoad, treeRoad;
		// roadDictionary contains:
		// + position of the road
		// + gameObject
		// + direction of the road
		Dictionary<Vector3, Tuple<GameObject,Vector3>> roadDictionary = new Dictionary<Vector3, Tuple<GameObject, Vector3>>();
		Dictionary<Vector3, Vector3> fixRoadCandidates = new Dictionary<Vector3, Vector3>();
		// RoadPositions contains:
		// + position of the road
		// + direction of the road
		public Dictionary<Vector3, Vector3> GetRoadPositions()
		{
			Dictionary<Vector3, Vector3> tempRoad = new Dictionary<Vector3, Vector3>();
			foreach ( var road in roadDictionary)
			{
				tempRoad.Add(road.Key, road.Value.Item2);
			}
			return tempRoad;
		}

		public Dictionary<Vector3, Tuple<GameObject, Vector3>> GetRoads()
		{
			return roadDictionary;
		}

		public bool findRoads(Vector3 position)
		{
			float delta = 0.0001f;
			foreach (var item in roadDictionary.Keys)
			{
				if (Math.Abs(item.x - position.x) < delta && Math.Abs(item.y - position.y) < delta && Math.Abs(item.z - position.z) < delta)
				{
					return true;
				}
			}
			return false;
		}

		// temporarily not use
		public GameObject checkRoads(Vector3 position)
		{
			float delta = 0.0001f;
			foreach (var item in roadDictionary.Keys)
			{
				if (Math.Abs(item.x - position.x) < delta && Math.Abs(item.y - position.y) < delta && Math.Abs(item.z - position.z) < delta)
				{
					return roadDictionary[item].Item1;
				}
			}
			return null;
		}

		public bool touchBorder(Vector3 position)
		{
			if (roadDictionary[position].Item1.tag == "borderRoad")
			{
				return true;
			}
			return false;

		}

		public void setBeginMap(int MapSize)
		{
			for(int x = 1 ; x <= MapSize; x++) {
				for (int y = 1; y <= MapSize; y++) {
					if (x == 1 || x == MapSize) {
						var position = new Vector3(x*50, 0, y*50);
						var rotation = Quaternion.identity;
						var direction = new Vector3(0,0,1);
						var road = Instantiate(roadStraight, position, rotation, transform);
						road.tag = "borderRoad";
						roadDictionary.Add(position, Tuple.Create(road, direction));
						fixRoadCandidates.Add(position, direction);
					}
					else if (y == 1 || y == MapSize) {
						var position = new Vector3(x*50, 0, y*50);
						var rotation = Quaternion.Euler(0, 90, 0);
						var direction = new Vector3(1,0,0);
						var road = Instantiate(roadStraight, position, rotation, transform);
						road.tag = "borderRoad";
						roadDictionary.Add(position, Tuple.Create(road, direction));
						fixRoadCandidates.Add(position, direction);
					}
				}
			}
		}

		public void PlaceStreetPositions(Vector3 startPosition, Vector3 direction, int length)
		{

			var rotation = Quaternion.identity;

			for (int i = 0; i < length; i++)
			{
				if (direction == new Vector3(50,0,0))
				{
					rotation = Quaternion.Euler(0, 90, 0);
				}
				if (direction == new Vector3(-50,0,0))
				{
					rotation = Quaternion.Euler(0, 90, 0);
				}
				if (direction == new Vector3(0, 0, -50))
				{
					rotation = Quaternion.Euler(0, -180, 0);
				}
				if (direction == new Vector3(0, 0, 50))
				{
						rotation = Quaternion.Euler(0, 180, 0);
				}
				var position = (startPosition + direction * i);
				if (roadDictionary.ContainsKey(position))
				{
					if (touchBorder(position)) {
						break;
					}
					continue;
				}
				if (findRoads(position))
				{
					continue;
				}
				Vector3 temp = new Vector3(0, 1, 0);
				Vector3 ans = Vector3.Cross(direction, temp);
				Vector3 up = position + ans;
				Vector3 down = position - ans;
				Vector3 upLeft = up + direction;
				Vector3 upRight = up - direction;
				Vector3 downLeft = down + direction;
				Vector3 downRight = down - direction;
				if (findRoads(upLeft) && findRoads(upRight))
				{
					continue;
				}
				if (findRoads(downLeft) && findRoads(downRight))
				{
					continue;
				}
				// var road = Instantiate(Resources.Load("2_straight2")) as GameObject;
				// if (direction == new Vector3(1,0,0) || direction == new Vector3(-1,0,0))
				// {
				// 	road = Instantiate(Resources.Load("2_straight2")) as GameObject;
				// 	road.transform.parent = transform;
       			// 	road.transform.position = position;
				// }
				// if (direction == new Vector3(0,0,1) || direction == new Vector3(0,0,-1))
				// {
				// 	road = Instantiate(Resources.Load("2_straight1")) as GameObject;
       			// 	road.transform.parent = transform;
       			// 	road.transform.position = position;
				// }
				var road = Instantiate(roadStraight, position, rotation, transform);
				road.tag = "GenRoad";
				roadDictionary.Add(position, Tuple.Create(road, direction));
				fixRoadCandidates.Add(position, direction);
			}
		}

		public void CreateBranchRoad(GameObject startPoint, int roadLength)
		{
			var startPointPosition = startPoint.transform.position;
			if (roadDictionary.ContainsKey(startPointPosition))
			{
				Vector3 temp = new Vector3(0, 1, 0);
				Vector3 directionStartPoint = new Vector3(0, 0, 0);
				if (Random.Range(0,2) == 0)
					directionStartPoint = Vector3.Cross(roadDictionary[startPointPosition].Item2, temp);
				else
					directionStartPoint = -Vector3.Cross(roadDictionary[startPointPosition].Item2, temp);
				DestroyImmediate(roadDictionary[startPointPosition].Item1);
				roadDictionary.Remove(startPointPosition);
				fixRoadCandidates.Remove(startPointPosition);
				PlaceStreetPositions(startPointPosition, directionStartPoint, roadLength);
			}
		}

		public void FixRoad()
		{
			foreach (var candidate in fixRoadCandidates)
			{

				List<Direction> neighborDirections = PlacementHelper.FindNeighbor(candidate.Key, candidate.Value, roadDictionary.Keys);
				Quaternion rotation = Quaternion.LookRotation(candidate.Value, Vector3.up);

				if (neighborDirections.Count == 1)
				{
					DestroyImmediate(roadDictionary[candidate.Key].Item1);
					if (neighborDirections.Contains(Direction.Down))
					{
						rotation *= Quaternion.Euler(0, 180, 0);
					}
					else if (neighborDirections.Contains(Direction.Left))
					{
						rotation *= Quaternion.Euler(0, 90, 0);
					}
					else if (neighborDirections.Contains(Direction.Right))
					{
						rotation *= Quaternion.Euler(0, 90, 0);
					}
					else if (neighborDirections.Contains(Direction.Up))
					{
						rotation *= Quaternion.Euler(0, 180, 0);
					}
					roadDictionary[candidate.Key] = Tuple.Create(Instantiate(roadEnd, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
				}
				else if (neighborDirections.Count == 2)
				{

					DestroyImmediate(roadDictionary[candidate.Key].Item1);
					if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Down))
					{
						//rotation *= Quaternion.Euler(0, 90, 0);
						roadDictionary[candidate.Key] = Tuple.Create(Instantiate(roadStraight, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
						continue;
					}
					if (neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Left))
					{
                    // roadDictionary[candidate.Key] = Tuple.Create(Instantiate(roadStraight, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
                    rotation *= Quaternion.Euler(0, 90, 0);
                    roadDictionary[candidate.Key] = Tuple.Create(Instantiate(roadStraight, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
						continue;
					}
					if (neighborDirections.Contains(Direction.Up) && neighborDirections.Contains(Direction.Right))
					{
						rotation *= Quaternion.Euler(0, 90, 0);
					}
					else if (neighborDirections.Contains(Direction.Right) && neighborDirections.Contains(Direction.Down))
					{
						rotation *= Quaternion.Euler(0, 180, 0);
					}
					else if (neighborDirections.Contains(Direction.Down) && neighborDirections.Contains(Direction.Left))
					{
						rotation *= Quaternion.Euler(0, -90, 0);
					}
					roadDictionary[candidate.Key] = Tuple.Create(Instantiate(roadCorner, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
				}
				else if (neighborDirections.Count == 3)
				{
					DestroyImmediate(roadDictionary[candidate.Key].Item1);
					if (neighborDirections.Contains(Direction.Right)
						&& neighborDirections.Contains(Direction.Down)
						&& neighborDirections.Contains(Direction.Left)
						)
					{
						rotation *= Quaternion.Euler(0, -90, 0);
					}
					else if (neighborDirections.Contains(Direction.Down)
						&& neighborDirections.Contains(Direction.Right)
						&& neighborDirections.Contains(Direction.Up))
					{
						rotation *= Quaternion.Euler(0, 180, 0);
					}
					else if (neighborDirections.Contains(Direction.Left)
						&& neighborDirections.Contains(Direction.Up)
						&& neighborDirections.Contains(Direction.Right))
					{
						rotation *= Quaternion.Euler(0, 90, 0);
					}
					roadDictionary[candidate.Key] = Tuple.Create(Instantiate(road3way, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
				}
				else if (neighborDirections.Count == 4)
				{
					DestroyImmediate(roadDictionary[candidate.Key].Item1);
					roadDictionary[candidate.Key] = Tuple.Create(Instantiate(road4way, candidate.Key, rotation, transform), roadDictionary[candidate.Key].Item2);
				}
			}
		}

		public void deleteRoads(Vector3 position)
		{
			DestroyImmediate(roadDictionary[position].Item1);
			roadDictionary.Remove(position);
		}

		public void GenerateAssetRoad()
		{
			GameObject[] pointRoads = new GameObject[500];
			int countPointRoads = 0;
			foreach ( var road in roadDictionary)
			{
				// add fourway traffic light
				if(road.Value.Item1.name.Contains("fourway"))
				{
					GameObject fourwayObject = road.Value.Item1;
					AddTrafficLightFourway(fourwayObject);
				}
				if(road.Value.Item1.name.Contains("fourway") || road.Value.Item1.name.Contains("Threeway"))
				{
					if (countPointRoads < pointRoads.Length) // Check if index is within bounds
					{
						pointRoads[countPointRoads] = road.Value.Item1;
						countPointRoads++;
					}
					else
					{
						Debug.LogError("pointRoads array is full. Increase its size or handle this case differently.");
					}
				}
			}
			AddAssetStraightRoad(pointRoads, countPointRoads);
		}

		private void AddTrafficLightFourway(GameObject fourwayObject)
		{
			for (int i = 0; i < 4; i++)
			{	
				var trafficLightGameObject = Instantiate(trafficLight, fourwayObject.transform);
				switch(i)
				{
					case 0: 
						trafficLightGameObject.transform.localPosition = new Vector3(17,0,17);
						trafficLightGameObject.transform.localRotation  = Quaternion.Euler(0, 180, 0);
						break;
					case 1: 
						trafficLightGameObject.transform.localPosition = new Vector3(-17,0,17);
						trafficLightGameObject.transform.localRotation  = Quaternion.Euler(0, 90, 0);
						break;
					case 2: 
						trafficLightGameObject.transform.localPosition = new Vector3(17,0,-17);
						trafficLightGameObject.transform.localRotation  = Quaternion.Euler(0, 270, 0);
						break;
					case 3: 
						trafficLightGameObject.transform.localPosition = new Vector3(-17,0,-17);
						trafficLightGameObject.transform.localRotation  = Quaternion.Euler(0, 0, 0);
						break;
				}
			}
		}

		private void AddAssetStraightRoad(GameObject[] pointRoads, int countPointRoads)
		{
			for (int i = 0; i < countPointRoads; i++)
			{
				var direction = new Vector3(50.00f,0,0);
				AddTrafficLightAndTreeStraightRoad(pointRoads[i], direction);
				direction = new Vector3(0,0,50.00f);
				AddTrafficLightAndTreeStraightRoad(pointRoads[i], direction);
			}
		}

		private void AddTrafficLightAndTreeStraightRoad(GameObject road, Vector3 direction)
		{
			var tempLength = 0;
			var position = road.transform.position + direction;
			var tempEditRoads = new GameObject[100];
			while(roadDictionary.ContainsKey(position))
			{
				var newPosition = position + direction;
				position = newPosition;
				if (roadDictionary.ContainsKey(position))
				{
					if (roadDictionary[position].Item1.name.Contains("straight")) {
						if (roadDictionary[position].Item1.tag == "AddedAssetRoad")
						{
							break;
						}
						else {
							tempEditRoads[tempLength] = roadDictionary[position].Item1;
							tempLength++;
						}
					}
				}
			}
			position = road.transform.position - direction;
			while(roadDictionary.ContainsKey(position))
			{
				var newPosition = position - direction;
				position = newPosition;
				if (roadDictionary.ContainsKey(position))
				{
					if (roadDictionary[position].Item1.name.Contains("straight")) {
						if (roadDictionary[position].Item1.tag == "AddedAssetRoad")
						{
							break;
						}
						else {
							tempEditRoads[tempLength] = roadDictionary[position].Item1;
							tempLength++;
						}
					}
				}
			}
			// If the road long enough add traffic light and tree
			if (tempLength > 10)
			{
				for (int j = 0; j < tempLength; j++)
				{
					if (j % 3 == 0) {
						GameObject lightRoadObject = Instantiate(lightRoad, tempEditRoads[j].transform);
						lightRoadObject.transform.localPosition = new Vector3(13,0,3);
						lightRoadObject.transform.localRotation  = Quaternion.Euler(0, 90, 0);
					}
					GameObject treeRoadObject = Instantiate(treeRoad, tempEditRoads[j].transform);
					treeRoadObject.transform.localPosition = new Vector3(13,0,0);
					GameObject treeRoadObject2 = Instantiate(treeRoad, tempEditRoads[j].transform);
					treeRoadObject2.transform.localPosition = new Vector3(-13,0,0);
					tempEditRoads[j].tag = "AddedAssetRoad";
				}
			}
		}

		public void clearAll()
		{
			roadDictionary = new Dictionary<Vector3, Tuple<GameObject, Vector3>>();
			fixRoadCandidates = new Dictionary<Vector3, Vector3>();
			while (transform.childCount != 0)
			{
				DestroyImmediate(transform.GetChild(0).gameObject);
			}
		}
	}


