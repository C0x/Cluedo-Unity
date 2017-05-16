using UnityEngine;

///<summary>
/// Script attached to camera (allows player to interact with camera)
///</summary>
public class CameraController : MonoBehaviour 
{
	[Header("Panning")]
	public float panSpeed = 20f;
	public float panBorderThickness = 10f;
	public Vector2 panLimit;

	[Header("Scrolling")]
	public float scrollSpeed = 20f;
	public float minY = 35f;
	public float maxY = 60f;


	void Update () {
		Vector3 pos = transform.position;

		//pan
		if (pos.y < maxY) 
		{

			if (Input.mousePosition.y >= Screen.height - panBorderThickness) {
				pos.z += panSpeed * Time.deltaTime;
			}
			if (Input.mousePosition.y <= panBorderThickness) {
				pos.z -= panSpeed * Time.deltaTime;
			}
			if (Input.mousePosition.x >= Screen.width - panBorderThickness) {
				pos.x += panSpeed * Time.deltaTime;
			}
			if (Input.mousePosition.x <= panBorderThickness) {
				pos.x -= panSpeed * Time.deltaTime;
			}

		}

		//scroll
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

		//limits
		pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

		this.transform.position = pos;
	}
}
