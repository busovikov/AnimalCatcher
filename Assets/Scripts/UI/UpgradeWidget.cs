using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWidget : MonoBehaviour
{
    
    public AnimalContainer container;
    public TMPro.TextMeshProUGUI upgradeName;
    public TMPro.TextMeshProUGUI level;
    public TMPro.TextMeshProUGUI price;
    public Button button;
    public Image image;
    public Image lockImage;
    public GameObject avatarConainer;
    public GameObject avatarPrefab;
    public GameObject animalWidgetPrefab;
    private void Init()
    {
        upgradeName.text = container.upgradeDef.name;
        image.sprite = container.upgradeDef.sprite;
        SetAvatars();
        UpdateData();
        UpdateButton();
    }

    public void UpdateButton()
    {
        button.interactable = container.UpgradeAvailable() && Wallet.Affordable(container.upgradeDef.GetPrice());
        lockImage.enabled = !container.UpgradeAvailable();

        bool includeInactive = true;
        foreach (var widget in GetComponentsInChildren<SupportedAnimalWidget>(includeInactive))
        {
            widget.UpdateButton();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
    private void SetAvatars()
    {
        foreach (var def in container.upgradeDef.available)
        {
            Instantiate(avatarPrefab, avatarConainer.transform).GetComponent<Image>().sprite = def.avatar;
        }

        //if (container.upgradeDef.Owned)
        {
            foreach (var def in container.upgradeDef.supported)
            {
                if (!container.upgradeDef.available.Contains(def))
                {
                    SupportedAnimalWidget widget = Instantiate(animalWidgetPrefab, transform).GetComponent<SupportedAnimalWidget>();
                    widget.SetUp(this, def);
                }
            }
        }
    }

    private void UpdateData()
    {
        int _price = container.upgradeDef.GetPrice();
        price.text = _price.ToString();
        level.text = container.upgradeDef.level.ToString();
    }

    public void OnButtonClicked()
    {
        if (container.upgradeDef.Owned && Wallet.Spend(container.upgradeDef.GetPrice()))
        {
            container.Upgrade();
            UpdateData();
            UpdateButton();
        }
    }

    private void OnEnable()
    {
        UpdateData();
        UpdateButton();
        
    }

    private void Start()
    {
        Init();
    }
}
