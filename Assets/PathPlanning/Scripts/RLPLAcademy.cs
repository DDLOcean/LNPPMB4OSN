using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLPLAcademy : MonoBehaviour
{
    public GameObject UnknownNode;
    public int NumofUnknownNodes;
    public int range;
    //public GameObject GetAera;
    //public int width = 6;
    //public int height = 6;
    //public float outerRadius = 10f;
    //float innerRadius;
    //public GameObject beaconPoint;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < NumofUnknownNodes; i++)
        {
            SpawnUnNode();
        }
        StartCoroutine(WriteDataWaitForSeconds());

        //innerRadius = outerRadius * 0.866025404f;
        //for (int z = 0, i = 0; z < height; z++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        CreateBeaconPoint(x, z, i++);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnUnNode()
    {
        GameObject UNs = Instantiate(UnknownNode);
        UNs.transform.parent = transform;
        UNs.transform.localPosition = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
        UNs.transform.localEulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
    }
    IEnumerator WriteDataWaitForSeconds()
    {
        yield return new WaitForSeconds(5.0f);
        transform.GetComponent<WriteUNsPosition>().WriteData();
    }

    //void CreateBeaconPoint(int x, int z, int i)
    //{
    //    Vector3 position;
    //    position.x = (x + z * 0.5f - z / 2) * (innerRadius * 2f);
    //    position.y = 0f;
    //    position.z = z * (outerRadius * 1.5f);

    //    GameObject cor = Instantiate(beaconPoint);
    //    cor.transform.parent = GetAera.transform;
    //    cor.transform.localPosition = position;
    //}
}
