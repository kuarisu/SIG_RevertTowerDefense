using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock_Management : MonoBehaviour {

        //The number of health points of the vehicle
    [SerializeField]
    int m_HealthPoints;

    Animator m_An;

	void Start () {
        m_An = this.GetComponent<Animator>();
	}

        //This function is used to start the destruction behavior. Is changes the bool required to switch state in the state machine. It sends the gameobject to destroy to the state machine
    void StartDestroyedBehavior()
    {
        m_An.SetBool("IsDestroyed", true);
        m_An.GetBehaviour<RoadBlock_Destroyed>().SetRoadBlock(this.gameObject);
    }

        //This function is used to decrease the number of HP of the roadblock. When it reaches 0, the roadblock is destroyed.
    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if (m_HealthPoints <= 0)
            StartDestroyedBehavior();

    }

        //When the roadblock collides with a vehicle's bullet the function to decrease health points is called, it destroys the bullet.
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.root.transform.tag == "BulletCar")
        {
            LooseHealthPoint();
            Destroy(col.transform.root.gameObject);
        }
    }
}
