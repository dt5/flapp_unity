using UnityEngine;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class MainCameraController : MonoBehaviour
{
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	public float distanceMin = .5f;
	public float distanceMax = 20f;
	public float smoothTime = 0.1f;
	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;
	float velocityX = 0.0f;
	float velocityY = 0.0f;

	Quaternion fromRotation;
	Quaternion toRotation;
	Quaternion rotation;
	// Use this for initialization
	void Start()
	{
		rotation = Quaternion.Euler (0, 0, 0);
		transform.rotation = rotation;
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	void LateUpdate()
	{
		if (target) {
			if (Input.GetMouseButton (0)) {
				velocityX = xSpeed * Input.GetAxis ("Mouse X") * distance * 0.02f;
				velocityY = ySpeed * Input.GetAxis ("Mouse Y") * distance * 0.02f;
			
				rotationYAxis += velocityX;
				rotationXAxis -= velocityY;
				rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
				fromRotation = Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
				rotation = toRotation;
				transform.rotation = rotation;
				velocityX = 0;
				velocityY = 0;
				//velocityX = Mathf.Lerp (velocityX, 0, Time.deltaTime * smoothTime);
				//velocityY = Mathf.Lerp (velocityY, 0, Time.deltaTime * smoothTime);
			}

			distance = Mathf.Clamp (distance - Input.GetAxis ("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) {
				distance -= hit.distance;
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.position = position;
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
