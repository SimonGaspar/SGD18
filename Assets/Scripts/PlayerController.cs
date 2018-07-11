using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    [SerializeField] private float jumpTakeOffSpeed = 7f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private bool canFly = false;
    [SerializeField] private bool canWalk = true;
    
    private SpriteRenderer _spriteRenderer;
    // Use this for initialization
    void Start()
    {

    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && (_grounded || canFly))
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        if (canWalk || (!_grounded && canFly))
            _targetVelocity = move * maxSpeed;
    }


}
