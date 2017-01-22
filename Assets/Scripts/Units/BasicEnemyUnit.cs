using UnityEngine;
using System.Collections.Generic;

public class BasicEnemyUnit : Unit {

	public enum EnemyEffect
	{
		FAT,
		DRUNK,
		PLAGUE,
		DUEL_WINNER,
		NONE
	}

	private BitSet activeEffects = new BitSet();
	private GraphMaker.GraphPoint graphPoint = null;
	private float[] temptationValues = new float[(int)EnemyEffect.NONE];
	private SpecialBuilding targetBuilding = null;
	private SpecialBuilding lastBuilding = null;
	private List<SpecialBuilding> visitedBuildings = new List<SpecialBuilding> ();

	// drunk variables
	[Header("Drunk Variables")]
	private float speedLostFromBeingDrunk;
	private Timer drunkTimer = new Timer(1.0f);
	private float bonusBuildingWant = 0.0f;


	// Use this for initialization
	void Start () {
		graphPoint = graph.GetGraphPoint (transform.position);

		for (int i = 0; i < temptationValues.Length; ++i) {
			temptationValues [i] = UnityEngine.Random.Range (0.1f, 0.9f);
		}
	}

	public override void  Update () {
        base.Update();


		// analyze surrounding if changing graph position
		GraphMaker.GraphPoint newGraphPoint = graph.GetGraphPoint (transform.position);
		if (graphPoint.pos != newGraphPoint.pos) {
			graphPoint = newGraphPoint;
			AnalyzeNewSurroundings ();
		}

		/*
		// update being drunk
		if (drunkTimer.Update (Time.deltaTime)) {
			RemoveDrunk ();
		}

		// look for target
		if (currTarget == null) {
			currTarget = GetTarget ();
			if (currTarget != null) {
				FocusOnUnit (currTarget);
			}
		} else {
			// try to attack the current target
		}
		*/
	}

	private void MoveToQueen(){
		MoveAlongShortestPath (graph.PointPos(graph.finalIndex));
	}

	protected MilitiaController GetTarget (){
		// see if a queen unit is in line of sight and in range
		GameObject closest = null;
		float distSqr = float.MaxValue;
		GameObject[] militia = GameObject.FindGameObjectsWithTag("QueenUnit");
		foreach (GameObject unit in militia) {
			Vector2 toTarget = unit.transform.position - transform.position;
			if (toTarget.sqrMagnitude <= (visionRange * visionRange) &&
			   Vector2.Dot (toTarget.normalized, GetCurrentDirection ()) >= Mathf.Cos (visionAngle * Mathf.Deg2Rad)) {
				if (toTarget.sqrMagnitude < distSqr) {
					distSqr = toTarget.sqrMagnitude;
					closest = unit;
				}
			}
		}

		if (closest == null)
			return null;
		else
			return closest.GetComponent<MilitiaController> ();
	}

	public void FocusOnUnit(MilitiaController unit){
		currTarget = unit;
		MoveAlongShortestPath (unit.transform.position);
		attackTimer.Activate ();
	}

	public void AnalyzeNewSurroundings(){
		// if moved into a new grid position, Analyze new surroundings for places to go
		int x = 0, y = 0;
		graph.GetGraphPointXYGridCoords (graphPoint, ref x, ref y);
		//Debug.Log ("current pos: " + x + ", " + y);

		// look at surrounding building to determine want to enter any
		SpecialBuilding buildingToEnter = null;
		float bestEnterValue = 0.0f;
		for (int i = x - 1; i <= x + 1; ++i) {
			for (int j = y - 1; j <= y + 1; ++j) {
				//Debug.Log ("checking x: " + i + " y: " + j);
				if (i < 0 || i >= graph.colLength || j < 0 || j >= graph.rowLength || i == x || j == y) {
					//Debug.Log ("skipping: " + i + " " + j);
					continue;
				}
				//Debug.Log ("type checked: " + graph.GetGridType (i, j));
				if (graph.GetGridType (i, j) == GraphMaker.GRID_TYPE.SPECIAL_BUILDING) {
					SpecialBuilding building = graph.GetGraphPoint (i, j).occupyingObj.GetComponent<SpecialBuilding>();

					// skip if just in that building
					if(building == lastBuilding || visitedBuildings.Contains(building)) continue;

					float want = WantsToEnterBuilding (building);
					//Debug.Log ("checking build: " + building.buildingType + " want: " + want);
					if (want != 0.0f && want > bestEnterValue) {
						want = bestEnterValue;
						buildingToEnter = building;
					}
				}
			}
		}

		if (buildingToEnter != null) {
			Debug.Log ("entering building");
			// move towards the target building
			targetBuilding = buildingToEnter;
			Move (targetBuilding.transform.position);
		}
	}

	public void EnterBuilding(SpecialBuilding building){
		targetBuilding = null;
		visitedBuildings.Add (building);
		building.EnterBuilding (this);
	}

	public void BuildingExited(){
		lastBuilding = targetBuilding;
		targetBuilding = null;
		MoveToQueen ();
		Debug.Log ("exited building");
	}

	public float WantsToEnterBuilding(SpecialBuilding building){
		float randomWant = Mathf.Min(1.0f, UnityEngine.Random.Range (0.0f, 1.0f) + bonusBuildingWant);
		if (randomWant > temptationValues [(int)building.buildingType]) {
			// determine the strength of the want
			return (randomWant - temptationValues [(int)building.buildingType]) / (1.0f - temptationValues [(int)building.buildingType]);
		} else {
			// don't want at all
			return 0.0f;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		// check to see if collided with target building
		if (targetBuilding != null && other.GetComponent<SpecialBuilding> () == targetBuilding) {
			EnterBuilding (targetBuilding);
		}
	}

	public void Slow(float percent)
    {
		AddEffect (EnemyEffect.FAT);
		speed = speed * percent;
		if (HasEffectActive (EnemyEffect.DRUNK)) {
			RemoveDrunk ();
		}
	}

	public void GetDrunk(float slowPercent, float drunkTime, float enterBuldingPercent){
		AddEffect (EnemyEffect.DRUNK);

		// go to a random position
		speedLostFromBeingDrunk = speed * (1.0f - slowPercent);
		speed = speed - speedLostFromBeingDrunk;
		bonusBuildingWant = enterBuldingPercent;
		MoveToRandomPosition ();
	}

	public void RemoveDrunk(){
		RemoveEffect (EnemyEffect.DRUNK);
		speed += speedLostFromBeingDrunk;
		bonusBuildingWant = 0.0f;
		MoveToQueen();
	}

	public void Plague(float deathTime){
		AddEffect (EnemyEffect.PLAGUE);
		// add plague componenet
		Plague plagueComp = gameObject.AddComponent<Plague>();
		plagueComp.Init (deathTime);
	}

	public void DuelStarted(){
		stunned = true;
	}

	public void DuelEnded(){
		AddEffect (EnemyEffect.DUEL_WINNER);
		stunned = false;
	}

	public void AddEffect(EnemyEffect effect){
		activeEffects.SetBit ((int)effect);
	}

	public void RemoveEffect(EnemyEffect effect){
		activeEffects.ClearBit((int) effect);
	}

	public bool HasEffectActive(EnemyEffect effect){
		return activeEffects.IsSet ((int)effect);
	}

	public bool HasEffectsActive(BitSet effects){
		return effects.IsSubsetOf (activeEffects);
	}

	protected virtual void OnDestroy(){
		GameManager.Instance.LivingEnemyWasDestroyed (this);
	}
}
