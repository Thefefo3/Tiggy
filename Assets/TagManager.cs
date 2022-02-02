using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tags::
//TAGGED
//NOT TAGGED



public class TagManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(this.tag != other.tag)
        {
            if (this.CompareTag("TAGGED"))
            {
                StartCoroutine(TagPlayer(other));
            }
            else if (this.CompareTag("NOT TAGGED"))
            {
                StartCoroutine(GetTagged());
            }
        }
        else
        {
            Debug.Log("You are on the same team!!");
        }
    }



    IEnumerator TagPlayer(Collider player)
    {
        player.tag = "TAGGED";
        yield return new WaitForSeconds(2f);
    }

    IEnumerator GetTagged()
    {
        this.tag = "TAGGED";
        yield return new WaitForSeconds(2f);
    }

}