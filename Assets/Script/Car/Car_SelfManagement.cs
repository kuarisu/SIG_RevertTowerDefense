using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_SelfManagement : MonoBehaviour {

    [SerializeField]
    float m_DrivingSpeed;
    [SerializeField]
    int m_HealthPoints;
    [SerializeField]
    float m_DistanceToTarget;
    [SerializeField]
    float m_SpeedRotation;

    [SerializeField]
    GameObject m_BulletSpawner;

    [SerializeField]
    GameObject m_Bullet;

    [SerializeField]
    float m_DistanceToRoadBlock;

    bool m_IsShooting = false;
    bool m_HasTarget;
    Quaternion m_Orientation;

    Animator m_An;
    NavMeshAgent m_Agent;


	// Use this for initialization
	void Start () {
        
        m_Agent = this.GetComponent<NavMeshAgent>();
        m_An = this.GetComponent<Animator>();
        m_An.GetBehaviour<Car_Driving>().SetKeyPointsPos(Manager_Objects.Instance.m_ListOfWaypoints);
        m_An.GetBehaviour<Car_Driving>().SetAgent(m_Agent);
        m_An.GetBehaviour<Car_Shooting>().SetParameters(m_SpeedRotation, this.gameObject);

        DrivingBehavior();

    }

    private void Update()
    {
        if(m_IsShooting && !m_HasTarget)
        {
            m_Orientation = Quaternion.LookRotation(Manager_Input.Instance.m_Target.transform.position - transform.position);
            m_An.GetBehaviour<Car_Shooting>().SetOrientation(m_Orientation);
            m_HasTarget = true;
        }
        else if(m_IsShooting && m_HasTarget && Manager_Input.Instance.m_Target == null)
        {
            DrivingBehavior();
        }


        if (Manager_Objects.Instance.m_RoadBlock != null && Vector3.Distance(this.transform.position, Manager_Objects.Instance.m_RoadBlock.transform.position) < m_DistanceToRoadBlock)
        {
            m_Agent.isStopped = true;
        }
        else if (m_An.GetBool("m_Driving"))
        {
            m_Agent.isStopped = false;
        }
    }


    public void DrivingBehavior()           
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", true);

        m_IsShooting = false;
        m_Agent.isStopped = false;
        m_Agent.speed = m_DrivingSpeed;
        m_HasTarget = false;


    }

    public void FiringBehavior()             
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", true);
        m_An.SetBool("m_Driving", false);

        m_IsShooting = true;
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
        m_Agent.destination = transform.position;
        StartCoroutine(InstantiateBullet());
    }

    public void DestroyedBehavior()             
    {
        m_An.SetBool("m_Destroyed", true);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", false);

        m_An.GetBehaviour<Car_Destroyed>().SetVehicle(this.gameObject.transform.root.gameObject);

        m_IsShooting = false;
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
    }

    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if (m_HealthPoints <= 0)
            DestroyedBehavior();

    }

    public void OnColliderEnter (Collision col)
    {
        if(col.collider.tag == "BulletTurret")
        {
            LooseHealthPoint();
        }
    }

    public void PrepareShooting()
    {
        Debug.Log("hello");
        if(Vector3.Distance(this.transform.position, Manager_Input.Instance.m_Target.transform.position) < m_DistanceToTarget)
        {
            Vector3 _Direction = this.transform.position - Manager_Input.Instance.m_Target.transform.position;
            if (Vector3.Dot(Manager_Input.Instance.m_Target.transform.forward, _Direction) < 0 && !m_IsShooting)
            {
                FiringBehavior();
            }
        }
    }

    IEnumerator InstantiateBullet()
    {
        while (m_IsShooting)
        {
            yield return new WaitForSeconds(0.418f);
            GameObject _bullet = (GameObject)Instantiate(m_Bullet, m_BulletSpawner.transform.position, transform.rotation);
            yield return new WaitForSeconds(0.585f);
        }
        yield break;

    }


    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.root.transform.tag == "BulletTurret")
        {
            LooseHealthPoint();
            Destroy(col.transform.root.gameObject);
        }


    }

}
