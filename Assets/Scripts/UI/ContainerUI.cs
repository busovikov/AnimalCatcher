using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContainerUI : MonoBehaviour
{

    public GameObject containerInfo;

    private List<AnimalContainer> containers = new List<AnimalContainer>();
    private void Start()
    {
        GamePlayEvents.ContainerChanged.AddListener(OnContainerChanged);
    }

    void OnContainerChanged(AnimalContainer container, AnimalContainer.ChangeType type)
    {
        if (type == AnimalContainer.ChangeType.amount)
        {
            int childIndex = containers.IndexOf(container);
            if (childIndex == -1)
            {
                containers.Add(container);
                GameObject ui = Instantiate(containerInfo, transform);

                childIndex = containers.Count - 1;
            }

            var uiContainer = transform.GetChild(childIndex).GetComponent<ContainerInfoUI>();
            uiContainer.AddAnimal(container.lastAddedAnimal.def);
            uiContainer.amount.text = container.Size.ToString();
            uiContainer.capacity.text = container.Max.ToString();
        }
    }


    private void Update()
    {

    }
}
