using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int RoomId;

    private List<PlatformEffector2D> _platforms;
    private IDisposable _d;

    // Start is called before the first frame update
    void Start()
    {
        _platforms = GetComponentsInChildren<PlatformEffector2D>().ToList();
        _d = MessageBroker.Default.Receive<XRayEvent>().Subscribe(ev =>
        {
            if (ev.CurRoom == RoomId)
                return;

            foreach (var plat in _platforms)
            {
                var pos = plat.transform.position;
                pos.x += 20 * (ev.CurRoom - RoomId);
                var go = Instantiate(plat, pos, plat.transform.rotation);
                go.GetComponent<Collider2D>().enabled = false;
                go.GetComponent<PlatformEffector2D>().enabled = false;
                go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .35f);
                Destroy(go.GetComponent<PlatformMover>());
                Destroy(go.gameObject, 1);
            }

        });
    }

    void OnDestroy()
    {
        _d?.Dispose();
    }
}
