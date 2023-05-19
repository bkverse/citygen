using System;
using System.Collections.Generic;
using UnityEngine;


    public static class PlacementHelper
    {
        public static List<Direction> FindNeighbor(Vector3 position, Vector3 direction, ICollection<Vector3> collection)
        {
            float delta = 0.0001f;
            List<Direction> neighborDirections = new List<Direction>();
            Vector3 temp = new Vector3(0, 1, 0);
            Vector3 ans = Vector3.Cross(direction, temp);
            Vector3 up = position + direction;
            Vector3 down = position - direction;
            Vector3 right = position - ans;
            Vector3 left = position + ans;
            foreach (var item in collection)
            {
                if (Math.Abs(item.x - up.x) < delta && Math.Abs(item.y - up.y) < delta && Math.Abs(item.z - up.z) < delta)
                {
                    neighborDirections.Add(Direction.Up);
                }

                if (Math.Abs(item.x - down.x) < delta && Math.Abs(item.y - down.y) < delta && Math.Abs(item.z - down.z) < delta)
                {
                    neighborDirections.Add(Direction.Down);
                }

                if (Math.Abs(item.x - right.x) < delta && Math.Abs(item.y - right.y) < delta && Math.Abs(item.z - right.z) < delta)
                {
                    neighborDirections.Add(Direction.Right);
                }

                if (Math.Abs(item.x - left.x) < delta && Math.Abs(item.y - left.y) < delta && Math.Abs(item.z - left.z) < delta)
                {
                    neighborDirections.Add(Direction.Left);
                }
            }
            return neighborDirections;
        }

        internal static Vector3 GetOffsetFromDirection(Direction direction, Vector3 direct)
        {
            Vector3 temp = new Vector3(0, 1, 0);
            switch (direction)
            {
                case Direction.Left:
                    direct = Vector3.Cross(direct, temp);
                    return direct;
                case Direction.Right:
                    direct = -Vector3.Cross(direct, temp);
                    return direct;
                case Direction.Up:
                    return direct;
                case Direction.Down:
                    return -direct;
                default:
                    break;
            }
            throw new System.Exception("No direction such as " + direction);
        }

        public static Direction GetReverseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    break;
            }
            throw new System.Exception("No direction such as " + direction);
        }
    }


