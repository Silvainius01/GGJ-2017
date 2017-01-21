using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour {

	protected KeyCode triggerKey;
	protected int gridX, gridY;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (triggerKey)) {
			ApplyTriggerEffect ();
		}
	}

	public void DisplayTriggerKey(){
		// TODO print text over the position showing the hotkey
	}

	public void Init(KeyCode key, int gridX, int gridY){
		triggerKey = key;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	public abstract void ApplyTriggerEffect ();
}
