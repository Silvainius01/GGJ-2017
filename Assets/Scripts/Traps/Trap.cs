using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour {

	protected GraphMaker graph;
	protected KeyCode triggerKey;
	protected int gridX, gridY;
	protected bool activated = false;

	// Use this for initialization
	public virtual void Start () {
		graph = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GraphMaker>();
	}

	// Update is called once per frame
	public virtual void Update () {
		if (Input.GetKeyDown (triggerKey) && !activated) {
			activated = true;
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
