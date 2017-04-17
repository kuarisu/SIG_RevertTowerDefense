using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Input : MonoBehaviour {

        //Reference of the vehicles target
    [HideInInspector]
    public GameObject m_Target;
        //New position for the camera
    [HideInInspector]
    public float m_NewCameraPosition;
        //Reference of the camera
    [SerializeField]
    GameObject m_MainCamera;

    //Start singleton
    public static Manager_Input Instance;

   

    private void Awake()
    {
        if (Manager_Input.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_Input.Instance = this;
    }
    //End singleton

    private void Update()
    {
        /*If the player left click the manager launches a raycast from the camera. If the raycast hits something it returns the gameobject.
        We check the gameobject's tag. If it's an obstacle (turret or roadblock) it's the new target for the vehicle.
        If it's not an obstacle, we get the hit point and use it as the new position for the camera, and call the function to move it.
        */
        if(Input.GetMouseButtonUp(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit))
            {
                if(_hit.collider.transform.root.gameObject.tag == "Obstacle")
                {
                    m_Target = _hit.collider.transform.root.gameObject;
                    foreach (GameObject _vehicle in Manager_Objects.Instance.m_ListOfVehicle)
                    {
                        _vehicle.GetComponent<Car_SelfManagement>().PrepareShooting();
                    }
                }
                if (_hit.collider.transform.root.gameObject.tag != "Obstacle")
                {
                    m_NewCameraPosition = _hit.point.x;
                    m_MainCamera.GetComponent<Camera_Follow>().GoNewPosition();

                }

            }

        }
    }
}
