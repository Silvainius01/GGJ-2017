using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	private Rigidbody rbody;

    [Header("Navigation Options")]
	public float initialSpeed = 10.0f;
	public float distToStop = 0.1f;
    public bool navigateGraph = false;
    public bool destroyOnPathCompletion = false;
	public float speed { get; set; }
    [Header("Navigation Info")]
    public bool hasCompletedPath = false;
	public Vector2 movePos;
    public List<int> path = new List<int>();

	[Header("Attack Stats")]
	public float range = 10.0f;
	public float killChance = 0.1f;
	public Timer attackTimer = new Timer(1.0f);
	protected MilitiaController currTarget = null;


	// vision variables
	[Header("attack vision")]
	public float visionAngle = 30.0f;
	public float visionRange = 10.0f;

    public bool ownedByPlayer = false;
    public bool isSelected = false;
	protected bool stunned = false;

	// Use this for initialization
	void Awake ()
    {
        rbody = GetComponent<Rigidbody> ();
		movePos = transform.position;
		speed = initialSpeed;

        if (path == null)
            path = new List<int>();
	}

    // Update is called once per frame
    public virtual void Update()
    {
        if (!stunned)
        {
            if (path.Count > 0)
                hasCompletedPath = false;

            if (!hasCompletedPath)
            {
                Vector2 toMoveTarget = (movePos - (Vector2)transform.position);

                if (toMoveTarget.magnitude <= distToStop)
                {
                    if (navigateGraph)
                    {
                        path.RemoveAt(0);
                        if (path.Count <= 0)
                        {
                            if (destroyOnPathCompletion)
                            {
                                Destroy(this.gameObject);
                                return;
                            }
                         hasCompletedPath = true;
                        }
                        else
                            movePos = GraphMaker.Instance.PointPos(path[0]);
                    }
                    else
                        hasCompletedPath = true;

                    rbody.velocity = Vector3.zero;
                }
                else
                {
                    rbody.velocity = toMoveTarget.normalized * speed;
                }
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
		path = GraphMaker.Instance.GetPath (transform.position, pos);
		navigateGraph = true;
	}
}
