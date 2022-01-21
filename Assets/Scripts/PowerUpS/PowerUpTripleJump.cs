using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTripleJump : MonoBehaviour
{
    public GameObject pickupEffect;

    public float adder = 1f;
    public float duration = 10f;

    public float messageDuration = 2f;
    public Text messages;
    public static string text;

    public AudioSource pickUpSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        movement.maxJumps += adder;

        messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
        text = "+1 Jump \n " + "Total Jumps = " + movement.maxJumps;
        messages.text = text;
        messages.enabled = true;

        //hide component
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(messageDuration);
        messages.enabled = false;


        //wait for time
        yield return new WaitForSeconds(duration-messageDuration);

        //return to default power
        movement.maxJumps -= adder;

        messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
        text = "Extra Jump Expired \n " + "Total Jumps = " + movement.maxJumps;
        messages.text = text;
        messages.enabled = true;
        yield return new WaitForSeconds(messageDuration);
        messages.enabled = false;

        //Remove powerup object
        Destroy(clonedEffect);
        Destroy(gameObject);

    }
}
