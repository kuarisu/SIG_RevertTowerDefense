using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Input : MonoBehaviour {


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
}
