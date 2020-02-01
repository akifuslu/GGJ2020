using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public bool AllCollected { get => Counter.Value == CollectCount;}
    public int CollectCount;
    public IntReactiveProperty Health;
    public IntReactiveProperty Counter = new IntReactiveProperty(0);
    public IntReactiveProperty Chaos = new IntReactiveProperty(0);

    private Vector3 _startPos;
    private Transform _player;
    private CameraController _camera;
    private IDisposable _d;
    private IDisposable _d1;
    private IDisposable _d2;
    private IDisposable _d3;
    private IDisposable _d4;
    private IDisposable _d5;

    private GameObject _hurt;

    // Start is called before the first frame update
    void Start()
    {
        Counter.Value = 0;
        _hurt = FindObjectOfType<Canvas>().transform.Find("HurtFade").gameObject;
        _camera = FindObjectOfType<CameraController>();
        _player = FindObjectOfType<PlayerController>().transform;
        _startPos = _player.transform.position;
        FindObjectOfType<HealthView>().Bind(this);
        FindObjectOfType<ShardView>().Bind(this);
        FindObjectOfType<ChaosView>().Bind(this);
        _d1 = MessageBroker.Default.Receive<CollectedEvent>().Subscribe(ev => 
        {
            Counter.Value++;
        });

        _d2 = MessageBroker.Default.Receive<RepairCrackEvent>().Subscribe(ev =>
        {
            if (Counter.Value != CollectCount)
                return;

            _player.GetComponent<PlayerController>().enabled = false;
            _camera.enabled = false;
            var target = ev.Crack.position;
            target.y = transform.position.y;
            target.z = transform.position.z;
            _d = Observable.EveryUpdate().Subscribe(up => 
            {
                transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, Time.deltaTime);
            });
            Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ti =>
            {
                ev.Crack.GetComponent<SpriteRenderer>().sprite = ev.Fixed;
                _d.Dispose();
            });
        });

        _d3 = MessageBroker.Default.Receive<PlayerDamagedEvent>().Subscribe(ev =>
        {
            Health.Value--;
            if(Health.Value <= 0)
            {
                Debug.Log("GAME LOST");
            }
            else
            {
                _hurt.SetActive(true);
                _camera.enabled = false;
                _player.GetComponent<PlayerController>().enabled = false;
                _player.GetComponent<Collider2D>().enabled = false;
                Physics2D.gravity = Physics2D.gravity * 10;
                Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ev2 =>
                {
                    _hurt.SetActive(false);
                    Physics2D.gravity = Physics2D.gravity / 10;
                    _camera.enabled = true;
                    _player.GetComponent<PlayerController>().enabled = true;
                    _player.GetComponent<Collider2D>().enabled = true;
                    _player.position = _startPos;
                    _camera.Reset();
                    _player.GetComponent<PlayerController>().Reset();
                });
            }
        });

        _d4 = MessageBroker.Default.Receive<ChaosEvent>().Subscribe(ev =>
        {
            Chaos.Value += ev.Amount;
            if(Chaos.Value >= 100)
            {
                MessageBroker.Default.Publish(new PlayerDamagedEvent());
                Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ev2 =>
                {
                    Chaos.Value = 0;
                });
            }
        });

        _d5 = Observable.Interval(TimeSpan.FromSeconds(.25f)).Subscribe(ev => { if(Chaos.Value > 0) Chaos.Value--; });
    }

    private void OnDestroy()
    {
        _d1?.Dispose();
        _d2?.Dispose();
        _d3?.Dispose();
        _d4?.Dispose();
        _d5?.Dispose();
    }
}

public class ChaosEvent
{
    public int Amount;
}