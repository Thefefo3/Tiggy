using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FovController : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] private Camera cam;
    private float wallRunFovMultiplier = 1.25f;
    private float wallRunFovTime = 1f;
    private float resetFovTime = 1.5f;

    private float runFOVMultiplier = 1.5f;
    private float runFovTime = 1f;

    PhotonView PV;

    float initialFOV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PV.IsMine)
            return;
        initialFOV = cam.fieldOfView;
        Debug.Log(initialFOV * runFOVMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setWallRunFov()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, initialFOV * wallRunFovMultiplier, wallRunFovTime * Time.deltaTime);
    }

    public void setRunFov()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, initialFOV * runFOVMultiplier, runFovTime * Time.deltaTime);
    }

    public void resetFov()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, initialFOV, resetFovTime * Time.deltaTime);
    }

    public void setFovToSpeed(float speed)
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, initialFOV + speed * 2, wallRunFovTime * Time.deltaTime);
    }

}
