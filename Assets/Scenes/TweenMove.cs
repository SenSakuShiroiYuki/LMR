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
    public float timer;
    public float firstTimer;
    private bool firstEnd = false;
    private bool Rotate = false;
    private int posIndex = -1;
    //--------
    public int Type;
    [SerializeField] private float[] nextPointTimer = { 24, 30, 3, 3, 3, 3, 3, 3, 3, 32, 32, 34 };
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(FirstMove(firstTimer));
    }

    private void Update()
    {
        if (posIndex > 0 && Rotate)
            RotatePeople(transform, NextPoint[posIndex-1]);
    }
    IEnumerator FirstMove(float timer)
    {
        yield return new WaitForSeconds(timer);
        switch (Type)
        {
            case 0:
                Tween_Move1();
                break;
            case 1:
                Tween_Move2();
                break;
            case 2:
                Tween_Move3();
                break;
            case 3:
                Tween_Move4();
                break;
            case 4:
                Tween_Move5();
                break;
        }
        StartPoint = transform.position;
        StartRotation = transform.eulerAngles;
        firstEnd = true;
    }

    private void OnEnable()
    {
        if (firstEnd)
        {
            switch (Type)
            {
                case 0:
                    Tween_Move1();
                    break;
                case 1:
                    Tween_Move2();
                    break;
                case 2:
                    Tween_Move3();
                    break;
                case 3:
                    Tween_Move4();
                    break;
                case 4:
                    Tween_Move5();
                    break;
            }
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
        //Vector3 rotate = Quaternion.LookRotation(dir).eulerAngles;
        //LeanTween.rotate(gameObject, rotate, .2f);
        //transform.LookAt(nextPoint);
    }

    void Tween_Move1()
    {
        posIndex++;
        LeanTween.move(gameObject, startPoint, timer).setOnComplete(() =>
        {
            if (NextPoint.Length > 0)
            {
                posIndex++;
                Rotate = true;
                //transform.LookAt(NextPoint[0]);
                LeanTween.move(gameObject, NextPoint[0], timer).setOnComplete(() =>
                {
                    posIndex++;
                    Rotate = true;
                    //transform.LookAt(NextPoint[1]);
                    LeanTween.move(gameObject, NextPoint[1], timer).setOnComplete(() =>
                    {
                        enabled = false;
                        GetComponent<ReStartTweenMove>().enabled = true;
                    });
                });

            }
            else
            {
                enabled = false;
                GetComponent<ReStartTweenMove>().enabled = true;
            }
        });

    }
    void Tween_Move2()
    {
        posIndex++;
        LeanTween.move(gameObject, startPoint, 3.5f).setOnComplete(() =>
        {
            if (NextPoint.Length > 0)
            {
                posIndex++;
                Rotate = true;
                //RotatePeople(startPoint, NextPoint[0]);
                LeanTween.move(gameObject, NextPoint[0], 7).setOnComplete(() =>
                {
                    posIndex++;
                    //RotatePeople(NextPoint[0], NextPoint[1]);
                    LeanTween.move(gameObject, NextPoint[1], 7).setOnComplete(() =>
                    {
                        posIndex++;
                        //RotatePeople(NextPoint[1], NextPoint[2]);
                        LeanTween.move(gameObject, NextPoint[2], 2).setOnComplete(() =>
                        {
                            enabled = false;
                            GetComponent<ReStartTweenMove>().enabled = true;
                        });
                    });
                });

            }
            else
            {
                enabled = false;
                GetComponent<ReStartTweenMove>().enabled = true;
            }
        });
    }
    void Tween_Move3()
    {
        posIndex++;
        LeanTween.move(gameObject, startPoint, 11f).setOnComplete(() =>
        {
            if (NextPoint.Length > 0)
            {
                posIndex++;
                Rotate = true;
                //RotatePeople(startPoint, NextPoint[0]);
                LeanTween.move(gameObject, NextPoint[0], 7).setOnComplete(() =>
                {
                    posIndex++;
                    //RotatePeople(NextPoint[0], NextPoint[1]);
                    LeanTween.move(gameObject, NextPoint[1], 7).setOnComplete(() =>
                    {
                        posIndex++;
                        //RotatePeople(NextPoint[1], NextPoint[2]);
                        LeanTween.move(gameObject, NextPoint[2], 2).setOnComplete(() =>
                        {
                            enabled = false;
                            GetComponent<ReStartTweenMove>().enabled = true;
                        });
                    });
                });

            }
            else
            {
                enabled = false;
                GetComponent<ReStartTweenMove>().enabled = true;
            }
        });
    }
    void Tween_Move4()
    {
        posIndex++;
        LeanTween.move(gameObject, startPoint, 10).setOnComplete(() =>
        {
            if (NextPoint.Length > 0)
            {
                posIndex++;
                Rotate = true;
                //RotatePeople(startPoint, NextPoint[0]);
                LeanTween.move(gameObject, NextPoint[0], 5).setOnComplete(() =>
                {
                    posIndex++;
                    //RotatePeople(startPoint, NextPoint[1]);
                    LeanTween.move(gameObject, NextPoint[1], 6).setOnComplete(() =>
                    {
                        enabled = false;
                        GetComponent<ReStartTweenMove>().enabled = true;
                    });
                });

            }
            else
            {
                enabled = false;
                GetComponent<ReStartTweenMove>().enabled = true;
            }
        });
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
            //RotatePeople(transform, NextPoint[posIndex]);
        });
    }
}
