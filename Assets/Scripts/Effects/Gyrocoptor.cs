using UnityEngine;
using System.Collections;

public class Gyrocoptor : MonoBehaviour {

	enum GYRO_STATE{
		TAKING_OFF,
		FLYING,
		CRASHING
	}

	private GraphMaker graph;
	private BasicEnemyUnit target = null;
	private int gridX, gridY;
	private GYRO_STATE state = GYRO_STATE.TAKING_OFF;
	public float flightHeight = 10.0f;
	public float speed = 10.0f;
	public float acceleration = 5.0f;
	public float diveDistance = 2.0f;
	public float liftSpeed = 2.0f;
	public float crashRadius = 5.0f;
	private Rigidbody rbody;
	private Vector3 startCrashPos;
	public Timer crashTimer = new Timer(0.5f);

	// Use this for initialization
	void Awake () {
		graph = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GraphMaker> ();
		rbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
		case GYRO_STATE.TAKING_OFF:
			if (transform.position.z <= -flightHeight) {
				transform.position = new Vector3(transform.position.x, transform.position.y, -flightHeight);
				rbody.velocity = new Vector3 (rbody.velocity.x, rbody.velocity.y, 0.0f);
				state = GYRO_STATE.FLYING;
			}
			break;
		case GYRO_STATE.FLYING:
			if (GetDistFromTarget () <= diveDistance) {
				rbody.velocity = new Vector2 (0.0f, 0.0f);
				startCrashPos = transform.position;
				crashTimer.Activate ();
				state = GYRO_STATE.CRASHING;
			}
			break;
		case GYRO_STATE.CRASHING:
			crashTimer.Update (Time.deltaTime);

			// lerp towards target
			Vector3 crashPos = target != null ? target.transform.position : (Vector3) graph.GetGraphPoint(gridX, gridY).pos;
			float t = crashTimer.timePassed / crashTimer.timerStart;

			if (t < 1.0f)
				Vector3.Lerp (startCrashPos, crashPos, Mathf.Clamp (t, 0.0f, 1.0f));
			else {
				// if close enough, explode
				Destroy(gameObject);

				// does aoe damage to enemies
				GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
				foreach (GameObject enemy in enemies) {
					Vector2 toEnemy = enemy.transform.position - transform.position;
					if (toEnemy.sqrMagnitude <= crashRadius * crashRadius) {
						Destroy (enemy);
					}
				}
			}
			break;
		}
	}

	void FixedUpdate(){
		switch(state){
		case GYRO_STATE.TAKING_OFF:
			break;
		case GYRO_STATE.FLYING:
			if (GetDistFromTarget () > diveDistance) {
				// change direction towards target
				rbody.velocity = GetVecToTarget ().normalized * rbody.velocity.magnitude;
				// accelerate
				if (rbody.velocity.magnitude < speed) {
					rbody.AddForce (rbody.velocity.normalized * acceleration * Time.fixedTime);
				} else {
					rbody.velocity = rbody.velocity.normalized * speed;
				}
			}
			break;
		case GYRO_STATE.CRASHING:

			break;
		}
	}

	public void Init(BasicEnemyUnit target){
		BaseInit ();
		this.target = target;
	}

	public void Init(int gridX, int gridY){
		BaseInit ();
		this.gridX = gridX;
		this.gridY = gridY;
	}

	private void BaseInit(){
		rbody.velocity = new Vector3 (0.0f, 0.0f, -liftSpeed);
	}

	private float GetDistFromTarget(){
		return GetVecToTarget ().magnitude;
	}

	private Vector2 GetVecToTarget(){
		if (target == null) {
			return (graph.GetGraphPoint(gridX, gridY).pos - (Vector2)transform.position);
		} else {
			return ((Vector2)target.transform.position - (Vector2)transform.position);
		}
	}
}
