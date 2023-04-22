using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    private GameObject clearWindow, barredWindow;
    private bool opening;
    void Awake()
    {
        clearWindow = transform.Find("Window").gameObject;
        barredWindow = transform.Find("Window_Barred").gameObject;
        clearWindow.SetActive(true);
        barredWindow.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            opening = !opening;
            if (opening)
            {
                clearWindow.SetActive(false);
                barredWindow.SetActive(true);
            }
            else
            {
                clearWindow.SetActive(true);
                barredWindow.SetActive(false);
            }
        }
    }
}
