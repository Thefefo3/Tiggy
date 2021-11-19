using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpRun : MonoBehaviour
{
    public GameObject pickupEffect;

    public float multiplier = 1.5f;
    public float duration = 4f;

    public float messageDuration = 2f;
    public Text messages;
    public static string text;

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
        movement.sprintSpeed *= multiplier;

        messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
        text = "*1.5 Speed \n " + "Running Speed = " + movement.sprintSpeed;
        messages.text = text;
        messages.enabled = true;



        //hide component
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(messageDuration);
        messages.enabled = false;

        //wait for time
        yield return new WaitForSeconds(duration);

        //return to default power
        movement.sprintSpeed /= multiplier;

        messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
        text = "Extra Speed Expired \n " + "Running Speed = " + movement.sprintSpeed;
        messages.text = text;
        messages.enabled = true;
        yield return new WaitForSeconds(messageDuration);
        messages.enabled = false;

        //Remove powerup object
        Destroy(clonedEffect);
        Destroy(gameObject);
       
    }
}
