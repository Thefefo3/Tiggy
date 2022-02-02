using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public bool tagged = false;
    public float messageDuration = 2f;
    public Text messages;
    public static string text;

   

    // Start is called before the first frame update
    void Start()
    {
        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tagged == true)
            {
                //Debug.Log("TAGGED");
                StartCoroutine(TagPlayer(other)); 
            }
        }
    }


    IEnumerator TagPlayer(Collider player)
    {
        PlayerController controller = player.GetComponentInParent<PlayerController>();
        if(controller.tagged == false) { 
            yield return new WaitForSeconds(0.001f);
            StartCoroutine(controller.getTagged());
            messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
            text = "Tagged Player!";
            messages.text = text;
            messages.enabled = true;

            yield return new WaitForSeconds(messageDuration);
            messages.enabled = false;
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator getTagged()
    {
        tagged = true;
        messages = GameObject.Find("PowerUpTXT").GetComponent<Text>();
        text = "Got Tagged!";
        messages.text = text;
        messages.enabled = true;

        yield return new WaitForSeconds(messageDuration);
        messages.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}