using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public AnimalDefinition def;

    public AnimalDefinition.Condition state = AnimalDefinition.Condition.Free;

    public NavMeshAgent agent;
    private void Start()
    {
    }

    public void SetOnNavMesh(bool active)
    {
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 500f, 1);
        agent.speed = def.speed;
        transform.position = hit.position;
        agent.enabled = active;
    }

    private void Update()
    {
        if (state == AnimalDefinition.Condition.Free &&
            agent.enabled &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(GetRandomDistenation());
        }
    }

    private Vector3 GetRandomDistenation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * def.radiusOfWander;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 500f, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void Free()
    {
        gameObject.transform.SetParent(null);
        gameObject.SetActive(false);
        def.Decrement(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GamePlayEvents.AnimalCollected.Invoke(this);
        }
    }

}


