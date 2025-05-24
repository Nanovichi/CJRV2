using UnityEngine;
using System.Reflection;

public class Swimming : MonoBehaviour
{
    public Transform playerCamera;
    public EasyPeasyFirstPersonController.FirstPersonController controller;

    public float minYRotationInWater = -10f;
    public float maxYRotationInWater = 90f;

    public float waterBobAmount = 0.02f;
    public float waterBobSpeed = 1f;

    public Transform arms; // 👈 Both arms in one object
    public float swingSpeed = 2f;
    public float swingAmount = 20f;

    private bool isInWater = false;
    private float swingTimer = 0f;

    private FieldInfo rotYField;
    private FieldInfo bobAmountField;
    private FieldInfo bobSpeedField;

    private float originalBobAmount;
    private float originalBobSpeed;

    void Start()
    {
        var type = typeof(EasyPeasyFirstPersonController.FirstPersonController);

        rotYField = type.GetField("rotY", BindingFlags.NonPublic | BindingFlags.Instance);
        bobAmountField = type.GetField("bobAmount", BindingFlags.Public | BindingFlags.Instance);
        bobSpeedField = type.GetField("bobSpeed", BindingFlags.Public | BindingFlags.Instance);

        if (controller)
        {
            originalBobAmount = (float)bobAmountField.GetValue(controller);
            originalBobSpeed = (float)bobSpeedField.GetValue(controller);
        }
    }

    void Update()
    {
        if (!controller || rotYField == null) return;

        if (isInWater)
        {
            float currentRotY = (float)rotYField.GetValue(controller);
            rotYField.SetValue(controller, Mathf.Clamp(currentRotY, minYRotationInWater, maxYRotationInWater));

            bobAmountField.SetValue(controller, waterBobAmount);
            bobSpeedField.SetValue(controller, waterBobSpeed);

            if (arms)
            {
                swingTimer += Time.deltaTime * swingSpeed;
                float swing = Mathf.Sin(swingTimer) * swingAmount;
                arms.localRotation = Quaternion.Euler(swing, 0f, 0f);
            }
        }
        else
        {
            bobAmountField.SetValue(controller, originalBobAmount);
            bobSpeedField.SetValue(controller, originalBobSpeed);

            if (arms)
            {
                arms.localRotation = Quaternion.Lerp(arms.localRotation, Quaternion.identity, Time.deltaTime * 5f);
                swingTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInWater = false;
    }
}
