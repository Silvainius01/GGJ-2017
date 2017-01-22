using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    //Camera Scroll
    float xAxisValue;
    float yAxisValue;
    float zAxisValue;
	public Vector2 scrollLimit;

    //Camera Zoom
    public float minFov = 10.0f;
    public float maxFov = 30.0f;
    public float zoomSensitivity = 10.0f;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{

        xAxisValue = Input.GetAxis("Horizontal");
        yAxisValue = Input.GetAxis("Vertical");
        zAxisValue = -Input.GetAxis("Mouse ScrollWheel");
        float fov = Camera.main.orthographicSize;

        if (Camera.current != null && (Camera.current.transform.position.x <= scrollLimit.x && Camera.current.transform.position.x >= -scrollLimit.x && Camera.current.transform.position.y <= scrollLimit.y && Camera.current.transform.position.y >= -scrollLimit.y   ) )
		{ 
			Camera.current.transform.Translate(new Vector3(xAxisValue, yAxisValue, 0));
		}

		if (transform.position.x >= scrollLimit.x)
			transform.position = new Vector3 (scrollLimit.x, transform.position.y, transform.position.z);
		if (transform.position.x <= -scrollLimit.x)
			transform.position = new Vector3 (-scrollLimit.x, transform.position.y, transform.position.z);
		if (transform.position.y >= scrollLimit.y)
			transform.position = new Vector3 (transform.position.x, scrollLimit.y, transform.position.z);
		if (transform.position.y <= -scrollLimit.y)
			transform.position = new Vector3 (transform.position.x, -scrollLimit.y, transform.position.z);

        fov += zAxisValue * zoomSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
	}
}
