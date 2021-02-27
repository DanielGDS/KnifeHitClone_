using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllKnife : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Knife" || col.tag == "Apple")
            Destroy(col.gameObject);
    }

}
