using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    PhotonView PV;

    PlayerManager playerManager;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (PV.IsMine)
            skinnedMesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(RoomManager.Instance.gameObject);
            Launcher.Instance.Disconnect();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PhotonNetwork.LoadLevel(0);
        }
    }
}