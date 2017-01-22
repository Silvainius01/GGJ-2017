using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour
{
    public bool first = true;
    public Vector2 sPos;
    public Vector2 fPos;
    public Texture2D texture;
    public  Rect rect = new Rect();

    LineRenderer lines;
	// Use this for initialization
	void Start ()
    {
        lines = GetComponent<LineRenderer>();

	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            fPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (first)
            {
                sPos = fPos;
                first = false;
                lines.SetPosition(0, sPos);
                lines.SetPosition(4, sPos);
                lines.enabled = true;
            }

            lines.SetPosition(1, new Vector2(sPos.x, fPos.y));
            lines.SetPosition(2, fPos);
            lines.SetPosition(3, new Vector2(fPos.x, sPos.y));
        }
        else if (!first)
        {
            var unitsOnMap = FindObjectsOfType<Unit>();

            foreach (var unit in unitsOnMap)
                if (unit.ownedByPlayer)
                {
                    if (Mathc.VectorIsBetween(unit.transform.position, sPos, fPos))
                        unit.isSelected = true;
                    else
                        unit.isSelected = false;
                }
            first = true;

            lines.enabled = false;
        }
    }

    private void OnGUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // GUI.color = Color.red;
            // GUI.DrawTexture(rect, texture);
        }
    }
}
