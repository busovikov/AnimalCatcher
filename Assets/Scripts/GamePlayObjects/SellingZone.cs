using System.Collections;
using UnityEngine;

public class SellingZone : MonoBehaviour
{
    public SellingPoint sellingPoint;
    public float timeForItem = 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Container")
        {
            Debug.Log("Selling " + other.ToString());
            var container = other.GetComponent<AnimalContainer>();
            sellingPoint.Sell(container, timeForItem);
        }
    }

    public void BindAnimalToZone(AnimalDefinition animal)
    {
        ContainerHolder.Instance.AddTargetAnimal(transform, animal);
    }
}
