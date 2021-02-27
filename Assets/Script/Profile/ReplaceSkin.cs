using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceSkin : MonoBehaviour
{
    public GameObject[] knives;
    GameController _gameController;

    private void Awake()
    {
        _gameController = GameObject
       .FindGameObjectWithTag("GameController")
       .GetComponent<GameController>();
    }

    private void Start()
    {
        if (_gameController.profile.PlayerKnife != null)
        {
            for (int i = 0; knives.Length > i; i++)
            {
                knives[i].transform.GetComponent<SpriteRenderer>().sprite = _gameController.profile.PlayerKnife;
            }
        }

    }
}
