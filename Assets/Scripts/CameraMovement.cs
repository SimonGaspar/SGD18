using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    public Camera cam;
    [SerializeField]
    bool toggleVisualDebug = true;

    [SerializeField]
    bool lookAroundEnabled = false;

    [SerializeField]
    float offsetX;
    [SerializeField]
    float offsetY;

    [SerializeField]
    [Range(0, 1)]
    float smoothAmmount;
    [SerializeField]
    [Range(0, 1)]
    float smoothOffsetAmmount;

    // this will probably not be in future updates, no more offset needed
    float horizontal;
    //    bool isLookingLeft = true;

    //checks if player left camera viev deadzone
    public bool outOfBoundsX = false;
    public bool outOfBoundsY = false;


    //if not on player, movement should be disabled
    public bool onPlayer = true;


    //our player or other focus point, can be set
    [SerializeField]
    Transform followTarget;

    float targetOffsetX;
    float targetPositionX;
    float targetPositionY;
    Vector3 offset;
    Vector2 lastCamPos = Vector3.zero;
    public Vector3 offsetView;

    public float horizontalView;
    public float verticalView;
    public float prevFrame;
    float timer;
    float offsetCamX = 0f;
    float offsetCamY = 0f;
    float longPressTimer = 0f;
    //  float flipTime = 0.9f;
    bool isLeft = false;
    bool isDown = false;

    //debug cam

    //rework, set always correct bar values, then input and folllow target check for merging issues

    //those four points serve as guidance for our camera, their images are for visual debugging, only need transform really, should change to transform tbh
    [SerializeField]
    Image upperBar;
    [SerializeField]
    Image lowerBar;
    [SerializeField]
    Image leftBar;
    [SerializeField]
    Image rightBar;
    [SerializeField]
    Texture GizmosTexture;

    //create screenspace bounds, if player moves out of the bounds, follow smmoothly
    public Bounds camDeadzone;

    [SerializeField]
    [Range(0, 1)]
    float relativeXdeadZone;

    [SerializeField]
    [Range(0, 1)]
    float DeadzoneWidth;

    [SerializeField]
    [Range(0, 1)]
    float relativeYdeadZone;

    [SerializeField]
    [Range(0, 1)]
    float DeadzoneHeight;




    Vector2 camSize;
    Vector2 relativeValues;
    Vector2 position;
    Vector3 screenTarget;
    Vector2 distanceFromCenter;

    Vector2 maxDif;



    void Start()
    {

        targetPositionX = followTarget.position.x;// + offset.x;
        targetPositionY = followTarget.position.y;// + offset.y;

        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.Log("No camera reference!");
        }
        if (followTarget == null)
        {
            Debug.Log("CameraScript: no player to follow, missing reference!");
        }

        //assign correct UI pivots and anchors, so changes in inspector dont affect code, they will only affect visual debugging
        lowerBar.rectTransform.pivot = new Vector2(0.5F, 0.5F);
        upperBar.rectTransform.pivot = new Vector2(0.5F, 0.5F);
        leftBar.rectTransform.pivot = new Vector2(0.5F, 0.5F);
        rightBar.rectTransform.pivot = new Vector2(0.5F, 0.5F);

        lowerBar.rectTransform.anchorMin = new Vector2(0.5F, 0.5F);
        upperBar.rectTransform.anchorMin = new Vector2(0.5F, 0.5F);
        leftBar.rectTransform.anchorMin = new Vector2(0.5F, 0.5F);
        rightBar.rectTransform.anchorMin = new Vector2(0.5F, 0.5F);

        lowerBar.rectTransform.anchorMax = new Vector2(0.5F, 0.5F);
        upperBar.rectTransform.anchorMax = new Vector2(0.5F, 0.5F);
        leftBar.rectTransform.anchorMax = new Vector2(0.5F, 0.5F);
        rightBar.rectTransform.anchorMax = new Vector2(0.5F, 0.5F);



        camSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
        //set correct values for deadzone at the begining, avoiding first frame bugs
        relativeValues = CalculateRelativePos(camSize, new Vector2(relativeXdeadZone, relativeYdeadZone));
        camDeadzone.center = new Vector3(relativeValues.x, relativeValues.y, 0f);
    }



    private void Update()
    {
        //used to turn on/off ui images for camera
        ToggleImages(toggleVisualDebug);
        //will get from input mamanger later
        //get arrow values , used for Viewing 

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
        /* 
		 * this will probably not be in future updates, no more offset needed
		horizontal = Input.GetAxisRaw("Horizontal");
		if (horizontal == 1)
		{
			isLookingLeft = false;
		}
		else if (horizontal == -1)
		{
			isLookingLeft = true;
		}
		*/

        //always adjusting position, if dispaly changes, should be in separate function and changed only if necessary
        lowerBar.rectTransform.position = new Vector2(camSize.y, relativeValues.y);
        upperBar.rectTransform.position = new Vector2(camSize.y, relativeValues.y + DeadzoneHeight * camSize.y);
        leftBar.rectTransform.position = new Vector2(relativeValues.x, 0f);
        rightBar.rectTransform.position = new Vector2(relativeValues.x + DeadzoneWidth * camSize.x, 0f);
        camDeadzone.size = new Vector3(rightBar.rectTransform.position.x - leftBar.rectTransform.position.x,
                                        upperBar.rectTransform.position.y - lowerBar.rectTransform.position.y, 0f);

    }

    void LateUpdate()
    {

        Vector3 currentPos = cam.transform.position;

        if (!onPlayer)
        {
            targetPositionX = lastCamPos.x + offsetView.x;
            targetPositionY = lastCamPos.y + offsetView.y;

        }
        else
        {
            if (outOfBoundsX)
            {
                targetPositionX = Mathf.Lerp(cam.transform.position.x, followTarget.position.x, 0.3f);
            }

            if (outOfBoundsY)
            {
                targetPositionY = Mathf.Lerp(cam.transform.position.y, followTarget.position.y, 0.3f);
            }
            lastCamPos = new Vector2(cam.transform.position.x, cam.transform.position.y);

        }
        //use arrows to look ahoead
        if (lookAroundEnabled)
            offsetView = VIewAhead();

        Vector3 targetPos = new Vector3(targetPositionX, targetPositionY, cam.transform.position.z) + offsetView;
        transform.position = Vector3.Lerp(currentPos, targetPos, smoothAmmount);



        //player in screen space
        screenTarget = cam.WorldToScreenPoint(followTarget.transform.position);

        //get camera size in pixels, should be called only if changed/necessary
        camSize = new Vector2(cam.pixelWidth, cam.pixelHeight);
        //	Vector2 camCenter = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);


        relativeValues = CalculateRelativePos(camSize, new Vector2(relativeXdeadZone, relativeYdeadZone));

        //get values for camere, when distance is bigger, move camera
        maxDif.x = Mathf.Abs(camDeadzone.extents.x);
        maxDif.y = Mathf.Abs(camDeadzone.extents.y);

        //players distance from center of the deadzone
        distanceFromCenter = new Vector2(camDeadzone.max.x - screenTarget.x, camDeadzone.max.y - screenTarget.y);

        //Debug.DrawRay(camDeadzone.max, Vector2.up * 100);
        //Debug.DrawLine(camDeadzone.max, screenTarget, Color.red);

        //is player in Deadzone?
        if (Mathf.Abs(distanceFromCenter.x) > maxDif.x) //bez abs potom elsef ci s abs aby som vedel z ktorej strany ide ?treba to? 
            outOfBoundsX = true;
        else
            outOfBoundsX = false;

        if (Mathf.Abs(distanceFromCenter.y) > maxDif.y)
            outOfBoundsY = true;
        else
            outOfBoundsY = false;

    }

    //create subbtle jump down effect
    public void AdjustYrelativeOffset(float target)
    {
        targetPositionY += target;
    }

    //logic for looking ahead
    Vector3 VIewAhead()
    {

        if (longPressTimer > 3f)
        {

            onPlayer = false;
            offsetCamX = (isLeft) ? horizontalView - 1.8f : horizontalView + 1.8f;
            offsetCamY = (isDown) ? verticalView - 1.2f : verticalView + 1.2f;
            longPressTimer = 0f;

        }
        if (horizontalView == 0 && verticalView == 0)
        {
            offsetCamX = 0f;
            offsetCamY = 0f;
            onPlayer = true;
        }
        else if (verticalView == 0)
        {
            offsetCamY = 0f;

        }
        else if (horizontalView == 0)
        {
            offsetCamX = 0f;
        }
        return new Vector3(offsetCamX, offsetCamY, 0f);
    }


    //returns value between 0 to 1, [0,0] is bottom left corner, [1,1] is upper right
    Vector2 CalculateRelativePos(Vector2 camSize, Vector2 deadZone)
    {
        Vector2 relativeDeadZone = Vector2.zero;
        relativeDeadZone.x = deadZone.x * camSize.x;
        relativeDeadZone.y = deadZone.y * camSize.y;
        return relativeDeadZone;
    }

    //only for debug purposes
    private void OnDrawGizmos()
    {

        camDeadzone.center = new Vector3(relativeValues.x, relativeValues.y, 0f);
        Rect rect = new Rect(camDeadzone.center, camDeadzone.size);
        Gizmos.DrawGUITexture(rect, GizmosTexture);

    }
    //only for dev builds
    void ToggleImages(bool turnOn)
    {
        lowerBar.enabled = turnOn;
        upperBar.enabled = turnOn;
        leftBar.enabled = turnOn;
        rightBar.enabled = turnOn;
    }

}
