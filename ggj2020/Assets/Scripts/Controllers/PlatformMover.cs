using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{

    public float Speed;
    public List<Transform> Waypoints;

    private int _cur = 1;

    private void Start()
    {
        transform.position = Waypoints[0].position;
    }

    private void Update()
    {
        var dir = Waypoints[_cur].position - transform.position;
        dir.Normalize();
        transform.Translate(dir * Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, Waypoints[_cur].position) < .1f)
        {
            _cur++;
            if (_cur == Waypoints.Count) _cur = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            var t = collision.gameObject.GetComponent<Transform>();
            var dir = Waypoints[_cur].position - transform.position;
            dir.Normalize();
            t.Translate(dir * Speed * Time.deltaTime);
        }
    }
}
