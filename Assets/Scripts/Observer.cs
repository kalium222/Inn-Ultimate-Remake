using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Observer : MonoBehaviour
{
    public UnityEvent OnNotify;
    public UnityEvent OnCatch;
    // handle in inspector
    public float CaughtTime = 1f;
    public float CaughtDistance = 0.5f;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != HeroController.Instance.gameObject) return;
        OnNotify.Invoke();
        StartCoroutine(CaughtCoroutine());
    }

    private IEnumerator CaughtCoroutine() {
        float startTime = Time.time;
        while (Time.time - startTime < CaughtTime) {
            if (Vector3.Distance(transform.position, HeroController.Instance.transform.position) > CaughtDistance) yield break;
            yield return null;
        }
        OnCatch.Invoke();
    }
}
