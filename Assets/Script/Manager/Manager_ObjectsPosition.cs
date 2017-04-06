using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_ObjectsPosition : MonoBehaviour {

    public static Manager_ObjectsPosition Instance;

    public List<Transform> m_ListOfWaypoints = new List<Transform>();

    private void Awake()
    {
        if (Manager_ObjectsPosition.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_ObjectsPosition.Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
