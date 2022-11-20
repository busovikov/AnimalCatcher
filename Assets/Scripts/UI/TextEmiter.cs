using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEmiter : MonoBehaviour
{
    TextMeshPro textMesh;
    List<TextMeshPro> texts = new List<TextMeshPro>();

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.transform.rotation = Quaternion.LookRotation(textMesh.transform.position - Camera.main.transform.position);
    }

    
    void Update()
    {
        foreach (var text in texts)
        {
            text.transform.localPosition += Vector3.up * Time.deltaTime;   
        }
    }

    public void EmitText(string text)
    {
        textMesh.text = text;
        texts.Add(Instantiate(textMesh, transform));
    }
}
