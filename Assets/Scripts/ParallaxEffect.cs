using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
	
    private Vector2 startingPosition;
    private float startingZ;
	Vector2 camMoveSinceStart=>(Vector2)cam.transform.position-startingPosition;
	float zDistanceFromTarget=>transform.position.z-followTarget.position.z;
	float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget < 0 ? cam.nearClipPlane:cam.farClipPlane));
	float parallaxFactor => Mathf.Abs(zDistanceFromTarget) * clippingPlane;

	private void Start()
	{
		startingPosition = transform.position;
		startingZ = transform.position.z;
	}

	private void Update()
	{
		Vector2 newPosition = startingPosition + camMoveSinceStart / parallaxFactor;
		transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
	}
}
