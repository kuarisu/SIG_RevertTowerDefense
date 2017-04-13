using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Input : MonoBehaviour {

    public GameObject m_Target;

    public float m_NewCameraPosition;

    [SerializeField]
    GameObject m_MainCamera;


    public static Manager_Input Instance;

   

    private void Awake()
    {
        if (Manager_Input.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_Input.Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    //Mouse input + raycast + checker les tags

    private void Update()
    {
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
