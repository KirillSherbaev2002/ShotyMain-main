﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = target.transform.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}

