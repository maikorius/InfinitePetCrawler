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
    public int xValue;
    public int yValue;
    public int minMovementTile;
    public int maxMovementTile;
    float xValueForMapTilePosition = 200;
    float yValueForMapTilePosition = 200;
    public int currenFloorCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMapTiles();
        LayoutMap();
        Debug.Log($"from the {mapTiles.Count} we could place {mapTiles.Where(w => w.activeInHierarchy).Count()}");
        mapTiles[0].GetComponent<SpriteRenderer>().color = Color.green;
        mapTiles[mapTiles.Count - 1].GetComponent<SpriteRenderer>().color = Color.red;
    }
    private void GenerateMapTiles()
    {
        int amountOfMaptiles = UnityEngine.Random.Range(minTiles, maxTiles + 1);
        for (int i = 0; i < amountOfMaptiles; i++)
        {
            GameObject floor = Instantiate(floorTile, new Vector2(0, 0), Quaternion.identity);
            floor.SetActive(false);
            floor.transform.parent = this.gameObject.transform;
            SpriteRenderer floorrenderer = floor.GetComponent<SpriteRenderer>();
            floorrenderer.drawMode = SpriteDrawMode.Tiled;
            floorrenderer.size = new Vector2(xValue, yValue);
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

            leftBorder.transform.localPosition = new Vector2((0 - ((floorrenderer.size.x / 2) - ((leftBorder.GetComponent<SpriteRenderer>().size.x / 2) / 5))), 0); // Left
            rightBorder.transform.localPosition = new Vector2(((floorrenderer.size.x / 2) - ((rightBorder.GetComponent<SpriteRenderer>().size.x / 2) / 5)), 0); // Right
            topBorder.transform.localPosition = new Vector2(0, (floorrenderer.size.y / 2) - ((topBorder.GetComponent<SpriteRenderer>().size.x / 2) / 5)); // Top
            bottomBorder.transform.localPosition = new Vector2(0, 0 - ((floorrenderer.size.y / 2) - ((bottomBorder.GetComponent<SpriteRenderer>().size.x / 2) / 5))); // Bottom


            topBorder.transform.Rotate(Vector3.back * 90);
            bottomBorder.transform.Rotate(Vector3.back * 90);

            leftBorder.name = "Left";
            rightBorder.name = "right";
            topBorder.name = "top";
            bottomBorder.name = "bottom";
            floor.tag = $"FLOOR";
            floor.name = i.ToString();
            mapTiles.Add(floor);
        }
    }
    private void LayoutMap()
    {
        mapTiles[0].SetActive(true);
        while (currenFloorCount < mapTiles.Count)
        {
            mapTiles[currenFloorCount].SetActive(true);
            GeneratePositionMapTile(mapTiles[currenFloorCount]);
            currenFloorCount++;
        }
    }
    public void GeneratePositionMapTile(GameObject currentFloor)
    {
        xValueForMapTilePosition = 0;
        yValueForMapTilePosition = 0;
        int spacing = 1;
        List<int> movement = GenerateMoveMents();
        DoMapTilesMovements(movement);
        DoLastMapTileMovement(currentFloor, spacing);
    }
    public List<int> GenerateMoveMents()
    {
        List<int> movement = new List<int>();
        int movements = UnityEngine.Random.Range(minMovementTile, maxMovementTile);
        for (int i = 0; i < movements; i++)
        {
            int current = UnityEngine.Random.Range(1, 5);
            movement.Add(current);
        }
        return movement;
    }
    public void DoMapTilesMovements(List<int> movement)
    {
        foreach (var moving in movement)
        {
            switch (moving)
            {
                //left
                case 1:
                    GameObject obj = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.x).FirstOrDefault();
                    if (obj != null)
                        xValueForMapTilePosition = obj.transform.position.x;
                    break;
                //right
                case 2:
                    GameObject obj2 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.x).FirstOrDefault();
                    if (obj2 != null)
                        xValueForMapTilePosition = obj2.transform.position.x;
                    break;
                //up
                case 3:
                    GameObject obj3 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.y).FirstOrDefault();
                    if (obj3 != null)
                        yValueForMapTilePosition = obj3.transform.position.y;
                    break;
                //down
                case 4:
                    GameObject obj4 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.y).FirstOrDefault();
                    if (obj4 != null)
                        yValueForMapTilePosition = obj4.transform.position.y;
                    break;
                default:
                    break;
            }
        }
    }
    public void DoLastMapTileMovement(GameObject currentFloor, int spacing)
    {
        int lastOne = UnityEngine.Random.Range(1, 5);
        switch (lastOne)
        {
            //right
            case 1:
                GameObject obj1 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.x).FirstOrDefault();
                AttachRight(obj1, currentFloor, spacing);
                break;
            //left
            case 2:
                GameObject obj2 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.x).FirstOrDefault();
                AttachLeft(obj2, currentFloor, spacing);
                break;
            //up
            case 3:
                GameObject obj3 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.y).FirstOrDefault();
                AttachUp(obj3, currentFloor, spacing);
                break;
            //down
            case 4:
                GameObject obj4 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.y).FirstOrDefault();
                AttachDown(obj4, currentFloor, spacing);
                break;
            default:
                break;
        }
        //currentFloor.GetComponent<CollisionDetection>().isPlacing = false;
    }
    private void SetFloorPosition(GameObject currentFloor, float xValue, float yValue)
    {
        currentFloor.transform.position = new Vector2(xValue, yValue);
    }
    public void AttachDown(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;

        float yValue = attachTo.transform.position.y;
        yValue -= (attachTo.GetComponent<SpriteRenderer>().size.y / 2);
        yValue -= (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
    }
    public void AttachUp(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;

        float yValue = attachTo.transform.position.y;
        yValue += (attachTo.GetComponent<SpriteRenderer>().size.y / 2);
        yValue += (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
    }
    public void AttachLeft(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;
        xValue -= (attachTo.GetComponent<SpriteRenderer>().size.x / 2);
        xValue -= (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;

        float yValue = attachTo.transform.position.y;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
    }
    public void AttachRight(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;
        xValue += (attachTo.GetComponent<SpriteRenderer>().size.x / 2);
        xValue += (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;

        float yValue = attachTo.transform.position.y;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
    }
}