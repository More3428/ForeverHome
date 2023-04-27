using UnityEngine;

public class BulletTrailController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float trailDuration = 0.1f;
    public float shootForce = 20f;

    private bool isTrailActive = false;

    private void Update()
    {
        if (isTrailActive)
        {
            // Update the trail position to follow the bullet.
            Vector3 endPosition = transform.position - transform.forward * 0.1f;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, endPosition);

            // Move the bullet trail forward.
            transform.Translate(Vector3.forward * shootForce * Time.deltaTime);
        }
    }

    public void StartTrail()
    {
        if (lineRenderer != null)
        {
            // Activate the trail effect.
            lineRenderer.enabled = true;

            // Set the initial trail position.
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position - transform.forward * 0.1f);

            // Deactivate the trail after a certain duration.
            Invoke("StopTrail", trailDuration);

            isTrailActive = true;
        }
    }

    private void StopTrail()
    {
        if (lineRenderer != null)
        {
            // Deactivate the trail effect.
            lineRenderer.enabled = false;

            isTrailActive = false;

            // Destroy the bullet trail object after the trail is finished.
            Destroy(gameObject);
        }
    }
}
