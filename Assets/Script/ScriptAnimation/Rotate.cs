using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float force;
    Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbody.transform.rotation = new Quaternion(rigidbody.transform.rotation.x + force, rigidbody.transform.rotation.y, rigidbody.transform.rotation.z, rigidbody.transform.rotation.w);
    }
}
