using UnityEngine;
using System.Collections;

public class ButtonPassActionForward : MonoBehaviour {

    [HideInInspector]
    public GraphMaker.GraphPoint Point; 

	public void ActivateAssignmentUi()
    {
        AssignmentUiManager.Instance.ActivateUI(Point);
    }

}
