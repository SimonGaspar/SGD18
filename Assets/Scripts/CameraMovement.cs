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
    private bool toggleVisualDebug = true;
    [SerializeField]
    private bool lookAroundEnabled = false;

    [SerializeField]
    [Range(0, 1)]
    float smoothAmmount =0.2f;
   
    //if not on player, movement should be disabled
    public bool onPlayer = true;

    //our player or other focus point, can be set
    [SerializeField]
    private Transform followTarget;
	


	//set caamera center offset from player
	[SerializeField]
	Vector3 offset = new Vector3(2f,1f,0f);
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
	Vector2 deadZone = new Vector2(2f,1f);
	
	void Start()
    {
        cam = GetComponent<Camera>();

        Assert.IsNotNull(cam);
        Assert.IsNotNull(followTarget);
		

        EventsManager.Instance.formChangeDelegate += OnFormChange;
		
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
            offsetView = VIewAhead();




		centerOfScreenInWS = followTarget.position + offset;

		lowerBar.transform.position = new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y - deadZone.y);
		upperBar.transform.position = new Vector2(centerOfScreenInWS.x, centerOfScreenInWS.y + deadZone.y);
		leftBar.transform.position = new Vector2(centerOfScreenInWS.x - deadZone.x, centerOfScreenInWS.y);
		rightBar.transform.position = new Vector2(centerOfScreenInWS.x + deadZone.x, centerOfScreenInWS.y);





		distanceFromCenter.x = cam.transform.position.x - centerOfScreenInWS.x;  //float disX = cam.transform.position.x - point.x) > deadZone.x;
		distanceFromCenter.y = centerOfScreenInWS.y - cam.transform.position.y;
		screenTarget.z = 0f;
		

		Debug.DrawLine(screenTarget, Vector3.up * 100, Color.yellow);




	
        //player in screen space

        //get camera size in pixels, should be called only if changed/necessary
        //camSize = new Vector2(cam.pixelWidth, cam.pixelHeight);


       

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
		transform.position = Vector3.Lerp(currentPos, targetPos, smoothAmmount);
	}
	//create subbtle jump down effect , not used currently and is WIP
	public void AdjustYrelativeOffset(float target)
    {
       // += target;
    }

    //logic for looking ahead
    Vector3 VIewAhead()
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
	
}
