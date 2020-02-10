using Assets.Scripts.Utils;
using System;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveScalar;
    public float JumpScalar;
    public float FallScalar;
    public GameObject Search;
    public Transform[] GroundPivots;

    private bool _isGrounded;
    private Rigidbody2D _body;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private bool _onXray;

    private int _roomCount;
    private int _currentRoom;
    private float _roomOffset;

    // Start is called before the first frame update
    void Start()
    {
        var cam = FindObjectOfType<CameraController>();
        _roomCount = cam.RoomCount;
        _currentRoom = 0;
        _roomOffset = cam.RoomOffset;
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (FindObjectOfType<Tutorial>() && FindObjectOfType<Tutorial>().enabled)
            return;

        if (Input.GetKeyDown("e") && _currentRoom < _roomCount - 1)
        {
            transform.position = new Vector3(transform.position.x + _roomOffset, transform.position.y, transform.position.z);
            _currentRoom++;
        }
        else if (Input.GetKeyDown("q") && _currentRoom > 0)
        {
            transform.position = new Vector3(transform.position.x - _roomOffset, transform.position.y, transform.position.z);
            _currentRoom--;
        }

        if(Input.GetKeyDown("x") && !_onXray)
        {
            _onXray = true;
            var go = Instantiate(Search);
            go.transform.position = transform.position;
            MessageBus.Publish(new XRayEvent() { CurRoom = _currentRoom });
            MessageBus.Publish(new ChaosEvent() { Amount = 35 });
            Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(ev => { _onXray = false; Destroy(go.gameObject); });
        }

        if (transform.position.y < 0) // fallen
        {
            MessageBus.Publish(new PlayerDamagedEvent());
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown("e") || Input.GetKeyDown("q"))
            return;

        _isGrounded = CheckGrounded();
        float hort = Input.GetAxis("Horizontal");
        var velocity = _body.velocity;
        velocity.x = hort * MoveScalar;
        if((Input.GetKey("space")||Input.GetKey("w")) && _isGrounded)
        {
            velocity.y = JumpScalar;
        }
        if (!_isGrounded && velocity.y < 0)
        {
            velocity.y -= FallScalar;
        }
        _body.velocity = velocity;

        //set character facing side
        if (hort > 0)
        {
            _sprite.flipX = false;
            _anim.SetBool("Walk", true);
        }
        else if (hort < 0)
        {
            _sprite.flipX = true;
            _anim.SetBool("Walk", true);
        }
        else
        {
            _anim.SetBool("Walk", false);
        }

    }

    private bool CheckGrounded()
    {
        for (int i = 0; i < GroundPivots.Length; i++)
        {
            if (Physics2D.Raycast(GroundPivots[i].position, Vector3.down, 0.5f))
            {
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        _currentRoom = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            MessageBus.Publish(new PlayerDamagedEvent());
        }
    }
}

public class PlayerDamagedEvent : GameEvent
{

}

public class XRayEvent : GameEvent
{
    public int CurRoom;
}