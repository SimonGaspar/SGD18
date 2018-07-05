using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private float _minGroundNormalY = .65f;

    protected bool _grounded;
    protected Rigidbody2D _rb2d = null;
    protected Vector2 _velocity = Vector2.zero;
    protected ContactFilter2D _contactFilter;
    protected RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    protected const float _minMoveDistance = 0.001f;
    protected const float _shellRadius = 0.01f;
    protected Vector2 _groundNormal;
    protected Vector2 _targetVelocity;

    private void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    protected virtual void ComputeVelocity()
    {

    }
    void Update()
    {
        _targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    private void FixedUpdate()
    {
        _grounded = false;

        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);

        Vector2 deltaPosition = _velocity * Time.deltaTime;

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if (distance > _minMoveDistance)
        {
            int count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);
            _hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    _grounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }
                float modifiedDistance = _hitBufferList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
            _rb2d.position += move.normalized * distance;
        }
    }
}
