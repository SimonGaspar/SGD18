using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPoint : MonoBehaviour
{
    [SerializeField] private bool _claimed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_claimed && other.gameObject.tag == "Player")
        {
            _claimed = true;
            GameManager.Instance.NextLevel();
        }
    }
}
