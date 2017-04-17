using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager_Victory : MonoBehaviour {

        //The UI elements to show when the player win the game
    [SerializeField]
    GameObject m_VictoryScreen;

        //Start singleton
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
        //End singleton

        //Used to make sure the game is running at the right time scale
    void Start()
    {
        Time.timeScale = 1;
    }
        //When the player wins, the game is paused by using the time scale and the UI elements are enabled
    public void Victory()
    {
        Time.timeScale = 0;
        m_VictoryScreen.SetActive(true);
    }

        //When the player click on the replay button, the scene is reloaded
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
