using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }   
    }

    void CreateController()
    {
        Transform spawnPoint;
        int tagged = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.LocalPlayer.NickName];
        if (tagged == 1)
        {
            spawnPoint = SpawnManager.Instance.GetTaggerSpawnpoint();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerTagged"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
        }
        else
        {
            spawnPoint = SpawnManager.Instance.GetRunnerSpawnpoint();
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerNotTagged"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { PV.ViewID });
        }
    }

    
}
