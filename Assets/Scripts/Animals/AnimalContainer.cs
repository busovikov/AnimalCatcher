using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimalContainer : MonoBehaviour
{
    public UpgradeDefenition upgradeDef;
    public ContainerMenu containerMenu;
    public Follower follower;
    public GameObject selectionCircle;
    public int Size { get { return transform.childCount; } }
    public int Max { get { return max; } }
    public int Rows { get { return rows; } set { rows = value; max = GetMaxAmount(); } }

    public Animal lastAddedAnimal;
    public enum ChangeType { amount, level, support, full, empty };
    internal void Reorder()
    {
        if (transform.childCount == 0)
        {
            GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.empty);
            return;
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).transform.localPosition = GetAnimalPosition(i);
        }
        GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.amount);
    }

    public bool Active 
    { 
        get => active; 
        set 
        { 
            active = value; 
            if (sphereCollider != null) 
                sphereCollider.enabled = !active;
            if (selectionCircle != null)
                selectionCircle.SetActive(!active); 
        } 
    }

    internal void Follow(Transform objectToFollow)
    {
        if (follower != null)
        {
            follower.objectToFollow = objectToFollow;
            Active = objectToFollow != null;
        }

    }

    public object NextSize { get { return GetMaxAmount(1); } }


    [SerializeField, Range(1, 8)] private int rows = 2;
    [SerializeField] private bool active = false;
    private int max;
    private SphereCollider sphereCollider;

    public Animal[] GetAnimals()
    {
        bool includeInactive = true;
        return GetComponentsInChildren<Animal>(includeInactive);
    }
    private void Awake()
    {
        max = GetMaxAmount();
    }

    public bool UpgradeAvailable()
    {
        return upgradeDef.Owned;
    }
    public void Upgrade()
    {
        if (UpgradeAvailable())
        {
            upgradeDef.level++;
            max = GetMaxAmount();
            GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.level);
        }
    }

    public void AddSupportedAnimal(AnimalDefinition def)
    {
        upgradeDef.available.Add(def);
        GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.support);
    }

    public int GetMaxAmount(int addition = 0)
    {
        int _size = upgradeDef.supported.Count > 0 ? (int)upgradeDef.supported[0].size : (int)AnimalDefinition.Size.small;
        return (upgradeDef.level + rows + addition) * 4 / _size;
    }
    public bool AddAnimal(Animal animal)
    {
        Debug.Log("Animal Add Attempt");
        if (active && upgradeDef.available.Contains(animal.def))
        {
            if (transform.childCount >= max)
            {
                GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.full);
                return false;
            }
            GamePlayEvents.Vibrate.Invoke(0.5f,0f);
            animal.Free();
            var captured = animal.def.Spawn(AnimalDefinition.Condition.Captured);
            captured.animal.transform.SetParent(transform);
            captured.animal.transform.localPosition = GetAnimalPosition(transform.childCount - 1);
            captured.animal.transform.localRotation = Quaternion.identity;
            lastAddedAnimal = animal;
            GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.amount);
            if (transform.childCount == max)
            {
                GamePlayEvents.ContainerChanged.Invoke(this, ChangeType.full);
            }
            return true;
        }
        return false;
    }

    private void Start()
    {
        ContainerHolder.Instance.Add(this);
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.enabled = !active;
        }
    }

    Vector3 GetAnimalPosition(int index)
    {
        var _size = upgradeDef.supported[0].size;
        float coef = .25f * (int)_size;
        float y = (int)(index * coef); // number of items on level
        BitArray b = new BitArray(new byte[] { (byte)(index % 4) });
        float x = 0;
        if (_size != AnimalDefinition.Size.big)
        {
            x = b.Get(0) ? 1f : 0f;
        }
        float z = 0;
        if (_size == AnimalDefinition.Size.small)
        {
            z = b.Get(1) ? 1f : 0f;
        }
        return (new Vector3(x, y * (int)_size, z) * .25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && containerMenu)
        {
            containerMenu.Fill(this, other.gameObject);
            containerMenu.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            containerMenu.gameObject.SetActive(false);
        }
    }

}
