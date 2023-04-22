using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetController : MonoBehaviour
{
    private bool opening;
    float speed = 0.05f;
    float timeCount = 0.0f;

    private GameObject leftKnob, rightKnob;
    private ParticleSystem waterStream;
    
    private readonly Vector3 openAngle = new(0f, 0f, 180f);
    private readonly Vector3 closeAngle = new(0f, 0f, 0f);

    void Start()
    {
        waterStream = transform.Find("WaterStream").gameObject.GetComponent<ParticleSystem>();
        leftKnob = transform.Find("LeftJoint").gameObject;
        rightKnob = transform.Find("RightJoint").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            opening = !opening;
            timeCount = 0;
            if (opening)
            {
                waterStream.Play();
            }
            else
            {
                waterStream.Stop();
            }
        }

        rightKnob.transform.rotation = Quaternion.Lerp(rightKnob.transform.rotation, Quaternion.Euler(opening ? openAngle : closeAngle), timeCount * speed);
        leftKnob.transform.rotation = Quaternion.Lerp(leftKnob.transform.rotation, Quaternion.Euler(opening ? -openAngle : closeAngle), timeCount * speed);

        // This is a memory leak
        timeCount += Time.deltaTime;
    }
}
