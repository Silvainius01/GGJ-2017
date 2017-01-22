using UnityEngine;
using System.Collections;

public class PidePiper : Trap {

	private GraphMaker graph;
	public int trapGridRange = 2;
	public float plagueDeathTime = 15.0f;

	public override void ApplyTriggerEffect (){
		// spawn some rats and plague in surounding 3 by 3 grid

		// get all enemy units
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		int offset = trapGridRange - 1;

		for (int x = gridX - offset; x <= gridX + offset; ++x) {
			for (int y = gridY - offset; y <= gridY + offset; ++y) {
				if (x < 0 || x >= graph.colLength || y < 0 || y >= graph.rowLength)
					continue;

				foreach (GameObject enemy in enemies) {
					// check to see if enemy is in affected grid pos
					if (graph.IsPosInGridPos (enemy.transform.position, x, y)) {
						// plague the enemy
						enemy.GetComponent<BasicEnemyUnit> ().Plague (plagueDeathTime);
					}
				}

				var gridType = graph.GetGridType (x, y);
				if (gridType == GraphMaker.GRID_TYPE.NONE || gridType == GraphMaker.GRID_TYPE.TRAP) {
					// spawn some rats
				}
			}
		}
		// destroy after use
		Destroy (gameObject);
	}
}
