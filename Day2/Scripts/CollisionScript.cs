using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private int waveNumber;
    public float distanceX, distanceZ;
    public float[] waveAmplitude;
    public float magnitudeDivider;
    public Vector2[] impactPos;
    public float[] distance;
    public float speedWaveSpread;

    Mesh mesh;
    Renderer Rend;

    // Start is called before the first frame update
    void Start()
    {
        Rend = GetComponent<Renderer>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            waveAmplitude[i] = Rend.material.GetFloat("_WaveAmplitude" + (i + 1));
            if(waveAmplitude[i] > 0)
            {
                distance[i] += speedWaveSpread;
                Rend.material.SetFloat("_Distance" + (i + 1), distance[i]);
                Rend.material.SetFloat("_WaveAmplitude" + (i + 1), waveAmplitude[i] * 0.98f);
            }

            if(waveAmplitude[i] < 0.01)
            {
                Rend.material.SetFloat("_WaveAmplitude" + (i + 1), 0);
                distance[i] = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            waveNumber++;
            if(waveNumber == 9)
            {
                waveNumber = 1;
            }
            waveAmplitude[waveNumber - 1] = 0;
            distance[waveNumber - 1] = 0;

            distanceX = this.transform.position.x - collision.gameObject.transform.position.x;
            distanceZ = this.transform.position.z - collision.gameObject.transform.position.z;

            // 注意　衝突位置はy
            impactPos[waveNumber - 1].x = collision.transform.position.x;
            impactPos[waveNumber - 1].y = collision.transform.position.z;

            Rend.material.SetFloat("_xImpact" + waveNumber, collision.transform.position.x);
            Rend.material.SetFloat("_zImpact" + waveNumber, collision.transform.position.z);

            // 水面のスケールに寄らずに振幅は一定
            Rend.material.SetFloat("_OffsetX" + waveNumber, distanceX / mesh.bounds.size.x * 2.5f);
            Rend.material.SetFloat("_OffsetZ" + waveNumber, distanceZ / mesh.bounds.size.z * 2.5f);

            // 衝突の強さによって振幅が変化する
            Rend.material.SetFloat("_WaveAmplitude" + waveNumber, collision.rigidbody.velocity.magnitude * magnitudeDivider);
        }
    }
}
