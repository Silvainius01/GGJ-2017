using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{

	[System.Serializable]
	public struct SpawnWave{
		public int numEnemies;
		public float spawnRate;

		public SpawnWave(int num, float rate){
			numEnemies = num;
			spawnRate = rate;
		}
	}

    [Header("Debug Stuff")]
    public int addEnemiesToQ = 0;
    [Header("Wave Options")]
    public int startIndex = 0;
    public int finalIndex = 99;
    public Timer spawnInterval = new Timer(1.0f, true);
    [Header("Wave Info")]
    public List<GameObject> spawnQ = new List<GameObject>();

    private GraphMaker graph;

    void AddToQ(GameObject u, int amount)
    {
        for(int a = 0; a < amount; a++)
            spawnQ.Add(u);
    }


	// Use this for initialization
	void Start ()
    {
        graph = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GraphMaker>();
        spawnQ = new List<GameObject>();
        spawnInterval = new Timer(1.0f, true);
    }

	public void InitWave(SpawnWave waveData){
		spawnQ.Clear ();
		spawnInterval = new Timer (waveData.spawnRate, true);
		AddToQ(Resources.Load("Prefabs/BasicEnemy") as GameObject, waveData.numEnemies);
		addEnemiesToQ = 0;
	}

	public BasicEnemyUnit UpdateSpawns(float dt){
		if(addEnemiesToQ > 0)
		{
			AddToQ(Resources.Load("Prefabs/BasicEnemy") as GameObject, addEnemiesToQ);
			addEnemiesToQ = 0;
		}

		spawnInterval.Update(Time.deltaTime);
		if(spawnInterval.hasFired && spawnQ.Count > 0)
		{
			BasicEnemyUnit newUnit = Instantiate(spawnQ[0].GetComponent<BasicEnemyUnit>());

			spawnQ.RemoveAt(0);
			newUnit.navigateGraph = true;
			newUnit.destroyOnPathCompletion = true;
			newUnit.transform.position = graph.PointPos(startIndex);
			newUnit.path = graph.GetPath(graph.PointPos(startIndex), graph.PointPos(finalIndex));
			spawnInterval.Activate();

			return newUnit;
		}
		return null;
	}
}
