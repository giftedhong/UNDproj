using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Game/MiniCamera")]
public class MiniCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float ratio = (float)Screen.width / (float)Screen.height;//获取屏幕分辨率比例

        /*rect前两个参数是XY位置  后两个参数是XY大小*/
        this.GetComponent<Camera>().rect = new Rect((1 - 0.2f), (1 - 0.2f * ratio), 0.2f, 0.2f * ratio);//摄像机视图朝向永远是正方形
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
