using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : Rocket {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Player") != 0)
        {
            return;
        }
           
        Destroy(this.gameObject);
    }

}
