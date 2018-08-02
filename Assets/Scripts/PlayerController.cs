using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private Rigidbody2D _rb2d;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _crouchCollider;
    private Animator animator;

    [Header("Animal identifier")]
    [SerializeField] private AnimalForm _animalForm;
    public AnimalForm CurrentAnimalForm { get { return _animalForm; } }

    [Space]
    [Header("Movement")]
    [SerializeField]
    [Range(0, 15)]
    private float _maximumMovementSpeed = 5f;
    [SerializeField] [Range(0, 10)] private float _jumpSpeed = 4f;
    [SerializeField] [Range(0, 10)] [Tooltip("Speed gain per second")] private float _acceleration = 2f;
    [SerializeField] [Range(0, 5)] private float _fallGravityMultiplier = 2.5f;
    [SerializeField] [Range(0, 1)] private float _pushingSpeedModifier = 0.1f;

    [Space]
    [Header("Crouching")]
    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Percentage of maximum speed while crouching")]
    private float _crouchingMovementModifier = 0.5f;
    [SerializeField] private Transform _ceilingCheckTransform;
    [SerializeField] [Range(0, 1)] private float _ceilingCheckRadius = 0.1f;

    [Space]
    [Header("Groundcheck")]
    [SerializeField]
    [Range(0, 1)]
    private float _groundCheckRadius = 0.1f;
    [SerializeField] [Tooltip("Only needed when crouching is enabled")] private Transform _groundCheckTransform;
    [SerializeField] [Tooltip("Layers to act as a ground")] private LayerMask _groundLayerMask;

    [Space]
    [Header("Player constrains")]
    [SerializeField]
    private bool _canFly = false;
    [SerializeField] private bool _canWalk = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canCrouch = true;

    [Space]
    [Header("Particle-Animation")]
    [SerializeField]
    private ParticleSystem _particle = null;
    static ParticleSystem _currentParticle = null;
    [SerializeField] private ParticleSystem _movingParticle;
    [SerializeField] private float movingPositionX;
    [SerializeField] private float movingPositionY;
    [SerializeField] private float movingPositionZ;
    ParticleSystem movingParticle;
    bool canMove = false;

    public bool IsMoving
    {
        get
        {
            return false;
            //    return (_rb2d.velocity.x != 0 || _rb2d.velocity.y != 0);
        }
    }
    public bool CanDestroy { get { return (_animalForm.Equals(AnimalForm.Bison) && (Mathf.Abs(_currentHorizontalSpeed) == _maximumMovementSpeed)); } }

    private bool _grounded = false;
    private bool _jumping = false;
    private bool _crouching = false;
    private bool _wantToStandUp = false;

    private float _currentHorizontalSpeed = 0f;
    private int _runningIntoWall = 0; // -1 - left, 0 - nothing, 1 - right
    private float _defaultMovementModifier;
    private float _movementModifier = 1f;

    private Vector2 _defaultColliderSize;
    private Vector2 _defaultColliderOffset;
    public Vector3 defLocalScale;

    // Use this for initialization
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Assert.IsNotNull(_groundCheckTransform);
        Assert.IsNotNull(_rb2d);
        Assert.IsNotNull(_spriteRenderer);

        _defaultMovementModifier = _movementModifier;
    }

    private void Awake()
    {
        movingParticle = Instantiate<ParticleSystem>(_movingParticle);
        movingParticle.Stop();
        movingParticle.transform.position = transform.position;

		defLocalScale = transform.localScale;
		//	transform.localScale = _defLocalScale;
	}

    private void Update()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask);
		animator.SetBool("IsGrounded", _grounded);
        // I know physics calculations shouldn't be done in `Update()`, but putting them into `FixedUpdate()` creates an awful input lag
        // Just leave it here (╯°□°）╯︵ ┻━┻
        //HandleCrouching();
        if (canMove)
        {
            CalculateMovement();
        }

        if (_currentParticle != null)
            _currentParticle.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        Destroy(movingParticle);
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
    private void HandleCrouching()
    {
        if (!_canCrouch) return;
        bool canStandUp = !Physics2D.OverlapCircle(_ceilingCheckTransform.position, _ceilingCheckRadius, _groundLayerMask);

        if (Input.GetButton("Crouch")) Crouch();

        if (Input.GetButtonUp("Crouch"))
        {
            if (canStandUp) Stand();
            else _wantToStandUp = true;
        }

        if (_wantToStandUp && canStandUp) Stand();

    }
    private void CalculateMovement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        if (inputHorizontal == 0 && _grounded)
            _rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        inputHorizontal = (inputHorizontal > 0) ? Mathf.Ceil(inputHorizontal) : Mathf.Floor(inputHorizontal);

        if (_runningIntoWall == inputHorizontal) // normal goes the other way
            inputHorizontal = 0;

        if (inputHorizontal != 0)
            _currentHorizontalSpeed += inputHorizontal * _acceleration * Time.fixedDeltaTime;
        else
            _currentHorizontalSpeed = 0;

        _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_maximumMovementSpeed * _movementModifier, _maximumMovementSpeed * _movementModifier);

        // horizontal movement
        if ((_currentHorizontalSpeed > 0 || _currentHorizontalSpeed < 0) && _grounded)
        {


            if (inputHorizontal > 0)
                movingParticle.transform.position = transform.position + new Vector3(-movingPositionX, movingPositionY, movingPositionZ);
            else
                movingParticle.transform.position = transform.position + new Vector3(+movingPositionX, movingPositionY, movingPositionZ);

            animator.SetBool("IsRunning", true);
            _movingParticle.Simulate(0.0f, true, true);
            movingParticle.Play();
        }
        else
        {

            //if (!(_currentHorizontalSpeed > 0 || _currentHorizontalSpeed < 0))
            if (_currentHorizontalSpeed == 0)
                animator.SetBool("IsRunning", false);
        }

        if (inputHorizontal > 0)
            transform.localScale = defLocalScale;
        if (inputHorizontal < -0.1)
            transform.localScale = new Vector3(-defLocalScale.x, defLocalScale.y, defLocalScale.z);

        if (_canWalk || (!_canWalk && !_grounded && _canFly))
            //_rb2d.velocity = new Vector2(((inputHorizontal>0)?_currentHorizontalSpeed:-_currentHorizontalSpeed), _rb2d.velocity.y);
            _rb2d.velocity = new Vector2(_currentHorizontalSpeed, _rb2d.velocity.y);

        // jump
        if (_rb2d.velocity.y <= 0) _jumping = false;

        // because animator.SetBool("IsJumping", _jumping); is too mainstream 4Head
        if (_jumping) animator.SetBool("IsJumping", true);
        else animator.SetBool("IsJumping", false);

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

    private void OnCollisionStay2D(Collision2D other)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[16];
        int contactsCount = other.GetContacts(contacts);
        for (int i = 0; i < contactsCount; i++)
        {
            if (contacts[i].normal.x == 1 || contacts[i].normal.x == -1)
            {
                Debug.DrawRay(contacts[i].point, Vector3.one * 100);
                //_runningIntoWall = -(int)contacts[i].normal.x;
                return;
            }
        }
        _runningIntoWall = 0;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _runningIntoWall = 0;
    }

    public void Stand()
    {
        //_crouchCollider.enabled = true;
        _crouchCollider.offset = new Vector2(0f, -_crouchCollider.size.y / 2);
        _movementModifier = _defaultMovementModifier;
        _crouching = false;
        _wantToStandUp = false;
        _crouchCollider.size = _defaultColliderSize;
        _crouchCollider.offset = _defaultColliderOffset;
    }

    public void Crouch()
    {
        //_crouchCollider.enabled = false;
        _crouchCollider.size = new Vector2(_defaultColliderSize.x, _defaultColliderSize.y / 2);
        _crouchCollider.offset = new Vector2(0f, -_crouchCollider.size.y / 2);
        _movementModifier = _crouchingMovementModifier;
        _crouching = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_groundCheckTransform.position, _groundCheckRadius);
    }


    public void PlayParticle()
    {
        if (_particle != null)
        {
            canMove = false;
            if (_currentParticle == null) _currentParticle = Instantiate<ParticleSystem>(_particle);

            _currentParticle.transform.position = transform.position;
            StartCoroutine(Particle());
        }
    }
    int ToBison = 10;
    IEnumerator Particle()
    {
        //if (chosenAnimalForm == AnimalForm.Bison)
        _rb2d.velocity = new Vector2(0, 0);
        _currentParticle.Play();
        {
            float i = 0;
            switch (ToBison)
            {
                case 0:
                    while (i++ <= 4)
                    {
                        var shape = _currentParticle.shape;
                        shape.scale = new Vector3(4 + i * 3, 1, 2);
                        yield return new WaitForSeconds(1f / 32f);
                    }
                    break;
                case 1:
                    while (i++ <= 4)
                    {
                        var shape = _currentParticle.shape;
                        shape.scale = new Vector3(16 - i * 2, 1, 2);
                        yield return new WaitForSeconds(1f / 32f);
                    }
                    break;
                default: break;
            }
        }
    }
    public void SetTransformBison(int ToBison)
    {
        this.ToBison = ToBison;
    }


    public void SetMovingToTrue()
    {
        canMove = true;
    }

    public void SetMovingToFalse()
    {
        canMove = false;
    }
}
