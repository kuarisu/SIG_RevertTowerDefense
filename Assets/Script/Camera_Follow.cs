using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    [SerializeField]
    float m_TimeToMove;

    public void GoNewPosition()
    {
        if(Manager_Input.Instance.m_NewCameraPosition > -11.5f && Manager_Input.Instance.m_NewCameraPosition < 106)
        StartCoroutine(SmoothFollow());
    }

    IEnumerator SmoothFollow()
    {
        while (Mathf.FloorToInt(transform.position.x) != Mathf.FloorToInt(Manager_Input.Instance.m_NewCameraPosition))
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(Manager_Input.Instance.m_NewCameraPosition, transform.position.y, transform.position.z), m_TimeToMove);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("hithere");
        yield break;
    }

}
