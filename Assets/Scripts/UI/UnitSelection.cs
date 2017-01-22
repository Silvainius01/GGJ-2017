using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour
{
    public bool first = true;
    public Vector2 sPos;
    public Vector2 fPos;
    public Texture2D texture;
    public  Rect rect = new Rect();
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    { 
	    if(Input.GetMouseButton(0))
        {
            fPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(first)
            {
                sPos = fPos;
                first = false;
            }
            rect = new Rect(Mathc.GetMidPoint(sPos, fPos), new Vector2(10.0f, 10.0f));
        }
        else if(!first)
        {
            var unitsOnMap = FindObjectsOfType<Unit>();

            foreach(var unit in unitsOnMap)
                if (Mathc.VectorIsBetween(unit.transform.position, sPos, fPos))
                    if (unit.ownedByPlayer)
                        unit.isSelected = true;
            first = true;
        }
	}

    private void OnGUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GUI.color = Color.red;
            GUI.DrawTexture(rect, texture);
        }
    }
}
