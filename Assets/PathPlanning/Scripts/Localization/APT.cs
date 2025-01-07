using System.Collections.Generic;
using UnityEngine;

public class APT : MonoBehaviour
{
    public List<Transform> beacons;
    Transform mobileNode;
    public Vector3 estimatedPosition;

    void Update()
    {
        //CalculateAPT();
        mobileNode = FindFirstObjectByType<MobileBeacon>().transform;
    }

    void CalculateAPT()
    {
        if (beacons.Count < 3) return; // 需要至少三个信标

        // 使用SortedList来存储RSS值和对应的位置，SortedList默认按键值升序排列
        SortedList<float, Vector3> sortedDistances = new SortedList<float, Vector3>();

        foreach (var beacon in beacons)
        {
            float rssi = CalculateRSSI(mobileNode.position, beacon.position);
            // 由于SortedList不能有重复的键，我们需要确保没有重复的RSS值
            while (sortedDistances.ContainsKey(rssi))
                rssi += 0.0001f; // 微调RSS值以避免键的重复
            sortedDistances.Add(rssi, beacon.position);
        }

        // 选择RSS值最小的三个信标
        if (sortedDistances.Count > 3)
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < 3; i++) // 只取前三个最小的RSS值对应的位置
            {
                positions.Add(sortedDistances.Values[i]);
            }
            estimatedPosition = Trilaterate(positions[0], positions[1], positions[2]);
        }
        Debug.Log("Estimated Position: " + estimatedPosition);
    }

    float CalculateRSSI(Vector3 mobilePos, Vector3 beaconPos)
    {
        // 简单的RSSI计算公式，根据距离计算衰减
        return Vector3.Distance(mobilePos, beaconPos);
    }

    Vector3 Trilaterate(Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        return (pos1 + pos2 + pos3) / 3;
    }
}
