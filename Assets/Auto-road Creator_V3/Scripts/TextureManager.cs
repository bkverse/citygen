using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{

    Texture2D empty, OneLaneStraight13, OneLaneStraight24, OneLaneTurn12, OneLaneTurn23, OneLaneTurn34, OneLaneTurn41, OneLaneThreeWay123, OneLaneThreeWay234, OneLaneThreeWay341, OneLaneThreeWay412, OneLanefourway, onelanebridge, onelanebridge2;
    Texture2D TwoLaneStraight13, TwoLaneStraight24, TwoLaneTurn12, TwoLaneTurn23, TwoLaneTurn34, TwoLaneTurn41, TwoLaneThreeWay123, TwoLaneThreeWay234, TwoLaneThreeWay341, TwoLaneThreeWay412, TwoLanefourway, Twolanebridge, Twolanebridge2;
    Texture2D ThreeLaneStraight13, ThreeLaneStraight24, ThreeLaneTurn12, ThreeLaneTurn23, ThreeLaneTurn34, ThreeLaneTurn41, ThreeLaneThreeWay123, ThreeLaneThreeWay234, ThreeLaneThreeWay341, ThreeLaneThreeWay412, ThreeLanefourway, Threelanebridge, Threelanebridge2;
    Texture2D CustomLaneStraight13, CustomLaneStraight24, CustomLaneTurn12, CustomLaneTurn23, CustomLaneTurn34, CustomLaneTurn41, CustomLaneThreeWay123, CustomLaneThreeWay234, CustomLaneThreeWay341, CustomLaneThreeWay412, CustomLanefourway, Customlanebridge, Customlanebridge2;
    Texture2D SpecialBuildingPlace;

    private void OnEnable()
    {
        LoadResources();
    }
    private void LoadResources()
    {
        empty = Resources.Load("IconTex/Road_0") as Texture2D;
        OneLaneStraight13 = Resources.Load("IconTex/Road_1L13") as Texture2D;
        OneLaneStraight24 = Resources.Load("IconTex/Road_1L24") as Texture2D;
        OneLaneTurn12 = Resources.Load("IconTex/Road_1L12") as Texture2D;
        OneLaneTurn23 = Resources.Load("IconTex/Road_1L23") as Texture2D;
        OneLaneTurn34 = Resources.Load("IconTex/Road_1L34") as Texture2D;
        OneLaneTurn41 = Resources.Load("IconTex/Road_1L14") as Texture2D;
        OneLaneThreeWay123 = Resources.Load("IconTex/Road_1L123") as Texture2D;
        OneLaneThreeWay234 = Resources.Load("IconTex/Road_1L234") as Texture2D;
        OneLaneThreeWay341 = Resources.Load("IconTex/Road_1L134") as Texture2D;
        OneLaneThreeWay412 = Resources.Load("IconTex/Road_1L124") as Texture2D;
        OneLanefourway = Resources.Load("IconTex/Road_1L1234") as Texture2D;
        onelanebridge = Resources.Load("IconTex/Road_1L1234B") as Texture2D;
        onelanebridge2 = Resources.Load("IconTex/Road_1L1234B2") as Texture2D;
        TwoLaneStraight13 = Resources.Load("IconTex/Road_2L13") as Texture2D;
        TwoLaneStraight24 = Resources.Load("IconTex/Road_2L24") as Texture2D;
        TwoLaneTurn12 = Resources.Load("IconTex/Road_2L12") as Texture2D;
        TwoLaneTurn23 = Resources.Load("IconTex/Road_2L23") as Texture2D;
        TwoLaneTurn34 = Resources.Load("IconTex/Road_2L34") as Texture2D;
        TwoLaneTurn41 = Resources.Load("IconTex/Road_2L14") as Texture2D;
        TwoLaneThreeWay123 = Resources.Load("IconTex/Road_2L123") as Texture2D;
        TwoLaneThreeWay234 = Resources.Load("IconTex/Road_2L234") as Texture2D;
        TwoLaneThreeWay341 = Resources.Load("IconTex/Road_2L134") as Texture2D;
        TwoLaneThreeWay412 = Resources.Load("IconTex/Road_2L124") as Texture2D;
        TwoLanefourway = Resources.Load("IconTex/Road_2L1234") as Texture2D;
        Twolanebridge = Resources.Load("IconTex/Road_2L1234B") as Texture2D;
        Twolanebridge2 = Resources.Load("IconTex/Road_2L1234B2") as Texture2D;
        ThreeLaneStraight13 = Resources.Load("IconTex/Road_3L13") as Texture2D;
        ThreeLaneStraight24 = Resources.Load("IconTex/Road_3L24") as Texture2D;
        ThreeLaneTurn12 = Resources.Load("IconTex/Road_3L12") as Texture2D;
        ThreeLaneTurn23 = Resources.Load("IconTex/Road_3L23") as Texture2D;
        ThreeLaneTurn34 = Resources.Load("IconTex/Road_3L34") as Texture2D;
        ThreeLaneTurn41 = Resources.Load("IconTex/Road_3L14") as Texture2D;
        ThreeLaneThreeWay123 = Resources.Load("IconTex/Road_3L123") as Texture2D;
        ThreeLaneThreeWay234 = Resources.Load("IconTex/Road_3L234") as Texture2D;
        ThreeLaneThreeWay341 = Resources.Load("IconTex/Road_3L134") as Texture2D;
        ThreeLaneThreeWay412 = Resources.Load("IconTex/Road_3L124") as Texture2D;
        ThreeLanefourway = Resources.Load("IconTex/Road_3L1234") as Texture2D;
        Threelanebridge = Resources.Load("IconTex/Road_3L1234B") as Texture2D;
        Threelanebridge2 = Resources.Load("IconTex/Road_3L1234B2") as Texture2D;

        CustomLaneStraight13 = Resources.Load("IconTex/Road_4L13") as Texture2D;
        CustomLaneStraight24 = Resources.Load("IconTex/Road_4L24") as Texture2D;
        CustomLaneTurn12 = Resources.Load("IconTex/Road_4L12") as Texture2D;
        CustomLaneTurn23 = Resources.Load("IconTex/Road_4L23") as Texture2D;
        CustomLaneTurn34 = Resources.Load("IconTex/Road_4L34") as Texture2D;
        CustomLaneTurn41 = Resources.Load("IconTex/Road_4L14") as Texture2D;
        CustomLaneThreeWay123 = Resources.Load("IconTex/Road_4L123") as Texture2D;
        CustomLaneThreeWay234 = Resources.Load("IconTex/Road_4L234") as Texture2D;
        CustomLaneThreeWay341 = Resources.Load("IconTex/Road_4L134") as Texture2D;
        CustomLaneThreeWay412 = Resources.Load("IconTex/Road_4L124") as Texture2D;
        CustomLanefourway = Resources.Load("IconTex/Road_4L1234") as Texture2D;
        Customlanebridge = Resources.Load("IconTex/Road_4L1234B") as Texture2D;
        Customlanebridge2 = Resources.Load("IconTex/Road_4L1234B2") as Texture2D;

        SpecialBuildingPlace = Resources.Load("IconTex/SpecialPlace") as Texture2D;
    }
    public Texture2D GetTextureByID(int id)
    {
        if (empty == null)
        {
            LoadResources();

        }

        switch (id)
        {
            case 0:
                return empty;
            case 101:
                return OneLaneStraight13;
            case 102:
                return OneLaneStraight24;
            case 103:
                return OneLaneTurn12;
            case 104:
                return OneLaneTurn23;
            case 105:
                return OneLaneTurn34;
            case 106:
                return OneLaneTurn41;
            case 107:
                return OneLaneThreeWay123;
            case 108:
                return OneLaneThreeWay234;
            case 109:
                return OneLaneThreeWay341;
            case 110:
                return OneLaneThreeWay412;
            case 111:
                return OneLanefourway;
            case 113:
                return onelanebridge;
            case 112:
                return onelanebridge2;
            case 201:
                return TwoLaneStraight13;
            case 202:
                return TwoLaneStraight24;
            case 203:
                return TwoLaneTurn12;
            case 204:
                return TwoLaneTurn23;
            case 205:
                return TwoLaneTurn34;
            case 206:
                return TwoLaneTurn41;
            case 207:
                return TwoLaneThreeWay123;
            case 208:
                return TwoLaneThreeWay234;
            case 209:
                return TwoLaneThreeWay341;
            case 210:
                return TwoLaneThreeWay412;
            case 211:
                return TwoLanefourway;
            case 213:
                return Twolanebridge;
            case 212:
                return Twolanebridge2;
            case 301:
                return ThreeLaneStraight13;
            case 302:
                return ThreeLaneStraight24;
            case 303:
                return ThreeLaneTurn12;
            case 304:
                return ThreeLaneTurn23;
            case 305:
                return ThreeLaneTurn34;
            case 306:
                return ThreeLaneTurn41;
            case 307:
                return ThreeLaneThreeWay123;
            case 308:
                return ThreeLaneThreeWay234;
            case 309:
                return ThreeLaneThreeWay341;
            case 310:
                return ThreeLaneThreeWay412;
            case 311:
                return ThreeLanefourway;
            case 313:
                return Threelanebridge;
            case 312:
                return Threelanebridge2;
            case 401:
                return CustomLaneStraight13;
            case 402:
                return CustomLaneStraight24;
            case 403:
                return CustomLaneTurn12;
            case 404:
                return CustomLaneTurn23;
            case 405:
                return CustomLaneTurn34;
            case 406:
                return CustomLaneTurn41;
            case 407:
                return CustomLaneThreeWay123;
            case 408:
                return CustomLaneThreeWay234;
            case 409:
                return CustomLaneThreeWay341;
            case 410:
                return CustomLaneThreeWay412;
            case 411:
                return CustomLanefourway;
            case 413:
                return Customlanebridge;
            case 412:
                return Customlanebridge2;
            case 501:
                return SpecialBuildingPlace;
        }
        return empty;
    }
}
