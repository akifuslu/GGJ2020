using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public int RoomCount;
    public int CurrentRoom;
    public float RoomOffset;
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

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("e") && CurrentRoom < RoomCount - 1)
        {
            transform.position = new Vector3(transform.position.x + RoomOffset, transform.position.y, transform.position.z);
            CurrentRoom++;
        }
        else if (Input.GetKeyDown("q") && CurrentRoom > 0)
        {
            transform.position = new Vector3(transform.position.x - RoomOffset, transform.position.y, transform.position.z);
            CurrentRoom--;
        }

        if(Input.GetKeyDown("x") && !_onXray)
        {
            _onXray = true;
            var go = Instantiate(Search);
            go.transform.position = transform.position;
            MessageBroker.Default.Publish(new XRayEvent() { CurRoom = CurrentRoom });
            MessageBroker.Default.Publish(new ChaosEvent() { Amount = 35 });
            Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(ev => { _onXray = false; Destroy(go.gameObject); });
        }

        if (transform.position.y < 0) // fallen
        {
            MessageBroker.Default.Publish(new PlayerDamagedEvent());
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
        CurrentRoom = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            MessageBroker.Default.Publish(new PlayerDamagedEvent());
        }
    }
}

public class PlayerDamagedEvent
{

}

public class XRayEvent
{
    public int CurRoom;
}