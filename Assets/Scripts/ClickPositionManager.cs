using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPositionManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject[] points;
    public int i;
    public bool state = false;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // get position click
            Vector3 clickPosition = -Vector3.one;

            Plane plane = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distanceToPlane;

            if (plane.Raycast(ray, out distanceToPlane))
            {
                clickPosition = ray.GetPoint(distanceToPlane);
            }

            // make an object point
            if (i < points.Length)
            {
                points[i] = Instantiate(pointPrefab, clickPosition, Quaternion.identity);
                i++;
            }

            if(i == points.Length)
            {
                state = true;
                i++;
            }
        }
    }
}
