using EasyPeasyFirstPersonController;
using SmallHedge.SoundManager;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private float boostMultiplier = 2f;
    private float boostDuration = 5f;

    [SerializeField] private float rotationSpeed = 90f; // degrees per second

    private void Update()
    {
        // Rotate around Y axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<FirstPersonController>();
        if (controller != null)
        {
            boostMultiplier = Random.Range(1.25f, 2f);
            boostDuration = Random.Range(3f, 6f);
            SoundManager.PlaySound(SoundType.BoosterTake);
            controller.ApplySpeedBoost(boostMultiplier, boostDuration);
            Destroy(gameObject); // optional: remove pickup after use
        }
        else if (other.gameObject.CompareTag("AI"))
        {
            boostMultiplier = Random.Range(1.25f, 2f);
            boostDuration = Random.Range(3f, 6f);
            other.gameObject.GetComponent<AiMovement>().ApplySpeedBoost(boostMultiplier, boostDuration);
            Destroy(gameObject);
        }
    }
}
