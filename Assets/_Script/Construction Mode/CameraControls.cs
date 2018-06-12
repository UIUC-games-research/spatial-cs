/**
 * Camera orbit controls.
 */

using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
	const string INPUT_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";
	const string INPUT_MOUSE_X = "Mouse X";
	const string INPUT_MOUSE_Y = "Mouse Y";
	const float MIN_CAM_DISTANCE = 80f;
	const float MAX_CAM_DISTANCE = 160f;

    public bool tutorialMode;
    public Vector3 orbitPoint = new Vector3(-70f, 30f, 100f);

	// how fast the camera orbits
	[Range(2f, 15f)]
	public float orbitSpeed = 6f;

	// how fast the camera zooms in and out
	[Range(5f,20f)]
	public float zoomSpeed = 7f;

	// the current distance from pivot point (locked to Vector3.zero)
	float distance = 0f;

	void Start()
	{
		//distance = Vector3.Distance(transform.position, Vector3.zero);
		distance = Vector3.Distance(transform.position, orbitPoint);

		Light spot = gameObject.AddComponent<Light>();
		spot.type = LightType.Spot;
		spot.spotAngle = 75f;
		spot.range = 300f;
	}

	void LateUpdate()
	{
        if (!tutorialMode) // only enable player camera controls when tutorial is not running
        {
            // orbits
            if (Input.GetMouseButton(0))
            {
                float rot_x = Input.GetAxis(INPUT_MOUSE_X);
                float rot_y = -Input.GetAxis(INPUT_MOUSE_Y);

                Vector3 eulerRotation = transform.localRotation.eulerAngles;

                eulerRotation.x += rot_y * orbitSpeed;
                eulerRotation.y += rot_x * orbitSpeed;

                eulerRotation.z = 0f;

                transform.localRotation = Quaternion.Euler(eulerRotation);
                transform.position = (transform.localRotation * (Vector3.forward * -distance)) + orbitPoint;
            }

            if (Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) != 0f)
            {
                float delta = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL);

                distance -= delta * (distance / MAX_CAM_DISTANCE) * (zoomSpeed * 1000) * Time.deltaTime;
                distance = Mathf.Clamp(distance, MIN_CAM_DISTANCE, MAX_CAM_DISTANCE);
                transform.position = (transform.localRotation * (Vector3.forward * -distance)) + orbitPoint;
            }
        }
	}

    //used so far only in tutorial - change camera angle automatically without mouse use
    public void autoRotateCamera(float rot_x, float rot_y, float rot_z, float pos_x, float pos_y, float pos_z, float time)
    {
        StartCoroutine(lerpRotateCamera(rot_x, rot_y, rot_z, pos_x, pos_y, pos_z, 2f));
    }

    private IEnumerator lerpRotateCamera(float rot_x, float rot_y, float rot_z, float pos_x, float pos_y, float pos_z, float time)
    {
        Quaternion originalRotation = transform.localRotation;
        Vector3 originalPosition = transform.position;
        Quaternion targetRot = Quaternion.Euler(rot_x, rot_y, rot_z);
        Vector3 targetPos = new Vector3(pos_x, pos_y, pos_z);
        float elapsedTime = 0;

        while (elapsedTime < time) {

            transform.SetPositionAndRotation(Vector3.Lerp(originalPosition, (transform.localRotation * (Vector3.forward * -distance)) + orbitPoint, (elapsedTime / time)),
                Quaternion.Lerp(originalRotation, targetRot, (elapsedTime / time)));
            elapsedTime += Time.deltaTime * orbitSpeed;
            yield return new WaitForEndOfFrame();
        }
        // Ensure it ends in the right place no matter what.
        yield return null;
        transform.SetPositionAndRotation(targetPos, targetRot);
    }

}
