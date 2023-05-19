using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("MyTag");

        if (objectsWithTag.Length > 0)
        {
            Debug.Log("Found " + objectsWithTag.Length + " objects with tag MyTag");
        }
        else
        {
            Debug.Log("No objects with tag MyTag were found");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
