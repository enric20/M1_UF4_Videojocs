using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    public GameObject CLEARSKY;
    public GameObject SKYCITY;
    public GameObject NIGHTSKY;

    public void Start()
    {
        CLEARSKY.SetActive(true);
        SKYCITY.SetActive(false);
    }

    public void ChangeSky(int option)
    {
        switch (option)
        {
            case 0:
                CLEARSKY.SetActive(false);
                SKYCITY.SetActive(false);
                NIGHTSKY.SetActive(false);
                break;
            case 1:
                CLEARSKY.SetActive(true);
                SKYCITY.SetActive(false);
                NIGHTSKY.SetActive(false);
                break;
            case 2:
                SKYCITY.SetActive(true);
                CLEARSKY.SetActive(false);
                NIGHTSKY.SetActive(false);
                break;
            case 3:
                NIGHTSKY.SetActive(true);
                SKYCITY.SetActive(false);
                CLEARSKY.SetActive(false);
                break;

        }
    }
    
}
