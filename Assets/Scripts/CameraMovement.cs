using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private bool lookAroundEnabled = false;


	[SerializeField]
	[Range(0, 1)]
	float smoothAmmount = 0.07f;

	//if not on player, movement should be disabled
	public bool onPlayer = true;

	//our player or other focus point, can be set
	[SerializeField]
	private Transform followTarget;


	//empty bubble image above player
	[SerializeField]
	SpriteRenderer bubbleHolder;
	//its child with bubble content
	private SpriteRenderer bubbleChildSprite;
	


	//set caamera center offset from player
	[SerializeField]
	Vector3 offset = new Vector3(0.06f, 0.5F, 0f);
	Vector2 lastCamPos = Vector3.zero;

	[SerializeField]
	Vector3 offsetView;
	Vector3 currentPos;


	public float horizontalView;
	public float verticalView;


	//used to look ahead
	Vector2 offsetCam;
	//variables for looking ahead
	float longPressTimer = 0f;
	bool isLeft = false;
	bool isDown = false;


	//those four points serve as guidance for our camera
	[SerializeField]
	Transform upperBar;
	[SerializeField]
	Transform lowerBar;
	[SerializeField]
	Transform leftBar;
	[SerializeField]
	Transform rightBar;

	[SerializeField]
	Vector3 lookAhead = new Vector3(3f, 4f, 0f);



	Vector3 screenTarget;

	Vector3 centerOfScreenInWS;

	//distance deazone on axis
	Vector2 distanceFromCenter;
	//deadzone if distance is greater along axis, move camera 
	[SerializeField]
	Vector2 deadZone = new Vector2(0.5f, 0.5f);
	SpriteRenderer[] spriteRenderers;
	[SerializeField] Vector3 bubbleOffset =new Vector3(1.35f, 1.35f,0f);
	Animator bubbleAnim;

	private void Awake()
	{

	
	}
	void Start()
	{
		cam = GetComponent<Camera>();

		Assert.IsNotNull(cam);
		Assert.IsNotNull(followTarget);


		EventsManager.Instance.formChangeDelegate += OnFormChange;
		//find first child object, ignore parent

		spriteRenderers = bubbleHolder.gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer sprite in spriteRenderers)
		{
			if (sprite.transform.parent != null)
				bubbleChildSprite = sprite;
		}
		//disable bubble at start
		//bubbleHolder.enabled = false;
		//bubbleChildSprite.enabled = false;
		bubbleAnim = bubbleHolder.GetComponent<Animator>();

	}

	public void OnFormChange()
	{
		followTarget = AnimalsManager.Instance.GetCurrentAnimalTransformComponent();
	}
	private void Update()
	{

		if (lookAroundEnabled)
		{
			horizontalView = Input.GetAxisRaw("HorizontalView");
			verticalView = Input.GetAxisRaw("VerticalView");

			if (horizontalView == -1)
			{
				isLeft = true;
				longPressTimer += 0.1f;

			}
			if (horizontalView == 1)
			{
				isLeft = false;
				longPressTimer += 0.1f;
			}

			if (verticalView == -1)
			{
				isDown = true;
				longPressTimer += 0.1f;

			}
			if (verticalView == 1)
			{
				isDown = false;
				longPressTimer += 0.1f;
			}
		}

	}

	void FixedUpdate()
	{

		currentPos = transform.position;

		//use arrows to look ahoead
		if (lookAroundEnabled)
			offsetView = ViewAhead();




		centerOfScreenInWS = followTarget.position + offset;

		lowerBar.transform.position = new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y - deadZone.y);
		upperBar.transform.position = new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y + deadZone.y);
		leftBar.transform.position = new Vector2(centerOfScreenInWS.x - deadZone.x, centerOfScreenInWS.y);
		rightBar.transform.position = new Vector2(centerOfScreenInWS.x + deadZone.x, centerOfScreenInWS.y);





		distanceFromCenter.x = cam.transform.position.x - centerOfScreenInWS.x;  //float disX = cam.transform.position.x - point.x) > deadZone.x;
		distanceFromCenter.y = centerOfScreenInWS.y - cam.transform.position.y;
		screenTarget.z = 0f;


		Debug.DrawLine(screenTarget, Vector3.up * 100, Color.yellow);



		if (!onPlayer)
		{
			screenTarget.x = lastCamPos.x + offsetView.x;
			screenTarget.y = lastCamPos.y + offsetView.y;

		}
		else
		{
			if (Mathf.Abs(distanceFromCenter.x) > deadZone.x)
			{
				screenTarget.x = centerOfScreenInWS.x + ((distanceFromCenter.x < 0) ? -deadZone.x : deadZone.x);
			}

			if (Mathf.Abs(distanceFromCenter.y) > deadZone.y)
			{
				screenTarget.y = centerOfScreenInWS.y + ((distanceFromCenter.y < 0) ? deadZone.y : -deadZone.y);
			}
			lastCamPos = new Vector2(cam.transform.position.x, cam.transform.position.y);
		}

		StartCoroutine(WaitForPlayerMovement());
	}
	IEnumerator WaitForPlayerMovement()
	{
		yield return new WaitForFixedUpdate();
		Vector3 targetPos = new Vector3(screenTarget.x, screenTarget.y, cam.transform.position.z);// + offsetView;
		bubbleHolder.transform.position = followTarget.transform.position+bubbleOffset;
		transform.position = Vector3.Lerp(currentPos, targetPos, smoothAmmount);
	}
	//create subbtle jump down effect , not used currently and is WIP
	/*public void AdjustYrelativeOffset(float target)
	{
		// += target;
	}*/

	//logic for looking ahead
	Vector3 ViewAhead()
	{

		if (longPressTimer > 3f)
		{

			onPlayer = false;
			offsetCam.x = (isLeft) ? horizontalView - lookAhead.x : horizontalView + lookAhead.x;
			offsetCam.y = (isDown) ? verticalView - lookAhead.y : verticalView + lookAhead.y;
			longPressTimer = 0f;

		}
		if (horizontalView == 0 && verticalView == 0)
		{
			offsetCam = Vector2.zero;
			onPlayer = true;
		}
		else if (verticalView == 0)
		{
			offsetCam.y = 0f;

		}
		else if (horizontalView == 0)
		{
			offsetCam.x = 0f;
		}
		return new Vector3(offsetCam.x, offsetCam.y, 0f);
	}

	public void DrawGuideBubble(Sprite spriteObject, bool enabled)
	{
		if (!enabled)
		{
			//bubbleHolder.enabled = false;
			bubbleChildSprite.enabled = false;
			bubbleAnim.Play("FadeOut");
		}
		else
		{
			//bubbleHolder.enabled = true;
			bubbleChildSprite.enabled = true;
			//relativeSize = new Vector2(0.3f, 0.3f);
			bubbleChildSprite.sprite = spriteObject;
			bubbleAnim.Play("FadeIn");
		}
	}



	private void OnDrawGizmos()
	{

		Gizmos.DrawIcon(new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y + deadZone.y), "arrowU", true);
		Gizmos.DrawIcon(new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y - deadZone.y), "arrowD", true);
		Gizmos.DrawIcon(new Vector2(centerOfScreenInWS.x - deadZone.x, centerOfScreenInWS.y), "arrowL", true);
		Gizmos.DrawIcon(new Vector2(centerOfScreenInWS.x + deadZone.x, centerOfScreenInWS.y), "arrowR", true);
	}
}
