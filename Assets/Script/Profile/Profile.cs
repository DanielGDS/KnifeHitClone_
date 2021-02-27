using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public static Profile Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite PlayerKnife;

    public void CurrentAvatar(Sprite image)
    {
        PlayerKnife = image;
    }

    public Image TempKnifeSprite;
}
