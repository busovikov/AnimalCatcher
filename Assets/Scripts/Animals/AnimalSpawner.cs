using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public float interval = 3f;
    public int max = 5;
    public AnimalDefinition def;
    public ObjectPool pool;

    private bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            active = true;
            bool includeInactive = true;

            foreach (Animal animal in GetComponentsInChildren<Animal>(includeInactive))
            {
                animal.agent.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            active = false;
            bool includeInactive = true;

            foreach (Animal animal in GetComponentsInChildren<Animal>(includeInactive))
            {
                animal.agent.enabled = false;
            }
        }
    }



    private float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (def.counter.Animated < max && timer > interval)
        {
            timer = 0;
            var pooled = def.Spawn(AnimalDefinition.Condition.Free);
            Animal a = pooled.animal;
            float size = .25f * (int)a.def.size;
            pooled.sphereCollider.radius = size;

            a.transform.SetParent(transform);
            a.transform.position = transform.position;
            a.SetOnNavMesh(active);
        }
    }
}
