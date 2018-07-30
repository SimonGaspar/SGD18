using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType { Feather, Horn };
public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType _type;
    private Animator _animator;

    private bool _picked = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !_picked)
        {
            //GameManager.Instance.CollectiblePickup(this.gameObject, _type);
            _picked = true;
            _animator.SetTrigger("isPicked");
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
