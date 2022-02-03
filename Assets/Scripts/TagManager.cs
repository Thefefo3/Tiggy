using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//Tags::
//TAGGED
//NOT TAGGED

public class TagManager : MonoBehaviour, IOnEventCallback
{
    PhotonView PV;

    PlayerController playerController;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        if (PV.IsMine && this.CompareTag("TAGGED"))
        { 
            playerController.PrintText("You are tagged!");
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("RPC_CheckProperties", RpcTarget.MasterClient);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(this.tag != other.tag)
        {
            if (this.CompareTag("TAGGED") && other.CompareTag("NOT TAGGED"))
            {
                TagPlayer(other);
            }
            else if (this.CompareTag("NOT TAGGED") && other.CompareTag("TAGGED"))
            {
                GetTagged();
            }
        }
    }



    void TagPlayer(Collider player)
    {
        Debug.Log("Tagged");
        player.gameObject.tag = "TAGGED";
        playerController.PrintText("You tagged " + player.GetComponent<PhotonView>().Owner.NickName);
        //player.tag = "TAGGED";
        //yield return new WaitForSeconds(0f);

    }

    void GetTagged()
    {
        //this.tag = "TAGGED";

        PV.RPC("RPC_Tag", RpcTarget.All);

        //yield return new WaitForSeconds(0f);
    }

    [PunRPC]
    void RPC_Tag()
    {
        Destroy(gameObject);

        if (PV.IsMine)
        {

            Transform spawnPoint = SpawnManager.Instance.GetTaggerSpawnpoint();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerTagged"), spawnPoint.position, spawnPoint.rotation);
        }
        PV.RPC("RPC_CheckProperties", RpcTarget.MasterClient);
    }

    [PunRPC]
    void RPC_CheckProperties()
    {
        GameObject[] taggedPlayers = GameObject.FindGameObjectsWithTag("NOT TAGGED");

        if (taggedPlayers.Length > 0)
            return;

        const byte b = 17;
        if(PhotonNetwork.PlayerList.Length > 1)
            PhotonNetwork.RaiseEvent(b, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        const byte b = 17;
        if(eventCode == b)
        {
            Destroy(RoomManager.Instance.gameObject);
            Launcher.Instance.Disconnect();
            SceneManager.LoadScene(3);
        }
        
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}