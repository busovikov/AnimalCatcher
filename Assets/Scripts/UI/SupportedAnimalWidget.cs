using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportedAnimalWidget : MonoBehaviour
{

    public UpgradeWidget parent;
    public Button button;
    public TMPro.TextMeshProUGUI price;
    public AnimalDefinition def;
    public Image avatar;

    private int _price;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClick);   
    }

    public void UpdateButton()
    { 
        button.interactable = parent.container.UpgradeAvailable() && Wallet.Affordable(_price);
    }
    public void SetUp(UpgradeWidget _parent, AnimalDefinition _def)
    {
        parent = _parent;
        avatar.sprite = _def.avatar;
        def = _def;
        _price = _def.unlockPrice;
        price.text = _price.ToString();
        UpdateButton();
    }

    void OnButtonClick()
    {
        if (Wallet.Spend(_price))
        {
            Instantiate(parent.avatarPrefab, parent.avatarConainer.transform).GetComponent<Image>().sprite = def.avatar;
            parent.container.AddSupportedAnimal(def);
            gameObject.SetActive(false);
            parent.UpdateButton();
        }
    }

}
