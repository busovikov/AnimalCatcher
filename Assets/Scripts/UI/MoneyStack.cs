using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    public Transform directionPoint;
    public Transform container;
    private Vector3 starPoint;
    private Vector3 endPoint;
    public GameObject moneyPrefab;
    public int capacity = 10;
    public float timeBetweenElements = .08f;
    public float animationTime = .2f;

    int moving = 0;
    int count = 0;
    private float[] timers;
    void Start()
    {
        GamePlayEvents.MoneyTransaction.AddListener(OnTransaction);
        timers = new float[capacity];
        for (int i = 0; i < capacity; ++i)
        {
            Instantiate(moneyPrefab, transform.position, Quaternion.identity, container).SetActive(false);
        }
    }

    public void OnTransaction(bool back, int _count)
    {
        StopAllCoroutines();
        StartCoroutine(Move(back, _count));
    }

    IEnumerator Move(bool back, int _count)
    {
        starPoint = back ? directionPoint.localPosition : Vector3.zero;
        endPoint = !back ? directionPoint.localPosition : Vector3.zero;

        var wait = new WaitForSeconds(timeBetweenElements);
        count = Mathf.Min(container.childCount, _count);
        GamePlayEvents.Vibrate.Invoke(0.3f, animationTime);
        for (int i = 0; i < count; ++i)
        {
            container.GetChild(i).gameObject.SetActive(true);
            moving++;
            yield return wait; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                var child = container.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    if (timers[i] >= animationTime)
                    {
                        timers[i] = 0;
                        child.gameObject.SetActive(false);
                        moving--;
                        continue;
                    }
                    timers[i] += Time.deltaTime;
                    float ratio = timers[i] / animationTime;
                    Vector3 pos = Vector3.Lerp(starPoint, endPoint, ratio);
                    child.localPosition = pos;
                    child.rotation = moneyPrefab.transform.rotation;
                }
            }
        }
    }
}
