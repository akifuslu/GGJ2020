using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform gunRight;
    public Transform gunLeft;
    
    Transform activeGun;
    Vector3 mousePosition;
    Vector3 gunPosition;
    float angle;
    Camera mainCam;
    Animator playerAnimator;
    bool gunLeftActive;
    bool gunRightActive;
    bool gunActive;
    
    
    private void Start()
    {
        mainCam = Camera.main;
        playerAnimator = GetComponent<Animator>();
        gunActive = false;
        activeGun = gunRight;
    }

    void Update()
    {
        Aim();
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (gunActive == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                gunRight.gameObject.SetActive(false);
                activeGun = gunLeft;
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                gunLeft.gameObject.SetActive(false);
                activeGun = gunRight;
            }
        }

        if (gunActive == true)
        {
            if (Input.GetKey(KeyCode.D) == false)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    gunRight.gameObject.SetActive(false);
                    activeGun = gunLeft;
                    activeGun.gameObject.SetActive(true);
                }
            }
            
            if (Input.GetKey(KeyCode.A) == false)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    gunLeft.gameObject.SetActive(false);
                    activeGun = gunRight;
                    activeGun.gameObject.SetActive(true);
                }
            }
        }
    }

    void Aim()
    {
        mousePosition = Input.mousePosition;
        gunPosition = mainCam.WorldToScreenPoint(activeGun.position);
        mousePosition.x = mousePosition.x - gunPosition.x;
        mousePosition.y = mousePosition.y - gunPosition.y;
        angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        activeGun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    
    void Shoot()
    {
        playerAnimator.SetBool("Shoot", true);
        
        if (gunActive == false )
        {
            activeGun.gameObject.SetActive(true);
            gunActive = true;
        }
    }
}