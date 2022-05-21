using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale, angle;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;
    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, 0.03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, 0.03f);

        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspect > 1f)
        {
            scaleY /= aspect;
        }
        else
        {
            scaleX *= aspect;
        }
        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
    }

    private void HandleInputs()
    {
        float val = Input.GetAxis("Mouse ScrollWheel");
        float Hori = Input.GetAxis("Horizontal");
        float Vert = Input.GetAxis("Vertical");
        if (val > 0.0f)
        {
            scale *= 0.7f;
        }
        if (val < 0.0f)
        {
            scale *= 1.3f;
        }
        if (Input.GetKey(KeyCode.E)){
            angle -= 0.01f;
        }
        if (Input.GetKey(KeyCode.Q)){
            angle += 0.01f;
        }

        Vector2 dir = new Vector2(0.01f * scale, 0);
        float s = Mathf.Sin(angle);
        float c = Mathf.Cos(angle);
        dir = new Vector2(dir.x * c, dir.x * s);
        
        if (Hori < 0.0f)
        {
            pos -= dir;
        }
        if (Hori > 0.0f)
        {
            pos += dir;
        }

        dir = new Vector2(-dir.y, dir.x);

        if (Vert < 0.0f)
        {
            pos -= dir;
        }
        if (Vert > 0.0f)
        {
            pos += dir;
        }
    }
    void FixedUpdate()
    {
        HandleInputs();
        UpdateShader();
    }
}
