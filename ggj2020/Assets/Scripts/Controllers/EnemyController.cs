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
    public List<AudioClip> Sounds;

    private int _cur;
    private IDisposable _d;
    private AudioSource _audio;
    private IDisposable _d1;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        transform.position = Waypoints[0].position;
        _cur = 1;
        float sec = UnityEngine.Random.Range(3, 10);

        if (Fire)
        {
            _d = Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(ev =>
            {
                var go = Instantiate(BoltPrefab, transform.position, transform.rotation);
                Destroy(go, 1);
            });
        }

        _d1 = Observable.Interval(TimeSpan.FromSeconds(sec)).Subscribe(ev =>
        {
            int i = UnityEngine.Random.Range(0, Sounds.Count);
            _audio.PlayOneShot(Sounds[i]);
        });
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
        _d?.Dispose();
        _d1?.Dispose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
