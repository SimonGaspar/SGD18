using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    private Animation _destroyAnimation;

    private void Start()
    {
        _destroyAnimation = GetComponent<Animation>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.CanDestroy)
            {
               // playerController.SetMovingToFalse();
				if (_destroyAnimation != null)
				{
					_destroyAnimation.Play();
					
					Destroy(GetComponent<DestroyableObject>());
				}
				else
				{ Destroy(gameObject);
				}
            }
        }
    }
}
