﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType { Feather, Arm };
public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType _type;

    private bool _picked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_picked)
        {
            GameManager.Instance.CollectiblePickup(_type);
            _picked = true;
            // play animation
            Destroy(gameObject);
        }
    }
}
