using System;
using System.Collections;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public MoneyDefenition playerMoney;
    static Wallet instance;

    public void Start()
    {
        instance = this;
        GamePlayEvents.MoneyChanged.Invoke(instance.playerMoney.money);
    }

    public static bool Spend(int price)
    {
        
        if (instance != null && price <= instance.playerMoney.money)
        {
            instance.playerMoney.money -= price;
            GamePlayEvents.MoneyChanged.Invoke(instance.playerMoney.money);
            GamePlayEvents.MoneyTransaction.Invoke(false, price);
            return true;
        }
        return false;
    }

    internal static string MoneyString()
    {
        if (instance != null)
            return instance.playerMoney.money.ToString();
        return "0";
    }

    internal static bool Affordable(int v)
    {
        return instance != null && v <= instance.playerMoney.money;
    }

    internal static bool GetCost(ref int filled, int price)
    {
        if (filled == price)
            return false;
        int newValue = Mathf.Clamp(filled + instance.playerMoney.money, 0, price);
        instance.playerMoney.money -= newValue - filled;
        GamePlayEvents.MoneyChanged.Invoke(instance.playerMoney.money);
        GamePlayEvents.MoneyTransaction.Invoke(false, newValue);
        filled = newValue;
        return true;
    }

    internal static void EarnMoney(int amount)
    {
        if (instance != null)
        {
            instance.playerMoney.money += amount;
            GamePlayEvents.MoneyChanged.Invoke(instance.playerMoney.money);
        }
    }
}
