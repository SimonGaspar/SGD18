using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineHelper : MonoBehaviour
{

	float flipTime = 0.9f;

	float screenXValue = 0.5f;
	float prevFrame;



	[SerializeField]
	bool enableOffset = true;


	float timer;




	float offsetX = 0.5f;
	float offsetY = 0.5f;
	float longPressTimer;

	[SerializeField]
	CinemachineVirtualCamera virtualCam;

	// Use this for initialization
	void Start()
	{
		virtualCam = GetComponent<CinemachineVirtualCamera>();
	}

	// Update is called once per frame
	void Update()
	{


		float horizontal = Input.GetAxisRaw("Horizontal");

		if (enableOffset)
		{
			if (prevFrame != horizontal)
			{
				if (horizontal == 1)
				{
					screenXValue = 0.35f;
					timer = flipTime;

				}
				else if (horizontal == -1)
				{
					screenXValue = 0.65f;
					timer = flipTime;
				}
				else
				{
					screenXValue = 0.5f;
					timer = 0.1f;
				}

			}
			prevFrame = horizontal;
			timer -= Time.deltaTime;
			if (timer <= 0)
			{

				virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = screenXValue;
				timer = flipTime;
			}
		}

		Vector2 offsetView = HandleView();


		virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = offsetView.x;
		virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = offsetView.y;

		
	}

	public void ResetOffset()
	{
		virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
	}

	Vector2 HandleView()
	{

		float horizontalView = Input.GetAxisRaw("HorizontalView");
		float verticalView = Input.GetAxisRaw("VerticalView");
		bool isLeft = false;
		bool isDown = false;

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






		
		if (longPressTimer > 3f)
		{
			if (horizontalView == 0)
				offsetX = 0.5f;
			else
				offsetX = (isLeft) ? horizontalView + 1.8f : horizontalView - 0.8f;


			if (verticalView == 0)
				offsetY = 0.5f;

			else
				offsetY = (isDown) ? verticalView + 1.2f : verticalView - 0.2f;

			longPressTimer = 0f;
			
		}
		

		//needs to reset back to 0.5f if not holding anithing
		return new Vector2(offsetX, offsetY);
	}
}
