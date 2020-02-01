using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Level : MonoBehaviour
{
    public bool AllCollected { get => _counter == CollectCount;}
    public int CollectCount;
    public IntReactiveProperty Health;
    
    private int _counter;
    private Vector3 _startPos;
    private Transform _player;
    private CameraController _camera;

    // Start is called before the first frame update
    void Start()
    {
        _counter = 0;
        _camera = FindObjectOfType<CameraController>();
        _player = FindObjectOfType<PlayerController>().transform;
        _startPos = _player.transform.position;
        FindObjectOfType<HealthView>().Bind(this);
        MessageBroker.Default.Receive<CollectedEvent>().Subscribe(ev => 
        {
            _counter++;
        });

        MessageBroker.Default.Receive<RepairCrackEvent>().Subscribe(ev =>
        {
            if (_counter != CollectCount)
                return;

            _player.GetComponent<PlayerController>().enabled = false;
            _camera.enabled = false;
            var target = ev.Crack.position;
            target.y = transform.position.y;
            target.z = transform.position.z;
            Observable.EveryUpdate().Subscribe(up => 
            {
                transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, Time.deltaTime);
            });
            Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ti =>
            {
                ev.Crack.GetComponent<SpriteRenderer>().sprite = ev.Fixed;
            });
        });

        MessageBroker.Default.Receive<PlayerDamagedEvent>().Subscribe(ev =>
        {
            Health.Value--;
            if(Health.Value <= 0)
            {
                Debug.Log("GAME LOST");
                Time.timeScale = 0;
            }
            else
            {
                _player.transform.position = _startPos;
                _camera.Reset();
            }
        });
    }    
}
