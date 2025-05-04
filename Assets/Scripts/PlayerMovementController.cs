using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
	private const string HORIZONTAL = "Horizontal";
	private const string VERTICAL = "Vertical";
	private const string MOUSE_X = "Mouse X";
	private const string MOUSE_Y = "Mouse Y";

	[SerializeField] Scope scope;
	[SerializeField] private IMUSerialReader imuReader;
	[SerializeField] private Transform rifleTransformParent;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float minMouseSensivity;
	[SerializeField] private float maxMouseSensivity;
	[SerializeField] private float mouseSensvityChangeRate;

	private Rigidbody rb;

	private float horizontalInput;
	private float verticalInput;
	private float mouseInputX;
	private float mouseInputY;
	private float currentRotationY;
	private float currentRotationX;
	private float mouseSensivity;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		mouseSensivity = maxMouseSensivity;
		currentRotationY = transform.eulerAngles.y;
		currentRotationX = transform.eulerAngles.x;
		Cursor.lockState = CursorLockMode.Locked;
	}


	private void FixedUpdate()
	{		
		HandleTranslation();		
	}

	private void HandleTranslation()
	{
		var moveVector = new Vector3(horizontalInput, 0f, verticalInput);
		var worldMoveVector = transform.TransformDirection(moveVector);
		worldMoveVector = new Vector3(worldMoveVector.x, 0f, worldMoveVector.z);
		rb.AddForce(worldMoveVector.normalized * Time.deltaTime * moveSpeed, ForceMode.Force);
	}

	

	private void GetInput()
	{
		horizontalInput = Input.GetAxisRaw(HORIZONTAL);
		verticalInput = Input.GetAxisRaw(VERTICAL);
		mouseInputX = Input.GetAxis(MOUSE_X);
		mouseInputY = Input.GetAxis(MOUSE_Y);
		mouseSensivity = minMouseSensivity + scope.GetZoomPrc() * Mathf.Abs(minMouseSensivity - maxMouseSensivity);
	}

	private void Update()
	{
		GetInput();
		HandleRotation();
	}

	private void HandleRotation()
	{
		float pitch = imuReader.pitch;
		float yaw = imuReader.yaw;

		// Akumulasi rotasi untuk kelancaran dengan filter penghalusan
		currentRotationX += pitch * Time.deltaTime * mouseSensvityChangeRate;
		currentRotationY += yaw * Time.deltaTime * mouseSensvityChangeRate;

		// Batasi rotasi vertikal untuk mencegah kamera terbalik
		currentRotationX = Mathf.Clamp(currentRotationX, -90f, 90f);

		// Rotasi horizontal tanpa batasan
		rifleTransformParent.localRotation = Quaternion.Euler(currentRotationX, 0, 0);
		transform.localRotation = Quaternion.Euler(0, currentRotationY, 0);
	}
}