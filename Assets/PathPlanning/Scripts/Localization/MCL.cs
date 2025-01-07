using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MCL : MonoBehaviour {

    public GameObject coor;
    public GameObject error;
    GameObject mobile_node;
    Vector3 mobile_node_last_pos;
    List<Vector3> coordinate_list = new List<Vector3>();
    List<float> rssi_list = new List<float>();
    Vector3 estimate_position;
    // Use this for initialization
    void Start () {
        mobile_node = GameObject.Find("mobile_node");
        mobile_node_last_pos = mobile_node.transform.localPosition;
        //Invoke("MCLTimer", 1.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //coor.GetComponent<Text>().text = mobile_node.transform.localPosition.x.ToString()
        //    + "\n" + mobile_node.transform.localPosition.z.ToString();
        coor.GetComponent<Text>().text = estimate_position.x.ToString() + "\n" + estimate_position.z.ToString();
        float rssi = (mobile_node_last_pos - mobile_node.transform.localPosition).magnitude;
        coordinate_list.Add(mobile_node.transform.localPosition);
        rssi_list.Add(rssi);
        CalculateMCL();
        mobile_node_last_pos = mobile_node.transform.localPosition;
    }
    void MCLTimer()
    {
        float rssi = (mobile_node_last_pos - mobile_node.transform.localPosition).magnitude;
        if (rssi > 0.5)
        {
            coordinate_list.Add(mobile_node.transform.localPosition);
            rssi_list.Add(rssi);
            CalculateMCL();
            mobile_node_last_pos = mobile_node.transform.localPosition;
        }
        Invoke("MCLTimer", 1.0f);
    }
    void CalculateMCL()
    {
        Vector3 position_all = new Vector3();
        float rssi_all = 0;
        for (int i = 0; i < coordinate_list.Count; i++)
        {
            position_all -= coordinate_list[i]* rssi_list[i];
            rssi_all -= rssi_list[i];
        }
        estimate_position =  position_all/ rssi_all;
    }
}
