using UnityEngine;
using System.Collections;

public class MilitiaController : Unit {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

	}

	public override void Attack (Unit unit){
		RangedAttack (unit);
	}

	protected BasicEnemyUnit GetTarget (){
		return null;
	}

	public void MoveToWorldPos(Vector2 pos){

	}

	public void MoveToGridPos(Vector2 pos){

	}
		
	private bool WasTargetKilled(){
		return true;
	}

	private void RangedAttack(Unit unit){

	}

	private void MeleeAttack(Unit unit){

	}
}
