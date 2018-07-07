using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    private float jumpTakeOffSpeed = 7f;
    private float maxSpeed = 7f;
    private bool canFly = false;

    [HideInInspector] public Animal Animal;
    private SpriteRenderer _spriteRenderer;
    // Use this for initialization
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateStats()
    {
        jumpTakeOffSpeed = Animal.JumpTakeOffSpeed;
        maxSpeed = Animal.MaxSpeed;
        canFly = Animal.CanFly;
        _spriteRenderer.color = Animal.Color;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && (_grounded || canFly))
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        _targetVelocity = move * maxSpeed;
    }
}
