using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Tags::
//TAGGED
//NOT TAGGED



public class TagManager : MonoBehaviour
{
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();    
    }

    void OnTriggerEnter(Collider other)
    {
        if(this.tag != other.tag)
        {
            if (this.CompareTag("TAGGED") && other.CompareTag("NOT TAGGED"))
            {
                StartCoroutine(TagPlayer(other));
            }
            else if (this.CompareTag("NOT TAGGED") && other.CompareTag("TAGGED"))
            {
                StartCoroutine(GetTagged());
            }
        }
        else
        {
            //Debug.Log("You are on the same team!!");
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
        PV.RPC("RPC_Tag", RpcTarget.All);
        yield return new WaitForSeconds(2f);
    }

    [PunRPC]
    void RPC_Tag()
    {
        Debug.Log("RPC_TAG");
        Destroy(gameObject);

        if (PV.IsMine)
        {

            Transform spawnPoint = SpawnManager.Instance.GetTaggerSpawnpoint();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerTagged"), spawnPoint.position, spawnPoint.rotation);
        }

        //Debug.Log(taggedPlayer.GetComponent<PhotonView>().Owner.NickName + " got tagged");
    }
}