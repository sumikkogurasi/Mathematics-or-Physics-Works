using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool Living { get; private set; } // このセルが生存状態か

    private GameObject Death;
    private GameObject Alive;

    private void Awake()
    {
        Death = transform.Find("Death").gameObject;
        Alive = transform.Find("Alive").gameObject;

        Death.SetActive(true);
        Alive.SetActive(false);
        Living = false;
    }

    private void Update()
    {
        Living = Alive.activeSelf;
    }

    // 誕生
    public void Birth()
    {
        Death.SetActive(false);
        Alive.SetActive(true);
    }

    // 死滅
    public void Die()
    {
        Death.SetActive(true);
        Alive.SetActive(false);
    }
}
