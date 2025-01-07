using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownNode : MonoBehaviour
{

    // Use this for initialization
    //public Transform OverlapSphereCube;
    //public float SearchRadius;
    //public Material[] materials;
    //int NumofRN = 0;

    void Start()
    {
        //Invoke("Timer", 1.0f);
    }


    // Update is called once per frame
    void Update()
    {
        //SearchNearUnits();
    }

    //void Timer()
    //{
    //    if (NumofRN >= 3)
    //    {
    //        gameObject.GetComponent<MeshRenderer>().materials[0].CopyPropertiesFromMaterial(materials[0]);
    //        transform.tag = "RefNode";
    //    }
    //    Invoke("Timer", 1.0f);
    //}

    //public void SearchNearUnits()
    //{
    //    NumofRN = 0;
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, SearchRadius, 1 << LayerMask.NameToLayer("Nodes"));

    //    if (colliders.Length <= 0) return;

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        if (colliders[i].tag == "RefNode" || colliders[i].tag == "AnchorNode" || colliders[i].tag == "MobileNode")
    //        {
    //            NumofRN++;
    //        }
    //    }
    //    //print(colliders[i].gameObject.name);

    //}
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        GameObject academy = GameObject.Find("RLArea");
        academy.GetComponent<RLPLAcademy>().SpawnUnNode();
    }
}
