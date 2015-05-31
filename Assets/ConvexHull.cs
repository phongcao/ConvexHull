using UnityEngine;
using System.Collections.Generic;

public class ConvexHull : MonoBehaviour
{
    // Number of polygon points
    private const int k_iNumPoints = 50;

    // Source points that are generated randomly
    private List<Vector3> m_points;

    // Convex hull
    private List<Vector3> m_convexHull;

    // Use this for initialization
    void Start()
    {
        m_points = new List<Vector3>();
        m_convexHull = new List<Vector3>();
        GeneratePoints();
    }

    // Update is called once per frame
    void Update()
    {
        // Generate points randomly
        if (Input.GetButton("Fire1"))
        {
            GeneratePoints();
        }

        // Render source points
        for (int i = 0; i < k_iNumPoints; i++)
        {
            Vector3 pt = m_points[i];

            Debug.DrawLine(pt, new Vector3(pt.x + 0.01f, pt.y, 0), Color.green, 0, false);
            Debug.DrawLine(pt, new Vector3(pt.x, pt.y + 0.01f, 0), Color.green, 0, false);
            Debug.DrawLine(pt, new Vector3(pt.x, pt.y - 0.01f, 0), Color.green, 0, false);
            Debug.DrawLine(pt, new Vector3(pt.x - 0.01f, pt.y, 0), Color.green, 0, false);
        }

        // Render convex hull
        if (m_convexHull.Count != 0)
        {
            for (int i = 0; i < m_convexHull.Count - 1; i++)
            {
                Debug.DrawLine(m_convexHull[i], m_convexHull[i + 1], Color.red, 0, false);
            }

            Debug.DrawLine(m_convexHull[m_convexHull.Count - 1], m_convexHull[0], Color.red, 0, false);
        }
    }

    /* Implements the theta function from Sedgewick: Algorithms in XXX, chapter 24 */
    /* z-axis is ignored */
    float theta(Vector3 p1, Vector3 p2)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        float ax = Mathf.Abs(dx);
        float ay = Mathf.Abs(dy);
        float t = (ax + ay == 0) ? 0 : dy / (ax + ay);

        if (dx < 0)
        {
            t = 2 - t;
        }
        else if (dy < 0)
        {
            t = 4 + t;
        }

        return t * 90.0f;
   }

    void GeneratePoints()
    {
        m_points.Clear();
        m_convexHull.Clear();

        // Random position
        for (int i = 0; i < k_iNumPoints; i++)
        {
            float x = Random.Range(-1.0f, 1.0f) * 5;
            float y = Random.Range(-1.0f, 1.0f) * 5;
            m_points.Add(new Vector3(x, y, 0));
        }

        // Detecting convex hull
        ConvexHullDetection();
    }

    /* Implements Gift wrapping algorithm */
    /* http://en.wikipedia.org/wiki/Convex_hull_algorithms */
    void ConvexHullDetection()
    {
        int nTotalPts = m_points.Count;
        Vector3[] convexHull = new Vector3[nTotalPts + 1];
        int min = 0;
        int nConvexHullPts = 0;
        float v = 0.0f;

        for (int i = 0; i < nTotalPts; i++)
        {
            convexHull[i] = m_points[i];

            if (convexHull[i].y < convexHull[min].y)
            {
                min = i;
            }
        }

        convexHull[nTotalPts] = convexHull[min];
        Swap(ref convexHull[0], ref convexHull[min]);

        while (min != nTotalPts)
        {
            float minAngle = 360.0f;

            for (int i = nConvexHullPts + 1; i < nTotalPts + 1; i++)
            {
                float angle = theta(new Vector2(convexHull[nConvexHullPts].x, convexHull[nConvexHullPts].y), new Vector2(convexHull[i].x, convexHull[i].y));

                if (angle > v && angle < minAngle)
                {
                    minAngle = angle;
                    min = i;
                }
            }

            v = minAngle;
            nConvexHullPts++;
            Swap(ref convexHull[nConvexHullPts], ref convexHull[min]);
        }

        for (int i = 0; i < nConvexHullPts; i++)
        {
            m_convexHull.Add(convexHull[i]);
        }
    }

    void Swap(ref Vector3 v1, ref Vector3 v2)
    {
        Vector3 tmp = v1;
        v1 = v2;
        v2 = tmp;
    }
}
