using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public GameObject pointerPrefab;

    class Arrow
    {
        public Arrow(GameObject o, Transform t)
        {
            pointer = o;
            target = t;
        }

        public GameObject pointer;
        public Transform target;
    }

    Dictionary<AnimalDefinition, Arrow> arrows = new Dictionary<AnimalDefinition, Arrow>();
    int counter = 0;
  

    private void Start()
    {
        GamePlayEvents.ContainerChanged.AddListener(OnContainerChange);
        foreach(var item in ContainerHolder.Instance.animalToContainer)
        {
            Arrow a = new Arrow(Instantiate(pointerPrefab,transform), item.Value);
            a.pointer.GetComponentInChildren<SpriteRenderer>().color = item.Key.tagColor;
            a.pointer.SetActive(false);
            arrows.Add(item.Key, a);
        }
    }

    void OnContainerChange(AnimalContainer container, AnimalContainer.ChangeType type)
    {

        if (type == AnimalContainer.ChangeType.amount)
        {
            arrows[container.lastAddedAnimal.def].pointer.SetActive(true);
        }
        else if (type == AnimalContainer.ChangeType.empty)
        {
            foreach(var animal in container.upgradeDef.available)
            {
                arrows[animal].pointer.SetActive(false);
            }
        }
    }

    public void TurnByDirection(GameObject pointer, Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
    void Update()
    {
        foreach (var arrow in arrows)
        {
            Vector3 direction = ClampY(arrow.Value.target.transform.position - transform.position);
            arrow.Value.pointer.transform.GetChild(0).localPosition = new Vector3(0f, 0.2f, Mathf.Clamp(direction.magnitude, 1f, 2.5f));
            TurnByDirection(arrow.Value.pointer, direction);
        }
    }

    public Vector3 ClampY(Vector3 vec)
    {
        return new Vector3(vec.x, 0f, vec.z);
    }
}
