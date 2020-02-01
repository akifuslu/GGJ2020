using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class EnemyController : MonoBehaviour
{

    public float Speed;
    public bool Fire;
    public GameObject BoltPrefab;
    public List<Transform> Waypoints;

    private int _cur;
    private IDisposable _d;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Waypoints[0].position;
        _cur = 1;

        if (Fire)
        {
            _d = Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(ev =>
            {
                var go = Instantiate(BoltPrefab, transform.position, transform.rotation);
                Destroy(go, 1);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Waypoints[_cur].position - transform.position;
        dir.Normalize();
        transform.Translate(dir * Speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, Waypoints[_cur].position) < .1f)
        {
            _cur++;
            if (_cur == Waypoints.Count) _cur = 0;
        }
    }

    private void OnDestroy()
    {
        _d.Dispose();
    }
}
