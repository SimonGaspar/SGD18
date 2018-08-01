using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushableObject : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    private bool _pushable = false;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        Assert.IsNotNull(_rb2d);
        _rb2d.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController touchingController = other.gameObject.GetComponent<PlayerController>();
            if (touchingController.CurrentAnimalForm == AnimalForm.Bison)
            {
                _rb2d.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            _rb2d.bodyType = RigidbodyType2D.Static;
            _rb2d.velocity = new Vector2(0f, _rb2d.velocity.y);
        }
    }

}
