using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	private Rigidbody rbody;
	public float initialSpeed = 10.0f;
	protected float speed = 10.0f;
	private Vector2 movePos;
	public float distToStop = 0.1f;
	protected bool stunned = false;

	// Use this for initialization
	void Awake () {
		rbody = GetComponent<Rigidbody> ();
		movePos = transform.position;
		speed = initialSpeed;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (!stunned) {
			Vector2 toMoveTarget = (movePos - (Vector2)transform.position);
			if (toMoveTarget.magnitude <= distToStop) {
				rbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			} else {
				rbody.velocity = toMoveTarget.normalized * speed;
			}
		}
	}
		
	public virtual void Attack (Unit unit) {}

	protected virtual Unit GetTarget () { return null; }

	protected void FaceDirection(Vector2 dir){
		dir = dir.normalized;
		transform.rotation = Quaternion.LookRotation (dir);
	}

	protected void FacePosition(Vector2 pos){
		Vector2 toPos = pos - (Vector2)transform.position;
		FaceDirection (toPos.normalized);
	}

	protected virtual void Move(Vector2 pos){
		movePos = pos;
	}
}
