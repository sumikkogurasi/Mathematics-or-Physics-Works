using System.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// PathfindingSystem 2022/06/01 えびふらいちゃん！
/// -------------------------------------------------------------------
/// [説明]
/// A*アルゴリズムはOpenとClosedの２つの状態遷移が存在する。
/// １マス移動するごとに必要なコストを割り当てて計算する。
/// 中心マスから周囲８マスへの移動ができるとして、上下左右への移動はコスト１０、斜め移動をコスト１４とする。
/// （これは正方形の辺と対角線の長さの比を１０倍にして小数点以降切り捨てた値である。）
/// 開始位置から目標位置までのコストを実コストとし、Gとする。
/// 目標位置から開始位置までのコストを発見コストとし、Hとする。
/// 実コストの発見コストの和をFとする。
/// これら３つの値をそれぞれのグリッド座標に割り当てていく。
/// 開始位置から周囲８マスの３つの値を参照してOpenにし、F＞Hの順番に小さい値を優先的にCloseする。
/// </summary>
public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();

    }

    private void Update()
    {
        // 捜索者と目的地の経路を探す
        FindPath(seeker.position, target.position);
    }

    // 引数開始位置と目標位置の間の経路探索メソッド
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // 開始位置のグリッド座標をnode型変数に代入
        Node startNode = grid.NodeFromWorldPoint(startPos);

        // 目標位置のグリッド座標をnode型変数に代入
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Openな状態であるグリッドをリストにまとめる
        List<Node> openSet = new List<Node>();

        // Closeな状態であるグリッドをリストにまとめる
        HashSet<Node> closedSet = new HashSet<Node>();

        // 開始位置の座標をはじめにOpenな状態のリストに入れる
        openSet.Add(startNode);

        // Openな状態にできるグリッド座標がある間ループする
        while(openSet.Count > 0)
        {
            // playerがいる位置を現在ノードとする
            Node currentNode = openSet[0];
            
            // 全てのOpenな状態にあるノードを調べる　（効率化の余地あり）
            for(int i = 1; i < openSet.Count; i++)
            {
                // 現在ノードよりコストFが小さかった場合、またはコストFが同じ場合、かつコストHが小さかった場合
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    // 現在ノードをよりコストFの値が小さいノードに変更する
                    currentNode = openSet[i];
                }
            }

            // 現在ノードをOpenな状態なノードのリストから外す
            openSet.Remove(currentNode);

            // 現在ノードをCloseな状態なノードのリストに加える
            closedSet.Add(currentNode);

            // 現在ノードが目標ノードならこれより下の命令は飛ばす
            if(currentNode == targetNode)
            {
                // 開始ノードから目標ノードまでのパスを再計算するメソッド
                RetracePath(startNode, targetNode);
                return;
            }

            // 現在ノードの周辺リストの要素全てを調べる
            foreach(Node neighbour in grid.GetNeighbours(currentNode))
            {
                // 周辺ノードの要素に進めない、あるいはその要素がCloseな状態の場合foreach文の次の要素の処理に飛ぶ
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                // 周辺ノードの移動コスト = 現在のGコスト + 現在位置から周辺ノードの距離
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                // 周辺ノードの移動コストが周辺のGコスト未満、或いはOpenな状態なノードが周辺にない場合
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // 周辺ノードのGコストの計算
                    neighbour.gCost = newMovementCostToNeighbour;
                    
                    // 周辺ノードのHコストは周辺ノードから目標ノードまでの距離
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    
                    // 周辺ノードの親（現在位置）
                    neighbour.parent = currentNode;

                    // 周辺ノードにOpenな状態のノードがない場合
                    if (!openSet.Contains(neighbour))
                    {
                        // 周辺ノードを加える
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    // パスの再計算
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            // リストに現在ノードを追加
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }

        // リストの中身を反転
        path.Reverse();

        // リストをpublic用の経路に代入
        grid.path = path;

    }

    // ２つのノード間の距離
    int GetDistance(Node nodeA, Node nodeB)
    {
        // ２つのノード間のXの差分の絶対値
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        
        // ２つのノード間のYの差分の絶対値
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // Xの差分の大きさがYの差分より大きい場合
        if (dstX > dstY)
            // 14y + 10(x - y)
            return 14 * dstY + 10 * (dstX - dstY);
        // 14x + 10(y - x)
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
