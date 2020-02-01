using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int RoomId;

    private List<PlatformEffector2D> _platforms;

    // Start is called before the first frame update
    void Start()
    {
        _platforms = GetComponentsInChildren<PlatformEffector2D>().ToList();
        MessageBroker.Default.Receive<XRayEvent>().Subscribe(ev =>
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
                Destroy(go.gameObject, 1);
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
