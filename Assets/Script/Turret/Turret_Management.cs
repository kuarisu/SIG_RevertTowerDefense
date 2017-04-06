using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Management : MonoBehaviour {

    [SerializeField]
    SphereCollider m_DetectionZone;

    [SerializeField]
    List<GameObject> m_ListTargetedVehicle = new List<GameObject>();

    [SerializeField]
    float m_SpeedRotation;

    [SerializeField]
    GameObject m_DynamicVisual;

    [SerializeField]
    Animator m_An;

    Quaternion m_Orientation;
    bool m_TurretIsShooting;



    void Start ()
    {
        m_An.GetBehaviour<Turret_Shooting>().SetParameters(m_SpeedRotation, m_DynamicVisual);
    }

    private void Update()
    {
        if(m_TurretIsShooting)
        {
            m_Orientation = Quaternion.LookRotation(m_ListTargetedVehicle[0].transform.position - transform.position);
            m_An.GetBehaviour<Turret_Shooting>().SetOrientation(m_Orientation);
        }
    }

    void StartShootingBehavior()
    {
        m_TurretIsShooting = true;
        m_An.SetBool("IsShooting", true);
    }

    void StartIddleBehavior()
    {
        m_TurretIsShooting = false;
        m_An.SetBool("IsShooting", false);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.tag == "Vehicle")
        {
            m_ListTargetedVehicle.Add(col.gameObject.transform.root.gameObject);
            if (m_ListTargetedVehicle.Count == 1)
            {
                StartShootingBehavior();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.root.tag == "Vehicle")
        {
            RemoveFromList(col.gameObject);
        }
    }

    void RemoveFromList(GameObject _gameObject)
    {
        int _i = 0;
        foreach (GameObject Vehicle in m_ListTargetedVehicle.ToArray())
        {
            if(_gameObject.transform.root.name == Vehicle.transform.name)
            {
                m_ListTargetedVehicle.RemoveAt(_i);

                if (m_ListTargetedVehicle.Count == 0)
                {
                    StartIddleBehavior();
                }
            }
            _i++;
        }

    }




}
