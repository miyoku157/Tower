using UnityEngine;
using System.Collections;
namespace AssemblyCSharp{
public class CameraControl : MonoBehaviour
{
	public Transform target;
	Transform newTarget;
	
	public float distance;
	public float xSpeed = 2.5f;
	public float ySpeed = 2.5f;
	
	public float yMinLimit = -89f;
	public float yMaxLimit = 89f;
	
	public float distanceMin = 0.5f;
	public float distanceMax = 15f;
	
	public float smoothTime = 2f;
	
	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;
	
	float rotatingVelocityX = 0.0f;
	float rotatingVelocityY = 0.0f;
	
	bool isChangingTarget = false;
	bool simpleClick = false;
	
	float timer;
	float delay = 0.5f;
	
	RaycastHit hit;
	Ray ray;
	
	Vector3 AimedPosition;
	Quaternion AimedRotation;
	Transform StartingPosition;

	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
		
		// Make the rigid body not change rotation
		if(GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	
	void LateUpdate()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(!simpleClick)
			{
				simpleClick = true;
				timer = Time.time;
			}
		}
		
		if(simpleClick)
		{
			// if the time now is delay seconds more than when the first click started. 
			if((Time.time - timer) > delay)
			{
				simpleClick = false;
			}
		}
		
		if (target && !isChangingTarget)
		{
			if (Input.GetMouseButton(0))
			{
				rotatingVelocityX += xSpeed * Input.GetAxis ("Mouse X") * distance * 0.02f;
				rotatingVelocityY += ySpeed * Input.GetAxis ("Mouse Y") * distance * 0.02f;
			}
			
			rotationYAxis += rotatingVelocityX;
			rotationXAxis -= rotatingVelocityY;
			
			rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
			
			Quaternion toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
			Quaternion rotation = toRotation;
			
			distance = Mathf.Clamp (distance - Input.GetAxis ("Mouse ScrollWheel") * 3, distanceMin, distanceMax);
			
			
			// Empecher la caméra de se placer derrière un objet
			/*RaycastHit hit;
			if (Physics.Linecast(target.position, transform.position, out hit))
			{
				distance -= hit.distance;
			}*/
			
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
			
			rotatingVelocityX = Mathf.Lerp(rotatingVelocityX, 0, Time.deltaTime * smoothTime);
			rotatingVelocityY = Mathf.Lerp(rotatingVelocityY, 0, Time.deltaTime * smoothTime);
		}
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
}
