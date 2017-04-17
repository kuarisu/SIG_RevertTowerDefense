using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint_Reached : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.gameObject.tag == "Vehicle")
        {
            Manager_Victory.Instance.Victory();
        }
    }

}
