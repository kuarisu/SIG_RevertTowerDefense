using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Movement : MonoBehaviour {

    [SerializeField]
    float m_Speed;

    [SerializeField]
    float m_Timer;

    void Start()
    {
        StartCoroutine(Timer());
    }

	// Update is called once per frame
	void LateUpdate () {
        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);

    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(m_Timer);
        Destroy(this.gameObject);
    }
}
