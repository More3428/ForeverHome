using UnityEngine;

public class ZoomIn : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float zoomedInFOV = 30f;
    [SerializeField] private float zoomedOutFOV = 60f;
    [SerializeField] private float aimDistance = 0.5f;

    private Transform gunTransform;
    private bool isZoomedIn = false;

    void Start()
    {
        gunTransform = GetComponent<Transform>();
    }

    void Update()
    {
        // Zoom in when right mouse button is held down.
        if (Input.GetMouseButtonDown(1))
        {
            isZoomedIn = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isZoomedIn = false;
        }

        if (isZoomedIn)
        {
            playerCamera.fieldOfView = zoomedInFOV;

            // Move the gun towards the camera to simulate aiming.
            Vector3 aimPosition = playerCamera.transform.position + playerCamera.transform.forward * aimDistance;
            gunTransform.position = Vector3.Lerp(gunTransform.position, aimPosition, Time.deltaTime * 10f);

            // Rotate the gun to face the same direction as the camera.
            gunTransform.rotation = Quaternion.LookRotation(playerCamera.transform.forward, playerCamera.transform.up);
        }
        else
        {
            playerCamera.fieldOfView = zoomedOutFOV;

            // Reset the gun's position and rotation when not aiming.
            gunTransform.localPosition = Vector3.zero;
            gunTransform.localRotation = Quaternion.identity;
        }
    }
}