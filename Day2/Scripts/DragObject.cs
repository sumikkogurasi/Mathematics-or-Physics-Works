using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    // 差分保管変数
    private Vector3 mOffset;

    // マウスの奥行座標
    private float mZCoord;

    // オブジェクト上でマウス左クリックした時
    void OnMouseDown()
    {
        // カメラの位置の前方(z軸)からオブジェクトまでの距離（スカラー）
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // store offset = gameobject world pos - mouse world pos;
        // オブジェクトの原点とオブジェクトを掴んだマウスの位置の差
        mOffset = gameObject.transform.position - GetMouseWorldPos();

    }

    // マウスのワールド座標取得
    private Vector3 GetMouseWorldPos()
    {
        // pixel cordinates (x, y)
        // 画面上（二次元）のマウスの位置
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        // マウスの位置をオブジェクトの距離とする
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // クリックした状態でドラッグした時
    private void OnMouseDrag()
    {
        // ゲームオブジェクトを掴んで引っ張った先の位置　＝　マウスの位置　＋　オブジェクトの原点とオブジェクトを掴んだマウスの位置の差
        gameObject.transform.position = GetMouseWorldPos() + mOffset;
    }
}
