using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTripleJump : MonoBehaviour
{
    public GameObject pickupEffect;

    public float adder = 1f;
    public float duration = 10f;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider player)
    {
        //var clonedEffect = pickupEffect;

        //Spawn a cool effect
        var clonedEffect = Instantiate(pickupEffect, transform.position, transform.rotation);


        //Apply Effect to the player
        PlayerMovement movement = player.GetComponentInParent<PlayerMovement>();
        movement.maxJumps += adder;


        //hide component
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;


        //wait for time
        yield return new WaitForSeconds(duration);

        //return to default power
        movement.maxJumps -= adder;

        //Remove powerup object
        Destroy(clonedEffect);
        Destroy(gameObject);

    }
}
