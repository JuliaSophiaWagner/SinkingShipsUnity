using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public float xScale = 0.7f;
    public float yScale = 0.7f;
    public float xStart;
    public float yStart;
    public GameObject prefab;
    public (float,float) [,] gameField;
    public bool isEnemy;
    public GameObject explosion;
    private GameObject explosionInstance;
    public GameObject fire;
    private List<PlayerShot> anmiatedShots;
    private List<GameFieldItem> fields;

    // Start is called before the first frame update
    void Start()
    {
        this.InstantiateGrid();
    }

    public void InstantiateGrid()
    {
        if (this.gameField == null)
        {
            this.fields = new List<GameFieldItem>();
            this.anmiatedShots = new List<PlayerShot>();

            this.gameField = new (float, float)[rows, cols];

            this.GenerateGrid();
        }
    }

    public void GenerateGrid()
    {
        string text = "Field_";

        if (isEnemy)
        {
            text += "enemy_";
        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                float xPos = xStart + (xScale * i);
                float yPos = yStart + (-yScale * j);
                var obj = Instantiate(prefab, new Vector3(xPos, yPos), Quaternion.identity);
                obj.transform.parent = transform;
                obj.name = text + i + "_" + j;
                obj.GetComponent<GameFieldElement>().FieldX = i;
                obj.GetComponent<GameFieldElement>().FieldY = j;
                obj.GetComponent<GameFieldElement>().PosX = xPos;
                obj.GetComponent<GameFieldElement>().PosY = yPos;
                obj.GetComponent<GameFieldElement>().IsEnemy = isEnemy;
                this.gameField[i, j] = (xPos, yPos);
                this.fields.Add(new GameFieldItem(i, j, obj));
            }
        }
        // 455 / 40 * 0.7 bei Schiffe == Position der Blöcke (X = Pos - + 0.7; Y = Pos - 0.7)
    }

    public List<GridElement> GetGameField()
    {
        List<GridElement> field = new List<GridElement>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                field.Add(new GridElement(i, j));
            }
        }

        return field;
    }

    public void InstantiateAnimation(List<GridElement> grid, float volume)
    {
        foreach (var item in grid)
        {
            if (item.HasBeenShot)
            {
                this.ShipHit(new PlayerShot(item.XCoordinate, item.YCoordinate, item.IsShip), volume);
            }
        }
    }

    public void EnemyHit(PlayerShot shot, float volume)
    {
        if (shot != null)
        {
            this.ShipHit(shot, volume);
        }
    }

    public void ShipHit(PlayerShot shot, float volume)
    {
        if (this.anmiatedShots == null )
        {
            this.InstantiateGrid();
        }

        if (shot == null || this.anmiatedShots.Any(x => x.XCoordinate == shot.XCoordinate && x.YCoordinate == shot.YCoordinate))
        {
            return;
        }

        var field = this.fields.Where(x => x.PosX == shot.XCoordinate && x.PosY == shot.YCoordinate).FirstOrDefault();
        field.GameObject.GetComponent<GameFieldElement>().ShipHit(shot.IsShip, volume);
        this.anmiatedShots.Add(shot);
    }

    IEnumerator CreateFire((float, float) coordinate)
    {
        yield return new WaitForSeconds(3);
        Instantiate(fire, new Vector3(coordinate.Item1, coordinate.Item2 + 0.1f), Quaternion.identity);
        this.fire.GetComponent<Animator>().Play("flame_0");
        Destroy(explosionInstance);
    }
}
