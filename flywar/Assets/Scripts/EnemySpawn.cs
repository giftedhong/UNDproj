using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public Transform m_enemy;//敌人的Prefab
    public Transform m_superenemy;//BOSS的Prefab
    protected Transform m_transform;

	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        StartCoroutine(SpawnEnemy());//协程
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }

    IEnumerator SpawnEnemy()//协同程序定义关键字IEnumerator
    {
        float randomnum = Random.Range(5, 15);
        yield return new WaitForSeconds(randomnum);//随机生成敌人
        if (randomnum > 12)
            Instantiate(m_superenemy, m_transform.position, Quaternion.identity);
        else
            Instantiate(m_enemy, m_transform.position, Quaternion.identity);

        StartCoroutine(SpawnEnemy());
    }
}
