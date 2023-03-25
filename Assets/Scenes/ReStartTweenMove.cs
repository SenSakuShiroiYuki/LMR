using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartTweenMove : MonoBehaviour
{
    public float timer;
    private void OnEnable()
    {
        if(GetComponent<TweenMove>())
        {
            GetComponent<TweenMove>().enabled = false;
            StartCoroutine(ReStart());
        }
    }

    IEnumerator ReStart()
    {
        yield return new WaitForSeconds(timer);
        GetComponent<TweenMove>().enabled = true;
    }

    private void OnDisable()
    {
        
    }
}
