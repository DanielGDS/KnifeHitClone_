using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeSpawner : MonoBehaviour
{
    public float throwerForce;
    public GameObject knifePrefab;
    public GameObject knife;

    public Transform target;

    public GameController _gameController;
    public UI _UI;

    [Range(0.01f, 0.9f)]
    public float NewKnifeSpeed;

    Transform m_rectXform;

    bool reachedDestination = false;

    private void Awake()
    {
        _gameController = GameObject
        .FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();
    }

    public void InstanseNewKnife()
    {
        _gameController.SetGameUIList(true);
        Vector3 finalKnife = new Vector3 (0, -45, 5);
        knife = Instantiate(knifePrefab, finalKnife, Quaternion.identity);
        knife.transform.parent = null;
        knife.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 25, ForceMode2D.Impulse);
        knife = null;
    }

    public void NewKnife()
    {
        if (_gameController.knifeCount != 0 && _gameController.isInvoke == false && _gameController.gameIsOver == false && knife == null)
        {
            knife = Instantiate(knifePrefab, transform);

            if (_gameController.profile.PlayerKnife != null)
            {
                knife.GetComponent<SpriteRenderer>().sprite = _gameController.profile.PlayerKnife;
                knife.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _gameController.profile.PlayerKnife;
            }

            StartCoroutine(MoveRoutine(_gameController.startPostion, _gameController.onscreenPosition, _gameController.timeToMove));
        }
        else
        {
            Invoke("NewKnife", 1f);
        }
    }

    void multiplierScore()
    {
        if (_gameController.multiplierScore != 99) { _gameController.multiplierTempScore++; }
        if (_gameController.multiplierTempScore == 30 && _gameController.multiplierScore != 99) 
        {
            _gameController.multiplierScore++; 
            _gameController.appleCountText.transform.GetChild(0).GetComponent<Text>().text = "x" + _gameController.multiplierScore;
            _gameController.multiplierTempScore = 0;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && knife != null && _gameController.gameIsOver == false && _gameController.isInvoke == false && reachedDestination)
        {
            _gameController.knifeCount--;
            _gameController.allKnifeCount++;
            _gameController.knifesText.text = "" + _gameController.allKnifeCount;

            multiplierScore();

            int Knife = _gameController.knifeCount;

            _UI.UpdateHealth(Knife);

            knife.transform.parent = null;
            knife.GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwerForce, ForceMode2D.Impulse);
            knife = null;
            if (_gameController.isInvoke == false && _gameController.gameIsOver == false) { Invoke("NewKnife", NewKnifeSpeed); }
        }
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        m_rectXform = knife.transform;
        if (m_rectXform != null)
        {
            m_rectXform.position = startPos;
        }

        reachedDestination = false;
        float elapsedTime = 0f;

        while (!reachedDestination)
        {
            if (Vector3.Distance(m_rectXform.position, endPos) < 0.01f && knife != null)
            {
                reachedDestination = true;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            t = t * t * t * (t * (t * 6 - 15) + 10);

            if (m_rectXform != null)
            {
                m_rectXform.position = Vector3.Lerp(startPos, endPos, t);
            }
            yield return null;
        }
    }

}
