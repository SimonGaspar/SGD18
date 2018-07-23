using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    public ParticleSystem particle;
    ParticleSystem _particle;
    Vector3 _position;

	// Use this for initialization
	void Start () {
        _particle = Instantiate<ParticleSystem>(particle);
        _particle.Stop();
        _position = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        _particle.transform.position = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            foreach (var contact in collision.contacts)
            {
                
                float Angle = GetCollisionAngle(transform, collision.gameObject.GetComponent<BoxCollider2D>(), contact.point);
                if (Angle > 135 && Angle < 225 || Angle>0 && Angle < 45 || Angle<360 && Angle > 315) { _particle.Stop(); break; }

                if (Angle <135 && Angle>45)
                {
                    _particle.transform.rotation = new Quaternion(0.5f, 0.5f, -0.5f, -0.5f);
                }
                else if ( Angle < 315 && Angle > 225)
                {
                    _particle.transform.rotation = new Quaternion(-0.5f, 0.5f, -0.5f, 0.5f);
                }
                _particle.Simulate(0.0f, true, true);
                _particle.Play();
                
                break;
                
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _particle.Stop();
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
