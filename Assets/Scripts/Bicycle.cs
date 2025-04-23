using EasyPeasyFirstPersonController;
using rayzngames;
using UnityEngine;

public class Bicycle : MonoBehaviour, IInteractable
{
    public FirstPersonController FirstPersonController;
    public CameraController CameraController;
    public Camera Camera;
    public Transform targetPosition;
    public void Interact()
    {
        FirstPersonController.canMove = false;

        // Attach the player to the bicycle
        FirstPersonController.gameObject.transform.parent = this.gameObject.transform;

        // Enable the bicycle movement script
        this.gameObject.transform.parent.GetComponent<BicycleVehicle>().enabled = true;

        // Move and rotate the camera
        Camera.transform.parent = targetPosition.transform;
        Camera.transform.position = targetPosition.position;

        // Rotate the camera 180 degrees around the Y-axis
        Camera.transform.rotation = Quaternion.Euler(Camera.transform.rotation.eulerAngles + new Vector3(0, 180f, 0));
    }

}
