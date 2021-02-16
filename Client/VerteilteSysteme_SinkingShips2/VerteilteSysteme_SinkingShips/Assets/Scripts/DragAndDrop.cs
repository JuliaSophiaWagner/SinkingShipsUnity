using Assets;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    public Canvas canvas;
    public ShipUnity ship;
    public new GameObject animation;
    public GameObject gridObject;
    public GameObject animation2;
    private Vector3 startingPosition;
    private GridManagerScript grid;
    private bool isDragging;
    private float distance;

    private void Start()
    {
        this.grid = this.gridObject.GetComponent<GridManagerScript>();
        this.startingPosition = transform.position;
        this.ship = new ShipUnity(transform.gameObject.name, int.Parse(transform.gameObject.name.ElementAt(4).ToString()), -1, -1);
        transform.parent.gameObject.GetComponent<ShipManager>().InstantiateShip(this.ship);
    }

    private void Update()
    {
        if (this.isDragging)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(new Vector3(0, 0, -90));
                ship.Rotated = !ship.Rotated;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 raypoint = ray.GetPoint(distance);
            raypoint.z = -1;
            transform.position = raypoint;
        }
    }

    public bool PlaceShip(int x, int y)
    {
       try
       {
            float scaleFactorX = 0.7f / transform.localScale.x;
            float scaleFactorY = 0.7f / transform.localScale.y;
            this.ship.PosX = x;
            this.ship.PosY = y;

            if (!CheckGridForAnotherShip() || this.ship.Field.Any(z => z.Item1 < 0 || z.Item2 < 0 || z.Item1 > 9 || z.Item2 > 9))
            {
                throw new IndexOutOfRangeException();
            }

            this.ConvertPosition(this.grid.gameField[this.ship.PosX, this.ship.PosY].Item1, this.grid.gameField[this.ship.PosX, this.ship.PosY].Item2, scaleFactorX, scaleFactorY);
            transform.parent.gameObject.GetComponent<ShipManager>().UpdateShipList(this.ship);
            return true;
       }
       catch (IndexOutOfRangeException)
       {
            transform.position = startingPosition;
            this.ship.PosX = -1;
            this.ship.PosY = -1;
            this.ship.Field.Clear();
            this.ship.FieldTemp.Clear();

            if (ship.Rotated)
            {
                transform.Rotate(new Vector3(0, 0, -90));
                this.ship.Rotated = false;
            }

            return false;
       }
    }


    public void OnMouseDown()
    {
        this.isDragging = true;
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    public void OnMouseUp()
    {
        this.isDragging = false;
        float scaleFactorX = 0.7f / transform.localScale.x;
        float scaleFactorY = 0.7f / transform.localScale.y;
        float convertedPositionX = transform.localPosition.x * scaleFactorX;
        float convertedPositionY = transform.localPosition.y * scaleFactorY;
        try
        {
            this.ship.PosX = this.CheckGridPosition(this.CalculatePosition(0, -8 + 0.35f, convertedPositionX), true);
            this.ship.PosY = this.CheckGridPosition(this.CalculatePosition(0, -2 + 0.35f, -convertedPositionY), false);

            if (!CheckGridForAnotherShip() || this.ship.Field.Any(z => z.Item1 < 0 || z.Item2 < 0 || z.Item1 > 9 || z.Item2 > 9))
            {
                throw new IndexOutOfRangeException();
            }

            this.ConvertPosition(this.grid.gameField[this.ship.PosX, this.ship.PosY].Item1, this.grid.gameField[this.ship.PosX, this.ship.PosY].Item2, scaleFactorX, scaleFactorY);
            transform.parent.gameObject.GetComponent<ShipManager>().UpdateShipList(this.ship);
        }
        catch (IndexOutOfRangeException)
        {
            transform.position = startingPosition;
            this.ship.PosX = -1;
            this.ship.PosY = -1;
            this.ship.Field.Clear();
            this.ship.FieldTemp.Clear();

            if (ship.Rotated)
            {
                transform.Rotate(new Vector3(0, 0, -90));
                this.ship.Rotated = false;
            }
        }
    }

    private bool CheckGridForAnotherShip()
    {
        foreach (var item in transform.parent.gameObject.GetComponent<ShipManager>().shipsUnity)
        {
            if (item == null)
            {
                throw new NullReferenceException("Ship item is null");
            }            
            else if (item != this.ship && item.Field.Count != 0 && item.FieldTemp.Intersect(this.ship.Field).Any())
            {
                return false;
            }
        }

        return true;
    }

    private int CheckGridPosition(int gridIndex, bool isXAxis)
    {
        if (!ship.Rotated && !isXAxis && gridIndex - ship.Offset < 0)
        {
            return ship.Offset;
        }
        else if (!ship.Rotated && !isXAxis && gridIndex + ship.Offset > 9)
        {
            if (ship.Uneven != 0)
            {
                return 9 - ship.Offset + 1;
            }

            return 9 - ship.Offset;
        }
        else if (ship.Rotated && isXAxis && gridIndex - ship.Offset < 0)
        {
            if (ship.Uneven != 0)
            {
                return ship.Offset - 1;
            }

            return ship.Offset;
        }
        else if (ship.Rotated && isXAxis && gridIndex + ship.Offset > 9)
        {            
            return 9 - ship.Offset;
        }

        return gridIndex;
    }

    private void ConvertPosition(float gridX, float gridY, float scaleFactorX, float scaleFactorY)
    {
        if (!ship.Rotated)
        {
            transform.position = new Vector3((gridX / scaleFactorX) * canvas.transform.localScale.x, (((gridY - (0.35f * ship.Uneven)) / scaleFactorY)) * canvas.transform.localScale.y, -1);
            return;
        }

        transform.position = new Vector3(((gridX - (0.35f * ship.Uneven)) / scaleFactorX) * canvas.transform.localScale.x, ((gridY / scaleFactorY)) * canvas.transform.localScale.y, -1);
    }

    private int CalculatePosition(int index, float offset, float objectPosition)
    {
        while (((index * 0.7f) + offset) < objectPosition)
        {
            index++;
        }

        return index;
    }
}
