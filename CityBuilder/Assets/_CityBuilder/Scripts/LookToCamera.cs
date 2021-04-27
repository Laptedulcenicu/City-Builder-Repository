﻿using UnityEngine;

namespace _CityBuilder.Scripts
{
    public class LookToCamera : MonoBehaviour
    {
  
        void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
    }
}
