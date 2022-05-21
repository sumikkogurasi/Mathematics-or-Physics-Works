using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGame : MonoBehaviour
{
    private const float CELL_SIZE = 1f; // セルのサイズ

    public int gridSize = 100;　// グリッドの一辺のセルの数
    public GameObject cellPrefab;　// セルのプレハブ

    public Cell[,] cells; // グリッド状のセル

    // Start is called before the first frame update
    void Start()
    {
        // グリッド状にセルを作成
        cells = new Cell[gridSize, gridSize];

        for(int x = 0; x < gridSize; x++)
        {
            for(int z = 0; z < gridSize; z++)
            {
                // セルの作成
                GameObject obj = Instantiate(cellPrefab) as GameObject;
                obj.transform.SetParent(transform);

                // 位置を設定
                float xPos = (x - gridSize * 0.5f) * CELL_SIZE;
                float zPos = (z - gridSize * 0.5f) * CELL_SIZE;
                obj.transform.localPosition = new Vector3(xPos, 0f, zPos);

                // Cellをセット
                cells[x, z] = obj.GetComponent<Cell>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // クリックしたセルの生存/死滅を切り替える
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if(Physics.Raycast(ray, out hit))
            {
                Cell cell = hit.collider.gameObject.transform.parent.GetComponent<Cell>();

                if (cell.Living)
                {
                    cell.Die();
                }
                else
                {
                    cell.Birth();
                }
            }
        }

        // 「S」キーで開始
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(LifeGameCoroutine());
        }

        // 「E」キーで停止
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();
        }
    }

    //ライフゲームの更新コルーチン
    private IEnumerator LifeGameCoroutine()
    {
        while (true)
        {
            // 全てのセルを更新
            for(int x = 0; x < gridSize; x++)
            {
                for(int z = 0; z < gridSize; z++)
                {
                    UpdateCell(x, z);
                }
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    // セルの状態を更新
    private void UpdateCell(int cellX, int cellZ)
    {
        // 周囲の生存セル数
        int count = 0;
        for(int x = cellX - 1; x <= cellX + 1; x++)
        {
            for(int z = cellZ -1; z <= cellZ + 1; z++)
            {
                if(x == cellX && z == cellZ)
                {
                    continue;
                }

                if(cells[(x + gridSize) % gridSize, (z + gridSize) % gridSize].Living)
                {
                    count++;
                }
            }
        }

        // 誕生と死滅
        Cell cell = cells[cellX, cellZ];
        if (cell.Living)
        {
            // 周囲の生存セルが1以下、または４以上の時死滅
            if(count <= 1 || count >= 4)
            {
                cell.Die();
            }
        }
        else
        {
            // 周囲の生存セルが３つのとき誕生
            if(count == 3)
            {
                cell.Birth();
            }
        }
    }
}
