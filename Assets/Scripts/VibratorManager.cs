using System.Collections;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class VibratorManager : MonoBehaviour
{

    static public HapticTypes HapticType = HapticTypes.MediumImpact;
    static public bool HapticAllowed = true;

    public void Start()
    {
        GamePlayEvents.Vibrate.AddListener(OnVibrate);
        MMVibrationManager.SetHapticsActive(HapticAllowed);
    }

    private void OnVibrate(float intensity, float duration)
    {
        if (Mathf.Epsilon > duration)
        {
            VibrateShort(intensity);
        }
        else
        {
            Vibrate(intensity, duration);
        }
    }

    public void VibrateShort(float intensity)
    {

        MMVibrationManager.TransientHaptic(intensity, 1f);
    }
    public void Vibrate(float intensity, float duration)
    {
        MMVibrationManager.ContinuousHaptic(intensity, 1f, duration);
    }

}
