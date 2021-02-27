using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KnifeScript : MonoBehaviour
{
    // Скорость вращения бревна
    public float speed = 1f;

    public float speedAnimation;

    // Шанс смены направления вращения
    public float chanceToChangeDirections = 0.1f;

    public Transform target;
    public bool isAnimate;

    public GameController _gameController;

    public SpriteRenderer colorWood;
    public Color color;
    public Color hitColor;


    private void Awake()
    {
        _gameController = GameObject
        .FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();
    }

    private void Start()
    {
        color = colorWood.color;
        isAnimate = true;

    }

    private void LateUpdate()
    {
        if (isAnimate && _gameController.isInvoke == false)
        {
            transform.position
                = Vector3.Lerp(transform.position,
                target.position, Time.deltaTime * 100f / speedAnimation);
            //colorWood.color = color;
        }
    }


    private void Update()
    {
        var rot = speed * Time.deltaTime;
        transform.Rotate(0, 0, rot);
    }

    void FixedUpdate()
    {
        if (Random.value < chanceToChangeDirections)
            speed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Knife" && _gameController.isInvoke == false && col.gameObject.GetComponent<Knife>().attachedToWood == true)
        {
            isAnimate = true;
            transform.position = new Vector3(0, 1, 10);
            Invoke("SetAnimatePosition", 0.1f);
            Invoke("SetDefaultPosition", 0.2f);
            colorWood.color = hitColor;
        }

    }

    public void SetAnimatePosition()
    {
        colorWood.color = color;
        target.position = new Vector3(0, 0, 9);
    }

    public void SetDefaultPosition()
    {
        target.position = new Vector3(0, 0, 10);
    }
}
