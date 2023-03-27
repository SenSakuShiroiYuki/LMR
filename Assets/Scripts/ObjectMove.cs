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

    List<Transform> path = null;
    List<float> distance = null;
    float totalDistance = 0f;

    int currentIndex = 0;

    private void Start()
    {
        Init();
        Move();
    }

    // 初始化
    private void Init()
    {
        path = new List<Transform>();
        distance = new List<float>();

        // 將 pathTransform 的子物件都加進 path 裡面
        foreach (Transform trans in pathTransform)
        {
            path.Add(trans);
        }

        totalDistance = 0f;
        distance.Add(0);

        // 記錄每段路的距離並計算總距離
        for (int i = 0; i < path.Count - 1; i++)
        {
            var _dis = Vector3.Distance(path[i].position, path[i + 1].position);
            distance.Add(_dis);
            totalDistance += _dis;
        }

        // 將物件歸零成 path 的第一個
        transform.position = path[0].position;
        transform.rotation = path[0].rotation;
    }

    /// <summary>
    /// 移動物體並套用預設delay時間
    /// </summary>
    public void Move()
    {
        // delay 呼叫
        LeanTween.delayedCall(delayTime, () => MoveObject());
    }

    /// <summary>
    /// 移動物體並自訂delay時間
    /// </summary>
    /// <param name="delay"></param>
    public void Move(float delay)
    {
        // delay 呼叫
        LeanTween.delayedCall(delay, () => MoveObject()); ;
    }

    private void MoveObject()
    {
        // 呼叫 onMoveStart 事件
        if (currentIndex == 0) onMoveStart?.Invoke();

        // 暫存上一個 index
        var _lastIndex = currentIndex;

        // 索引值 +1，並且超過 path.Count 時歸零
        currentIndex++;
        currentIndex %= path.Count;

        // 計算本段路程的移動時間
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

            // 計算旋轉時間
            var _rotateTime = Mathf.Abs(path[currentIndex].eulerAngles.y - path[_lastIndex].eulerAngles.y) / 90f * rotateSpeed;
            LeanTween.rotate(gameObject, path[currentIndex].eulerAngles, _rotateTime).setOnComplete(() => Move(0));
        });
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
