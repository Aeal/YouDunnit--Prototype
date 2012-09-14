//*****************************************************************************
// SetupUICamera
// Purpose: Perform initial setup, gather variables for this menu system.
// Author: Tyler Ortiz
// Date: 1/20/2012
//*****************************************************************************
using UnityEngine;
using System;
public enum AspectRatioTarget
{
    Aspect_SIXNINE = 0,
    Aspect_FOURTHREE = 1,
    Aspect_SIXTEENTEN = 2,
};
//*****************************************************************************
public class SetupUICamera : MonoBehaviour
{
    Camera      guiCamera;          // Camera this menu system is using
    Light       menuLight;          // Overall Menu Lighting- directional light
    Transform cameraOrigins,      // Transform of the camera object
        backPlane;
    bool        error = false;      // Has an error occurred in setup
    public float mAspectRatio = 1.0f;

    public AspectRatioTarget mRatio = AspectRatioTarget.Aspect_SIXNINE;
    //*************************************************************************
    void Awake()
    {
       // ResetRatio();

        //cameraOrigins = gameObject.transform.FindChild("Editor");
        //cameraOrigins.localPosition = Vector3.zero;
        //cameraOrigins.localRotation = Quaternion.identity;

        //backPlane = cameraOrigins.transform.FindChild("MenuBackplane");
        //guiCamera = cameraOrigins.GetComponent<Camera>();
        //menuLight = cameraOrigins.GetComponent<Light>();

        //if (cameraOrigins == null)
        //{
        //    error = true;
        //}
        //else if (guiCamera == null)
        //{
        //    error = true;
        //}
        //else if (menuLight == null)
        //{
        //    error = true;
        //}

        //if (error == true)
        //{
        //   //Debug.Log("ERR: An error setting up Menu System has occurred.");
        //}
        //else
        //{
        //   //Debug.Log("Menu System Prefab set up properly.");
        //} 
    }
    public void ResetRatio()
    {
        if (cameraOrigins == null)
        {
            cameraOrigins = this.transform.FindChild("Editor");
        }
        switch (mRatio)
        {
            case AspectRatioTarget.Aspect_FOURTHREE:
                mAspectRatio = 4.0f / 3.0f;

                break;
            case AspectRatioTarget.Aspect_SIXNINE:
                mAspectRatio = 16.0f / 9.0f;

                break;
            case AspectRatioTarget.Aspect_SIXTEENTEN:
                mAspectRatio = 16.0f / 10.0f;

                break;
        }
        cameraOrigins.localScale = new Vector3(-mAspectRatio, -1, 1);
    }
    void Start()
    {
        //backPlane.gameObject.active = false;

    }

    //*************************************************************************
}
//*****************************************************************************