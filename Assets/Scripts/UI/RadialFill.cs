using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialFill : MonoBehaviour
{
    public TMPro.TextMeshPro currentAmount;
    public TMPro.TextMeshPro totalAmount;
    public UnlockableLevel unlockable;
    public int price = 100;
    public float animationTime = 1f;
    public SpriteRenderer fill;
    int filled = 0;
    private float timer;

    private void Start()
    {
        totalAmount.text = price.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unlockable != null || other.tag == "Player")
        {
            Fill();
        }
    }

    public void Fill()
    {
        if (Wallet.GetCost(ref filled, price))
        {
            currentAmount.text = filled.ToString();
            float ratio = (float)filled / price;
            float fillValue = Mathf.Lerp(360f, 0f, ratio);
            float current = fill.material.GetFloat("_Arc2");
            StartCoroutine(FillCoroutine(current, fillValue));
            StartCoroutine(unlockable.Unlock(ratio, animationTime));
        }
    }

    private IEnumerator FillCoroutine(float current, float target)
    {
        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            if (timer > animationTime)
            {
                timer = animationTime;
            }

            float ratio = timer / animationTime;

            fill.material.SetFloat("_Arc2", Mathf.Lerp(current, target, ratio));
            yield return null;
        }
        timer = 0;
    }
}
