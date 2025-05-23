using EasyPeasyFirstPersonController;
using SmallHedge.SoundManager;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<FirstPersonController>();
        if (controller != null)
        {
            SoundManager.PlaySound(SoundType.BoosterTake);
            controller.ApplySpeedBoost(boostMultiplier, boostDuration);
            Destroy(gameObject); // optional: remove pickup after use
        }
    }
}
