using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);


        //Apply Effect to the player

        //Remove powerup object
        Destroy(gameObject);
    }
}
