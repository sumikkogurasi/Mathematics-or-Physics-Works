using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRotation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rb = GetComponent<Rigidbody>();

            // y軸方向への角速度ベクトル
            var omega = new Vector3(0f, 1f, 0f);

            // 現在の姿勢
            var R = transform.rotation;

            // その逆行列
            var RI = Quaternion.Inverse(transform.rotation);

            // 対角行列（中身は３次元ベクトル）
            var Id = rb.inertiaTensor; 

            // 正則行列（逆行列を持つ）（行列）
            var Ir = rb.inertiaTensorRotation;

            // 上の正則行列の逆行列
            var IrI = Quaternion.Inverse(Ir);

            // T=I*omega 慣性テンソル
            // T=Ir*Id*IrI*omega Iを対角化したIr*Id*IrIに変換
            //var torque = Ir * Vector3.Scale(Id, IrI * omega);

            // T=R*Ir*Id*IrI*RI*omega
            var torque = R * Ir * Vector3.Scale(Id, IrI * RI * omega);

            // 力積（フレームレートに由らない力を加える）
            rb.AddTorque(torque, ForceMode.Impulse);
        }
    }

    [SerializeField]
    Text RotX;
    [SerializeField]
    Text RotZ;

    // sliderのvalueをgameobjectに渡す関数 (X軸)
    public void SliderValueToGameobjectX(float newValue)
    {
        Vector3 rot = this.transform.localEulerAngles;

        rot.x = newValue;

        RotX.text = newValue.ToString();

        this.transform.localEulerAngles = rot;
    }

    // sliderのvalueをgameobjectに渡す関数 (Z軸)
    public void SliderValueToGameobjectZ(float newValue)
    {
        Vector3 rot = this.transform.localEulerAngles;

        rot.z = newValue;

        RotZ.text = newValue.ToString();

        this.transform.localEulerAngles = rot;
    }
}
