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
                TagPlayer(other);
            }
            else if (this.CompareTag("NOT TAGGED") && other.CompareTag("TAGGED"))
            {
                GetTagged();
            }
        }
        else
        {
            //Debug.Log("You are on the same team!!");
        }
    }



    void TagPlayer(Collider player)
    {
        //player.tag = "TAGGED";
        //yield return new WaitForSeconds(0f);
    }

    void GetTagged()
    {
        this.tag = "TAGGED"; 
        Hashtable setTagged = new Hashtable();
        setTagged.Add(PhotonNetwork.LocalPlayer.NickName, 1);

        do
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(setTagged);
        } while ((int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.LocalPlayer.NickName] == 0);

        

        PV.RPC("RPC_Tag", RpcTarget.All);
        PV.RPC("RPC_CheckProperties", RpcTarget.MasterClient);

        //yield return new WaitForSeconds(0f);
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

            PV.RPC("RPC_CheckProperties", RpcTarget.MasterClient);
        }

        //Debug.Log(taggedPlayer.GetComponent<PhotonView>().Owner.NickName + " got tagged");
    }

    [PunRPC]
    void RPC_CheckProperties()
    {
        Debug.Log("RPC_CheckProperties");
        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log("players length: " + players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            int tagged = (int)PhotonNetwork.CurrentRoom.CustomProperties[players[i].NickName];
            Debug.Log("name: " + players[i].NickName + " tagged val: " + tagged);
        }
        for (int i = 0; i < players.Length; i++)
        {
            int tagged = (int)PhotonNetwork.CurrentRoom.CustomProperties[players[i].NickName];

            if (tagged == 0)
                return;
        }
        //Destroy(RoomManager.Instance.gameObject);
        //Launcher.Instance.Disconnect();
        //PhotonNetwork.LoadLevel(3);
        const byte b = 17;
        PhotonNetwork.RaiseEvent(b, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        const byte b = 17;
        if(eventCode == b)
        {
            Debug.Log("ciao");
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