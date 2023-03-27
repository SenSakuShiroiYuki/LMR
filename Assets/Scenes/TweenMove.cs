using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TweenMove : MonoBehaviour
{
    public Transform startPoint;
    public Transform[] NextPoint;
    public Vector3 StartPoint;
    public Vector3 StartRotation;
    public float delayTimer;
    private bool firstEnd = false;
    private bool Rotate = false;
    private int posIndex = -1;
    //--------    
    [SerializeField] private float[] nextPointTimer;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(FirstMove(delayTimer));
    }
    private void Update()
    {
        if (posIndex > 0 && Rotate)
            RotatePeople(transform, NextPoint[posIndex - 1]);
    }
    IEnumerator FirstMove(float timer)
    {
        yield return new WaitForSeconds(timer);
        Tween_Move5();
        StartPoint = transform.position;
        StartRotation = transform.eulerAngles;
        firstEnd = true;
    }
    private void OnEnable()
    {
        if (firstEnd)
        {
            Tween_Move5();
            GetComponent<ReStartTweenMove>().enabled = false;
        }
    }
    private void OnDisable()
    {
        transform.position = StartPoint;
        transform.eulerAngles = StartRotation;
        posIndex = -1;
        Rotate = false;
    }

    void RotatePeople(Transform previousPoint, Transform nextPoint)
    {
        Vector3 dir = nextPoint.position - previousPoint.position;
        Quaternion newQuaternion = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newQuaternion, 150 * Time.deltaTime);
    }
    void Tween_Move5()
    {
        LeanToPos(gameObject, startPoint.position);
    }
    void LeanToPos(GameObject gameObject, Vector3 pos)
    {
        Rotate = true;
        if (posIndex >= NextPoint.Length - 1)
        {
            enabled = false;
            GetComponent<ReStartTweenMove>().enabled = true;
            return;
        }
        posIndex++;
        LeanTween.move(gameObject, pos, nextPointTimer[posIndex]).setOnComplete(() =>
        {
            LeanToPos(gameObject, NextPoint[posIndex].position);
        });
    }
    private void OnDrawGizmosSelected()
    {
        if (NextPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, startPoint.position);
        Gizmos.DrawLine(startPoint.position, NextPoint[0].position);
        for (int i = 0; i < NextPoint.Length; i++)
        {
            if (i < NextPoint.Length - 1)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(NextPoint[i].position, NextPoint[i + 1].position);
            }
                
        }
    }
}
