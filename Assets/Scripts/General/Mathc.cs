using UnityEngine;
using System.Collections;


public static class Mathc
{
    public const float HALF_PI = Mathf.PI / 2;
    public const float QUARTER_PI = Mathf.PI / 4;
    public const float EIGTH_PI = Mathf.PI / 8;
    public const float TWO_PI = Mathf.PI * 2;

    public static float fMod(float a, float b)
    {
        return (a % b + b) % b;
    }

    /// <summary>
    /// Returns true if a - b is less than threshold
    /// </summary>
    public static bool Approximately(float a, float b, float threshold)
    {
        return Mathf.Abs(a - b) <= threshold;
    }

    /// <summary>
    /// Returns a normal vector corresponding to an angle.
    /// </summary>
    /// <param name="angle">USE RADIANS</param>
    /// <returns></returns>
    public static Vector2 GetVectorFromAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); ;
    }

    /// <summary>
    /// Returns a point on a cirle.
    /// </summary>
    /// <param name="angle">USE RADIANS</param>
    /// <returns></returns>
    public static Vector2 GetPointOnCircle(Vector2 pos, float radius, float angle)
    {
        return (GetVectorFromAngle(angle) * radius) + pos;
    }

    /// <summary>
    /// Return a point on a line
    /// </summary>
    /// <param name="p1">Point one of the line</param>
    /// <param name="p2">Point two of the line</param>
    /// <param name="x">X-coordinate of the point you want</param>
    public static Vector2 GetPointOnLine(Vector2 p1, Vector2 p2, float x)
    {
        float m = (p2.y - p1.y) / (p2.x - p1.x);
        return GetPointOnLine(m, x, p1.y - (m * p1.x));
    }

    /// <summary>
    /// Return a point on a line.
    /// </summary>
    /// <param name="m">Slope of the line</param>
    /// <param name="x">X-coordinate of the point you want</param>
    /// <param name="b">Height of the line at x == 0</param>
    public static Vector2 GetPointOnLine(float m, float x, float b)
    {
        return new Vector2(x, (m * x) + b);
    }

    /// <summary>
    /// Returns the midpoint between two vectors
    /// </summary>
    public static Vector2 GetMidPoint(Vector2 a, Vector2 b)
    {
        return a + ((b - a).normalized * (Vector2.Distance(a, b) / 2));
    }

    /// <summary>
    /// Returns the midpoint between two floats.
    /// </summary>
    public static float GetMidValue(float a, float b)
    {
        if (a == b)
            return a;
        return ((a - b) / 2) + a;
    }

    /// <summary>
    /// Returns true if A is between B and C
    /// </summary>
    public static bool ValueIsBetween(float a, float b, float c)
    {
        if (a >= b && a <= c) return true;
        else return (a >= c && a <= b);
    }

    /// <summary>
    /// Returns the square distance between two vectors.
    /// </summary>
    /// <returns></returns>
    public static float SqrDist(Vector2 a, Vector2 b)
    {
        return (a - b).sqrMagnitude;
    }

    /// <summary>
    /// Returns true if point1 is between point2 and point3 (on both axis)
    /// </summary>
    public static bool VectorIsBetween(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        if (Approximately(p1.x, p2.x, 0.1f))
            p1.x = p2.x;
        else if (Approximately(p1.x, p3.x, 0.1f))
            p1.x = p3.x;

        if (Approximately(p1.y, p2.y, 0.1f))
            p1.y = p2.y;
        else if (Approximately(p1.y, p3.y, 0.1f))
            p1.y = p3.y;

        if ((p1.x >= p2.x && p1.x <= p3.x) || (p1.x >= p3.x && p1.x <= p2.x))
        {
            if (p1.y >= p2.y && p1.y <= p3.y)
                return true;
            return (p1.y <= p2.y && p1.y >= p3.y);
        }
        return false;
    }

    public static Vector2 FindIntersectionPoint(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out bool intersected)
    {
        intersected = false;


        // Organize points based on the greatest X value
        if (p2.x < p1.x)
        {
            Vector2 t = p1;
            p1 = p2;
            p2 = t;
        }
        if (p4.x < p3.x)
        {
            Vector2 t = p3;
            p3 = p4;
            p4 = t;
        }

        // If the line is completely to the right of the other, they don't intersect.
        if (p2.x < p3.x && p2.x < p4.x)
            return Vector2.zero;
        // If the lione is completely to the left of the other, they don't intersect
        else if (p3.x < p1.x && p4.x < p1.x)
            return Vector2.zero;

        // Organize points based on the greatest Y value
        if (p2.y < p1.y)
        {
            Vector2 t = p1;
            p1 = p2;
            p2 = t;
        }
        if (p4.y < p3.y)
        {
            Vector2 t = p3;
            p3 = p4;
            p4 = t;
        }

        // If both points are above the highest point, they can't intersect
        if (p2.y < p3.y && p2.y < p4.y)
            return Vector2.zero;
        // If both points are below the lowest point, they can't intersect.
        else if (p3.y < p1.y && p4.y < p1.y)
            return Vector2.zero;

        // Y = MX + B
        float line1M = GetMValue(p1, p2); // Slope of L1
        float line2M = GetMValue(p3, p4); // Slope of L2
        float line1B = p1.y - (line1M * p1.x); // B value of L1
        float line2B = p3.y - (line2M * p3.x); // B value of L2
        Vector2 intersectPoint;
        // (-32.1, 0)

        if (float.IsInfinity(line1M))
            intersectPoint = GetPointOnLine(line2M, p1.x, line2B);
        else if (float.IsInfinity(line2M))
            intersectPoint = GetPointOnLine(line1M, p3.x, line1B);
        else
            intersectPoint = GetPointOnLine(line1M, (line2B - line1B) / (line1M - line2M), line1B);



        // (0, (0 - B) / (M - 0), 0)

        if (VectorIsBetween(intersectPoint, p1, p2))
            if (VectorIsBetween(intersectPoint, p3, p4))
                intersected = true;

        return intersectPoint;
    }

    /// <summary>
    /// Returns the m value of a line (y = mx + b)
    /// </summary>
    public static float GetMValue(Vector2 p1, Vector2 p2)
    {
        return (p2.y - p1.y) / (p2.x - p1.x);
    }

    /// <summary>
    /// Returns true if the two vectors are within the distance defined by threshold.
    /// </summary>
    public static bool VectorApprox(Vector2 p1, Vector2 p2, float threshold = 0.0001f)
    {
        return Vector2.Distance(p1, p2) <= 0.0001;
    }
}
