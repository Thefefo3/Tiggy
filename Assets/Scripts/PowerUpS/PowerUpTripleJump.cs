using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerUpTripleJump : MonoBehaviour
{
    public GameObject pickupEffect;

    public float adder = 2f;
    public float duration = 10f;

    public float messageDuration = 2f;

    public static string text;

    public AudioSource pickUpSound;



    public AudioSource endPowerUpSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LocalPlayer") || other.CompareTag("Player") || other.CompareTag("TAGGED") || other.CompareTag("NOT TAGGED"))
        {
            pickUpSound.Play();
            StartCoroutine(Pickup(other));
        }
    }



    IEnumerator Pickup(Collider player)
    {
        

        //Spawn a cool effect
        var clonedEffect = Instantiate(pickupEffect, transform.position, transform.rotation);
        
     

        //Apply Effect to the player
        PlayerMovement movement = player.GetComponentInParent<PlayerMovement>();
        PlayerController playerController = player.GetComponentInParent<PlayerController>();

        movement.maxJumps += adder;


        text = "+2 Jumps \n " + "Total Jumps = " + movement.maxJumps;

   
            playerController.PrintText(text);


        //hide component
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;



        //wait for time
        yield return new WaitForSeconds(duration);

        //return to default power
        movement.maxJumps -= adder;
        endPowerUpSound.Play();
      
        text = "Extra Jumps Expired \n " + "Total Jumps = " + movement.maxJumps;
        

            playerController.PrintText(text);

        //Remove powerup object
        Destroy(clonedEffect);
        Destroy(gameObject);

    }
}
