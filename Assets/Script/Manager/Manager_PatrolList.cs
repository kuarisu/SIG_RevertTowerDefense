using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_PatrolList : MonoBehaviour {

    public static Manager_PatrolList Instance;

    public List<Transform> m_ListOfWaypoints = new List<Transform>();

    private void Awake()
    {
        if (Manager_PatrolList.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_PatrolList.Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
