using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Management : MonoBehaviour {

    [SerializeField]
    int m_HealthPoints;

    [SerializeField]
    BoxCollider m_DetectionZone;

    [SerializeField]
    List<GameObject> m_ListTargetedVehicle = new List<GameObject>();

    [SerializeField]
    float m_SpeedRotation;

    [SerializeField]
    GameObject m_DynamicVisual;

    [SerializeField]
    Animator m_An;

    [SerializeField]
    GameObject m_BulletSpawner;

    [SerializeField]
    GameObject m_Bullet;

    Quaternion m_Orientation;
    Quaternion m_InitialOritention;
    bool m_TurretIsShooting = false;



    void Start ()
    {
        m_An.GetBehaviour<Turret_Shooting>().SetParameters(m_SpeedRotation, m_DynamicVisual);
        m_An.GetBehaviour<Turret_Iddle>().SetParameters(m_SpeedRotation * 2, m_DynamicVisual);
        m_InitialOritention = m_DynamicVisual.transform.localRotation;
    }

    private void Update()
    {
        if(m_ListTargetedVehicle.Count == 1)
        {
            if (m_ListTargetedVehicle[0] == null)
            {
                m_ListTargetedVehicle.RemoveAt(0);
            }
            else
            {
                m_Orientation = Quaternion.LookRotation(m_ListTargetedVehicle[0].transform.localPosition - transform.localPosition);

                if (m_Orientation.eulerAngles.y >= m_InitialOritention.eulerAngles.y - 45 && m_Orientation.eulerAngles.y <= m_InitialOritention.eulerAngles.y + 45)
                {     
                    if(!m_TurretIsShooting)
                        StartShootingBehavior();

                    m_An.GetBehaviour<Turret_Shooting>().SetOrientation(m_Orientation);
                    
                }
                else if (m_Orientation.eulerAngles.y <= m_InitialOritention.eulerAngles.y - 45 && m_Orientation.eulerAngles.y >= m_InitialOritention.eulerAngles.y + 45)
                {
                    StartIddleBehavior();
                }
            }
        }
        else if (m_ListTargetedVehicle.Count == 0 && m_TurretIsShooting)
        {
            StartIddleBehavior();
        }
    }

    void StartShootingBehavior()
    {
 
        m_TurretIsShooting = true;
        m_An.SetBool("IsShooting", true);
        StartCoroutine(InstantiateBullet());
    }

    void StartIddleBehavior()
    {
        m_TurretIsShooting = false;
        m_An.SetBool("IsShooting", false);
        m_An.GetBehaviour<Turret_Iddle>().SetOrientation(m_InitialOritention);
    }

    void StartDestroyedBehavior()
    {
        m_TurretIsShooting = false;
        m_An.SetBool("IsShooting", false);
        m_An.SetBool("IsDestroyed", true);

        m_An.GetBehaviour<Turret_Destroyed>().SetTurret(this.gameObject.transform.root.gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.tag == "Vehicle")
        {
            m_ListTargetedVehicle.Add(col.gameObject.transform.root.gameObject);
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
            if (_gameObject.transform.root.name == Vehicle.transform.name)
            {
                m_ListTargetedVehicle.RemoveAt(_i);

                if (m_ListTargetedVehicle.Count == 0)
                {
                    StartIddleBehavior();
                   // m_TurretIsShooting = false;
                }
                break;
            }
            else
            {
                _i++;
            }
        }

    }

    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if (m_HealthPoints <= 0)
            StartDestroyedBehavior();

    }


    IEnumerator InstantiateBullet()
    {
        while(m_TurretIsShooting)
        {
            yield return new WaitForSeconds(0.368f);
            GameObject _bullet = (GameObject)Instantiate(m_Bullet, m_BulletSpawner.transform.position, m_DynamicVisual.transform.localRotation);
            yield return new WaitForSeconds(0.63f);
        }
        yield break;

    }



    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.root.transform.tag == "BulletCar")
        { 
                LooseHealthPoint();
            Destroy(col.transform.root.gameObject);
        }
    }

}
