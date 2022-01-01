using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject attachedTo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToUpper().Contains("FLOOR") && this.gameObject.tag.ToUpper().Contains("FLOOR") && collision.gameObject.activeInHierarchy)
        {
            Debug.Log($"CollisionDetected between {this.gameObject.name} and {collision.gameObject.name}");
            try
            {
                int collisionNumber = Convert.ToInt32(collision.gameObject.name);
                int thisNumber = Convert.ToInt32(this.gameObject.name);
                if (thisNumber > collisionNumber)
                {
                    this.gameObject.SetActive(false);
                }
            }
            catch (Exception)
            {
                Debug.Log($"<color=red> Wuttt??? {collision.gameObject.name}</color>");
                Debug.Log($"<color=red> Wuttt??? {this.gameObject.name}</color>");
            }
        }
    }
}
