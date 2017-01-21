using UnityEngine;
using System.Collections;

public class PidePiper : Trap {

	private GraphMaker graph;

	// Use this for initialization
	void Start () {
		graph = GameObject.FindGameObjectWithTag ("GameBoard");
	}

	public override void ApplyTriggerEffect (){
		// spawn some rats and plague in surounding 3 by 3 grid


	}
}
