using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalTrigger : MonoBehaviour {


    public ParticleSystem particle;
    private ParticleSystem _particle;
    public bool IsDrowing = false;
    public bool IsPlaying = false;
    // Use this for initialization
    void Start()
    {
        _particle = Instantiate<ParticleSystem>(particle);
        IsPlaying = false;
    }

    private void Awake()
    {
        IsPlaying = false;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            int middlePoint = collision.contacts.Length / 2;
            float Angle = GetCollisionAngle(transform, collision.gameObject.GetComponent<BoxCollider2D>(), collision.contacts[middlePoint].point);

           

            if (Angle < 90 && Angle>=0 || Angle >=270 && Angle<=360)
            {
                if (!IsPlaying)
                {
                    _particle.transform.position = new Vector3(collision.contacts[middlePoint].point.x, collision.contacts[middlePoint].point.y, -0.2f);
                    _particle.Simulate(0.0f, true, true);
                    _particle.Play();
                }
            }
            if (IsDrowing && !IsPlaying)
            {
                StartCoroutine(Drowing());
            }
        }
    }
	

    private IEnumerator Drowing()
    {
        IsPlaying = true;
        yield return new WaitForSeconds(1 / 5);
        for (int i = 0; i < 50; i++) {
            var _transform = transform;
            _transform.position = _transform.position - new Vector3(0, 0.023f / 12f, 0);
            yield return new WaitForSeconds(1 / 50);
        }
        for (int i = 0; i < 50; i++)
        {
            var _transform = transform;
            _transform.position = _transform.position + new Vector3(0, 0.033f/12f,0 );
            yield return new WaitForSeconds(1 / 50);
        }
        for (int i = 0; i < 50; i++)
        {
            var _transform = transform;
            _transform.position = _transform.position - new Vector3(0, 0.010f / 12f, 0);
            yield return new WaitForSeconds(1 / 50);
        }
        IsPlaying = false;
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
