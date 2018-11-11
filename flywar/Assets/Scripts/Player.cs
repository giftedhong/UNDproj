using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/Player")]//代码添加C#脚本
public class Player : MonoBehaviour {

    public float m_speed = 1;//定时控制飞行的速度
    public Transform m_rocket;
    public float m_life = 3;
    protected Transform m_transform;

    float m_rocketTimer = 0;

    //定位主角位置
    protected Vector3 m_targetPos;//目标位置
    public LayerMask m_inputMask;//鼠标射线碰撞层

    //音效
    public AudioClip m_shootClip;//声音
    protected AudioSource m_audio;//声音源
    public Transform m_explosionFX;//爆炸音效 

    // Use this for initialization
    void Start () {
        m_transform = this.transform;
        m_audio = this.GetComponent<AudioSource>();
        m_targetPos = this.m_transform.position;//添加代码，初始化目标点位置
    }

    //鼠标移动函数
    void MoveTo() {
        if (Input.GetMouseButton(0))
        {
            Vector3 ms = Input.mousePosition;//获得鼠标屏幕位置
            Ray ray = Camera.main.ScreenPointToRay(ms);//将屏幕位置转为射线
            RaycastHit hitinfo;//用于记录射线碰撞信息
            bool iscast = Physics.Raycast(ray, out hitinfo, 1000, m_inputMask);

            if (iscast)
            {
                //射中目标
                m_targetPos = hitinfo.point;
            }
        }
        //使用Vector提供MoveTowards函数，获得超目标移动的位置
        Vector3 pos = Vector3.MoveTowards(this.m_transform.position,  m_targetPos, m_speed * Time.deltaTime);

        //更新当前位置
        this.m_transform.position = pos;
    }

	
	// Update is called once per frame
	void Update () {
        
        //纵向移动距离
        float movev = 0;
        //水平移动距离
        float moveh = 0;

        
        
        //按上键
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movev -= m_speed * Time.deltaTime;
        }
        //按下键
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movev += m_speed * Time.deltaTime;
        }
        //按左键
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveh += m_speed * Time.deltaTime;
        }
        //按右键
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveh -= m_speed * Time.deltaTime;
        }

        m_rocketTimer -= Time.deltaTime;
        if (m_rocketTimer <= 0)
        {
            m_rocketTimer = 0.1f;
            /*按空格键/鼠标左键发射子弹*/
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                Instantiate(m_rocket, this.transform.position, this.transform.rotation);
                m_audio.PlayOneShot(m_shootClip);
            }
        }

        //移动
        m_transform.Translate(new Vector3(moveh, 0, movev));
    }
    private void OnTriggerEnter(Collider other)
    {
        /*碰撞任何子弹以外的物体*/
        if (other.tag.CompareTo("PlayerRocket") != 0)
        {
            m_life -= 1;
            GameManager.Instance.ChangeLife(m_life);//更新生命
            if (m_life <= 0)
            {
                Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);//爆炸音效
                Destroy(this.gameObject);//自我销毁
            }
        }
    }
}
