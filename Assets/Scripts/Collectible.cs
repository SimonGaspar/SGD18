﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private AnimalForm _collectibleFor;
    private Animator _animator;

    private bool _picked = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !_picked)
        {
            GameManager.Instance.CollectiblePickup(this.gameObject, _collectibleFor);
            _picked = true;
            if (_animator) _animator.SetTrigger("isPicked");
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
