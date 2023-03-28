using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] Transform pathTransform;
    [SerializeField] float totalTime;
    [SerializeField] float delayTime;
    [SerializeField] float rotateSpeed;
    [SerializeField] bool repeatMove;
    [SerializeField] UnityEvent onMoveStart;
    [SerializeField] UnityEvent onMoveEnd;
    [SerializeField] int targetIndex;

    List<Transform> path = null;
    List<float> distance = null;
    float totalDistance = 0f;

    int _lastIndex;
    int currentIndex = 0;
    private void Start()
    {
        Init();
        Move();
    }

    // ��l��
    private void Init()
    {
        path = new List<Transform>();
        distance = new List<float>();

        // �N pathTransform ���l���󳣥[�i path �̭�
        foreach (Transform trans in pathTransform)
        {
            path.Add(trans);
        }

        totalDistance = 0f;
        distance.Add(0);

        // �O���C�q�����Z���íp���`�Z��
        for (int i = 0; i < path.Count - 1; i++)
        {
            var _dis = Vector3.Distance(path[i].position, path[i + 1].position);
            distance.Add(_dis);
            totalDistance += _dis;
        }

        // �N�����k�s�� path ���Ĥ@��
        transform.position = path[targetIndex].position;
        transform.rotation = path[targetIndex].rotation;

        currentIndex = targetIndex;
    }

    private void Update()
    {
        RotatePeople(path[_lastIndex], path[currentIndex]);
    }
    /// <summary>
    /// ���ʪ���îM�ιw�]delay�ɶ�
    /// </summary>
    public void Move()
    {
        // delay �I�s
        LeanTween.delayedCall(delayTime, () => MoveObject());
    }

    /// <summary>
    /// ���ʪ���æۭqdelay�ɶ�
    /// </summary>
    /// <param name="delay"></param>
    public void Move(float delay)
    {
        // delay �I�s
        LeanTween.delayedCall(delay, () => MoveObject()); ;
    }

    private void MoveObject()
    {       

        // �Ȧs�W�@�� index
        _lastIndex = currentIndex;

        // ���ޭ� +1�A�åB�W�L path.Count ���k�s
        currentIndex++;
        currentIndex %= path.Count;

        // �p�⥻�q���{�����ʮɶ�
        var _time = totalTime * (distance[currentIndex] / totalDistance);
        LeanTween.move(gameObject, path[currentIndex], _time).setOnComplete(() =>
        {
            if (currentIndex == path.Count - 1)
            {
                onMoveEnd?.Invoke();

                if (!repeatMove)
                {
                    LeanTween.cancel(gameObject);
                    return;
                }
            }

            // �p�����ɶ�
            //var _rotateTime = Mathf.Abs(path[currentIndex].eulerAngles.y - path[_lastIndex].eulerAngles.y) / 90f * rotateSpeed;
            //LeanTween.rotate(gameObject, path[currentIndex].eulerAngles, _rotateTime);
            
            Move(0);
        });
    }
    void RotatePeople(Transform previousPoint, Transform nextPoint)
    {
        Vector3 dir = nextPoint.position - previousPoint.position;
        if (dir == Vector3.zero) return; 
        Quaternion newQuaternion = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newQuaternion, rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (path == null) return; 

        for (int i = 0; i < path.Count; i++)
        {
            if (i == 0)
                Gizmos.color = Color.green;
            else if (i == path.Count - 1)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.gray;

            Gizmos.DrawCube(path[i].position, Vector3.one);

            if (i < path.Count - 1)
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
        }
    }
}
