using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridBox : ScriptableObject
{
    public int co, ro;
    public int HelperInt;
    public bool IsOn;
    public Rect rect;
    GUIStyle style;
    public int textID;
    
    public GridBox()
    {
    } 
    public GridBox(Vector2 Pos,Texture2D texxx, int TextureId,int Co,int Ro)
    {
        rect = new Rect(Pos.x, Pos.y, 30, 30);
        Texture2D empty = Resources.Load("Road_0") as Texture2D;
        style = new GUIStyle();
        style.normal.background =texxx;
        textID = TextureId;
        IsOn = (TextureId!=0);
        co = Co;
        co = Ro;
    }   

    public void Drag(Vector2 Delta)
    {
        rect.position += Delta;
    }
    public void Draw()
    {
        GUI.Box(rect, "", style);
    }
    public bool ProcessEvent(Event E)
    {
        switch (E.type)
        {
            case EventType.MouseDown:
                if (E.button == 0)
                {
                    if (rect.Contains(E.mousePosition))
                    {
                        HelperInt = 1;
                        GUI.changed = true;
                        return true;
                    }
                }
                return false;
            case EventType.MouseDrag:
                if (E.button == 0)
                {
                    if (rect.Contains(E.mousePosition))
                    {
                        GUI.changed = true;
                        E.Use();
                        return true;
                    }
                }
                return false;
            case EventType.MouseUp:
                HelperInt = 0;
                break;
        }
        return false;
    }
    public void UpdateTexture(Texture2D Tex,int Id)
    {
        style.normal.background = Tex;
        textID = Id;
    }
}
