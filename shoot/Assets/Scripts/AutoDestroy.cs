using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float m_timer = 1.0f;


	// Use this for initialization
	void Start () {
        //采用缓存方式避免游戏运行时使用Instantiate/Destroy
        Destroy(this.gameObject, m_timer);
	}
	

}
