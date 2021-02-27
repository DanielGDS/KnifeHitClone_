using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    // Простой скрипт для уничтожения обьектов спустя время
    public float Time;
    void Start()
    {
        StartCoroutine("DestroyTime");
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(Time);
        Destroy(gameObject);
    }

}
