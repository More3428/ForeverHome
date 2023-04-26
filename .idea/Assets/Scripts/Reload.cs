using UnityEngine;
using System.Collections;
public class Reload : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;
    public int maxAmmo = 30;
    public float reloadTime = 2f;

    [HideInInspector]
    public int currentAmmo;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        // Trigger the muzzle flash particle system.
        if (muzzleFlash != null && !muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        currentAmmo--;

        // Cast a ray from the center of the screen in the direction of the camera's forward vector.
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            // Apply damage to the hit object if it has a collider.
            if (hit.collider != null)
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
