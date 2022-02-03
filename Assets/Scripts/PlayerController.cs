using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    PhotonView PV;
    public float messageDuration = 3f;

    public Text messages;

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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial")){
                SceneManager.LoadScene("MenuLobby");
            }
            else
            {
                Destroy(RoomManager.Instance.gameObject);
                Launcher.Instance.Disconnect();
                PhotonNetwork.LoadLevel(0);
            }
        }
    }


    public void PrintText(string text)
    {
        Debug.Log("Printing text" + text);
        if (PV.IsMine)
            StartCoroutine(PrintTextU(text)); 

    }

    IEnumerator PrintTextU(string text)
    {
        messages.text = text;
        messages.enabled = true;

        yield return new WaitForSeconds(messageDuration);

        messages.enabled = false;
    }
}