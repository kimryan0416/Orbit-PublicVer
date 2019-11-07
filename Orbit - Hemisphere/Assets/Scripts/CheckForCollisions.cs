using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckForCollisions : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("orb"))
        {
            Debug.Log("Collided");
        }
    }

}