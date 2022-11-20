using System.Collections;
using UnityEngine;

public class UpgradeZone : MonoBehaviour
{
    public GameObject upgradePopUp;
    public float timeForItem = 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            upgradePopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            upgradePopUp.SetActive(false);
        }
    }
}
