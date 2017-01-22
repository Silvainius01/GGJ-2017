using UnityEngine;
using System.Collections;

public class PidePiper : Trap {
    
	public int trapGridRange = 2;
	public float plagueDeathTime = 15.0f;

	public override void ApplyTriggerEffect (){
		// spawn some rats and plague in surounding 3 by 3 grid

		// get all enemy units
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		foreach (GameObject enemy in enemies) {
			for (int x = gridX - (trapGridRange - 1); x < gridX + (trapGridRange - 1); ++x) {
				for (int y = gridY - (trapGridRange - 1); x < gridY + (trapGridRange - 1); ++x) {
					if (x < 0 || x >= GraphMaker.Instance.colLength || y < 0 || y >= GraphMaker.Instance.rowLength)
						continue;

					// check to see if enemy is in affected grid pos
					if(GraphMaker.Instance.IsPosInGridPos(enemy.transform.position, x, y)){
						// plague the enemy
						enemy.GetComponent<BasicEnemyUnit>().Plague(plagueDeathTime);
					}
				}
			}
		}

		// destroy after use
		Destroy (gameObject);
	}
}
