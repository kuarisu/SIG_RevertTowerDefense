using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Car_SelfManagement : MonoBehaviour {

        //The movement speed of the agent
    [SerializeField]
    float m_DrivingSpeed;
        //The number of health points of the vehicle
    [SerializeField]
    int m_HealthPoints;
        //The maximum distance required to shoot
    [SerializeField]
    float m_DistanceToTarget;
        //The speed of rotation when targeting an obstacle
    [SerializeField]
    float m_SpeedRotation;
        //The GameObject use as reference to spawn bullet
    [SerializeField]
    GameObject m_BulletSpawner;
        //The prefab of bullet that will instantiate
    [SerializeField]
    GameObject m_Bullet;
        //The minimal distance between the vehicle and roadblock before the vehicle stop
    [SerializeField]
    float m_DistanceToRoadBlock;

        //The UI text to display the health points
    [SerializeField]
    Text m_HPText;
 
        //The bool used to check if the car is already shooting or not.
    bool m_IsShooting = false;
        //The bool used to check if the car has already a target or not
    bool m_HasTarget;
        //The Quaternion use to set the rotation of the vehicle when targeting an obstacle
    Quaternion m_Orientation;

        //Reference to the State Machine
    Animator m_An;
        //The NavMeshAgent used for the pathfinding
    NavMeshAgent m_Agent;




	void Start () {
        
            /*At start, all the important informations are set such as: the lsit of waypoints the vehicle will follow, the nav mesh agent, 
            the speed rotation and the object that will rotate, the UI text with the right number of HP*/
        m_Agent = this.GetComponent<NavMeshAgent>();
        m_An = this.GetComponent<Animator>();
        m_An.GetBehaviour<Car_Driving>().SetKeyPointsPos(Manager_Objects.Instance.m_ListOfWaypoints);
        m_An.GetBehaviour<Car_Driving>().SetAgent(m_Agent);
        m_An.GetBehaviour<Car_Shooting>().SetParameters(m_SpeedRotation, this.gameObject);
        m_HPText.text = m_HealthPoints.ToString();

            //The vehicle starts with its driving behavior.
        DrivingBehavior();

    }

    private void Update()
    {
        /*First, we check if the vehicle is shooting and if it has no target
        If so, we set the orientation that will be used to rotate the vehicle toward its target.
        Then we send this orientation to the state machine to will do the rotation.
        The vehicle has now a target and can't change it until it's destroyed

        If the vehicle is shooting and already has a target, we check if this target is no longer in the world.
        When it's null, it means the target has been destroyed. If so, the vehicle can go back to its driving behavior.
        */
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


        /*
        We check if the RoadBlock is in the world and if the distance between the vehicle and the roadblock is less than the maximum distance.
        If so, we stop the agent.
        Else, if the RoadBlock is no longer in the world or the vehicle is far enough, we check if the vehicle is driving, by using the bool inside the state machine.
        If so, the agent can move.
        */
        if (Manager_Objects.Instance.m_RoadBlock != null && Vector3.Distance(this.transform.position, Manager_Objects.Instance.m_RoadBlock.transform.position) < m_DistanceToRoadBlock)
        {
            m_Agent.isStopped = true;
        }
        else if (m_An.GetBool("m_Driving"))
        {
            m_Agent.isStopped = false;
        }
    }


        /*This function is used to start the driving behavior. It will set all the bool required to change the state of the state machine.
         
        When the vehicle is driving, it's not shooting, the agent is moving, its speed is the one set in inspector and it doesn't have target*/
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

    /*This function is used to start the shooting behavior. It will set all the bool required to change the state of the state machine

      The vehicle is shooting, its velocity is set to 0, its agent is stopped, the coroutine used to instantiate bullet is called*/
    public void FiringBehavior()             
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", true);
        m_An.SetBool("m_Driving", false);

        m_IsShooting = true;
        m_Agent.velocity = Vector3.zero;
        m_Agent.isStopped = true;
        StartCoroutine(InstantiateBullet());
    }

    /*This function is used to start the destruction behavior. It will set all the bool required to change the state of the state machine

      The gameObject to destroy is send to the state machine, the vehicle is not shooting, its velocity is set to 0 and the agent is stopped*/
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


        //This function is used to decrease the number of HP of the vehicle. It will also be used to change the UI text. If the number of HP reaches 0, the vehicle is destroyed.
    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if(m_HealthPoints >= 0)
            m_HPText.text = m_HealthPoints.ToString();

        if (m_HealthPoints <= 0)
            DestroyedBehavior();

    }


        /* This function is used to do several check before starting the firing behavior.
        We check if the distance between the vehicle and the target is small enough to fire
        If so, we check if the vehicle is behind the turret and if it's not already shooting*/
    public void PrepareShooting()
    {
        if(Vector3.Distance(this.transform.position, Manager_Input.Instance.m_Target.transform.position) < m_DistanceToTarget)
        {
            Vector3 _Direction = this.transform.position - Manager_Input.Instance.m_Target.transform.position;

            if (Vector3.Dot(Manager_Input.Instance.m_Target.transform.forward, _Direction) < 0 && !m_IsShooting)
            {
                FiringBehavior();
            }
        }
    }

        //This coroutine is used to instantiate the vehicle's bullets. The delays are used to synch the animation. If the vehicle is no longer shooting, the coroutine stops.
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

    //If the vehicle collide if a turret's bullet, the function to decrease the number of HP is called. It destroys the bullet.
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.root.transform.tag == "BulletTurret")
        {
            LooseHealthPoint();
            Destroy(col.transform.root.gameObject);
        }
    }

}
