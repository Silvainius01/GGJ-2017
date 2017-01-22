using UnityEngine;
using System.Collections.Generic;

public class AviatorSchoolBuilding : SpecialBuilding {

	struct GridLocation{
		public int x, y;

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
			Debug.Log ("fat im leaving");
			return true;
		}
		// otherwise, bye bye
		else {
			GameObject gyro = Instantiate (Resources.Load ("Prefabs/Effects/Gyrocoptor"), transform.position, Quaternion.identity) as GameObject;
			// if drunk pick a target and fly as long as you need to to crash into him
			List<BasicEnemyUnit> enemies = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetLivingEnemies();
			if (unit.HasEffectActive (BasicEnemyUnit.EnemyEffect.DRUNK) && enemies.Count > 0) {
				// attack a random enemy
				int enemy = UnityEngine.Random.Range (0, enemies.Count - 1);
				gyro.GetComponent<Gyrocoptor> ().Init (enemies [enemy]);
				Debug.Log ("attacking random enemy");
				return true;
			}
			// if not drunk, find target within default distance and crash into him
			else {
				int gridX = 0, gridY = 0;
				GraphMaker.GraphPoint graphPoint = graph.GetGraphPoint (transform.position);
				graph.GetGraphPointXYGridCoords (graphPoint, ref gridX, ref gridY);

				List<GridLocation> streetLocations = new List<GridLocation> ();

				int offset = Mathf.Max(crashRange - 1, 0);
				for (int x = gridX - offset; x <= gridX + offset; ++x) {
					for (int y = gridY - offset; y <= gridY + offset; ++y) {
						if (x < 0 || x >= graph.colLength || y < 0 || y >= graph.rowLength || x == gridX || y == gridY)
							continue;

						var type = graph.GetGridType (x, y);
						if (type == GraphMaker.GRID_TYPE.NONE || type == GraphMaker.GRID_TYPE.TRAP) {
							// add street location to list
							streetLocations.Add (new GridLocation (x, y));
						}
					}
				}

				if (streetLocations.Count > 0) {
					Debug.Log("attacking random location: " + streetLocations.Count);
					GridLocation selectedLocation = streetLocations [UnityEngine.Random.Range (0, streetLocations.Count)];
					gyro.GetComponent<Gyrocoptor> ().Init(selectedLocation.x, selectedLocation.y);
					return true;
				} else {
					Debug.Log("no where to attack");
					return false;
				}
			}
		}
	}
}
