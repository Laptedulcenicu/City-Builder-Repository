using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public LayerMask groundMask;

    public Vector3Int? RaycastGround(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Transform objectHit = hit.transform;
            Vector3Int positionInt = Vector3Int.RoundToInt(hit.point);
            Debug.DrawRay(ray.origin,ray.direction, Color.red, Single.MaxValue);
            return positionInt;
        }
        print("NotFinded");
        print(hit.transform.gameObject);
        Debug.DrawRay(ray.origin,ray.direction, Color.red, Single.MaxValue);
        return null;
    }

    public GameObject RaycastAll(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}
