using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Waves")]
    public GameObject FirstWave;
    public GameObject EnemyWave;
    public GameObject ParkourWave;
    public GameObject ThrowingWave;
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
    private Transform player;            //координаты игрока    
    private bool canRush=false;

    [Header("Second Wave")]
    public List<GameObject> enemies;     //список врагов, появившихся в комнате
    public Transform[] spawners;         //места появления врагов
    public GameObject enemyType;
    public Transform bosspoint2;
    private int hpBeforeWave;

    [Header("Third Wave")]
    public Transform bosspoint3;

    [Header("Fourth Wave")]
    public Transform bosspoint4;

    [Header("Other")]
    public Transform PlayerPositionPoint;
    public GameObject blackoutEffect;   //эффект затемнения
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
        stage = 2;
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
    void Stage1() {
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
            }

        }
        if (Vector2.Distance(transform.position, points[0].position) < 0.1f)
        {
            animator.SetInteger("State", 10);
            canDamage = false;
            if (!blocking)
            {
                blocking = true;
                canTakeDamage = true;
                StartCoroutine(CooldownBlockingDamage(2f,2, FirstWave));
            }
        }
    }
    void Stage2() {
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
            if (GetComponent<HealthBar>().GetHP()!=hpBeforeWave&&!blocking)
            {
                blocking = true;
                StartCoroutine(CooldownBlockingDamage(1.5f,3, EnemyWave));
            }
        }
    }
    void Stage3() {
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
            StartCoroutine(CooldownBlockingDamage(1.5f, 4, EnemyWave));
        }
    }
    void Stage4() { 
    }
    void Shot() {
        difference = player.position - shotPoint.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shotPoint.rotation = Quaternion.Euler(0f, 0f, rotZ-90);
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);       
    }
    private IEnumerator WaitToRush()
    {
        yield return new WaitForSeconds(2.2f);
        canRush = true;
    }
    private IEnumerator CooldownBlockingDamage(float blockTime,int StageToChange,GameObject wave)
    {
        yield return new WaitForSeconds(blockTime);
        player.GetComponent<PlayerController>().canControl = false;
        player.GetComponent<Animator>().SetInteger("State", 1);         
        blackoutEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("State", 0);
        player.position = PlayerPositionPoint.position;
        wave.SetActive(false);
        stage = StageToChange;
        if (player.GetComponent<PlayerController>().normalSize)
            player.GetComponent<PlayerController>().changeSize();
        yield return new WaitForSeconds(1.5f); //ожидание окончания анимации телепортации
        blackoutEffect.SetActive(false);
        blocking = false;
        canTakeDamage = false;
        player.GetComponent<PlayerController>().canControl = true;        
    }
    public void GetDamage(int damage) {
        if(canTakeDamage)
        GetComponent<HealthBar>().GetDamage(damage);    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&canDamage)
            player.gameObject.GetComponent<HealthBar>().GetDamage(110);
    }
}
