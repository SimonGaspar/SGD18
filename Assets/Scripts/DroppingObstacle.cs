using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DroppingObstacle : MonoBehaviour
{

    [SerializeField] private float _timeToDrop = 1f;

    private Rigidbody2D _rb2d;
    private bool _shouldIDrop = false;
    private bool _disableBlock = false;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rb2d);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !_disableBlock)
        {
            StartCoroutine(DropObject());
            _disableBlock = true;
        }
    }

    IEnumerator DropObject()
    {
        yield return new WaitForSeconds(_timeToDrop);
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "drop", true);
    }
}
