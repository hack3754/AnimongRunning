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
}
