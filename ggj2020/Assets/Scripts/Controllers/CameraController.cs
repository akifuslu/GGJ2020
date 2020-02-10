using Assets.Scripts.Game;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int RoomCount;
    public IntReactiveProperty CurrentRoom;
    public float RoomOffset;
    public List<AudioClip> Warps;
    private AudioSource _audio;
    private float _targetPos;
    private LocationView _locationView;
    private PlayerController _pl;
    private CameraController _cm;
    private SceneLoader _sm;
    private WeaponController _wc;
    private IDisposable _d;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _targetPos = transform.position.x;
        _locationView = FindObjectOfType<LocationView>();
        _locationView.Bind(this);
        _pl = FindObjectOfType<PlayerController>();
        _cm = FindObjectOfType<CameraController>();
        _sm = FindObjectOfType<SceneLoader>();
        _wc = FindObjectOfType<WeaponController>();
        _pl.enabled = false;
        _cm.enabled = false;
        _sm.enabled = false;
        _wc.enabled = false;
        transform.position = new Vector3(transform.position.x + (RoomCount-1) * RoomOffset, transform.position.y, transform.position.z);
        _d = Observable.EveryUpdate().Subscribe(ev =>
        {
            transform.Translate(Vector3.left * Time.deltaTime * 5);
            if (transform.position.x <= -10)
            {
                _d.Dispose();
                _pl.enabled = true;
                _cm.enabled = true;
                _sm.enabled = true;
                _wc.enabled = true;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Tutorial>() && FindObjectOfType<Tutorial>().enabled)
            return;

        if (Input.GetKeyDown("e") && CurrentRoom.Value < RoomCount - 1)
        {
            _audio.PlayOneShot(Warps[0]);
            _targetPos += RoomOffset;
            CurrentRoom.Value++;
            MessageBus.Publish(new ChaosEvent() { Amount = 5});
        }
        else if(Input.GetKeyDown("q") && CurrentRoom.Value > 0)
        {
            _audio.PlayOneShot(Warps[1]);
            _targetPos -= RoomOffset;
            CurrentRoom.Value--;
            MessageBus.Publish(new ChaosEvent() { Amount = 5 });
        }
        
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, _targetPos, Time.deltaTime * 5), transform.position.y, transform.position.z);
    }

    public void Reset()
    {
        _targetPos -= CurrentRoom.Value * RoomOffset;
        CurrentRoom.Value = 0;
    }
}
