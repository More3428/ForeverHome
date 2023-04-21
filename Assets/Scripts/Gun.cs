using UnityEngine;

public class HitscanGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        // Set the gun's position to match the camera's position, but at the same height.
        Vector3 position = transform.position;
        position.x = cameraTransform.position.x;
        position.y = transform.position.y;
        position.z = cameraTransform.position.z;
        transform.position = position;
    }

    void Update()
    {
        // Make the gun follow the camera's position, but stay at the same height.
        Vector3 position = transform.position;
        position.x = cameraTransform.position.x;
        position.z = cameraTransform.position.z;
        transform.position = position;

        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        // Stop the muzzle flash particle system if the mouse button is not pressed.
        if (muzzleFlash != null && muzzleFlash.isPlaying && !Input.GetButton("Fire1"))
        {
            muzzleFlash.Stop();
        }
    }

    void Shoot()
    {
        // Trigger the muzzle flash particle system.
        if (muzzleFlash != null && !muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        // Cast a ray from the center of the screen in the direction of the camera's forward vector.
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // Apply damage to the hit object if it has a collider.
            if (hit.collider != null)
            {
                hit.collider.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}