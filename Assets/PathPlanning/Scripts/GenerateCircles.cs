using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCircles : MonoBehaviour
{
    public Vector2 co = new Vector2(0.5f, 0.5f);
    public float d0 = 0.2f, arc0 = 0.05f, d = 1;
    List<Vector2> waypoints_list;
    // Start is called before the first frame update
    void Start()
    {
        waypoints_list = new List<Vector2>();
        //CalculateCircles(new Vector2(0.5f,0.5f),0.1f, 0.05f, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<Vector2> GetWaypoints()
    {
        waypoints_list.Clear();
        waypoints_list.Add(co);
        int level = Mathf.RoundToInt(0.5f * d / d0);
        for (int i = 0; i < level; i++)
        {
            float d1 = i * d0;
            int number = Mathf.RoundToInt(2 * Mathf.PI * d1 / arc0);
            for (int j = 0; j < number; j++)
            {
                float arc2 = j * arc0;
                float theta = arc2 / d1;
                float xco = d1 * Mathf.Cos(theta);
                float yco = d1 * Mathf.Sin(theta);
                Vector2 coordinate = new Vector2(xco + co.x, yco + co.y);
                waypoints_list.Add(coordinate);
            }
            //waypoints_list.Add(new Vector2(d1 + co.x, 0 + co.y));
        }
        return waypoints_list;
    }
}
