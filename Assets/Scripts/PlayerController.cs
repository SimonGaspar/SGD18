using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    [SerializeField] private float jumpTakeOffSpeed = 7f;

    [SerializeField] private bool canFly = false;
    [SerializeField] private bool canWalk = true;
    [SerializeField] private bool canJump = true;

    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float acceleration = 1f;

    private float currentSpeed = 0f;

    private SpriteRenderer _spriteRenderer;


    // Use this for initialization
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ComputeVelocity()
    {
        _targetVelocity = Vector2.zero;
        float horizontalSpeed = Input.GetAxis("Horizontal");

        horizontalSpeed = (horizontalSpeed > 0) ? Mathf.Ceil(horizontalSpeed) : Mathf.Floor(horizontalSpeed);

        if (horizontalSpeed != 0)
        {
            currentSpeed += horizontalSpeed * acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = 0;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        if (Mathf.Abs(currentSpeed) == maxSpeed)
        {
            _spriteRenderer.color = Color.red;
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }

        Vector2 move = Vector2.zero;
        move.x = currentSpeed;
        if (canJump && Input.GetButtonDown("Jump") && (_grounded || canFly))
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        if (canWalk || (!_grounded && canFly))
            _targetVelocity = move;
    }
}
