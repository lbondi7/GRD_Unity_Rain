﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    Vector2 move;
    Vector2 look;
    Animator playerAnimator;
    GameObject playerCameraObj;

    public Transform playerRoot;

    public bool amWet;
    public float speedModifier = 1;
    bool amCrouching = false;
    List<ParticleSystem> rainEffects = new List<ParticleSystem>();


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerCameraObj = GetComponentInChildren<Camera>().gameObject;
        var rainBits = GetComponentsInChildren<ParticleSystem>();
        foreach (var item in rainBits)
        {
            rainEffects.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ControlUpdate();
    }
    void ControlUpdate() 
    {
        playerCameraObj.transform.RotateAround
            (playerRoot.position, Vector3.up, look.x / 1.5f);
        if (amWet)
        {
            playerAnimator.SetFloat("Turn", move.x * speedModifier);
            playerAnimator.SetFloat("Forward", move.y * speedModifier);
            foreach (var item in rainEffects)
            {
                item.gameObject.SetActive(true);
            }
        }
        else
        {
            playerAnimator.SetFloat("Turn", move.x);
            playerAnimator.SetFloat("Forward", move.y);
            foreach (var item in rainEffects)
            {
                item.gameObject.SetActive(false);
            }
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        //Do a flip here?
    }

    public void OnLook(InputAction.CallbackContext context) 
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
    
    }

    public void OnCrouch(InputAction.CallbackContext context) 
    {
        amCrouching = !amCrouching;

        if (amCrouching)
        {
            playerAnimator.SetBool("Crouch", true);
        }
        else
        {
            playerAnimator.SetBool("Crouch", false);
        }
    }

    public Camera getPlayerCam() 
    {
        return playerCameraObj.GetComponent<Camera>();
    }


}
