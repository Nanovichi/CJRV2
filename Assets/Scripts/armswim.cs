using UnityEngine;

public class SwimArmSwing : MonoBehaviour
{
    public Transform arms; // Both arms in one object
    public float swingSpeed = 2f;
    public float swingAmount = 20f;
    private bool isSwimming = false;
    private float timer = 0f;

    void Update()
    {
        if (isSwimming)
        {
            timer += Time.deltaTime * swingSpeed;

            // Back and forth motion on X axis (like paddling)
            float swing = Mathf.Sin(timer) * swingAmount;

            arms.localRotation = Quaternion.Euler(swing, 0f, 0f);
        }
        else
        {
            // Reset arms rotation when not swimming
            arms.localRotation = Quaternion.Lerp(arms.localRotation, Quaternion.identity, Time.deltaTime * 5f);
        }
    }

    public void SetSwimming(bool state)
    {
        isSwimming = state;
    }
}
