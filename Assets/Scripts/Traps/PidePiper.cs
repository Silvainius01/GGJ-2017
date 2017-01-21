using UnityEngine;
using System.Collections;

public class PidePiper : Trap {

	private GraphMaker graph;
	public int trapGridRange = 2;

	// Use this for initialization
	void Start () {
		graph = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GraphMaker>();
	}

	public override void ApplyTriggerEffect (){
		// spawn some rats and plague in surounding 3 by 3 grid

		// get all enemy units
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		foreach (GameObject enemy in enemies) {
			// check to see if enemy is in affected grid pos

		}
	}
}
