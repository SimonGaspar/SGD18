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
        Debug.Log(SettingsManager.Instance.GAME_RESOLUTION);
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = jumpTakeOffSpeed;
        }
        _targetVelocity = move * maxSpeed;
    }
}
