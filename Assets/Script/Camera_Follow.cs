using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    [SerializeField]
    float m_TimeToMove;

    bool m_CanMoveCamera = true;

    public void GoNewPosition()
    {
        if (Manager_Input.Instance.m_NewCameraPosition > -11.5f && Manager_Input.Instance.m_NewCameraPosition < 106 && m_CanMoveCamera)
        {
            StartCoroutine(SmoothFollow());
            m_CanMoveCamera = false;
        }
    }

    IEnumerator SmoothFollow()
    {
        while (Mathf.FloorToInt(transform.position.x) != Mathf.FloorToInt(Manager_Input.Instance.m_NewCameraPosition))
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(Manager_Input.Instance.m_NewCameraPosition, transform.position.y, transform.position.z), m_TimeToMove);
            yield return new WaitForEndOfFrame();
        }
        m_CanMoveCamera = true;
        yield break;
    }

}
