using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    [HideInInspector]
    public static ObjectPool Instance;
    public abstract class PooledObjectBase
    {
        virtual public void Init(GameObject container, GameObject prefab)
        {
            obj = Instantiate(container);
            animal = obj.GetComponent<Animal>();
            sphereCollider = obj.GetComponent<SphereCollider>();
            animal.agent = obj.GetComponent<NavMeshAgent>();
            Instantiate(prefab, obj.transform);
            obj.SetActive(false);
        }

        public GameObject obj;
        public Animal animal;
        public SphereCollider sphereCollider;

        public System.Type extansion;
    }
    public class Pooled 
    {
        private List<PooledObjectBase>[] pool;
        public int amount = 10;

        [SerializeField]
        public int Amount { get => amount; }
        public int[] Index { set; get; }

        public virtual PooledObjectBase[][] GetPooledObjects(int amount) { return null; }
        public virtual void Init()
        {
            var pools = GetPooledObjects(Amount);
            pool = new List<PooledObjectBase>[pools.Length];
            Index = new int[pools.Length];
            for (int i = 0; i < pools.Length; ++i)
            {
                pool[i] = pools[i].ToList();
            }
        }

        public PooledObjectBase Add(int amount, int type)
        {
            var pools = GetPooledObjects(Amount);
            for(int i = 0; i < pools.Length; ++i)
            {
                pool[i].AddRange(pools[i]);
            }
            return pool[type][pool[type].Count - amount];
        }

        public PooledObjectBase Get(int type) 
        {
            if (pool == null)
            {
                Init();
            }
            int offset = Index[type]++;
            for (int i = 0; i < pool[type].Count; i++)
            {
                int index = (i + offset) % pool[type].Count;
                if (pool[type][index].obj.activeInHierarchy == false)
                {
                    return pool[type][index];
                }
            }

            return Add(3, type);
        }

        public virtual PooledObjectBase MakePooled(GameObject prefab)
        { 
            return null;
        }
    }

    [System.Serializable]
    public class PooledAnimal : Pooled
    {
        public enum Types
        {
            Static = 0,
            Animated = 1,
        }
        public class StaticAnimal : PooledObjectBase
        {
            public override void Init(GameObject container, GameObject prefab)
            {
                base.Init(container, prefab);
            }
            
        }

        public class AnimatedAnimal : PooledObjectBase
        {
            public override void Init(GameObject container, GameObject prefab)
            {
                base.Init(container, prefab);
                anim = obj.GetComponent<Animator>();
            }
            public Animator anim;
            
        }
        public AnimalDefinition defs;
        public Animal container;

        public override void Init()
        {
            defs.InitWithPool(this);
            base.Init();
        }

        public PooledObjectBase Get(Types type)
        {
            return base.Get((int)type);
        }
        public override PooledObjectBase[][] GetPooledObjects(int amount)
        {
            PooledObjectBase[][] objects = new PooledObjectBase[2][];
            objects[(int)PooledAnimal.Types.Static] = new PooledObjectBase[amount];
            objects[(int)PooledAnimal.Types.Animated] = new PooledObjectBase[amount];

            for (int i = 0; i < amount; ++i)
            {
                StaticAnimal animalStatic = MakePooled<StaticAnimal>(container.gameObject, defs.staticObject);
                animalStatic.animal.def = defs;
                animalStatic.animal.state = AnimalDefinition.Condition.Captured;
                objects[(int)PooledAnimal.Types.Static][i] = animalStatic;
                var animalAnimated = MakePooled<AnimatedAnimal>(container.gameObject, defs.skinnedObject);
                animalAnimated.animal.def = defs;
                animalAnimated.animal.state = AnimalDefinition.Condition.Free; 
                objects[(int)PooledAnimal.Types.Animated][i] = animalAnimated;
            }

            return objects;
        }

        public T MakePooled<T>(GameObject container, GameObject prefab) where T : PooledObjectBase, new()
        {
            T pooledObject = new();
            pooledObject.Init(container, prefab);
            return pooledObject;
        }
    }

    public PooledAnimal[] Animals;

    void Awake()
    {
        Instance = FindObjectOfType<ObjectPool>();
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var animalPool in Animals)
        {
            animalPool.Init();
        }
    }

}
