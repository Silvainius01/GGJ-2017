using UnityEngine;
using System.Collections.Generic;

public class GraphMaker : MonoBehaviour
{
    [System.Serializable]
    public class GraphPoint
    {
        [System.Serializable]
        public class ConnectionData
        {
            public int index;
            public float dist;

            public ConnectionData(int index, float distance)
            {
                this.index = index;
                dist = distance;
            }
        }
        [System.Serializable]
        public class NavData
        {
            public int pIndex;
            public bool wasTarget;
            public bool evaluated;
            public float tDist;

            public NavData(int index, float distance)
            {
                pIndex = index;
                tDist = distance;
                wasTarget = false;
                evaluated = false;
            }
        }

        public bool isBlocked = false;
        public Vector2 pos = Vector2.zero;
        public NavData navData = null;
        public List<ConnectionData> connections = new List<ConnectionData>();
    }

    [Header("Graph Actions")]
    public bool generate = false;
    public bool navigate = false;
    [Header("Graph Gen Options")]
    public float squareSideSize = 5.0f;
    [Header("Graph Information")]
    public int rowLength = 0, colLength = 0;
    public List<GraphPoint> graphPoints = new List<GraphPoint>();
    [Header("Graph Debug NavData")]
    public int startIndex = 0;
    public int finalIndex = 0;
    public List<int> navPath = new List<int>();


    Vector2 gameBoardScale;

    private void Awake()
    {
        // Collect scale of the plane we are operating on.
        gameBoardScale = new Vector2(transform.localScale.x, transform.localScale.z) * 5;
    }

    // Graph Internal Utility
    #region

    void AddPoint(Vector2 pos)
    {
        GraphPoint newPoint = new GraphPoint();
        newPoint.pos = pos;
        graphPoints.Add(newPoint);
    }

    void ConnectPoints(int indexA, int indexB)
    {
        float dist = Vector2.Distance(graphPoints[indexA].pos, graphPoints[indexB].pos);

        graphPoints[indexA].connections.Add(new GraphPoint.ConnectionData(indexB, dist));
        graphPoints[indexB].connections.Add(new GraphPoint.ConnectionData(indexA, dist));
    }

    void DisconnectPoints(int indexA, int indexB)
    {
        foreach (var link in graphPoints[indexA].connections)
            if (link.index == indexB)
            {
                graphPoints[indexA].connections.Remove(link);
                break;
            }
        foreach (var link in graphPoints[indexB].connections)
            if (link.index == indexA)
            {
                graphPoints[indexA].connections.Remove(link);
                break;
            }
    }

    void ClearPointNavData()
    {
        foreach (var point in graphPoints)
            point.navData = new GraphPoint.NavData(0, float.MaxValue);
    }

    Vector2 PointPos(int index)
    {
        return graphPoints[index].pos;
    }

    #endregion

    // Graph Generation
    #region
    void GeneratePoints()
    {
        bool zfirst = false;
        Vector2 startPos = (Vector2)transform.position + (-gameBoardScale); // The lower right corner of the board.

        if (graphPoints == null)
            graphPoints = new List<GraphPoint>();
        graphPoints.Clear();

        rowLength = 0;
        colLength = 0;
        for (float x = startPos.x; x <= gameBoardScale.x; x += squareSideSize)
        {
            for (float y = startPos.y; y <= gameBoardScale.y; y += squareSideSize)
            {
                AddPoint(new Vector2(x, y));
                if (!zfirst) // This counts how long a row is.
                    rowLength++;
            }
            colLength++;
            zfirst = true;
        }
    }

    void FinalizeConections()
    {
        // index = col + (rowLength * row)
        int index = 0;
        for (int r = 0; r < colLength; r++)
        {
            index = r * rowLength;
            for (int c = 0; c < rowLength; c++, index++)
            {
                if (index < graphPoints.Count - 1)
                    ConnectPoints(index, index + 1);
                if (r < colLength - 1)
                    ConnectPoints(index, index + rowLength);
            }
            if (index < graphPoints.Count - 1)
                DisconnectPoints(index, index - 1);
        }
    }
    #endregion

    // Graph Navigation.
    #region

    void NavigateBetween(int indexA, int indexB)
    {
        int nextIndex = indexB;
        var q = new List<int>();
        int cIndex = 0;
        bool foundTarget = false;
        float dist = float.MaxValue;

        startIndex = indexA;
        finalIndex = indexB;

        if (startIndex == finalIndex)
            return;

        ClearPointNavData();
        graphPoints[indexA].navData = new GraphPoint.NavData(indexA, 0.0f);
        graphPoints[indexB].navData.wasTarget = true;

        foreach (var link in graphPoints[indexA].connections)
        {
            float comp = Mathc.SqrDist(PointPos(link.index), PointPos(indexB));
            if (comp < dist)
            {
                dist = comp;
                cIndex = graphPoints[indexA].connections.IndexOf(link);
            }
            else
                graphPoints[link.index].navData.evaluated = true;
        }

        dist = graphPoints[indexA].connections[cIndex].dist;
        cIndex = graphPoints[indexA].connections[cIndex].index;
        graphPoints[cIndex].navData = new GraphPoint.NavData(indexA, dist);
        q.Add(cIndex);

        while (q.Count > 0)
        {
            int curIndex = q[0];

            if (curIndex == indexB)
                foundTarget = true;
            else if (graphPoints[curIndex].navData.evaluated)
            {
                q.Remove(curIndex);
                continue;
            }

           cIndex = 0;
            dist = float.MaxValue;
            foreach (var link in graphPoints[curIndex].connections)
            {
                float comp = Mathc.SqrDist(PointPos(link.index), PointPos(indexB));
                if (comp < dist)
                {
                    dist = comp;
                    cIndex = graphPoints[curIndex].connections.IndexOf(link);
                }
                else
                    graphPoints[link.index].navData.evaluated = true;
            }

            dist = graphPoints[curIndex].connections[cIndex].dist + graphPoints[curIndex].navData.tDist;
            cIndex = graphPoints[curIndex].connections[cIndex].index;

            //if (dist < graphPoints[cIndex].navData.tDist)
            graphPoints[cIndex].navData = new GraphPoint.NavData(curIndex, dist);

            if (!foundTarget)
                q.Add(cIndex);


            graphPoints[curIndex].navData.evaluated = true;
            q.Remove(curIndex);
        }

        // Record path.
        q.Clear();
        q.Add(nextIndex);
        for (int a = 0; a < graphPoints.Count && nextIndex != indexA; a++)
        {
            nextIndex = graphPoints[nextIndex].navData.pIndex;
            q.Add(nextIndex);
        }

        navPath.Clear();
        for (int a = 0; a < q.Count; a++)
            navPath.Add(q[q.Count - (a + 1)]);
    }


    #endregion

    // Public Graph Data
    #region

    public int GetClosestPointTo(Vector2 pos)
    {
        int cIndex = 0;
        float dist = float.MaxValue;
        foreach (var point in graphPoints)
        {
            float comp = Mathc.SqrDist(point.pos, pos);
            if (comp < dist)
            {
                dist = comp;
                cIndex = graphPoints.IndexOf(point);
            }
        }

        return cIndex;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (generate)
        {
            Awake();
            GeneratePoints();
            FinalizeConections();
            generate = false;
        }

        if(navigate)
        {
            NavigateBetween(startIndex, finalIndex);
            navigate = false;
        }

        if (graphPoints != null)
        {
            foreach (var point in graphPoints)
            {
                if (point.navData.evaluated)
                    Gizmos.color = Color.cyan;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(point.pos, 1.0f);
            }

            foreach (var point in graphPoints)
                if (point.connections != null)
                    foreach (var link in point.connections)
                    {
                        if (link.index < graphPoints.IndexOf(point))
                        {
                            if (graphPoints[link.index].navData.evaluated)
                                Gizmos.color = Color.cyan;
                            else
                                Gizmos.color = Color.magenta;
                            Gizmos.DrawLine(graphPoints[link.index].pos, point.pos);
                        }
                    }

            if (navPath.Count > 0)
            {
                Gizmos.color = Color.green;
                for (int a = 0; a < navPath.Count - 1; a++)
                    Gizmos.DrawLine(PointPos(navPath[a]), PointPos(navPath[a + 1]));
                foreach (var index in navPath)
                {
                    Gizmos.color = Color.green;
                    if (index == startIndex)
                        Gizmos.color = Color.blue;
                    else if (index == finalIndex)
                        Gizmos.color = Color.yellow;

                    Gizmos.DrawSphere(PointPos(index), 1.0f);
                }
            }
        }
    }
}
