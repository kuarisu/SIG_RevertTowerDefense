using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret_Management : MonoBehaviour {

        //The number of the turret's HP
    [SerializeField]
    int m_HealthPoints;
        //The list of vehicles inside the detection area. The turret shoots at the first gameobject of the list.
    [SerializeField]
    List<GameObject> m_ListTargetedVehicle = new List<GameObject>();
        //Speed to rotate the turret
    [SerializeField]
    float m_SpeedRotation;
        //The dynamic elements that rotates
    [SerializeField]
    GameObject m_DynamicVisual;
        //Reference of the state machine
    [SerializeField]
    Animator m_An;
        //Reference of the gameobject's position used instantiate bullets
    [SerializeField]
    GameObject m_BulletSpawner;
        //The bullet's prefab to instantiate
    [SerializeField]
    GameObject m_Bullet;
        //The UI text to display the number of HP
    [SerializeField]
    Text m_HPText;
        //The orientation used to rotate the turret
    Quaternion m_Orientation;
        //The orientation of the turret at the start
    Quaternion m_InitialOritention;
        //Bool to check if the turret is shooting
    bool m_TurretIsShooting = false;


        //Used to set several parameters, mostly inside the state machine. The rotation of the turret at the start is stored and the UI is set to display the right number of HP
    void Start ()
    {
        m_An.GetBehaviour<Turret_Shooting>().SetParameters(m_SpeedRotation, m_DynamicVisual);
        m_An.GetBehaviour<Turret_Iddle>().SetParameters(m_SpeedRotation * 2, m_DynamicVisual);
        m_InitialOritention = m_DynamicVisual.transform.localRotation;
        m_An.GetBehaviour<Turret_Iddle>().SetOrientation(m_InitialOritention);
        m_HPText.text = m_HealthPoints.ToString();
    }

    /*If the list of targets is not empty, we check if the first gameobject is in the world. If not we remove it.

    If so, we set the orientation (forward and upward) twoard which the turret will rotate.
    With Vector3.Dot, we check if turret will rotate more than +- 45° of its initial rotation
    If not, if the turret is not already shooting, it starts shooting. A new orientation is set.

    If the rotation is more than +- 45°, the turret starts its iddle behavior

    If the List is empty and the turret is shooting, it starts its iddle behavior */
    private void Update()
    {
        if(m_ListTargetedVehicle.Count > 0)
        {
            if (m_ListTargetedVehicle[0] == null)
            {
                m_ListTargetedVehicle.RemoveAt(0);
            }
            else
            {
                
                m_Orientation = Quaternion.LookRotation(m_ListTargetedVehicle[0].transform.position - transform.position, m_ListTargetedVehicle[0].transform.position - transform.position);

                if (Vector3.Dot(- m_DynamicVisual.transform.right, m_ListTargetedVehicle[0].transform.position - transform.position) >=  0.7f)
                {
                    if (!m_TurretIsShooting)
                    {
                        m_TurretIsShooting = true;
                        StartShootingBehavior();
                    }
   
                    m_An.GetBehaviour<Turret_Shooting>().SetOrientation(m_Orientation);
                    
                }
                else if (Vector3.Dot(-m_DynamicVisual.transform.right, m_ListTargetedVehicle[0].transform.position - transform.position) < 0.7f)
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


        //This function is used to start the shooting behavior. It sets all the bool required to switch state. The turret is shooting, the coroutine to instantiate bullets is called
    void StartShootingBehavior()
    {
 
        m_TurretIsShooting = true;
        m_An.SetBool("IsShooting", true);
        StartCoroutine(InstantiateBullet());
    }

        //This function is used to start the iddle behavior. It sets all the bool required to switch state. The turret is not shooting, it sends the orientation to go back to its initial rotation.
    void StartIddleBehavior()
    {
        m_TurretIsShooting = false;
        m_An.SetBool("IsShooting", false);
        m_An.GetBehaviour<Turret_Iddle>().SetOrientation(m_InitialOritention);
    }

        //This function is used to start the destruction behavior. It sets all the bool required to switch state. The turret is not shooting, it sends the right gameobject to be destroyed.
    void StartDestroyedBehavior()
    {
        m_TurretIsShooting = false;
        m_An.SetBool("IsShooting", false);
        m_An.SetBool("IsDestroyed", true);

        m_An.GetBehaviour<Turret_Destroyed>().SetTurret(this.gameObject.transform.root.gameObject);
    }

        //If a vehicle enters the trigger used as the detection area, it's added inside the target list.
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.root.tag == "Vehicle")
        {
            m_ListTargetedVehicle.Add(col.gameObject.transform.root.gameObject);
        }
    }
        
        //If a vehicle exits the trigger used as the detection area, the function to remove it is called.
    private void OnTriggerExit(Collider col)
    {
        if (col.transform.root.tag == "Vehicle")
        {
            RemoveFromList(col.gameObject);
        }
    }

        //This function is used to remove vehicle from the target list. It checks if both the vehicle to remove and the gameobject inside the list are inside the world. If not, the function is stopped.
        //If so, the function checks if the gameobject's name inside the list is the same as the vehicle. If so, it removes it from the list, if not, it looks for the next gameobject inside the list.
    void RemoveFromList(GameObject _gameObject)
    {
        int _i = 0;

        foreach (GameObject Vehicle in m_ListTargetedVehicle.ToArray())
        {
            if(_gameObject == null ||Vehicle == null)
            {
                break;
            }

            if ( _gameObject.transform.root.name == Vehicle.transform.name)
            {
                m_ListTargetedVehicle.RemoveAt(_i);

                if (m_ListTargetedVehicle.Count == 0)
                {
                    StartIddleBehavior();
                }
                break;
            }
            else
            {
                _i++;
            }
        }

    }

        //This function is used to decreased the number of HP of the turret. If the number reaches 0, the turret is destroyed. It also set the UI text to the current number of HP
    public void LooseHealthPoint()
    {
        m_HealthPoints--;

        if (m_HealthPoints >= 0)
            m_HPText.text = m_HealthPoints.ToString();

        if (m_HealthPoints <= 0)
            StartDestroyedBehavior();

    }


        //This coroutine is used to instantiate bullets as long as the turret is shooting. The delay are used to synch it with the animation.
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

        //If the turret collides with a vehicle's bullet, the function to decrease the number of HP is called and the bullet is destroyed.
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.root.transform.tag == "BulletCar")
        { 
            LooseHealthPoint();
            Destroy(col.transform.root.gameObject);
        }
    }

}
