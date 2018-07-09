using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boostrapper : MonoBehaviour
{

    [SerializeField] private GameObject[] _managers = new GameObject[3];

    private void Awake()
    {
        foreach (GameObject m in _managers)
        {
            Instantiate(m, transform);
        }
    }

}
