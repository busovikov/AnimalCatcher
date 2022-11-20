using System.Collections;
using UnityEngine.Events;

public static class GamePlayEvents
{
    public static UnityEvent<Animal> AnimalCollected = new UnityEvent<Animal>();
    public static UnityEvent<int> MoneyChanged = new UnityEvent<int>();
    public static UnityEvent<AnimalContainer,AnimalContainer.ChangeType> ContainerChanged = new UnityEvent<AnimalContainer, AnimalContainer.ChangeType>();
    public static UnityEvent<bool,int> MoneyTransaction = new UnityEvent<bool,int>();
    public static UnityEvent<float, float> Vibrate = new UnityEvent<float, float>();
}
