using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContainerInfoUI : MonoBehaviour
{
    private Dictionary<AnimalDefinition,Image> avatars = new Dictionary<AnimalDefinition, Image>();
    public GameObject avatar;

    private void Start()
    {
        
    }
    public void Clear()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void AddAnimal(AnimalDefinition animal)
    {
        if (avatars.ContainsKey(animal))
        {
            avatars[animal].gameObject.SetActive(true);
        }
        else
        {
            Image image = Instantiate(avatar, transform).GetComponent<Image>();
            avatars[animal] = image;
            image.sprite = animal.avatar;
        }
    }

    public TextMeshProUGUI amount;
    public TextMeshProUGUI capacity;
    public Button accessButton;
}