using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameObject prefabLeft;
    public GameObject prefabRight;

    private GameController _gameController;

    private void Awake()
    {
        _gameController = GameObject
       .FindGameObjectWithTag("GameController")
       .GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Knife")
        {
            _gameController.appleInWood--;

            Vector3 applePos = transform.position;
            //prefabRight.transform.Rotate(0, 5, 5);
            Instantiate(prefabRight, applePos, Quaternion.identity);
            //prefabLeft.transform.Rotate(0, 30, 25);
            Instantiate(prefabLeft, applePos, Quaternion.identity);
            Destroy(gameObject);

        }
    }
}
