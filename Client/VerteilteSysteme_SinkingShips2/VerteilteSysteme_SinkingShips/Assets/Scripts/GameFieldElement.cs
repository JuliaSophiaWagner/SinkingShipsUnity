using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameFieldElement : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public GameObject water;
    public Material shipHitTexture;
    public GameObject explosionInstance;
    private GameManager gameManager;
    private float waitForAnimationTime;

    public float PosX { get; set; }
    public float PosY { get; set; }
    public int FieldX { get; set; }
    public int FieldY { get; set; }
    public bool IsEnemy { get; set; }
    public bool IsHitted { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked on field: x = " + this.FieldX + " and y = " + this.FieldY);

        if (this.IsEnemy || this.IsHitted)
        {
            var task = Task.Run(async () => await gameManager.SendClickedField(this.FieldX, this.FieldY));
        }
    }

    public void ShipHit(bool isShip, float volume)
    {
        this.waitForAnimationTime = 0;

        if (isShip)
        {
            var explosionInstance = Instantiate(this.explosion, new Vector3(this.PosX, this.PosY), Quaternion.identity);
            explosionInstance.GetComponent<AudioSource>().volume = volume;
            explosionInstance.GetComponent<Animator>().Play("explosion_0");
            this.waitForAnimationTime = 3.5f;
            StartCoroutine(CreateFire(explosionInstance));
            return;
        }

        var waterAnim = Instantiate(this.water, new Vector3(this.PosX, this.PosY), Quaternion.identity);
        waterAnim.GetComponent<AudioSource>().volume = volume;
        waterAnim.GetComponent<Animator>().Play("water1_0 1");
        StartCoroutine(DestroyWaterAnimation(waterAnim));
        transform.GetComponent<MeshRenderer>().material = this.shipHitTexture;
    }

    private IEnumerator CreateFire(GameObject fireAnim)
    {
        yield return new WaitForSeconds(waitForAnimationTime);
        Instantiate(fire, new Vector3(this.PosX, this.PosY + 0.1f), Quaternion.identity);
        this.fire.GetComponent<Animator>().Play("flame_0");
        Destroy(fireAnim);
    }

    private IEnumerator DestroyWaterAnimation(GameObject obj)
    {
        yield return new WaitForSeconds(1.4f);
        Destroy(obj);
    }
}
