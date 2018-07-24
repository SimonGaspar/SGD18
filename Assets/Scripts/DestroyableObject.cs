using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            print(playerController.CanDestroy);
            if (playerController.CanDestroy)
            {
                playerController.DisableMovement();
                Destroy(gameObject);
            }
        }
    }
}
