﻿using System;
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
	private float initialYaw = 0f;
	private float initialPitch = 0f;
	private bool isCalibrated = false;
	private float smoothYaw = 0f;
	private float smoothPitch = 0f;
	public float smoothSpeed = 5f;

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
    // Kalibrasi awal hanya sekali
    if (!isCalibrated)
    {
        initialYaw = imuReader.gx;
        initialPitch = imuReader.gy;
        isCalibrated = true;
    }

    // Hitung offset dari nilai awal
    float yawOffset = imuReader.gx - initialYaw;
    float pitchOffset = -imuReader.gy - initialPitch;

    // Batasi offset agar tidak terlalu ekstrem
    pitchOffset = Mathf.Clamp(pitchOffset, -90f, 90f);
    yawOffset = Mathf.Clamp(yawOffset, -90f, 90f);

    // Interpolasi nilai agar lebih halus
    smoothPitch = Mathf.Lerp(smoothPitch, pitchOffset, Time.deltaTime * smoothSpeed);
    smoothYaw = Mathf.Lerp(smoothYaw, yawOffset, Time.deltaTime * smoothSpeed);

    // Terapkan rotasi
    rifleTransformParent.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
    transform.localRotation = Quaternion.Euler(0f, smoothYaw, 0f);
}

}