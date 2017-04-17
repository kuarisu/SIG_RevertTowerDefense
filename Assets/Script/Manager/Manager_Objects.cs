using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Objects : MonoBehaviour {

        //Singleton
    public static Manager_Objects Instance;

        //List of the waypoints the vehicle will follow
    public List<Transform> m_ListOfWaypoints = new List<Transform>();
        //List of the vehicle, it's used by the turret to check if their target is in the world
    public List<GameObject> m_ListOfVehicle = new List<GameObject>();
        //Reference of the RoadBlock
    public GameObject m_RoadBlock;

    //Singleton
    private void Awake()
    {
        if (Manager_Objects.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_Objects.Instance = this;
    }

        //This function is used to add a vehicle to the vehicles list
    public void AddVehicle(GameObject _vehicle)
    {
        m_ListOfVehicle.Add(_vehicle);
    }


        //This function is used to remove a vehicle from the list when it's destroyed. 
        //The function goes through the list and check the name of each gameobject in the list. When it has the same name as the destroyed gameobject, it removes it from the list.
    public void RemoveVehicle(GameObject _vehicleToRemove)
    {
        int _i = 0;
        foreach (GameObject m_Vehicle in m_ListOfVehicle.ToArray())
        {
            if (_vehicleToRemove.transform.root.name == m_Vehicle.transform.name)
            {
                m_ListOfVehicle.RemoveAt(_i);
                break;
            }
            else
            {
                _i++;
            }
        }

    }

}
