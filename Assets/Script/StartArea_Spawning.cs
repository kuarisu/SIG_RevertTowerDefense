using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea_Spawning : MonoBehaviour {

    [SerializeField]
    float m_RateOfSpawning;
    [SerializeField]
    GameObject m_VehicleToSpawn;



	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnNewVehicle());
	}
	
    IEnumerator SpawnNewVehicle()
    {
        int _i = 0;
        while (true)
        {
            GameObject _vehicleClone = (GameObject)Instantiate(m_VehicleToSpawn, transform.position, transform.rotation);
            _vehicleClone.transform.name = _vehicleClone.transform.name + " " + _i;
            _i++;

            Manager_Objects.Instance.AddVehicle(_vehicleClone);
            yield return new WaitForSeconds(m_RateOfSpawning);
        }
    }
}
