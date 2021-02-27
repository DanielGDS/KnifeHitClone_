using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScirpt : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Knife" || col.tag == "Apple" || col.tag == "Wood")
            Destroy(col.gameObject);
    }

}
