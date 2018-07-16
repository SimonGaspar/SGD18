using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rb2d;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] [Range(0, 15)] private float _maximumMovementSpeed = 5f;
    [SerializeField] [Range(0, 10)] private float _jumpSpeed = 4f;
    [SerializeField] [Range(0, 10)] [Tooltip("Speed gain per second")] private float _acceleration = 2f;
    [SerializeField] [Range(0, 5)] private float _fallGravityMultiplier = 2.5f;

    [SerializeField] [Range(0, 1)] private float _groundCheckRadius = 0.1f;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] [Tooltip("Layers to act as a ground")] private LayerMask _groundLayerMask;

    [SerializeField] private bool _canFly = false;
    [SerializeField] private bool _canWalk = true;
    [SerializeField] private bool _canJump = true;

    private bool _grounded = false;
    private float _currentHorizontalSpeed = 0f;
    // Use this for initialization
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Assert.IsNotNull(_groundCheckTransform);
        Assert.IsNotNull(_rb2d);
        Assert.IsNotNull(_spriteRenderer);
    }

    private void Update()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask);
        // I know physics calculations shouldn't be done in `Update()`, but putting them into `FixedUpdate()` creates an awful input lag
        // Just leave it here (╯°□°）╯︵ ┻━┻
        CalculateMovement();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // jump modifier for prettier falling
        if (_rb2d.velocity.y < 0)
            _rb2d.velocity += Vector2.up * Physics2D.gravity.y * (_fallGravityMultiplier - 1) * Time.deltaTime;

        _spriteRenderer.color = (_rb2d.velocity.x >= _maximumMovementSpeed) ? Color.red : Color.white;
    }

    private void CalculateMovement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");

        inputHorizontal = (inputHorizontal > 0) ? Mathf.Ceil(inputHorizontal) : Mathf.Floor(inputHorizontal);

        if (inputHorizontal != 0)
            _currentHorizontalSpeed += inputHorizontal * _acceleration * Time.fixedDeltaTime;
        else
            _currentHorizontalSpeed = 0;

        _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_maximumMovementSpeed, _maximumMovementSpeed);
        // horizontal movement
        if (_canWalk || (!_canWalk && !_grounded && _canFly))
            _rb2d.velocity = new Vector2(_currentHorizontalSpeed, _rb2d.velocity.y);

        // jump
        if (Input.GetButtonDown("Jump") && (_grounded || _canFly) && _canJump)
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, _jumpSpeed);

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundCheckTransform.position, _groundCheckRadius);
    }
}
