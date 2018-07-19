using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {



    public ParticleSystem particle;
    private ParticleSystem _particle;
	// Use this for initialization
	void Start () {
        _particle = Instantiate<ParticleSystem>(particle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _particle.transform.position = collision.contacts[0].point;
            _particle.Simulate(0.0f, true, true);
            _particle.Play();
        }
    }
}
