using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapData : ScriptableObject
{
    public int columns = 20, rows = 20;
    public int LastLaneUsed=0;
}