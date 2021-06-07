using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Waves")]
    public GameObject FirstWave;
    public GameObject EnemyWave;
    public GameObject ParkourWave;
    public GameObject FightWave;
    private int stage;

    [Header("First Wave")]
    public Transform[] points;
    public Transform shotPoint;
    public Transform bosspoint1;
    public GameObject bullet;
    public GameObject button;
    public float speed;
    private float rotZ;
    private int i;
    private Vector2 difference;                 
    private bool canRush = false;

    [Header("Second Wave")]
    public List<GameObject> enemies;     //список врагов, появившихся в комнате
    public Transform[] spawners;         //места появления врагов
    public GameObject enemyType;
    public Transform bosspoint2;
    private int hpBeforeWave;

    [Header("Third Wave")]
    public Transform bosspoint3;

    [Header("Fourth Wave")]
    public Transform point1;
    public Transform point2;
    public GameObject bullet1;
    public float ReloadTime;
    public float TakingDamageTime;
    private bool goingLeft;
    private bool recharged;
    private bool canGo;

    [Header("Other")]
    public Transform[] gasPoints;
    public GameObject GasEffect;
    public Transform PlayerPositionPoint;
    public GameObject blackoutEffect;   //эффект затемнения
    private Transform player;           //координаты игрока   
    private bool canTakeDamage = false;
    private bool canDamage = false;
    private bool dirRight = false;
    private bool blocking = false;
    private Animator animator;          //аниматор содержащий анимации атаки и ходьбы

    void flipRight()//функция для поворота босса вправо, при этом меняем соответствующую булевую переменную
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        dirRight = true;
    }
    void flipLeft()//функция для поворота босса налево, при этом меняем соответствующую булевую переменную
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        dirRight = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();//аниматор, включающий все анимации босса
        stage = 4;
    }


    // Update is called once per frame
    void Update()
    {
        if (stage == 1)
            Stage1();
        else if (stage == 2)
            Stage2();
        else if (stage == 3)
            Stage3();
        else if (stage == 4)
            Stage4();


    }
    void Stage1()
    {
        if (!FirstWave.activeSelf)
        {
            FirstWave.SetActive(true);
            transform.position = bosspoint1.position;
            flipLeft();
            canRush = false;
            canTakeDamage = false;
            canDamage = true;
            i = 1;
        }
        if (i != points.Length)
        {
            if (transform.position.x < player.position.x && !dirRight)
                flipRight();
            else if (transform.position.x > player.position.x && dirRight)
                flipLeft();

            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, points[i].position) < 0.1f)
                ++i;
            if (animator.GetInteger("State") != 1)
                animator.SetInteger("State", 1);
        }
        else
        {
            if (!dirRight)
                flipRight();
            if (!canRush)
            {
                animator.SetInteger("State", 2);
                StartCoroutine(WaitToRush());
            }
            else
            {
                animator.SetInteger("State", 3);
                transform.position = Vector2.MoveTowards(transform.position, points[0].position, 7.5f * speed * Time.deltaTime);
                canTakeDamage = true;
            }

        }
        if (Vector2.Distance(transform.position, points[0].position) < 0.1f)
        {
            animator.SetInteger("State", 10);
            canDamage = false;
            if (!blocking)
            {
                blocking = true;
                StartCoroutine(ChangeStage(2f, 2, FirstWave));
            }
        }
    }
    void Stage2()
    {
        if (!EnemyWave.activeSelf)
        {
            EnemyWave.SetActive(true);
            transform.position = bosspoint2.position;
            flipLeft();
            canTakeDamage = false;
            canDamage = false;
            hpBeforeWave = GetComponent<HealthBar>().GetHP();
            foreach (Transform spawner in spawners)
            {
                if (Random.Range(0f, 1f) < 0.75f)
                {
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                }
            }
        }
        if (enemies.Count == 0)
        {
            animator.SetInteger("State", 10);
            canTakeDamage = true;
            print(GetComponent<HealthBar>().GetHP());
            if (GetComponent<HealthBar>().GetHP() != hpBeforeWave && !blocking)
            {
                blocking = true;
                StartCoroutine(ChangeStage(1.5f, 3, EnemyWave));
            }
        }
    }
    void Stage3()
    {
        if (!EnemyWave.activeSelf)
        {
            ParkourWave.SetActive(true);
            transform.position = bosspoint3.position;
            flipRight();
            canTakeDamage = false;
            canDamage = false;
            hpBeforeWave = GetComponent<HealthBar>().GetHP();
        }
        animator.SetInteger("State", 10);
        canTakeDamage = true;
        if (GetComponent<HealthBar>().GetHP() != hpBeforeWave && !blocking)
        {
            blocking = true;
            StartCoroutine(ChangeStage(1.5f, 4, EnemyWave));
        }
    }
    void Stage4()
    {
        if (!FightWave.activeSelf)
        {
            FightWave.SetActive(true);
            transform.position = point1.position;
            flipLeft();
            shotPoint.rotation = Quaternion.Euler(0f, 0f, 90f);
            canTakeDamage = false;
            canDamage = false;
            goingLeft = true;
            recharged = true;
            canGo = true;
        }
        if (canGo) {
            if (transform.position.x < player.position.x && !dirRight)
                flipRight();
            else if (transform.position.x > player.position.x && dirRight)
                flipLeft();
            if (goingLeft)
            {
                if (Vector2.Distance(transform.position, point2.position) > 0.1f)
                    transform.position = Vector2.MoveTowards(transform.position, point2.position, speed * Time.deltaTime);
                else
                    goingLeft = false;
            }
            else
            {
                if (Vector2.Distance(transform.position, point1.position) > 0.1f)
                    transform.position = Vector2.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);
                else
                    goingLeft = true;
            } 
        }        
        if (recharged)
            StartCoroutine(Shoot());
    }
    private IEnumerator Shoot() {
        animator.SetInteger("State", 4);
        recharged = false;
        for (int i = 0; i < 5; ++i) {
            Instantiate(bullet, shotPoint.position, shotPoint.rotation);
            yield return new WaitForSeconds(ReloadTime);
        }
        canGo = false;
        animator.SetInteger("State", 5);
        yield return new WaitForSeconds(0.4f);
        animator.SetInteger("State", 10);
        canTakeDamage = true;
        yield return new WaitForSeconds(TakingDamageTime);
        canTakeDamage = false;
        animator.SetInteger("State", 6);
        yield return new WaitForSeconds(1.5f);
        canGo = true;
        recharged = true;
    }
    void ThrowingBullet()
    {
        difference = player.position - shotPoint.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shotPoint.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
    }
    private IEnumerator WaitToRush()
    {
        yield return new WaitForSeconds(2.2f);
        canRush = true;
    }
    private IEnumerator ChangeStage(float blockTime, int StageToChange, GameObject wave)
    {
        foreach (Transform spawner in gasPoints)
            Instantiate(GasEffect, spawner.position, spawner.rotation); //создаем эффект выпуска газа
        yield return new WaitForSeconds(blockTime);
        player.GetComponent<PlayerController>().canControl = false;
        player.GetComponent<Animator>().SetInteger("State", 1);
        blackoutEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("State", 0);
        player.position = PlayerPositionPoint.position;
        wave.SetActive(false);
        stage = StageToChange;
        if (stage == 4) {
            if (!player.GetComponent<PlayerController>().normalSize)
                player.GetComponent<PlayerController>().changeSize();
        }else if (player.GetComponent<PlayerController>().normalSize)
            player.GetComponent<PlayerController>().changeSize();
        yield return new WaitForSeconds(1.5f); //ожидание окончания анимации телепортации
        blackoutEffect.SetActive(false);
        blocking = false;
        canTakeDamage = false;
        player.GetComponent<PlayerController>().canControl = true;
    }
    public void GetDamage(int damage)
    {
        if (canTakeDamage) {
            if (stage == 4)
                GetComponent<HealthBar>().GetDamage(damage / 2);
            else {
                if (!canRush)
                    GetComponent<HealthBar>().GetDamage(damage);
                else
                    GetComponent<HealthBar>().GetDamage(2 * damage);
            }            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
            player.gameObject.GetComponent<HealthBar>().GetDamage(110);
    }
}
