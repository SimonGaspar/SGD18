using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTrigger : MonoBehaviour {



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
           
            int middlePoint = collision.contacts.Length / 2;
            float Angle = GetCollisionAngle(transform, collision.gameObject.GetComponent<BoxCollider2D>(), collision.contacts[middlePoint].point);

            _particle.transform.position = new Vector3(collision.contacts[middlePoint].point.x, collision.contacts[middlePoint].point.y, -0.2f);
            _particle.Simulate(0.0f, true, true);

            if ((collision.gameObject.transform.localScale.x > 0 || Angle < 135) && Angle > 45)
            {
                _particle.transform.rotation = new Quaternion(0f, 1f, 0f, 0f);

                _particle.Play();
            }
            else if ((collision.gameObject.transform.localScale.x<0 ||Angle < 315) && Angle > 225 )
            {
                _particle.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);

                _particle.Play();
            }

        }
    }

    public float GetCollisionAngle(Transform hitobjectTransform, Collider2D collider, Vector2 contactPoint)
    {
        Vector2 collidertWorldPosition = new Vector2(hitobjectTransform.position.x, hitobjectTransform.position.y);
        Vector3 pointB = contactPoint - collidertWorldPosition;

        float theta = Mathf.Atan2(pointB.x, pointB.y);
        float angle = (360 - ((theta * 180) / Mathf.PI)) % 360;
        return angle;
    }
}
