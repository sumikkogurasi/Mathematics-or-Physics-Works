using System.Collections;
using UnityEngine;

/// <summary>
/// Node 2022/06/01 えびふらいちゃん！
/// </summary>
public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;
    // コンストラクタ
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // コストFの計算　引数なしで値を得る
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
