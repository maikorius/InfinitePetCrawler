using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public int minTiles;
    public int maxTiles;
    public GameObject floorTile;
    public GameObject border;
    public List<GameObject> mapTiles = new List<GameObject>();
    public float wallThickness;
    public int minWidth;
    public int maxWidth;
    public int minHeight;
    public int maxHeight;
    public int minMovementTile;
    public int maxMovementTile;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMapTiles();
        LayoutMap();
        mapTiles.ForEach(fl => fl.GetComponent<CollisionDetection>().ShouldCheckForCollision = true);
        foreach (var item in mapTiles)
        {
            item.GetComponent<CollisionDetection>().Checkpriority();
        }
    }

    private void LayoutMap()
    {
        for (int floorcount = 1; floorcount < mapTiles.Count; floorcount++)
        {
            GeneratePositionMapTile(mapTiles[floorcount]);
        }
    }

    public void GeneratePositionMapTile(GameObject currentFloor)
    {
        int spacing = 1;
        float xValueForMapTilePosition = 0;
        float yValueForMapTilePosition = 0;
        List<int> movement = new List<int>();
        int movements = UnityEngine.Random.Range(minMovementTile, maxMovementTile);
        for (int i = 0; i < movements; i++)
        {
            int current = UnityEngine.Random.Range(1, 5);
            movement.Add(current);
        }
        foreach (var moving in movement)
        {
            switch (moving)
            {
                //left
                case 1:
                    GameObject obj = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition).OrderByDescending(fl => fl.transform.position.x).FirstOrDefault();
                    xValueForMapTilePosition = obj.transform.position.x;
                    break;
                //right
                case 2:
                    GameObject obj2 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition).OrderBy(fl => fl.transform.position.x).FirstOrDefault();
                    xValueForMapTilePosition = obj2.transform.position.x;
                    break;
                //up
                case 3:
                    GameObject obj3 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition).OrderByDescending(fl => fl.transform.position.y).FirstOrDefault();
                    yValueForMapTilePosition = obj3.transform.position.y;
                    break;
                //down
                case 4:
                    GameObject obj4 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition).OrderBy(fl => fl.transform.position.y).FirstOrDefault();
                    yValueForMapTilePosition = obj4.transform.position.y;
                    break;
                default:
                    break;
            }
        }
        int lastOne = UnityEngine.Random.Range(1, 5);
        switch (lastOne)
        {
            //right
            case 1:
                GameObject obj = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition).OrderByDescending(fl => fl.transform.position.x).FirstOrDefault();
                xValueForMapTilePosition = obj.transform.position.x;
                xValueForMapTilePosition += (obj.GetComponent<SpriteRenderer>().size.x / 2);
                xValueForMapTilePosition += (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;
                currentFloor.GetComponent<CollisionDetection>().attachedTo = obj;
                break;
            //left
            case 2:
                GameObject obj2 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition).OrderBy(fl => fl.transform.position.x).FirstOrDefault();
                xValueForMapTilePosition = obj2.transform.position.x;
                xValueForMapTilePosition -= (obj2.GetComponent<SpriteRenderer>().size.x / 2);
                xValueForMapTilePosition -= (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;
                currentFloor.GetComponent<CollisionDetection>().attachedTo = obj2;
                break;
            //up
            case 3:
                GameObject obj3 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition).OrderByDescending(fl => fl.transform.position.y).FirstOrDefault();
                yValueForMapTilePosition = obj3.transform.position.y;
                yValueForMapTilePosition += (obj3.GetComponent<SpriteRenderer>().size.y / 2);
                yValueForMapTilePosition += (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;
                currentFloor.GetComponent<CollisionDetection>().attachedTo = obj3;
                break;
            //down
            case 4:
                GameObject obj4 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition).OrderBy(fl => fl.transform.position.y).FirstOrDefault();
                yValueForMapTilePosition = obj4.transform.position.y;
                yValueForMapTilePosition -= (obj4.GetComponent<SpriteRenderer>().size.y / 2);
                yValueForMapTilePosition -= (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;
                currentFloor.GetComponent<CollisionDetection>().attachedTo = obj4;
                break;
            default:
                break;
        }
       currentFloor.transform.position = new Vector2(xValueForMapTilePosition, yValueForMapTilePosition);
    }

    private void GenerateMapTiles()
    {
        int amountOfMaptiles = UnityEngine.Random.Range(minTiles, maxTiles + 1);

        for (int i = 0; i < amountOfMaptiles; i++)
        {
            int xValue = UnityEngine.Random.Range(minWidth, maxWidth);
            int yValue = UnityEngine.Random.Range(minHeight, maxHeight);
            GameObject floor = Instantiate(floorTile, new Vector2(0, 0), Quaternion.identity);
            floor.transform.parent = this.gameObject.transform;
            SpriteRenderer floorrenderer = floor.GetComponent<SpriteRenderer>();
            floorrenderer.drawMode = SpriteDrawMode.Tiled;
            floorrenderer.size = new Vector2((xValue - 0.4F), (yValue -0.4F));
            floor.GetComponent<BoxCollider2D>().size = floorrenderer.size;
            GameObject leftBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Left
            GameObject rightBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Right
            GameObject topBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Top
            GameObject bottomBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Bottom

            leftBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            leftBorder.GetComponent<SpriteRenderer>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.y);
            leftBorder.GetComponent<BoxCollider2D>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.y);


            rightBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            rightBorder.GetComponent<SpriteRenderer>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.y);
            rightBorder.GetComponent<BoxCollider2D>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.y);

            topBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            topBorder.GetComponent<SpriteRenderer>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.x);
            topBorder.GetComponent<BoxCollider2D>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.x);

            bottomBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            bottomBorder.GetComponent<SpriteRenderer>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.x);
            bottomBorder.GetComponent<BoxCollider2D>().size = new Vector2(wallThickness, floor.GetComponent<SpriteRenderer>().size.x);

            leftBorder.transform.parent = floor.transform;
            rightBorder.transform.parent = floor.transform;
            topBorder.transform.parent = floor.transform;
            bottomBorder.transform.parent = floor.transform;

            leftBorder.transform.localPosition = new Vector2((0 - (floorrenderer.size.x / 2) + (wallThickness / 10)), 0); // Left
            rightBorder.transform.localPosition = new Vector2(((floorrenderer.size.x / 2) - (wallThickness / 10)), 0); // Right
            topBorder.transform.localPosition = new Vector2(0, (floorrenderer.size.y / 2) - (wallThickness / 10)); // Top
            bottomBorder.transform.localPosition = new Vector2(0, 0 - (floorrenderer.size.y / 2) + (wallThickness / 10)); // Bottom


            topBorder.transform.Rotate(Vector3.back * 90);
            bottomBorder.transform.Rotate(Vector3.back * 90);
            
            leftBorder.name = "Left";
            rightBorder.name = "right";
            topBorder.name = "top";
            bottomBorder.name = "bottom";
            floor.name = $"floor{i}";
            mapTiles.Add(floor);
        }
    }
}