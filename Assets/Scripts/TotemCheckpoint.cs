using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemCheckpoint : MonoBehaviour
{
    [SerializeField] private bool _claimed = false;
    public bool Claimed { get { return _claimed; } set { _claimed = value; } }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_claimed && other.tag == "Player")
        {
            GameManager.Instance.SaveGame(this.transform);
            _claimed = true;
        }
    }

}
