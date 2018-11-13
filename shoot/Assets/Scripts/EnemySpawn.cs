using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/EnemySpawn")]
public class EnemySpawn : MonoBehaviour {

    public Transform m_enemy;//敌人的Prefab

    public int m_enemyCount = 0;//生成敌人数量

    public int m_maxEnemy = 3;//敌人生成最大数量

    public float m_timer = 0;//生成敌人时间间隔

    protected Transform m_transform;


	// Use this for initialization
	void Start () {
        m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        //生成数量到达最大
        if (m_enemyCount >= m_maxEnemy)
        {
            //Debug.Log("已经生成" + m_enemyCount);
            return;
        }

        /*间隔时间 生成敌人*/
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            m_timer = Random.value * 15.0f;
            if (m_timer < 5)
                m_timer = 5;

            Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity);//生成敌人

            Enemy enemy = obj.GetComponent<Enemy>();//获取敌人脚本组件

            enemy.Init(this);//敌人初始化方法


        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }
}
