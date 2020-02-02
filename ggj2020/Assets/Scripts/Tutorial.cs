using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private PlayerController _pl;
    private CameraController _cm;
    private SceneManagement _sm;
    private WeaponController _wc;

    // Start is called before the first frame update
    void Start()
    {
        _pl = FindObjectOfType<PlayerController>();
        _cm = FindObjectOfType<CameraController>();
        _sm = FindObjectOfType<SceneManagement>();
        _wc = FindObjectOfType<WeaponController>();
        _pl.enabled = false;
        _cm.enabled = false;
        _sm.enabled = false;
        _wc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("x"))
        {
            _pl.enabled = true;
            _cm.enabled = true;
            _sm.enabled = true;
            _wc.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
