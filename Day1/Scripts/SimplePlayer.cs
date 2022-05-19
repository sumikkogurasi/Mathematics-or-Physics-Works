using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    void FixedUpdate()
    {
        //var hori = Input.GetAxis("Horizontal") * Time.deltaTime;
        //var vert = Input.GetAxis("Vertical") * Time.deltaTime;
        //transform.position += new Vector3(hori, vert, 0f);

        var hori = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        var force = new Vector3(hori, vert, 0);

        GetComponent<Rigidbody>().AddForce(force * 100f);
    }
}
