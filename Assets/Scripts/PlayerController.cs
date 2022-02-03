using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer skinnedMesh;

    PhotonView PV;
    public float messageDuration = 3f;

    public Text messages;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
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


    public void PrintText(string text)
    {
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