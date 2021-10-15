using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCollider : MonoBehaviour
{
    public Transform RespawnPoint;
    
    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider other)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = RespawnPoint.position;
            other.transform.rotation = Quaternion.identity;
        }
    }
}
