using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea_Spawning : MonoBehaviour {

    [SerializeField]
    float m_RateOfSpawning;
    [SerializeField]
    GameObject m_VehicleToSpawn;

    [SerializeField]
    float m_MaxNbOfVehicle;

	// Use this for initialization
	void Start () {
		
	}
	
    IEnumerator SpawnNewVehicle()
    {
        while (true)
        {
            yield return null;
        }
    }
}
