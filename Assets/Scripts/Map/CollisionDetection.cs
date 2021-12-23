using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject attachedTo;
    public bool ShouldCheckForCollision = false;
    public int PrioAttachBackwards;
    public int PrioAttachForward;
    public List<GameObject> connectedTiles;
    public bool isPlacing = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.ToUpper().Contains("FLOOR") && this.gameObject.tag.ToUpper().Contains("FLOOR"))
        {
            Debug.Log($"CollisionDetected between {this.gameObject.name} and {collision.gameObject.name}");
            try
            {
                int collisionNumber = Convert.ToInt32(collision.gameObject.name);
                int thisNumber = Convert.ToInt32(this.gameObject.name);
                if (collisionNumber > thisNumber)
                {
                    collision.gameObject.SetActive(false);
                }
                else
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
