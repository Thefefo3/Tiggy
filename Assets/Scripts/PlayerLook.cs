using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WallRunning wallRunning;

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();

     
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("pressed F");
            cam.LookAt(GetClosestEnemy());
        }
        else
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRunning.tilt);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    Transform GetClosestEnemy()
    {
        GameObject[] enemies = null;

        if (enemies == null)
            enemies = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(enemies.Length);


        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Transform potentialTargetTransform = potentialTarget.transform;
            Vector3 directionToTarget = potentialTargetTransform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTargetTransform;
            }
        }

        Debug.Log(bestTarget);
        return bestTarget;
    }
}
