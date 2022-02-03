using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

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
        if (PhotonNetwork.LocalPlayer.NickName == Launcher.Instance.FirstTagger.NickName)
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
