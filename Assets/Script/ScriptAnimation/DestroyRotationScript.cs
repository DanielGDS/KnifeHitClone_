using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRotationScript : MonoBehaviour
{
    public int speedAnimation;
    public int Rot;
    public Vector3 target;

    private void FixedUpdate()
    {
        //var random = Random.Range(-2, 2);
        //target = new Vector3(target.x + random, target.y + random, target.z);

        transform.position
            = Vector3.Lerp(transform.position,
            target, Time.deltaTime * 100f / speedAnimation);

        //Rot = 100;
        transform.Rotate(Random.value, -Rot + Random.value, Rot * Random.value);
    }

}
