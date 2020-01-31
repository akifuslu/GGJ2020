using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float MoveScalar;
    public float JumpScalar;
    public float FallScalar;
    public Transform[] GroundPivots;

    private bool _isGrounded;
    private Rigidbody _body;
    private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        _isGrounded = CheckGrounded();
        float hort = Input.GetAxis("Horizontal");
        var velocity = _body.velocity;
        velocity.x = hort * MoveScalar;
        if(Input.GetKey("w") && _isGrounded)
        {
            velocity.y = JumpScalar;
        }
        if(!_isGrounded && velocity.y < 0)
        {
            velocity.y -= FallScalar;
        }
        _body.velocity = velocity;

        //set character facing side
        if(hort > 0)
        {
            _sprite.flipX = false;
        }
        else if(hort < 0)
        {
            _sprite.flipX = true;
        }

    }

    private bool CheckGrounded()
    {
        for(int i = 0; i < GroundPivots.Length; i++)
        {
            if (Physics.Raycast(GroundPivots[i].position, Vector3.down, 0.5f))
            {
                return true;
            }
        }
        return false;
    }
}
