using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Objects : MonoBehaviour {

    public static Manager_Objects Instance;

    public List<Transform> m_ListOfWaypoints = new List<Transform>();
    public List<GameObject> m_ListOfVehicle = new List<GameObject>();

    public GameObject m_RoadBlock;

    private void Awake()
    {
        if (Manager_Objects.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_Objects.Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddVehicle(GameObject _vehicle)
    {
        m_ListOfVehicle.Add(_vehicle);
    }

    private void Update()
    {
        if(Input.GetKeyUp("space"))
        {
            Destroy(m_RoadBlock.gameObject);
            m_RoadBlock = null;
        }
    }

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
