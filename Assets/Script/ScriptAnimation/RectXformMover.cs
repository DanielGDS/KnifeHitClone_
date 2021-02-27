using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectXformMover : MonoBehaviour
{
    public Vector3 startPostion;
    public Vector3 onscreenPosition;
    public Vector3 endPosition;

    Vector3 smoothPos;

    public float timeToMove = 1f;
    public float delay;

    RectTransform m_rectXform;
    bool m_isMoving = false;

    void Awake()
    {
        m_rectXform = GetComponent<RectTransform>();
        transform.position = startPostion;
    }

    private void Start()
    {
        Invoke("MovePOS", delay);
    }

    private void OnEnable()
    {
        transform.position = startPostion;
        Invoke("MovePOS", delay);
    }

    public void MovePOS()
    {
        StartCoroutine(MoveRoutine(startPostion, onscreenPosition, timeToMove));
    }

    void Move(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        if (!m_isMoving)
        {
            StartCoroutine(MoveRoutine(startPos, endPos, timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove)
    {

        if (m_rectXform != null)
        {
            m_rectXform.anchoredPosition = startPos;
        }
        bool reachedDestination = false;
        float elapsedTime = 0f;
        m_isMoving = true;


        while (!reachedDestination)
        {
            if (Vector3.Distance(m_rectXform.anchoredPosition, endPos) < 0.01f)
            {
                reachedDestination = true;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            t = t * t * t *(t * (t * 6 - 15) + 10);

            if (m_rectXform != null)
            {
                m_rectXform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            }
            yield return null;
        }
        m_isMoving = false;
    }

    public void MoveOn()
    {
        Move(startPostion, onscreenPosition, timeToMove);
    }

    public void MoveOff()
    {
        Move(onscreenPosition, endPosition, timeToMove);
    }
}
