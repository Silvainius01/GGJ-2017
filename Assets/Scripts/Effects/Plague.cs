using UnityEngine;
using System.Collections;

public class Plague : MonoBehaviour {

	private Timer deathTimer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deathTimer.Update (Time.deltaTime)) {
			Destroy (gameObject);
		}
	}

	public void Init(float timeTillDeath){
		deathTimer.Activate (timeTillDeath);
	}
}
