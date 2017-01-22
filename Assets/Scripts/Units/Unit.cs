using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	private Rigidbody rbody;

    [Header("Navigation Options")]
    GraphMaker graph;
	public float speed = 10.0f;
	public float distToStop = 0.1f;
	public Vector2 movePos;
    public bool navigateGraph = false;
    public List<int> path;
	public float initialSpeed = 10.0f;
	protected bool stunned = false;

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
		}

    }
		
	public virtual void Attack (Unit unit) {}

	protected virtual Unit GetTarget () { return null; }

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
		movePos = pos;
	}
}
