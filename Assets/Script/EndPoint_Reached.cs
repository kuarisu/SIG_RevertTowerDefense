using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint_Reached : MonoBehaviour {

        //When a vehicle reach the end point, it calls the victory function inside the game manager
    void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.gameObject.tag == "Vehicle")
        {
            Manager_Victory.Instance.Victory();
        }
    }

}
