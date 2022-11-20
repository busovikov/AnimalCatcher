using System;
using System.Collections;
using UnityEngine;

public class UnlockableLevel : MonoBehaviour
{
    private float timer;
    public float endPoint = 0f;
    private float startPoint;
    // Use this for initialization
    void Start()
    {
        startPoint = transform.position.z;
    }

    public IEnumerator Unlock(float ratio, float animationTime)
    {
        float target = Mathf.Lerp(startPoint, endPoint, ratio);
        Vector3 current = transform.localPosition;
        Debug.Log("Current y " + current.z);
        Debug.Log("Target y " + target);
        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            if (timer > animationTime)
            {
                timer = animationTime;
            }

            float t = timer / animationTime;
            float z = Mathf.Lerp(current.z, target, t);
            transform.localPosition = new Vector3(current.x, current.y, z);
            yield return null;
        }
        timer = 0;
        if (target == endPoint)
        {
            gameObject.SetActive(false);
        }
    }
}
