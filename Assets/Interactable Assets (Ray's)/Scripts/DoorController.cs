using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool opening;
    float speed = 0.05f;
    float timeCount = 0.0f;
    
    private readonly Vector3 openAngle = new(0f, -85f, 0f);
    private readonly Vector3 closeAngle = new(0f, 0f, 0f); 
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            opening = !opening;
            timeCount = 0;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(opening ? openAngle : closeAngle), timeCount * speed);
        
        // This is a memory leak
        timeCount += Time.deltaTime;
    }
}
