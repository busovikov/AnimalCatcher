using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerMenu : MonoBehaviour
{
    public Button button;
    public GameObject price;
    public ContainerInfoUI containerInfo;
    public TMPro.TextMeshProUGUI priceCaption;
    public TMPro.TextMeshProUGUI buttonCaption;

    AnimalContainer _container;
    GameObject _objectToFollow;
    int _price;
    public void Take()
    {
        ContainerHolder.Instance.Follow(_container, _objectToFollow.transform);
        gameObject.SetActive(false);
    }

    public void Buy()
    {
        if (Wallet.Spend(_price))
        {
            _container.upgradeDef.Owned = true;
            ContainerHolder.Instance.Follow(_container, _objectToFollow.transform);
            gameObject.SetActive(false);
        }
    }


    public void Fill(AnimalContainer container, GameObject objectToFollow)
    {
        _price = container.upgradeDef.GetPrice();
        _container = container;
        _objectToFollow = objectToFollow;
        price.SetActive(!container.upgradeDef.Owned);
        buttonCaption.text = container.upgradeDef.Owned ? "Take" : "Buy";

        if (!container.upgradeDef.Owned)
        {
            priceCaption.text = _price.ToString();
            button.interactable = Wallet.Affordable(_price);
        }
        containerInfo.Clear();
        foreach (var animal in container.upgradeDef.supported)
        {
            containerInfo.AddAnimal(animal);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(container.upgradeDef.Owned ? Take : Buy );
    }

}
