using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHero : PhysicsObject
{

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

    private void ChangeForm()
    {
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
