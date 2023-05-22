using System;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
	public class StructureHelper : MonoBehaviour
	{
		public BuildingDataType[] buildingTypes;
		public BuildingDataType[] naturePrefabs;
		public bool randomNaturePlacement = false;
		[Range(0, 1)]
		public float randomNaturePlacementThreshold = 0.3f;
		public Dictionary<Vector3, GameObject> structuresDictionary = new Dictionary<Vector3, GameObject>();
		public Dictionary<Vector3, GameObject> natureDictionary = new Dictionary<Vector3, GameObject>();
		public class spacesInsideClass
		{
			public int spaceX;
			public int spaceY;
			public Vector3 position;

			public spacesInsideClass()
			{
				spaceX = 0;
				spaceY = 0;
				position = new Vector3(0,0,0);
			}
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

		public void PlaceStructuresAroundRoad(Dictionary<Vector3, Tuple<GameObject, Vector3>> roadPositions)
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
				if (structuresDictionary.ContainsKey(freeSpot.Key) || roadPositions.ContainsKey(freeSpot.Key))
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
				if (VerifyIfBuildingFits(buildingTypes[buildingIndex].sizeRequiredX, buildingTypes[buildingIndex].sizeRequiredY, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
				{
					blockedPositions.Add(freeSpot.Key);
					blockedPositions.AddRange(tempPositionsBlocked);
					var halfSize = Mathf.FloorToInt(buildingTypes[buildingIndex].sizeRequiredX / 2.0f);
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
					if ( buildingTypes[buildingIndex].sizeRequiredX % 2 == 0 )
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
		}

		private bool VerifyIfBuildingFits(
			int sizeRequiredX,
			int sizeRequiredY,
			Dictionary<Vector3, Tuple<Direction, Vector3>> freeEstateSpots,
			KeyValuePair<Vector3, Tuple<Direction, Vector3>> freeSpot,
			List<Vector3> blockedPositions,
			ref List<Vector3> tempPositionsBlocked)
		{
			if (sizeRequiredX > 1 || sizeRequiredY > 1)
			{
				var halfSize = Mathf.FloorToInt(sizeRequiredX / 2.0f);
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
				int countY = 1;
				while (countY <= sizeRequiredY)
				{
					for (int i = 1; i <= halfSize; i++)
					{
						var pos1 = freeSpot.Key + direction * i;
						var pos2 = freeSpot.Key - direction * i;
						var keys = new List<Vector3>(freeEstateSpots.Keys);
						if ( sizeRequiredX % 2 == 0 && i == halfSize ) {
							if (findFreeEstateSpots(pos2, keys) && !findFreeEstateSpots(pos2, blockedPositions) && !structuresDictionary.ContainsKey(pos2))
							{
								tempPositionsBlocked.Add(pos2);
								return true;
							} else if (findFreeEstateSpots(pos1, keys) && !findFreeEstateSpots(pos1, blockedPositions) && !structuresDictionary.ContainsKey(pos1))
							{
								tempPositionsBlocked.Add(pos1);
								return true;
							} else return false;
						}
						if (!findFreeEstateSpots(pos1, keys) || !findFreeEstateSpots(pos2, keys) || findFreeEstateSpots(pos1, blockedPositions) || findFreeEstateSpots(pos2, blockedPositions) || !structuresDictionary.ContainsKey(pos1) || !structuresDictionary.ContainsKey(pos2))
						{
							return false;
						}
						tempPositionsBlocked.Add(pos1);
						tempPositionsBlocked.Add(pos2);
					}
					countY++;
				}
				return true;
			} else return true;
		}

		public void PlaceRandomStructuresInside(int MapSize, Dictionary<Vector3, Vector3> roadPositions)
		{
			// Get list of free spaces inside
			int currentX = 1;
			int currentY = 1;
			int count = 0;
			int[,] visitedPlace = new int[MapSize,MapSize];
			for (int x = 0; x < MapSize; x++)
			{
				visitedPlace[x,0] = 1;
				visitedPlace[x,MapSize-1] = 1;
				visitedPlace[0,x] = 1;
				visitedPlace[MapSize-1,x] = 1;
			}
			while (!checkAllVisited(MapSize, visitedPlace)) {
				if (count == 20) {
					for (int i = 0; i < MapSize; i++)
					{
						for (int j = 0; j < MapSize; j++)
						{
							visitedPlace[i, j] = 1;
						}
					}
				}
				var position = new Vector3(currentX*50, 0, currentY*50);
				spacesInsideClass space = GetSpacesInside(currentX, currentY, MapSize, roadPositions, visitedPlace);
				for (int x = currentX; x < (space.spaceX + currentX); x++) {
					for (int y = currentY; y < (space.spaceY + currentY); y++) {
						visitedPlace[x,y] = 1;
					}
				}
				spacesInside.Add(space);
				for (int y = 1; y < MapSize; y++)
        {
					bool breakLoops = false;
					for (int x = 1; x < MapSize; x++)
					{
						if (visitedPlace[x, y] == 0)
						{
							position = new Vector3(x*50, 0, y*50);
							if (roadPositions.ContainsKey(position) || structuresDictionary.ContainsKey(position)) {
								visitedPlace[x, y] = 1;
								continue;
							}
							currentX = x;
							currentY = y;
							count ++;
							breakLoops = true;
							break;
						}
					}
					if (breakLoops)
					{
						break;
					}
        }
			}

			for(int i = 0; i < spacesInside.Count; i++) {
				int spaceArea = spacesInside[i].spaceX * spacesInside[i].spaceY;
				if (spaceArea > 100) {
					AddTerrain(spacesInside[i]);
				}
				else if (spaceArea < 25) {
					continue;
				}
				else {
					AddRandomPrefab(spacesInside[i]);
				}
			}
		}

		public bool checkAllVisited (int MapSize, int [,] visitedPlace) {
			for (int i = 0; i < MapSize; i++)
			{
				for (int j = 0; j < MapSize; j++)
				{
					if (visitedPlace[i, j] != 1)
					{
						return false;
					}
				}
			}
			return true;
		}

		public spacesInsideClass GetSpacesInside(int currentX, int currentY, int MapSize, Dictionary<Vector3, Vector3> roadPositions, int [,] visitedPlace) {
			bool xBlocked = false;
			bool yBlocked = false;
			spacesInsideClass temp = new spacesInsideClass();
			temp.position = new Vector3(currentX*50, 0, currentY*50);
			var rotation = Quaternion.Euler(0, 0, 0);
			for (int x = currentX; x < MapSize; x++) {
				for (int y = currentY; y < MapSize; y++) {
					if (xBlocked && yBlocked)
					{
						return temp;
					}
					Vector3 position = new Vector3(x*50, 0, y*50);
					if (yBlocked && temp.spaceY == (y - currentY)) {
						break;
					}
					if (roadPositions.ContainsKey(position) || structuresDictionary.ContainsKey(position) || visitedPlace[x,y] == 1) {
						if ((x > currentX && !xBlocked) || x == MapSize-1) {
							temp.spaceX = x - currentX;
							xBlocked = true;
							break;
						}
						if ((y > currentY && !yBlocked) || y == MapSize-1) {
							temp.spaceY = y - currentY;
							yBlocked = true;
							break;
						}
					}
				}
			}
			if (xBlocked && yBlocked)
			{
				return temp;
			}
			return new spacesInsideClass();
		}

		private void AddTerrain(spacesInsideClass space) {
			// TODO: add terrain
		}

		private void AddRandomPrefab(spacesInsideClass space) {
			int area = space.spaceX * space.spaceY;
			int placedArea = 0;
			int count = 0;
			List<Vector3> blockedPositions = new List<Vector3>();
			if (space.spaceX >= 3 && space.spaceY >= 3) {
				while (placedArea < area/2 && count < 10)
				{
					count++;
					int natureIndex = Random.Range(0,naturePrefabs.Length);
					int placeX = Random.Range(2, space.spaceX - 2);
					int placeY = Random.Range(2, space.spaceY - 2);
					Vector3 placePosition = new Vector3(placeX*50, 0, placeY*50) + space.position;
					if (structuresDictionary.ContainsKey(placePosition))
					{
						continue;
					}
					var rotation = Quaternion.Euler(0, 0, 0);
					GameObject building = Instantiate(naturePrefabs[natureIndex].GetPrefab(), placePosition, rotation, transform);
					structuresDictionary.Add(placePosition, building);
					placedArea = placedArea + naturePrefabs[natureIndex].sizeRequiredX * naturePrefabs[natureIndex].sizeRequiredY;
					// TODO: check if have sth in place position
				}
			}
		}

		private Dictionary<Vector3, Tuple<Direction, Vector3>> FindFreeSpacesAroundRoad(Dictionary<Vector3, Tuple<GameObject, Vector3>> roadPositions)
		{
			Dictionary<Vector3, Tuple<Direction, Vector3>> freeSpaces = new Dictionary<Vector3, Tuple<Direction, Vector3>>();
			foreach (var positionOfRoad in roadPositions)
			{
				if (positionOfRoad.Value.Item1.tag == "borderRoad") {
					continue;
				}
				var neighborDirections = PlacementHelper.FindNeighbor(positionOfRoad.Key, positionOfRoad.Value.Item2, roadPositions.Keys);
				foreach (Direction direction in Enum.GetValues(typeof(Direction)))
				{
					if (neighborDirections.Contains(direction) == false)
					{
						var newFreePosition = positionOfRoad.Key + PlacementHelper.GetOffsetFromDirection(direction, positionOfRoad.Value.Item2);
						if (findFreeSpaces(newFreePosition, freeSpaces))
						{
							continue;
						}
						freeSpaces.Add(newFreePosition, Tuple.Create(direction, positionOfRoad.Value.Item2));
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


