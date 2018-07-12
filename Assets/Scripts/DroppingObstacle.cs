using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DroppingObstacle : MonoBehaviour
{

    [SerializeField] private float _timeToDrop = 1f;

    private Rigidbody2D _rb2d;
    private bool _shouldIDrop = false;
    private float _dropCountdown;
    private bool _disableBlock = false;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rb2d);
    }

    private void Update()
    {
        if (!_disableBlock && _shouldIDrop)
        {
            _dropCountdown -= Time.deltaTime;
            if (_dropCountdown <= 0f)
            {
                _rb2d.bodyType = RigidbodyType2D.Dynamic;
                _disableBlock = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!_shouldIDrop) _dropCountdown = _timeToDrop;
            _shouldIDrop = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "drop", true);
    }

}
