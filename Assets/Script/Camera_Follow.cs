using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

        //float to set the speed of the camera's movement .
    [SerializeField]
    float m_SpeedToMove;

        //Bool used to avoid the camera to move before finishing its current movement.
    bool m_CanMoveCamera = true;

        //This function check if the new of the camera is inside the borders of the level and if the camera can move before starting the coroutine
    public void GoNewPosition()
    {
        if (Manager_Input.Instance.m_NewCameraPosition > -11.5f && Manager_Input.Instance.m_NewCameraPosition < 106 && m_CanMoveCamera)
        {
            StartCoroutine(SmoothFollow());
            m_CanMoveCamera = false;
        }
    }

        //This coroutine is used to move the camera. While the camera hasn't reach its new position, the slerp changes its transform. 
    IEnumerator SmoothFollow()
    {
        while (Mathf.FloorToInt(transform.position.x) != Mathf.FloorToInt(Manager_Input.Instance.m_NewCameraPosition))
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(Manager_Input.Instance.m_NewCameraPosition, transform.position.y, transform.position.z), m_SpeedToMove);
            yield return new WaitForEndOfFrame();
        }
        m_CanMoveCamera = true;
        yield break;
    }

}
