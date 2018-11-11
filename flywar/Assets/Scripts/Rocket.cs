using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[AddComponentMenu("MyGame/Rocket")]
public class Rocket : MonoBehaviour {
    public float m_speed = 10;//子弹速度
    public float m_liveTime = 1;//生存时间
    public float m_power = 1.0f;//伤害
    protected Transform m_transform;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        Destroy(this.gameObject, m_liveTime);//一定时间后自我销毁
		
	}
	
	// Update is called once per frame
	void Update () {
        //向前移动
        m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //碰到非敌机 不作反应
        if (other.tag.CompareTo("Enemy") != 0)
        {
            return;
        }
        //遇到敌机
        Destroy(this.gameObject);
    }
}
