using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Victory : MonoBehaviour {

    [SerializeField]
    GameObject m_VictoryScreen;

    public static Manager_Victory Instance;
    
    private void Awake()
    {
        if (Manager_Victory.Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Manager_Victory.Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            Victory();
        }
    }

    public void Victory()
    {
        Time.timeScale = 0;
        m_VictoryScreen.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
