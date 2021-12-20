using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public int minTiles;
    public int maxTiles;
    public GameObject floorTile;
    public GameObject border;
    List<GameObject> mapTiles = new List<GameObject>();
    public float wallThickness;
    public int minWidth;
    public int maxWidth;
    public int minHeight;
    public int maxHeight;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        SortTiles();
    }

    private void SortTiles()
    {
        for (int i = 0; i < mapTiles.Count; i++)
        {
            if (i == 0)
            {
                mapTiles[i].transform.position = new Vector2(0, 0);
            }
            else
            {
                float distance = ((mapTiles[i - 1].transform.position.x + (mapTiles[i - 1].GetComponent<SpriteRenderer>().size.x / 2)) + (mapTiles[i].GetComponent<SpriteRenderer>().size.x / 2)) + 0.1F;
                mapTiles[i].transform.position = new Vector2( distance, 0);
            }
        }
    }

    private void GenerateMap()
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
            floorrenderer.size = new Vector2(xValue, yValue);
            GameObject leftBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Left
            GameObject rightBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Right
            GameObject topBorder = Instantiate(border, new Vector2(0, 0), Quaternion.identity); // Top
            GameObject bottomBorder = Instantiate(border, new Vector2(0,0), Quaternion.identity); // Bottom

            leftBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            leftBorder.GetComponent<SpriteRenderer>().size = new Vector2(leftBorder.GetComponent<SpriteRenderer>().size.x, yValue);

            rightBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            rightBorder.GetComponent<SpriteRenderer>().size = new Vector2(rightBorder.GetComponent<SpriteRenderer>().size.x, yValue);

            topBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            topBorder.GetComponent<SpriteRenderer>().size = new Vector2(topBorder.GetComponent<SpriteRenderer>().size.x, xValue);

            bottomBorder.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            bottomBorder.GetComponent<SpriteRenderer>().size = new Vector2(bottomBorder.GetComponent<SpriteRenderer>().size.x, xValue);

            leftBorder.transform.parent = floor.transform;
            rightBorder.transform.parent = floor.transform;
            topBorder.transform.parent = floor.transform;
            bottomBorder.transform.parent = floor.transform;

            leftBorder.transform.localPosition = new Vector2((0 - (floorrenderer.size.x / 2) + (wallThickness / 2)), 0); // Left
            rightBorder.transform.localPosition = new Vector2(((floorrenderer.size.x / 2) - (wallThickness / 2)), 0); // Right
            topBorder.transform.localPosition = new Vector2(0, (floorrenderer.size.y / 2) - (wallThickness / 2)); // Top
            bottomBorder.transform.localPosition = new Vector2(0, 0 - (floorrenderer.size.y / 2) + (wallThickness / 2)); // Bottom


            topBorder.transform.Rotate(Vector3.back * 90);
            bottomBorder.transform.Rotate(Vector3.back * 90);
            
            leftBorder.name = "Left";
            rightBorder.name = "right";
            topBorder.name = "top";
            bottomBorder.name = "bottom";

            mapTiles.Add(floor);
        }
    }
}
