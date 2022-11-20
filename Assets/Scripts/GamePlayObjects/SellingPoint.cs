using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SellingPoint : MonoBehaviour
{
    public AnimalDefinition[] supported;
    class EntryPoint
    {
        public float timer;
        public Vector3 start;
        public Vector3 scale;
        public Animal item;
        public int price;
    }

    //public TextEmiter textEmiter;
    public AnimationCurve elevationCurve;
    public AnimationCurve scallingCurve;
    public float animationTime = 1f;
    private List<EntryPoint> entries = new List<EntryPoint>();
    private List<EntryPoint> lateDelete = new List<EntryPoint>();
    // Use this for initialization
    void Start()
    {
        SellingZone zone = transform.parent.GetComponent<SellingZone>();
        foreach (var animal in supported)
        {
            zone.BindAnimalToZone(animal);
        }
    }

    public void Sell(AnimalContainer container, float timeForItem)
    {
        Debug.Log(container.ToString());
        StartCoroutine(SellProgress(container, timeForItem));
    }

    private IEnumerator SellProgress(AnimalContainer container, float timeForItem)
    {
        var time = new WaitForSeconds(timeForItem);
        bool atLeastOne = false;
        foreach (var animal in container.GetAnimals().Where(animal => supported.Contains(animal.def)).Reverse())
        {
            animal.transform.SetParent(transform);
            GamePlayEvents.ContainerChanged.Invoke(container, AnimalContainer.ChangeType.amount);
            GamePlayEvents.Vibrate.Invoke(0.2f, 0f);
            EntryPoint entry = new EntryPoint
            {
                timer = 0,
                start = animal.transform.position,
                scale = animal.transform.localScale,
                item = animal,
                price = animal.def.price
            };
            atLeastOne = true;
            entries.Add(entry);
            yield return time;
        }
        if (atLeastOne)
        { 
            GamePlayEvents.MoneyTransaction.Invoke(true, entries.Count());
        }
        container.Reorder();
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EntryPoint entry in entries)
        {
            entry.timer += Time.deltaTime;
            if (entry.timer > animationTime)
            {
                entry.timer = animationTime;
            }

            float ratio = entry.timer / animationTime;

            Vector3 direction = Vector3.Lerp(entry.start, transform.position, ratio);
            Vector3 elevation = Vector3.up * elevationCurve.Evaluate(ratio);
            Vector3 scalling = Vector3.one * scallingCurve.Evaluate(ratio);
            entry.item.transform.position = direction + elevation;
            entry.item.transform.localScale = entry.scale + scalling;
            if (ratio == 1)
            {
                entry.item.transform.localScale = entry.scale;
                entry.item.Free();
                lateDelete.Add(entry);
                Wallet.EarnMoney(entry.price);
            }
        }
        foreach (EntryPoint entry in lateDelete)
        {
            entries.Remove(entry);
        }
    }
}
