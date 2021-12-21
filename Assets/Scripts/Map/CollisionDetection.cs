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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ShouldCheckForCollision)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            ShouldCheckForCollision = false;
            if ((collision.gameObject.GetComponent<CollisionDetection>().PrioAttachBackwards + collision.gameObject.GetComponent<CollisionDetection>().PrioAttachForward) > (PrioAttachBackwards + PrioAttachForward))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                foreach (var item in connectedTiles)
                {
                    item.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
            else if (collision.gameObject.GetComponent<CollisionDetection>().PrioAttachBackwards == PrioAttachBackwards)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
    }

    public void Checkpriority()
    {
        CheckBackwards();
        CheckForward(this.gameObject);
        //PrioAttachForward = connectedTiles.Count;
    }

    private void CheckBackwards()
    {
        GameObject obj = this.gameObject;
        while (obj.GetComponent<CollisionDetection>().attachedTo != null)
        {
            PrioAttachBackwards++;
            obj = obj.GetComponent<CollisionDetection>().attachedTo;
        }
    }

    private void CheckForward(GameObject obj)
    {
        List<GameObject> tiles = GameObject.Find("MapGeneration").GetComponent<MapGeneration>().mapTiles;
        List<GameObject> currentAddedTiles = new List<GameObject>();
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].GetComponent<CollisionDetection>().attachedTo != null)
            {
                if (tiles[i].GetComponent<CollisionDetection>().attachedTo.name == obj.name)
                {
                    PrioAttachForward++;
                    connectedTiles.Add(tiles[i]);
                    currentAddedTiles.Add(tiles[i]);
                }
            }
        }
        foreach (var item in currentAddedTiles)
        {
            CheckForward(item);
        }
    }
}
