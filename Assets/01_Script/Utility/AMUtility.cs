using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagName
{
    Player,
    TopCollider,
    BottomCollider,
    UICamera,
    Lane,
    Trap,
    Block,
    Map,
    Obstacle
}

public static class AMUtility
{
    //Object Key
    public readonly static string TRAP = "Trap";
    public readonly static string ITEM = "Item";
    public readonly static string BLOCK = "Block";

    public static WaitForEndOfFrame m_WaitForEndOfFrame = new WaitForEndOfFrame();
    public static void RandomSeed()
    {
        GameData.m_RadomSeed++;
        if (GameData.m_RadomSeed > 9999) GameData.m_RadomSeed = 1;
        float temp = Time.time * GameData.m_RadomSeed;
        UnityEngine.Random.InitState((int)temp);
    }
}
