using System;
using UnityEngine;
using static ObjectPool;

[CreateAssetMenu(menuName = "AnimalDefinition")]
public class AnimalDefinition : ScriptableObject
{
    public static int active = 0;

    public Sprite avatar;
    public Color tagColor;
    public enum Size
    {
        small = 1,
        middle = 2,
        big = 4,
    }

    public enum Condition
    {
        Captured = 0,
        Free = 1,
    }

    public Size size = Size.small;
    public int price = 0;
    public int unlockPrice = 0;
    public GameObject skinnedObject;
    public GameObject staticObject;
    [Range(1, 100)] public float radiusOfWander = 1f;
    [Range(0.1f, 10)] public float speed = 0.1f;
    public void Decrement(Condition condition)
    {
        if (condition == Condition.Captured)
        {
            --counter.Static;
        }
        if (condition == Condition.Free)
        {
            --counter.Animated;
        }
    }
    public PooledObjectBase Spawn(Condition condition)
    {
        if (pool == null)
        {
            return null;
        }

        if (condition == Condition.Captured)
        {
            ++counter.Static;
            return GetStatic(pool);
        }
        if (condition == Condition.Free)
        {
            ++counter.Animated;
            return GetAnimated(pool);
        }

        return null;
    }

    static public PooledObjectBase GetAnimated(PooledAnimal _pool)
    {
        PooledObjectBase o = _pool.Get(PooledAnimal.Types.Animated);
        o.obj.SetActive(true);
        o.sphereCollider.enabled = true;
        return o;
    }

    static public PooledObjectBase GetStatic(PooledAnimal _pool)
    {
        PooledObjectBase o = _pool.Get(PooledAnimal.Types.Static);
        o.obj.SetActive(true);
        o.sphereCollider.enabled = false;
        o.animal.agent.enabled = false;
        return o;
    }

    public void InitWithPool(ObjectPool.PooledAnimal _pool)
    {
        pool = _pool;
    }
    private ObjectPool.PooledAnimal pool;
    public Counter counter = new Counter();
    

    public class Counter
    {
        private int countAnimated = 0;
        private int countStatic = 0;

        public int Animated { get => countAnimated; set => countAnimated = value; }
        public int Static { get => countStatic; set => countStatic = value; }
    }
}