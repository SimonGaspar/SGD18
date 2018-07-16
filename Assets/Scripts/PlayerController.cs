using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rb2d;
    private SpriteRenderer _spriteRenderer;

    [Space]
    [Header("Movement")]
    [SerializeField] [Range(0, 15)] private float _maximumMovementSpeed = 5f;
    [SerializeField] [Range(0, 10)] private float _jumpSpeed = 4f;
    [SerializeField] [Range(0, 10)] [Tooltip("Speed gain per second")] private float _acceleration = 2f;
    [SerializeField] [Range(0, 5)] private float _fallGravityMultiplier = 2.5f;
    [SerializeField] [Range(0, 1)] [Tooltip("Percentage of maximum speed while crouching")] private float _crouchingMovementModifier = 0.5f;

    [Space]
    [Header("Groundcheck")]
    [SerializeField] [Range(0, 1)] private float _groundCheckRadius = 0.1f;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] [Tooltip("Layers to act as a ground")] private LayerMask _groundLayerMask;

    [Space]
    [Header("Player contrains")]
    [SerializeField] private bool _canFly = false;
    [SerializeField] private bool _canWalk = true;
    [SerializeField] private bool _canJump = true;

    private bool _grounded = false;
    private bool _jumping = false;
    private float _currentHorizontalSpeed = 0f;
    private bool _runningIntoWall = false;
    private float _movementModifier = 1f;
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

        Vector2 clampedVelocity = new Vector2();
        clampedVelocity.x = Mathf.Clamp(_rb2d.velocity.x, -_maximumMovementSpeed, _maximumMovementSpeed);
        clampedVelocity.y = Mathf.Clamp(_rb2d.velocity.y, -1000f, _jumpSpeed);

        _rb2d.velocity = clampedVelocity;
        _spriteRenderer.color = (_rb2d.velocity.x >= _maximumMovementSpeed) ? Color.red : Color.white;
    }

    private void CalculateMovement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (_runningIntoWall)
            inputHorizontal = 0;

        if (inputHorizontal == 0 && _grounded)
            _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

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
        if (_rb2d.velocity.y <= 0) _jumping = false;
        if (Input.GetButtonDown("Jump") && (_grounded || _canFly) && _canJump)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, _jumpSpeed);
            _jumping = true;
        }

        if (!_jumping)
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, Mathf.Clamp(_rb2d.velocity.y, -1000f, 2f));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[16];
        int contactsCount = other.GetContacts(contacts);
        for (int i = 0; i < contactsCount; i++)
        {
            if (contacts[i].normal.x == 1 || contacts[i].normal.x == -1)
            {
                _runningIntoWall = true;
                return;
            }
        }
        _runningIntoWall = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundCheckTransform.position, _groundCheckRadius);
    }
}
