using EasyPeasyFirstPersonController;
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
            controller.ApplySpeedBoost(boostMultiplier, boostDuration);
            Destroy(gameObject); // optional: remove pickup after use
        }
    }
}
