using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    [SerializeField] private float jumpTakeOffSpeed = 7f;
    [SerializeField] private float maxSpeed = 7f;

    // Use this for initialization
    void Start()
    {

    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (_velocity.y > 0) _velocity.y = _velocity.y * .5f;
        }
        _targetVelocity = move * maxSpeed;
    }
}
