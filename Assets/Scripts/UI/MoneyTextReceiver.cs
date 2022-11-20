using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoneyTextReceiver : MonoBehaviour
{
    Text money;
    void OnEarnMoneyEvent(int value)
    {
        money.text = value.ToString(); 
    }

    void Start()
    {
        money = GetComponent<Text>();
        money.text = Wallet.MoneyString();
        GamePlayEvents.MoneyChanged.AddListener(OnEarnMoneyEvent); 
    }
}
