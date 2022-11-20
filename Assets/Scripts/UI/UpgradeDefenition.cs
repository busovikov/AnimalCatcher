using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradeDefenition")]
public class UpgradeDefenition : ScriptableObject
{
    public new string name;
    public int price;
    public int upgradePrice;
    public int level = 0;
    public bool Owned = false;
    public Sprite sprite;
    [SerializeField] public List<AnimalDefinition> supported;
    [SerializeField] public List<AnimalDefinition> available;

    public void Move(AnimalDefinition def)
    {
        available.Add(def);
    }

    public int GetPrice()
    { 
        return Owned ? ((int)Mathf.Pow(level * 5, 2f)  + upgradePrice + ((level & 1) != 0 ? 25:0)) : price;
    }


}
