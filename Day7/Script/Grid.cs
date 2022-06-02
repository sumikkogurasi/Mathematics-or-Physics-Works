using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// GridSystem 2022/06/01 えびふらいちゃん！
/// </summary>
public class Grid : MonoBehaviour
{
    // プレイヤーのトランスフォーム
    //public Transform player;

    // 障害物のレイヤー
    public LayerMask unwalkableMask;

    // ワールドサイズ
    public Vector2 gridWorldSize;

    // ノード半径
    public float nodeRadius;

    // 二次元座標(x, z)
    Node[,] grid;

    // ノード直径
    float nodeDiameter;

    // グリッドサイズ（スカラー、整数）
    int gridSizeX, gridSizeY;

    private void Start()
    {
        // ノード直径
        nodeDiameter = nodeRadius * 2;

        // グリッド数（X）　＝　ワールドサイズ（X）/ ノード直径
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);

        // グリッド数（Y）　＝　ワールドサイズ（Y）/ ノード直径
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // グリッド生成
        CreateGrid();

    }

    // グリッド生成
    void CreateGrid()
    {
        // グリッドの２次元配列の大きさを決定
        grid = new Node[gridSizeX, gridSizeY];

        // グリッドの左下を原点に設定
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; 

        // (0, 0)から(x-1, y-1)までの座標を計算して２次元配列に代入
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // 正のX軸がright、正のZ軸がforward
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                
                // worldPointからノード半径までの間に当たり判定が在ったらtrueを返して、その対のfalseをwalkable変数に代入する
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // 各二次元配列にNodeメソッドの戻り値を代入
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // 周囲８マスのノードの状態をOpenリストとCloseリストに代入する
    public List<Node> GetNeighbours(Node node)
    {
        // 周辺リスト
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                // 中心のマスはスキップ
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // グリッドサイズの範囲内だけ、範囲の外は周辺リストと見做さない
                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // 周辺リストに座標（checkX, checkY）を加える
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    // playerの位置をグリッドに代入
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // unityのワールド原点が0.5になるように割合を設定
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        // 値を0と1の間に制限する
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // グリッドの外に出さないために1を引く
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> path;

    // スクリプトがゲームオブジェクトに設定されている間呼ばれるメソッド
    private void OnDrawGizmos()
    {
        // Gizmosでグリッドサイズを可視化（但しy軸は三次元ではz軸とする）
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            // playerのワールド位置をグリッド座標に変換し、それをNode型の変数に代入
            //Node playerNode = NodeFromWorldPoint(player.position);

            // 配列内の要素を全て参照
            foreach(Node n in grid)
            {
                // walkable変数が真の時白色のキューブを描画、偽ならば赤色のキューブを描画
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                // 経路の描画
                if(path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                // playerがいるノードの色をシアンにする
                //if(playerNode == n)
                //{
                //    Gizmos.color = Color.cyan;
                //}

                // 配列nのワールド位置からノード直径の大きさのキューブを描画
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
