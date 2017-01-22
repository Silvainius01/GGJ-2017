using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ConstructionButton : MonoBehaviour {

    public GameObject ConstructPrefab;

	public void AssignToTile()
    {
        AssignmentUiManager.Instance.SetConstruct(ConstructPrefab);
    }
}
