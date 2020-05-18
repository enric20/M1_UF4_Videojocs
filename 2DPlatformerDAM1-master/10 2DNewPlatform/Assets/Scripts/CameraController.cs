using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float yOffset;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3 (player.position.x, transform.position.y, transform.position.z);

        transform.position = new Vector3(player.position.x, player.position.y + yOffset, transform.position.z);

    }
}


