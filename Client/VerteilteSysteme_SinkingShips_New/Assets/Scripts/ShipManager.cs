using Assets;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public List<ShipUnity> shipsUnity;
    public GameObject grid;
    public GameObject shipSize2;
    public GameObject shipSize3;
    public GameObject shipSize4;
    public GameObject shipSize5;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void AutoPlaceShips()
    {
        System.Random rand = new System.Random();
        int counter = 0;
        Dictionary<int, Transform> objTemp = new Dictionary<int, Transform>();
        this.ClearTransformChilds();

        for (int i = 0; i < transform.childCount; i++)
        {
            objTemp.Add(i, transform.GetChild(i));
        }

        Dictionary<int, Transform> sortedList = objTemp.OrderByDescending(x => x.Value.GetComponent<DragAndDrop>().ship.Size).ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < sortedList.Count; i++)
        {
            int x = rand.Next(0, 10);
            int y = rand.Next(0, 10);
            bool rotatedTemp = rand.Next(2) == 0;
            bool rotated = transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().ship.Rotated;

            if (shipsUnity.Any(z => z.FieldTemp.Any(s => s.Item1 == x && s.Item2 == y)))
            { 
                counter++;
                i--;
                continue;
            }

            if (rotated != rotatedTemp)
            {
                transform.GetChild(sortedList.ElementAt(i).Key).Rotate(new Vector3(0, 0, -90));
                transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().ship.Rotated = !rotated;
            }

            if (transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().PlaceShip(x, y))
            {
                counter = 0;
                continue;
            }

            rotated = !transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().ship.Rotated;

            if (rotated != rotatedTemp)
            {
                transform.GetChild(sortedList.ElementAt(i).Key).Rotate(new Vector3(0, 0, -90));
                transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().ship.Rotated = rotated;
            }

            if (transform.GetChild(sortedList.ElementAt(i).Key).GetComponent<DragAndDrop>().PlaceShip(x, y))
            {
                counter = 0;
                continue;
            }

            i--;
            counter++;

            if (counter > 1000)
            {
                this.ClearTransformChilds();
                counter = 0;
                i = -1;
                continue;
            }
        }
    }

    private void ClearTransformChilds()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<DragAndDrop>().ship.PosX = -1;
            child.GetComponent<DragAndDrop>().ship.PosY = -1;
            child.GetComponent<DragAndDrop>().PlaceShip(-1, -1);
            child.GetComponent<DragAndDrop>().ship.Field.Clear();
            child.GetComponent<DragAndDrop>().ship.FieldTemp.Clear();
            this.UpdateShipList(child.GetComponent<DragAndDrop>().ship);
        }
    }

    public void UpdateShipList(ShipUnity ship)
    {
        this.shipsUnity.ElementAt(this.shipsUnity.IndexOf(ship)).PosX = ship.PosX;
        this.shipsUnity.ElementAt(this.shipsUnity.IndexOf(ship)).PosY = ship.PosY;
        this.shipsUnity.ElementAt(this.shipsUnity.IndexOf(ship)).Rotated = ship.Rotated;
    }

    public void InstantiateShip(ShipUnity ship)
    {
        if (this.shipsUnity == null)
        {
            this.shipsUnity = new List<ShipUnity>();
        }

        this.shipsUnity.Add(ship);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<Ship> GetShips()
    {
        List<Ship> ships = new List<Ship>();

        foreach (var item in this.shipsUnity)
        {
            List<GridElement> grid = new List<GridElement>();


            if (item.PosX == -1 || item.PosY == -1)
            {
                return null;
            }

            foreach (var field in item.Field)
            {
                grid.Add(new GridElement(field.Item1, field.Item2));
            }

            ships.Add(new Ship(item.Size, grid, item.Rotated, item.PosX, item.PosY));
        }

        return ships;
    }

    public void DeactivateDragAndDrop()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<DragAndDrop>().enabled = false;
        }
    }

    public bool InstanitateFieldWithData(List<Ship> ships)
    {
        if (ships == null || ships.Count < 1)
        {
            return false;
        }

        GridManagerScript field = grid.GetComponent<GridManagerScript>();
        field.InstantiateGrid();

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        foreach (var item in ships)
        {
            GameObject prefab = null;
            switch (item.Fields.Count)
            {
                case 2:
                    prefab = shipSize2;
                    break;
                case 3:
                    prefab = shipSize3;
                    break;
                case 4:
                    prefab = shipSize4;
                    break;
                case 5:
                    prefab = shipSize5;
                    break;
            }

            this.InstantiateShip(prefab, field.gameField[item.PosX, item.PosY].Item1, field.gameField[item.PosX, item.PosY].Item2, item.Fields.Count, item.PosX, item.PosY, item.Rotated);
        }

        return true;
    }

    private void InstantiateShip(GameObject prefab, float x, float y, int size, int xPos, int yPos, bool rotated)
    {
        string name = "Ship";
        var obj = Instantiate(prefab, new Vector3(-1, -1), Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(40, 40, 0);
        obj.name = name + size + "_" + yPos;
        obj.GetComponent<DragAndDrop>().enabled = false;
        float scaleFactorX = 0.7f / obj.transform.localScale.x;
        float scaleFactorY = 0.7f / obj.transform.localScale.y;
        int uneven = size % 2 - 1;

        if (!rotated)
        {
            obj.transform.position = new Vector3(x / scaleFactorX * canvas.transform.localScale.x, ((y - (0.35f * uneven)) / scaleFactorY) * canvas.transform.localScale.y, -1);
            return;
        }

        obj.transform.Rotate(0, 0, 90);
        obj.transform.position = new Vector3((x - (0.35f * uneven)) / scaleFactorX * canvas.transform.localScale.x, (y / scaleFactorY) * canvas.transform.localScale.y, -1);
    }
}
