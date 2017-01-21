using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssignmentUiManager : MonoBehaviour {

    public static AssignmentUiManager Instance;

    public List<GameObject> BuildingButtons = new List<GameObject>();
    public List<GameObject> TrapButtons = new List<GameObject>();
    public float UiRadius = 1f;

    const float uiDistanceFromZero = -5;
    bool Active = false;
    GraphMaker.GraphPoint curTile;
    
    void Awake()
    {
        if(AssignmentUiManager.Instance == null)
        {
            Instance = this;
        }
    }

    public void SetConstruct (GameObject _construction)
    {
        // assign a construct to a space
    }

    public void ActivateUI (GraphMaker.GraphPoint _gridPoint)
    {
        curTile = _gridPoint;
        Active = true;

        if(curTile.gridType == GraphMaker.GRID_TYPE.EMPTY_BUILDING)
        {
            PopulateUiRing(BuildingButtons);
        } else if (curTile.gridType == GraphMaker.GRID_TYPE.TRAP)
        {
            PopulateUiRing(TrapButtons);
        } else
        {
            Debug.Log("This space type cannot be populated");
        }
    }

    void PopulateUiRing(List<GameObject> _toSpawn)
    {
        float _theta = 360 / _toSpawn.Count;
        for (int i = 0; i < _toSpawn.Count; i++)
        {
            float _degreeItteration = _theta * i;
            Vector2 _2dPoint = new Vector2(Mathf.Cos(_theta), Mathf.Sin(_theta)) * UiRadius;
            Vector3 _spawnPoint = new Vector3(_2dPoint.x, _2dPoint.y, uiDistanceFromZero);
            GameObject _uiElement = (GameObject)Instantiate(_toSpawn[i], _spawnPoint, Quaternion.identity);

        }
    }
}
