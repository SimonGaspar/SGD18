using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBubble : MonoBehaviour {

	Camera cam;
	CameraMovement cameraMovement;
	[SerializeField] Sprite spriteToShow;
	/*public Transform player;
		public void OnFormChange()
	{
		player = AnimalsManager.Instance.GetCurrentAnimalTransformComponent();
	}*/

	private void Start()
	{
		cam = Camera.main;
		cameraMovement = cam.GetComponent<CameraMovement>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag =="Player")
		{
			cameraMovement.DrawGuideBubble(spriteToShow, true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{

			cameraMovement.DrawGuideBubble(spriteToShow, false);
		}
	}
	void OnDrawGizmos()
	{

		Gizmos.DrawIcon(transform.position, "bubbleGuide", true);
	}
}
