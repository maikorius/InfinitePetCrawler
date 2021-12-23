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
    float xValueForMapTilePosition = 200;
    float yValueForMapTilePosition = 200;
    public int currenFloorCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMapTiles();
        #region testing purpose
        //mapTiles[0].SetActive(true);
        //mapTiles[0].GetComponent<SpriteRenderer>().color = Color.grey;
        //CheckHighestNumber(mapTiles[0], mapTiles[0].transform.position.x + mapTiles[0].GetComponent<SpriteRenderer>().size.x / 2, mapTiles[0].transform.position.x - (mapTiles[0].GetComponent<SpriteRenderer>().size.x / 2), true);
        //CheckHighestNumber(mapTiles[0], mapTiles[0].transform.position.y + mapTiles[0].GetComponent<SpriteRenderer>().size.y / 2, mapTiles[0].transform.position.y - (mapTiles[0].GetComponent<SpriteRenderer>().size.y / 2), false);

        //mapTiles[1].SetActive(true);
        //mapTiles[1].transform.position = new Vector2(100, 100);
        //AttachLeft(mapTiles[0], mapTiles[1], 1);
        #endregion
        LayoutMap();
        Debug.Log($"from the {mapTiles.Count} we could place {mapTiles.Where(w => w.activeInHierarchy).Count()}");
        mapTiles.ForEach(k =>
        {
            if (k.name.ToUpper() != "0")
            {
                if (!k.GetComponent<CollisionDetection>().attachedTo.gameObject.activeInHierarchy)
                {
                    k.SetActive(false);
                }
            }
        });
    }
    private void GenerateMapTiles()
    {
        int amountOfMaptiles = UnityEngine.Random.Range(minTiles, maxTiles + 1);

        for (int i = 0; i < amountOfMaptiles; i++)
        {
            int xValue = UnityEngine.Random.Range(minWidth, maxWidth);
            int yValue = UnityEngine.Random.Range(minHeight, maxHeight);
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
            mapTiles[0].GetComponent<CollisionDetection>().isPlacing = false;
            if (!mapTiles[currenFloorCount - 1].GetComponent<CollisionDetection>().isPlacing)
            {
                //Debug.Log($"Starting on {mapTiles[currenFloorCount]}");
                mapTiles[currenFloorCount].SetActive(true);
                GeneratePositionMapTile(mapTiles[currenFloorCount]);
                currenFloorCount++;
            }
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
    private List<int> GenerateMoveMents()
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
    private void DoMapTilesMovements(List<int> movement)
    {
        foreach (var moving in movement)
        {
            switch (moving)
            {
                //left
                case 1:
                    GameObject obj = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.x).FirstOrDefault();
                    xValueForMapTilePosition = obj.transform.position.x;
                    break;
                //right
                case 2:
                    GameObject obj2 = mapTiles.Where(fl => fl.transform.position.y == yValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.x).FirstOrDefault();
                    xValueForMapTilePosition = obj2.transform.position.x;
                    break;
                //up
                case 3:
                    GameObject obj3 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderByDescending(fl => fl.transform.position.y).FirstOrDefault();
                    yValueForMapTilePosition = obj3.transform.position.y;
                    break;
                //down
                case 4:
                    GameObject obj4 = mapTiles.Where(fl => fl.transform.position.x == xValueForMapTilePosition && fl.activeInHierarchy).OrderBy(fl => fl.transform.position.y).FirstOrDefault();
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

        if (Checkcollision(currentFloor))
        {
            CheckSides(currentFloor, spacing);
            if (Checkcollision(currentFloor))
            {
                //currentFloor.GetComponent<SpriteRenderer>().color = Color.black;
                //GeneratePositionMapTile(currentFloor);
                //Debug.Log($"{currentFloor.name} could not find a good spot. So going to the last added room ({currentFloor.GetComponent<CollisionDetection>().attachedTo})");

                for (int i = mapTiles.Count(x => x.activeInHierarchy) - 2; i >= 0; i--)
                {
                    currentFloor.GetComponent<CollisionDetection>().attachedTo = mapTiles[i];
                    //Debug.Log($"{currentFloor.name} changed attached room to {currentFloor.GetComponent<CollisionDetection>().attachedTo}");
                    CheckSides(currentFloor, spacing);
                    if (!Checkcollision(currentFloor))
                    {
                        //Debug.Log($"YEAAAAA Found a position!!! {currentFloor.name}");
                        //currentFloor.GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                    }
                }
                if (Checkcollision(currentFloor))
                {
                    currentFloor.GetComponent<CollisionDetection>().isPlacing = false;
                    currentFloor.SetActive(false);
                }
            }
        }
        if (!Checkcollision(currentFloor))
        {
            currentFloor.GetComponent<CollisionDetection>().isPlacing = false;
        }
    }
    private void CheckSides(GameObject currentFloor, int spacing)
    {
        //currentFloor.GetComponent<SpriteRenderer>().color = Color.green;
        List<int> options = new List<int> { 1, 2, 3, 4 };
        for (int i = 1; i <= 4; i++)
        {
            System.Random r = new System.Random();
            int currentOption = r.Next(options.Count);
            switch (currentOption)
            {
                case 1:
                    AttachLeft(currentFloor.GetComponent<CollisionDetection>().attachedTo, currentFloor, spacing);
                    //currentFloor.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case 2:
                    AttachUp(currentFloor.GetComponent<CollisionDetection>().attachedTo, currentFloor, spacing);
                    //currentFloor.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 3:
                    AttachRight(currentFloor.GetComponent<CollisionDetection>().attachedTo, currentFloor, spacing);
                    //currentFloor.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 4:
                    AttachDown(currentFloor.GetComponent<CollisionDetection>().attachedTo, currentFloor, spacing);
                    //currentFloor.GetComponent<SpriteRenderer>().color = Color.magenta;
                    break;
            }
            options.Remove(currentOption);
            if (!Checkcollision(currentFloor))
                break;
        }
    }
    private void SetFloorPosition(GameObject currentFloor, float xValue, float yValue)
    {
        currentFloor.transform.position = new Vector2(xValue, yValue);
        //Debug.Log($"Position({currentFloor.transform.position.x},{currentFloor.transform.position.y})|| minX: {currentFloor.transform.position.x + (currentFloor.GetComponent<SpriteRenderer>().size.x / 2)}|| MaxX: {currentFloor.transform.position.x - (currentFloor.GetComponent<SpriteRenderer>().size.x / 2)}" +
        //$"|| minY: {currentFloor.transform.position.y + (currentFloor.GetComponent<SpriteRenderer>().size.y / 2)}|| MaxY: {currentFloor.transform.position.y - (currentFloor.GetComponent<SpriteRenderer>().size.y / 2)}");
    }
    public void AttachDown(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;

        float yValue = attachTo.transform.position.y;
        yValue -= (attachTo.GetComponent<SpriteRenderer>().size.y / 2);
        yValue -= (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
        //Debug.Log($"{currentFloor.name} Checking Down postition({currentFloor.transform.position.x}, {currentFloor.transform.position.y})");
    }
    public void AttachUp(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;

        float yValue = attachTo.transform.position.y;
        yValue += (attachTo.GetComponent<SpriteRenderer>().size.y / 2);
        yValue += (currentFloor.GetComponent<SpriteRenderer>().size.y / 2) + spacing;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
        //Debug.Log($"{currentFloor.name} Checking Up postition({currentFloor.transform.position.x}, {currentFloor.transform.position.y})");
    }
    public void AttachLeft(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;
        xValue -= (attachTo.GetComponent<SpriteRenderer>().size.x / 2);
        xValue -= (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;

        float yValue = attachTo.transform.position.y;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
        //Debug.Log($"{currentFloor.name} Checking Left postition({currentFloor.transform.position.x}, {currentFloor.transform.position.y})");
    }
    public void AttachRight(GameObject attachTo, GameObject currentFloor, int spacing)
    {
        float xValue = attachTo.transform.position.x;
        xValue += (attachTo.GetComponent<SpriteRenderer>().size.x / 2);
        xValue += (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + spacing;

        float yValue = attachTo.transform.position.y;

        currentFloor.GetComponent<CollisionDetection>().attachedTo = attachTo;
        SetFloorPosition(currentFloor, xValue, yValue);
        //Debug.Log($"{currentFloor.name} Checking Right postition({currentFloor.transform.position.x}, {currentFloor.transform.position.y})");
    }
    public bool Checkcollision(GameObject currentFloor)
    {
        foreach (var item in mapTiles.Where(m => m.activeInHierarchy))
        {
            float currentFloorCalculatedX1 = currentFloor.transform.position.x - (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + 1;
            float currentFloorCalculatedX2 = currentFloor.transform.position.x + (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + 1;
            float currentFloorCalculatedY1 = currentFloor.transform.position.y - (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + 1;
            float currentFloorCalculatedY2 = currentFloor.transform.position.y + (currentFloor.GetComponent<SpriteRenderer>().size.x / 2) + 1;

            float currentFloorMinX = 0;
            float currentFloorMaxX = 0;
            float currentFloorMinY = 0;
            float currentFloorMaxY = 0;
            if (currentFloorCalculatedX1 > currentFloorCalculatedX2)
            {
                currentFloorMinX = currentFloorCalculatedX2;
                currentFloorMaxX = currentFloorCalculatedX1;
            }
            else
            {
                currentFloorMinX = currentFloorCalculatedX1;
                currentFloorMaxX = currentFloorCalculatedX2;
            }

            if (currentFloorCalculatedY1 > currentFloorCalculatedY2)
            {
                currentFloorMinY = currentFloorCalculatedY2;
                currentFloorMaxY = currentFloorCalculatedY1;
            }
            else
            {
                currentFloorMinY = currentFloorCalculatedY1;
                currentFloorMaxY = currentFloorCalculatedY2;
            }

            float DetectionCalculatedY1 = item.transform.position.y + (item.transform.GetComponent<SpriteRenderer>().size.y / 2);
            float DetectionCalculatedY2 = item.transform.position.y - (item.transform.GetComponent<SpriteRenderer>().size.y / 2);
            float DetectionCalculatedX1 = item.transform.position.x + (item.transform.GetComponent<SpriteRenderer>().size.x / 2);
            float DetectionCalculatedX2 = item.transform.position.x - (item.transform.GetComponent<SpriteRenderer>().size.x / 2);

            float detectionMinX = 0;
            float detectionMaxX = 0;
            float detectionMinY = 0;
            float detectionMaxY = 0;

            if (DetectionCalculatedX1 > DetectionCalculatedX2)
            {
                detectionMinX = DetectionCalculatedX2;
                detectionMaxX = DetectionCalculatedX1;
            }
            else
            {
                currentFloorMinX = DetectionCalculatedX1;
                currentFloorMaxX = DetectionCalculatedX2;
            }

            if (DetectionCalculatedY1 > DetectionCalculatedY2)
            {
                detectionMinY = DetectionCalculatedY2;
                detectionMaxY = DetectionCalculatedY1;
            }
            else
            {
                currentFloorMinY = DetectionCalculatedY2;
                currentFloorMaxY = DetectionCalculatedY1;
            }
            if (item.activeInHierarchy && item.name != currentFloor.name)
            {
                bool xValue = false;
                bool yValue = false;

                #region try1
                // check which positiion the currentfloor has in relaiton to the checked object
                //if (currentFloorMinX < detectionMinX && detectionMinX < currentFloorMaxX)
                //{
                //    if (currentFloorMinY < detectionMinY && detectionMinY < currentFloorMaxY)
                //    {
                //        return true;
                //    }
                //    else if (currentFloorMinY < detectionMaxY && detectionMaxY < currentFloorMaxY)
                //    {
                //        //debug.log($"Position of the currentfloor({currentFloor.name}) = ({currentFloor.transform.position.x}, {currentFloor.transform.position.y}) Minimal x = {currentFloorMinX} Maximal x = {currentFloorMaxX}|| Minimal Y = {currentFloorMinY} Maximal Y = {currentFloorMaxY} || Checking values x: {detectionMinX} and Y: {detectionMaxY}");
                //        //debug.log($"Position of the floor that will be checked({item.name}) = ({item.transform.position.x}, {item.transform.position.y}) Minimal x = {detectionMinX} Maximal x = {detectionMaxX}|| Minimal Y = {detectionMinY} Maximal Y = {detectionMaxY}");
                //        return true;
                //    }
                //}
                //else if (currentFloorMinX < detectionMaxX && detectionMaxX < currentFloorMaxX)
                //{
                //    if (currentFloorMinY < detectionMinY && detectionMinY < currentFloorMaxY)
                //    {
                //        //debug.log($"Position of the currentfloor({currentFloor.name}) = ({currentFloor.transform.position.x}, {currentFloor.transform.position.y}) Minimal x = {currentFloorMinX} Maximal x = {currentFloorMaxX}|| Minimal Y = {currentFloorMinY} Maximal Y = {currentFloorMaxY} || Checking values x: {detectionMaxX} and Y: {detectionMinY}");
                //        //debug.log($"Position of the floor that will be checked({item.name}) = ({item.transform.position.x}, {item.transform.position.y}) Minimal x = {detectionMinX} Maximal x = {detectionMaxX}|| Minimal Y = {detectionMinY} Maximal Y = {detectionMaxY}");
                //        return true;
                //    }
                //    if (currentFloorMinY < detectionMaxY && detectionMaxY < currentFloorMaxY)
                //    {
                //        //debug.log($"Position of the currentfloor({currentFloor.name}) = ({currentFloor.transform.position.x}, {currentFloor.transform.position.y}) Minimal x = {currentFloorMinX} Maximal x = {currentFloorMaxX}|| Minimal Y = {currentFloorMinY} Maximal Y = {currentFloorMaxY} || Checking values x: {detectionMaxX} and Y: {detectionMaxY}");
                //        //debug.log($"Position of the floor that will be checked({item.name}) = ({item.transform.position.x}, {item.transform.position.y}) Minimal x = {detectionMinX} Maximal x = {detectionMaxX}|| Minimal Y = {detectionMinY} Maximal Y = {detectionMaxY}");
                //        return true;
                //    }
                //}
                #endregion
                #region try2
                //if (currentFloorMinX < detectionMinX && detectionMinX < currentFloorMaxX)
                //    xValue = true;
                //else if (currentFloorMinX < detectionMaxX && detectionMaxX < currentFloorMaxX)
                //    xValue = true;
                //else if (currentFloorMinX < item.transform.position.x && item.transform.position.x < currentFloorMaxX)
                //    xValue = true;

                //if (currentFloorMinY < detectionMinY && detectionMinY < currentFloorMaxY)
                //    yValue = true;
                //else if (currentFloorMinY < detectionMaxY && detectionMaxY < currentFloorMaxY)
                //    yValue = true;
                //else if (currentFloorMinY < item.transform.position.y && item.transform.position.y < currentFloorMaxY)
                //    yValue = true;
                #endregion

                for (int i = Convert.ToInt32(detectionMinX); i < Convert.ToInt32(detectionMaxX); i++)
                {
                    if (currentFloorMinX <= i && i <= currentFloorMaxX)
                    {
                        xValue = true;
                        break;
                    }
                }

                for (int i = Convert.ToInt32(detectionMinY); i < Convert.ToInt32(detectionMaxY); i++)
                {
                    if (currentFloorMinY <= i && i <= currentFloorMaxY)
                    {
                        yValue = true;
                        break;
                    }
                }

                if (xValue && yValue)
                {
                    //Debug.Log($"Position of the currentfloor({currentFloor.name}) = ({currentFloor.transform.position.x}, {currentFloor.transform.position.y}) Minimal x = {currentFloorMinX} Maximal x = {currentFloorMaxX}|| Minimal Y = {currentFloorMinY} Maximal Y = {currentFloorMaxY} || Checking values x: {detectionMinX} and Y: {detectionMinY}");
                    //Debug.Log($"Position of the floor that will be checked({item.name}) = ({item.transform.position.x}, {item.transform.position.y}) Minimal x = {detectionMinX} Maximal x = {detectionMaxX}|| Minimal Y = {detectionMinY} Maximal Y = {detectionMaxY}");
                    return true;
                }
            }
        }
        return false;
    }
}