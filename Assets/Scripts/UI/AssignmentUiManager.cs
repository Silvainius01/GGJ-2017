using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssignmentUiManager : MonoBehaviour {

    public static AssignmentUiManager Instance;

    public List<ConstructionButton> ConstructButtons = new List<ConstructionButton>();

    bool Active = false;
    GameObject Tile;
    
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

    //public void ActivateUI (GameObject _tile, List<ConstructionButton> _constructionOptions)
}
