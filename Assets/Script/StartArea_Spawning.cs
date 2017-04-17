using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea_Spawning : MonoBehaviour {

        //The time between the spawn of vehicles. If set to 0, no new vehicle will spawn
    [SerializeField]
    float m_RateOfSpawning;

        //The prefab of vehicle to instantiate
    [SerializeField]
    GameObject m_VehicleToSpawn;


	void Start () {
        StartCoroutine(SpawnNewVehicle());
	}
	
        //This coroutine will spawn a new vehicule after a delay and add the new vehicle to the game manager list of vehicle    
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
