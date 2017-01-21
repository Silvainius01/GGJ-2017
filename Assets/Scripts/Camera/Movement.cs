using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


	public Vector2 scrollLimit;

	void Start () {
	



	}
	
	// Update is called once per frame
	void Update () 
	{
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");
		float zAxisValue = Input.GetAxis ("Mouse ScrollWheel");
		if(Camera.current != null && (Camera.current.transform.position.x <= scrollLimit.x && Camera.current.transform.position.x >= -scrollLimit.x && Camera.current.transform.position.y <= scrollLimit.y && Camera.current.transform.position.y >= -scrollLimit.y   ) )
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
	
	}
}
