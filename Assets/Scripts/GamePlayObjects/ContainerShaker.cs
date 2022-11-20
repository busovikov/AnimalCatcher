using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerShaker : MonoBehaviour
{
    public int shakesNumber;
    public float oneShakeTime;
    public float degree;
    public Vector3 axis;
    public AnimalContainer container;
    private MeshRenderer meshRenderer;
    bool shaking = false;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GamePlayEvents.ContainerChanged.AddListener(OnContainerFull);
    }

    void OnContainerFull(AnimalContainer ac, AnimalContainer.ChangeType type)
    {
        if (type == AnimalContainer.ChangeType.full && !shaking && ac == container)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        shaking = true;
        Color current = meshRenderer.material.color;
        Vector3 currentRotation = transform.localEulerAngles;
        meshRenderer.material.color = Color.red;
        for (int i = 0; i < shakesNumber; ++i)
        {
            float timer = -oneShakeTime;
            bool right = (i & 1) == 0;
            while (timer < oneShakeTime)
            {
                timer += Time.deltaTime;
                if (timer > oneShakeTime)
                {
                    timer = oneShakeTime;
                }

                float ratio = timer / oneShakeTime;
                float start = right ? -degree : degree;
                float end = right ? degree : -degree;
                transform.localRotation = Quaternion.Euler(axis * Mathf.Lerp(start, end, ratio) + currentRotation);
                yield return null;
            }
            GamePlayEvents.Vibrate.Invoke(0.5f, 0f);
        }
        meshRenderer.material.color = current;
        transform.localRotation = Quaternion.Euler(currentRotation);
        shaking = false;
    }
}
