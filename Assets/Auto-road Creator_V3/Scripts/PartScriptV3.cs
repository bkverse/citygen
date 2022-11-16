using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartScriptV3 : MonoBehaviour
{
    [HideInInspector]
    public string Name;
    [HideInInspector]
    public int I, J;

    public void Set(string nam, int II, int JJ)
    {
        Name = nam;
        I = II;
        J = JJ;
    }
    public void SetName(string nam)
    {
        Name = nam;
    }
    public string GetName()
    {
        return Name;
    }
    public int GetI()
    {
        return I;
    }
    public int GetJ()
    {
        return J;
    }
    public void setIJ(int ii ,int jj)
    {
        I = ii;
        J = jj;
    }
}