using UnityEngine;
using System.Collections.Generic;

public class AviatorSchoolBuilding : SpecialBuilding {

	struct GridLocation{
		int x, y;

		public GridLocation(int x, int y){
			this.x = x;
			this.y = y;
		}
	}

	private int defaultFlightRange = 2;
	public int testTargetX = 0, testTargetY = 0;
	public int crashRange = 2;
	private GraphMaker graph;

	void Awake(){
		buildingType = SpecialBuilding.SpecialBuildingType.AVIATION;
		graph = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GraphMaker> ();
		//GameObject gyro = Instantiate (Resources.Load ("Prefabs/Effects/Gyrocoptor"), transform.position, Quaternion.identity) as GameObject;
		//gyro.GetComponent<Gyrocoptor> ().Init (testTargetX, testTargetY);
	}

	protected override bool ApplySpecialEffect (BasicEnemyUnit unit)
	{
		// if fat from backery, enemy may leave
		if (unit.HasEffectActive(BasicEnemyUnit.EnemyEffect.FAT)) {
			return true;
		}
		// otherwise, bye bye
		else {
			GameObject gyro = Instantiate (Resources.Load ("Prefabs/Effects/Gyrocoptor"), transform.position, Quaternion.identity) as GameObject;
			// if drunk pick a target and fly as long as you need to to crash into him
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
			if (unit.HasEffectActive (BasicEnemyUnit.EnemyEffect.DRUNK) && enemies.Length > 0) {
				// attack a random enemy
				int enemy = UnityEngine.Random.Range (0, enemies.Length - 1);
				gyro.GetComponent<Gyrocoptor> ().Init (enemies [enemy].GetComponent<BasicEnemyUnit>());
				return true;
			}
			// if not drunk, find target within default distance and crash into him
			else {
				int gridX = 0, gridY = 0;
				GraphMaker.GraphPoint graphPoint = graph.GetGraphPoint (transform.position);
				graph.GetGraphPointXYGridCoords (graphPoint, ref gridX, ref gridY);

				List<GridLocation> streetLocations = new List<GridLocation> ();

				for (int x = gridX - (crashRange - 1); x < gridX + (crashRange - 1); ++x) {
					for (int y = gridY - (crashRange - 1); x < gridY + (crashRange - 1); ++x) {
						if (x < 0 || x >= graph.colLength || y < 0 || y >= graph.rowLength)
							continue;
						// add street location to list
						streetLocations.Add (new GridLocation (x, y));
					}
				}

				if (streetLocations.Count > 0) {
					GridLocation selectedLocation = streetLocations [UnityEngine.Random.Range (0, streetLocations.Count)];
					gyro.GetComponent<Gyrocoptor> ().Init (unit.gameObject.GetComponent<BasicEnemyUnit>());
					return true;
				} else {
					return false;
				}
			}
		}
	}
}
