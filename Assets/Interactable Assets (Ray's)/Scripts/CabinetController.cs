using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CabinetController : MonoBehaviour
{
    private bool opening;
    float speed = 0.05f;
    float timeCount = 0.0f;

    private GameObject leftDoor, rightDoor;
    private readonly Vector3 leftOpenAngle = new(0f, 160f, 0f);
    private readonly Vector3 rightOpenAngle = new(0f,-160f,0f);
    private readonly Vector3 closeAngle = new(0f, 0f, 0f); 
    
    
    void Start()
    {
        leftDoor = transform.Find("Left Door").gameObject;
        rightDoor = transform.Find("Right Door").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            opening = !opening;
            timeCount = 0;
        }

        rightDoor.transform.rotation = Quaternion.Lerp(rightDoor.transform.rotation, Quaternion.Euler(opening ? rightOpenAngle : closeAngle), timeCount * speed);
        leftDoor.transform.rotation = Quaternion.Lerp(leftDoor.transform.rotation, Quaternion.Euler(opening ? leftOpenAngle : closeAngle), timeCount * speed);
        
        // This is a memory leak
        timeCount += Time.deltaTime;
    }
}
