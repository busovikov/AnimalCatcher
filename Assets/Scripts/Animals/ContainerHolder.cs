using System;
using System.Collections.Generic;
using UnityEngine;

internal class ContainerHolder
{
    public static ContainerHolder Instance { get { return instance; } }
    private static ContainerHolder instance = new ContainerHolder();

    public HashSet<AnimalContainer> registry = new HashSet<AnimalContainer>();
    public Dictionary<AnimalDefinition,Transform> animalToContainer = new Dictionary<AnimalDefinition, Transform>();

    public ContainerHolder()
    {
        GamePlayEvents.AnimalCollected.AddListener(AddToContainer);
    }

    public void AddTargetAnimal(Transform target, AnimalDefinition animal)
    { 
        if (!animalToContainer.ContainsKey(animal))
        {
            animalToContainer.Add(animal, target);
        }
    }
    private void AddToContainer(Animal animal)
    {
        foreach (var container in registry)
        {
            if (container.AddAnimal(animal))
            {
                break;
            }
        }
    }

    internal void Follow(AnimalContainer container, Transform follow)
    {
        foreach (var _container in registry)
        {
            _container.Follow(container == _container ? follow : null);
        }
    }

    public void Add(AnimalContainer animalContainer)
    {
        registry.Add(animalContainer);
    }
}