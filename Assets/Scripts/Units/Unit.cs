using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	private Rigidbody rbody;
	public float initialSpeed = 10.0f;
	protected bool stunned = false;

    [Header("Navigation Options")]
    protected GraphMaker graph;
	public float speed = 10.0f;
	public float distToStop = 0.1f;
	public Vector2 movePos;
    public bool navigateGraph = false;
    public List<int> path;

	[Header("Attack Stats")]
	public float range = 10.0f;
	public float killChance = 0.1f;
	public Timer attackTimer = new Timer(1.0f);
	protected MilitiaController currTarget = null;

	// vision variables
	[Header("attack vision")]
	public float visionAngle = 30.0f;
	public float visionRange = 10.0f;

	// Use this for initialization
	void Awake ()
    {
        graph = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GraphMaker>();
		rbody = GetComponent<Rigidbody> ();
		movePos = transform.position;
		speed = initialSpeed;
	}

    // Update is called once per frame
    public virtual void Update()
    {
		if (!stunned) {
			// move
			if (navigateGraph)
			{
				if (path.Count <= 0)
					path = graph.GetRandomPathFrom(transform.position);
				movePos = graph.PointPos(path[0]);
			}

			Vector2 toMoveTarget = (movePos - (Vector2)transform.position);
			if (toMoveTarget.magnitude <= distToStop)
			{
				if (navigateGraph)
					path.RemoveAt(0);
				rbody.velocity = Vector3.zero;
			}
			else
			{
				rbody.velocity = toMoveTarget.normalized * speed;
			}

			// attack
			if (currTarget != null) {
				Attack (currTarget);
			}
		}

    }
		
	public virtual void Attack (Unit unit) {
		attackTimer.Update (Time.deltaTime);

		if (attackTimer.hasFired && inRange(unit.transform.position, range)) {
			if (UnityEngine.Random.Range (0.0f, 1.0f) <= killChance) {
				// kill the unit
				Destroy (unit.gameObject);
			}
			attackTimer.Activate ();
		}
	}

	protected bool inRange(Vector2 pos, float range){
		Vector2 toPos = pos - (Vector2)transform.position;
		return toPos.sqrMagnitude <= range * range;
	}

	public float GetCurrentAngle(bool returnDegrees = false)
	{
		if (returnDegrees)
			return (transform.eulerAngles.z);
		return (transform.eulerAngles.z) * Mathf.Deg2Rad;
	}

	public Vector2 GetCurrentDirection()
	{
		float currentAngle = GetCurrentAngle();
		return new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
	}

	protected void FaceDirection(Vector2 dir)
    {
		dir = dir.normalized;
		transform.rotation = Quaternion.LookRotation (dir);
	}

	protected void FacePosition(Vector2 pos)
    {
		Vector2 toPos = pos - (Vector2)transform.position;
		FaceDirection (toPos.normalized);
	}

	protected virtual void Move(Vector2 pos)
    {
		navigateGraph = false;
		path.Clear ();
		movePos = pos;
	}

	protected virtual void MoveAlongShortestPath(Vector2 pos){
		path = graph.GetPath (transform.position, pos);
		navigateGraph = true;
	}
}
