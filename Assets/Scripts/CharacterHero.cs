using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHero : PhysicsObject{

    CharacterAttributes _attributes = new CharacterAttributes();
    List<CharacterAttributes> _animalForm = new List<CharacterAttributes>();
    CharacterAttributes _currentForm = null;
    int _indexOfForm = -1;

    public CharacterHero()
    {
        _animalForm.Add(_attributes);
        _animalForm.Add(new CharacterFox());
        _currentForm = _attributes;
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && (_grounded || _currentForm.CanFly))
        {
            _velocity.y = _currentForm.JumpTakeOffSpeed;
        }
        _targetVelocity = move * _currentForm.MaxSpeed;
}

    protected override void Update()
    {
        if (_animalForm.Count > 0)
            ChangeForm();
        
        _targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected override void FixedUpdate()
    {
        _grounded = false;

        _velocity += _currentForm.GravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;

        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);

        Vector2 deltaPosition = _velocity * Time.deltaTime;

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    protected override void Movement(Vector2 move, bool yMovement)
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
                if (currentNormal.y > _currentForm.MinGroundNormalY)
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

    private void ChangeForm() {
        if (Input.GetButtonDown("ChangeNext"))
        {
            _indexOfForm = _animalForm.IndexOf(_currentForm) >= _animalForm.Count - 1 ? -1 : _animalForm.IndexOf(_currentForm);
            _currentForm = _animalForm[++_indexOfForm];
        }
        else
        {
            if (Input.GetButtonDown("ChangePrevious"))
            {
                _indexOfForm = _animalForm.IndexOf(_currentForm) <= 0 ? _animalForm.Count : _animalForm.IndexOf(_currentForm);
                _currentForm = _animalForm[--_indexOfForm];
            }
        }
    }
}
