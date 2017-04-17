using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock_Management : MonoBehaviour {

    [SerializeField]
    int m_HealthPoints;

    Animator m_An;

	// Use this for initialization
	void Start () {
        m_An = this.GetComponent<Animator>();
	}

    void StartDestroyedBehavior()
    {
        m_An.SetBool("IsDestroyed", true);
        m_An.GetBehaviour<RoadBlock_Destroyed>().SetRoadBlock(this.gameObject);
    }


    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if (m_HealthPoints <= 0)
            StartDestroyedBehavior();

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
