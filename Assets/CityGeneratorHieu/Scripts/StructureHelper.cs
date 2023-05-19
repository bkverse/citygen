using System;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

	public class StructureHelper : MonoBehaviour
	{
		public BuildingDataType[] buildingTypes;
		public GameObject[] naturePrefabs;
		public bool randomNaturePlacement = false;
		[Range(0, 1)]
		public float randomNaturePlacementThreshold = 0.3f;
		public Dictionary<Vector3, GameObject> structuresDictionary = new Dictionary<Vector3, GameObject>();
		public Dictionary<Vector3, GameObject> natureDictionary = new Dictionary<Vector3, GameObject>();
    [System.Serializable]
		public class spacesInsideClass
		{
			public int spaceX = 0;
			public int spaceY = 0;
			public Vector3 position;
		}
		public List<spacesInsideClass> spacesInside = new List<spacesInsideClass>();
		public bool findFreeSpaces(Vector3 position, Dictionary<Vector3, Tuple<Direction, Vector3>> freeSpaces)
		{
			float delta = 0.0001f;
			foreach (var item in freeSpaces.Keys)
			{
				if (Math.Abs(item.x - position.x) < delta && Math.Abs(item.y - position.y) < delta && Math.Abs(item.z - position.z) < delta)
				{
					return true;
				}
			}
			return false;
		}

		public bool findFreeEstateSpots(Vector3 position, List<Vector3> Spots)
		{
			float delta = 0.0001f;
			foreach (var item in Spots)
			{
				if (Math.Abs(item.x - position.x) < delta && Math.Abs(item.y - position.y) < delta && Math.Abs(item.z - position.z) < delta)
				{
					return true;
				}
			}
			return false;
		}

		public void PlaceStructuresAroundRoad(Dictionary<Vector3, Vector3> roadPositions)
		{
			// freeEstateSpots contain:
			// + position of it
			// + info about road it face (only one)
			Dictionary<Vector3, Tuple<Direction, Vector3>> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
			List<Vector3> blockedPositions = new List<Vector3>();
			foreach (var freeSpot in freeEstateSpots)
			{
				// placed position
				if (findFreeEstateSpots(freeSpot.Key, blockedPositions))
				{
					continue;
				}
				var rotation = Quaternion.LookRotation(freeSpot.Value.Item2, Vector3.up);

				switch (freeSpot.Value.Item1)
				{
					case Direction.Down:
						rotation *= Quaternion.Euler(0, 90, 0);
						break;
					case Direction.Up:
						rotation *= Quaternion.Euler(0, -90, 0);
						break;
					case Direction.Left:
						rotation *= Quaternion.Euler(0, 180, 0);
						break;
					default:
						break;
				}
				int buildingIndex = Random.Range(0,buildingTypes.Length);
				if (buildingTypes[buildingIndex].quantity != -1)
				{
					if (!buildingTypes[buildingIndex].IsBuildingAvailable())
					{
						continue;
					}
				}
				List<Vector3> tempPositionsBlocked = new List<Vector3>();
				if (VerifyIfBuildingFits(buildingTypes[buildingIndex].sizeRequired, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
				{
					blockedPositions.AddRange(tempPositionsBlocked);
					var halfSize = Mathf.FloorToInt(buildingTypes[buildingIndex].sizeRequired / 2.0f);
					Vector3 direction = freeSpot.Value.Item2;
					if (freeSpot.Value.Item1 == Direction.Down || freeSpot.Value.Item1 == Direction.Up)
					{
						Vector3 temp = new Vector3(0, 1, 0);
						direction = Vector3.Cross(direction, temp);
					}
					else
					{
						direction = freeSpot.Value.Item2;
					}
					var pos1 = freeSpot.Key + direction * halfSize;
					var pos2 = freeSpot.Key - direction * halfSize;
					var placePosition =  freeSpot.Key;
					// TODO: check nearby building if it same building type then get another prefab
					if ( buildingTypes[buildingIndex].sizeRequired % 2 == 0 )
					{
						if (tempPositionsBlocked.Contains(pos1)) {
							placePosition = freeSpot.Key + direction/2;
						} else if (tempPositionsBlocked.Contains(pos2)) {
							placePosition = freeSpot.Key - direction/2;
						}
					}
					GameObject building = Instantiate(buildingTypes[buildingIndex].GetPrefab(), placePosition, rotation, transform);
					structuresDictionary.Add(freeSpot.Key, building);
					foreach (var pos in tempPositionsBlocked)
					{
						structuresDictionary.Add(pos, building);
					}
				}
			}
				// for (int i = 0; i < buildingTypes.Length; i++)
				// {
				// 	if (buildingTypes[i].quantity == -1)
				// 	{
				// 		if (randomNaturePlacement)
				// 		{
				// 			var random = UnityEngine.Random.value;
				// 			if (random < randomNaturePlacementThreshold)
				// 			{
				// 				var nature = SpawnPrefab(naturePrefabs[UnityEngine.Random.Range(0, naturePrefabs.Length)], freeSpot.Key, rotation);
				// 				natureDictionary.Add(freeSpot.Key, nature);
				// 				break;
				// 			}
				// 		}
				// 		var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
				// 		structuresDictionary.Add(freeSpot.Key, building);
				// 		break;
				// 	}
				// 	if (buildingTypes[i].IsBuildingAvailable())
				// 	{
				// 		if (buildingTypes[i].sizeRequired > 1)
				// 		{
				// 			var halfSize = Mathf.FloorToInt(buildingTypes[i].sizeRequired / 2.0f);

				// 			List<Vector3> tempPositionsBlocked = new List<Vector3>();
				// 			if (VerifyIfBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
				// 			{
				// 				blockedPositions.AddRange(tempPositionsBlocked);
				// 				var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
				// 				building.transform.localScale  = new Vector3(1,1,1) * buildingTypes[i].sizeRequired;
				// 				structuresDictionary.Add(freeSpot.Key, building);
				// 				foreach (var pos in tempPositionsBlocked)
				// 				{
				// 					structuresDictionary.Add(pos, building);
				// 				}
				// 				break;
				// 			}
				// 		}
				// 		else
				// 		{
				// 			var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
				// 			structuresDictionary.Add(freeSpot.Key, building);
				// 		}
				// 		break;
				// 	}
				// }
		}

		private bool VerifyIfBuildingFits(
			int sizeRequired,
			Dictionary<Vector3, Tuple<Direction, Vector3>> freeEstateSpots,
			KeyValuePair<Vector3, Tuple<Direction, Vector3>> freeSpot,
			List<Vector3> blockedPositions,
			ref List<Vector3> tempPositionsBlocked)
		{
			if (sizeRequired > 1)
			{
				var halfSize = Mathf.FloorToInt(sizeRequired / 2.0f);
				Vector3 direction = freeSpot.Value.Item2;
				if (freeSpot.Value.Item1 == Direction.Down || freeSpot.Value.Item1 == Direction.Up)
				{	
					Vector3 temp = new Vector3(0, 1, 0);
					direction = Vector3.Cross(direction, temp);
				}
				else
				{
					direction = freeSpot.Value.Item2;
				}
				for (int i = 1; i <= halfSize; i++)
				{
					var pos1 = freeSpot.Key + direction * i;
					var pos2 = freeSpot.Key - direction * i;
					var keys = new List<Vector3>(freeEstateSpots.Keys);
					if ( sizeRequired % 2 == 0 && i == halfSize ) {
						if (!findFreeEstateSpots(pos1, keys) && findFreeEstateSpots(pos2, keys) && !findFreeEstateSpots(pos2, blockedPositions))
						{
							tempPositionsBlocked.Add(pos2);
							return true;
						} else if (findFreeEstateSpots(pos1, keys) && !findFreeEstateSpots(pos2, keys) && !findFreeEstateSpots(pos1, blockedPositions))
						{
							tempPositionsBlocked.Add(pos1);
							return true;
						} else return false;
					}
					if (!findFreeEstateSpots(pos1, keys) || !findFreeEstateSpots(pos2, keys) || findFreeEstateSpots(pos1, blockedPositions) || findFreeEstateSpots(pos2, blockedPositions))
					{
						return false;
					}
					tempPositionsBlocked.Add(pos1);
					tempPositionsBlocked.Add(pos2);
				}
				return true;
			} else return true;
		}

		public void PlaceRandomStructuresInside(int MapSize)
		{
			// Get list of free spaces inside
			int currentX = 2;
			int currentY = 2;
			while (currentY <= MapSize-1) {
				spacesInsideClass space = GetSpacesInside(currentX, currentY, MapSize);
				if (space.spaceX != 1)
					currentX = currentX + space.spaceX - 1;
				if (space.spaceY != 1)
					currentY = currentY + space.spaceY - 1;
				if (currentX == MapSize-1) {
					currentX = 2;
					currentY++;
				}
				spacesInside.Add(space);
			}
			
			for(int i = 0; i < spacesInside.Count(); i++) {
				spaceArea = spacesInside[i].spaceX * spacesInside[i].spaceY;
				if (spaceArea > 25) {
					AddTerrain(spacesInside[i]);
				}
				else if (spaceArea < 5) {
					continue;
				}
				else {
					AddRandomPrefab();
				}
			}
		}

		public spacesInsideClass GetSpacesInside(int currentX, int currentY, int MapSize) {
			int x = currentX;
			int y = currentY;
			bool xBlocked = false;
			bool yBlocked = false;
			Vector3 currentPosition = new Vector3(currentX*50, 0, currentY*50);
			while (x <= MapSize-1 || y <= MapSize-1) {
				Vector3 position = new Vector3(x*50, 0, y*50);
				if (roadPositions.ContainsKey(position) || structuresDictionary.ContainsKey(position)) {
					if (x == currentX && y == currentY)
						break;
					if (x > currentX && !xBlocked) {
						x--;
						xBlocked = true;
					}
					if (y > currentY && !yBlocked) {
						y--;
						yBlocked = true;
					}
				}
				if (xBlocked && yBlocked) {
					spacesInsideClass temp = new SpacesInsideClass();
					temp.position = currentPosition;
					if (x == currentX) 
						temp.spaceX = 1;
					else 
						temp.spaceX = x - currentX;
					if (y == currentY) 
						temp.spaceY = 1;
					else 
						temp.spaceY = y - currentY;
					return temp;
				} else if (xBlocked && !yBlocked) {
					y++;
				} else if (!xBlocked && yBlocked) {
					x++;
				} else {
					if (x <= y)
						x++; 
					else 
						y++;
				}
			}
		}

		private void AddTerrain(spacesInsideClass space) {
			Debug.Log("Add terrain");
			// TODO: add terrain
		}

		private void AddRandomPrefab(spacesInsideClass space) {
			Debug.Log("Add random prefab");
			// TODO: add random prefab included: park, parking lot, random house, etc
		}

		// private GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
		// {
		// 	var newStructure = Instantiate(prefab, position, rotation, transform);
		// 	// newStructure.AddComponent<FallTween>();
		// 	return newStructure;
		// }

		private Dictionary<Vector3, Tuple<Direction, Vector3>> FindFreeSpacesAroundRoad(Dictionary<Vector3, Vector3> roadPositions)
		{
			Dictionary<Vector3, Tuple<Direction, Vector3>> freeSpaces = new Dictionary<Vector3, Tuple<Direction, Vector3>>();
			foreach (var positionOfRoad in roadPositions)
			{
				var neighborDirections = PlacementHelper.FindNeighbor(positionOfRoad.Key, positionOfRoad.Value, roadPositions.Keys);
				foreach (Direction direction in Enum.GetValues(typeof(Direction)))
				{
					if (neighborDirections.Contains(direction) == false)
					{
						var newFreePosition = positionOfRoad.Key + PlacementHelper.GetOffsetFromDirection(direction, positionOfRoad.Value);
						if (findFreeSpaces(newFreePosition, freeSpaces))
						{
							// already contains in dictionary
							continue;
						}
						freeSpaces.Add(newFreePosition, Tuple.Create(direction, positionOfRoad.Value));
					}
				}
			}
			return freeSpaces;
		}

		public void clearAll()
		{
			for (int i = 0; i < buildingTypes.Length; i++)
			{
				buildingTypes[i].Reset();
			}
			structuresDictionary = new Dictionary<Vector3, GameObject>();
			natureDictionary = new Dictionary<Vector3, GameObject>();
			while (transform.childCount != 0)
			{
				DestroyImmediate(transform.GetChild(0).gameObject);
			}
		}

	}


