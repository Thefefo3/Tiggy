using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRun : MonoBehaviour
{
    public GameObject pickupEffect;

    public float multiplier = 1.1f;
    public float duration = 4f;

    public float messageDuration = 2f;

    public static string text;


    public AudioClip endPowerUpClip;
    public AudioSource pickUpSound;


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
        //var clonedEffect = pickupEffect;

        //Spawn a cool effect
        var clonedEffect = Instantiate(pickupEffect, transform.position, transform.rotation);

        

        //Apply Effect to the player
        PlayerMovement movement = player.GetComponentInParent<PlayerMovement>();

        PlayerController playerController = player.GetComponentInParent<PlayerController>();
        movement.movementMultiplier *= multiplier;


        text = "Running speed increased!";
        playerController.PrintText(text);



        //hide component
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        //wait for time
        yield return new WaitForSeconds(duration);

        //return to default power
        movement.movementMultiplier /= multiplier;
        AudioSource.PlayClipAtPoint(endPowerUpClip, player.transform.position);

        text = "Extra Speed Expired";


            playerController.PrintText(text);

        yield return new WaitForSeconds(messageDuration);

        //Remove powerup object
        Destroy(clonedEffect);
        Destroy(gameObject);
       
    }
}
