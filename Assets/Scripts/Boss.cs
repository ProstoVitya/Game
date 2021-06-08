using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Waves")]
    public GameObject FirstWave;//первая волна
    public GameObject EnemyWave;//вторая волна с врагами
    public GameObject ParkourWave;//третья волна с паркуром
    public GameObject FightWave;//четвертая волна
    private int stage;//текущая стадия

    [Header("First Wave")]
    public Transform[] points;//массив точек для передвижания босса    
    public Transform bosspoint1;//место спавна босса на текущей волне
    public GameObject bullet;//снаряд которым стреляет босс    
    private float rotZ;//угол поворота по оси z
    private int i;//итератор для точек передвижания босса
    private Vector2 difference;//разница между двумя точками
    private bool canRush = false;//проверка можно ли боссу бежать
    private bool x2 = false;//проверка можно ли нанести двойной урон

    [Header("Second Wave")]
    public List<GameObject> enemies;     //список врагов, появившихся в комнате
    public Transform[] spawners;         //места появления врагов
    public GameObject enemyType;//тип врага
    public Transform bosspoint2;//место спавна босса на текущей волне
    private int hpBeforeWave;//количество жизней перед волной

    [Header("Third Wave")]
    public Transform bosspoint3;//место спавна босса на текущей волне

    [Header("Fourth Wave")]
    public Transform point1;//точка №1 для передвижения босса
    public Transform point2;//точка №2 для передвижения босса
    public GameObject bullet1;//снаряд для стрельбы босса
    public float ReloadTime;//время перезарядки
    public float TakingDamageTime;//сколько времени может получать урон босс
    private bool goingLeft;//проверка на текущее направление движения босса
    private bool recharged;//проверка перезаряжен ли босс
    private bool canGo;//проверка может ли дальше передвигаться босс

    [Header("Other")]
    public float speed;//переменна скорости босса
    public Transform shotPoint;//точка откуда стреляет босс
    public Transform[] gasPoints;//точки спавна эффекта распыления газа
    public GameObject GasEffect;//эффект распыления газа
    public Transform PlayerPositionPoint;//место для перемещенния игрока в начале волны
    public GameObject blackoutEffect;   //эффект затемнения
    public GameObject buttonFininsh;    //кнопка для выхода с комнаты
    public GameObject key;              //ключ выпадающий с босса при смерти
    private Transform player;           //координаты игрока   
    private bool canTakeDamage = false; //проверка может ли получать урон босс
    private bool canDamage = false;     //проверка может ли наносить урон босс
    private bool dirRight = false;      //направление поворота босса
    private bool blocking = false;      //блокировщик
    private Animator animator;          //аниматор содержащий анимации атаки и ходьбы
    private AudioSource bossFX;         //источник звука
    public AudioClip ThrowSound;        //звук бросания снаряда  
    public AudioClip ShootSound;        //звук выстрела

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
        player = GameObject.FindGameObjectWithTag("Player").transform;//инициализируем игрока
        animator = GetComponent<Animator>();//аниматор, включающий все анимации босса
        bossFX = GetComponent<AudioSource>();//инициализируем источник звука
        stage = 1;//текущая волна номер 1
    }

    //метод вызывается каждый фрейм
    //в зависимости от номера текущей стадии обрабатываем ее
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
        //проверяем кончились ли хп у босса, 
        //при смерти выключаем все волны - 1
        //спавним ключ и кнопку для выхода с комнаты босса - 2
        //меняем игроку переменную проверки мертв ли босс - 3
        //удаляем босса - 4 
        if (gameObject.GetComponent<HealthBar>().GetHP() <= 0)
        {
            FirstWave.SetActive(false);//1
            EnemyWave.SetActive(false);
            ParkourWave.SetActive(false);
            FightWave.SetActive(false);
            Instantiate(buttonFininsh, PlayerPositionPoint.position, Quaternion.identity);//2
            Instantiate(key, transform.position, Quaternion.identity);
            player.GetComponent<PlayerController>().bossIsDead = true;//3
            Destroy(gameObject);//4
        }
    }
    //функция 1ой стадии босса
    void Stage1()
    {
        //если объект включающий все элементы первой стадии не активен включаем его,
        //задаем стартовую позицию босса
        if (!FirstWave.activeSelf)
        {
            FirstWave.SetActive(true);
            transform.position = bosspoint1.position;
            flipLeft();
            canRush = false;//запрещаем боссу бежать
            canTakeDamage = false;//запрещаем боссу получать урон
            canDamage = true;//запрещаем боссу наносить урон
            x2 = false;//выключаем двойной урон по боссу
            i = 1;//задаем итератор текущей точки для перемещения босса
        }
        if (i != points.Length)//если точка не последняя
        {
            //поворачиваем босса лицом к игроку 
            if (transform.position.x < player.position.x && !dirRight)
                flipRight();
            else if (transform.position.x > player.position.x && dirRight)
                flipLeft();
            //перемещения босса от текущего положения к i-ой точке с заданной скоростью 
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            //если достигнута точка увеличиваем итератор
            if (Vector2.Distance(transform.position, points[i].position) < 0.1f)
                ++i;
            if (animator.GetInteger("State") != 1)//если текущая анимация не бросание снарядов включаем ее
                animator.SetInteger("State", 1);
        }
        else
        {//направляем босса направо
            if (!dirRight)
                flipRight();
            //если босс еще не может бежать
            if (!canRush)
            {
                animator.SetInteger("State", 2);//включаем анимацию подготовки к бегу
                canTakeDamage = true;//включаем урон по боссу
                x2 = true;//включаем двойной урон по боссу
                StartCoroutine(WaitToRush());//запускаем корутину ожидания бега
            }//иначе включаем анимацию бега и перемещаем босса к последней точке с увеличенной скоростью
            else
            {
                animator.SetInteger("State", 3);
                transform.position = Vector2.MoveTowards(transform.position, points[0].position, 7.5f * speed * Time.deltaTime);
            }

        }//если босс достиг последней точки
        if (Vector2.Distance(transform.position, points[0].position) < 0.1f)
        {
            animator.SetInteger("State", 10);//включаем анимацию уязвимости босса
            x2 = false;//выключаем двойной урон по боссу
            canDamage = false;//запрещаем боссу наносить урон игроку
            if (!blocking)
            {
                blocking = true;//включаем блокировщик
                StartCoroutine(ChangeStage(2f, 2, FirstWave));//запускаем корутину смены стадии
            }
        }
    }

    //функция 2ой стадии босса
    void Stage2()
    {
        //если объект включающий все элементы второй стадии не активен включаем его,
        //задаем стартовую позицию босса
        if (!EnemyWave.activeSelf)
        {
            EnemyWave.SetActive(true);
            transform.position = bosspoint2.position;
            flipLeft();
            canTakeDamage = false;//запрещаем боссу получать урон
            canDamage = false;//запрещаем боссу наносить урон
            hpBeforeWave = GetComponent<HealthBar>().GetHP();//получаем количество хп перед началом стадии
            foreach (Transform spawner in spawners)//для каждой точки спавна, спавним врага с шансом 75%
            {
                if (Random.Range(0f, 1f) < 0.75f)
                {
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                }
            }
        }
        if (enemies.Count == 0)//если все враги текущей стадии мертвы
        {
            animator.SetInteger("State", 10);//включаем анимацию уязвимости босса
            canTakeDamage = true;//разрешаем боссу получать урон
            if (GetComponent<HealthBar>().GetHP() != hpBeforeWave && !blocking)//если игрок нанес первый удар по боссу
            {
                blocking = true;//включаем блокировщик
                StartCoroutine(ChangeStage(1.5f, 3, EnemyWave));//запускаем корутину смены стадии
            }
        }
    }

    //функция 3ей стадии босса
    void Stage3()
    {
        //если объект включающий все элементы третьей стадии не активен включаем его,
        //задаем стартовую позицию босса
        if (!ParkourWave.activeSelf)
        {
            ParkourWave.SetActive(true);
            transform.position = bosspoint3.position;
            flipRight();
            canTakeDamage = false;//запрещаем боссу получать урон
            canDamage = false;//запрещаем боссу наносить урон
            hpBeforeWave = GetComponent<HealthBar>().GetHP();//получаем количество хп перед началом стадии
        }
        animator.SetInteger("State", 10);//включаем анимацию уязвимости босса
        canTakeDamage = true;//разрешаем боссу получать урон
        if (GetComponent<HealthBar>().GetHP() != hpBeforeWave && !blocking)//если игрок нанес первый удар по боссу
        {
            blocking = true;//включаем блокировщик
            StartCoroutine(ChangeStage(1.5f, 4, ParkourWave));//запускаем корутину смены стадии
        }
    }

    //функция 4ой стадии босса
    void Stage4()
    {   //если объект включающий все элементы четвертой стадии не активен включаем его,
        //задаем стартовую позицию босса
        if (!FightWave.activeSelf)
        {
            FightWave.SetActive(true);
            transform.position = point1.position;
            flipLeft();
            shotPoint.rotation = Quaternion.Euler(0f, 0f, 90f);
            canTakeDamage = false;//запрещаем боссу получать урон
            canDamage = false;//запрещаем боссу наносить урон
            goingLeft = true;//задаем боссу перемещение влево
            recharged = false;//задаем боссу состояние перезаряжен
            canGo = true;//задаем боссу состояние разрешающее ему идти
            canDamage = true;
            StartCoroutine(waitpls());//запускаем корутину чтоб успеть игроку отойти
        }
        if (canGo) {//если босс может идти 
            //поворачиваем босса лицом к игроку
            if (transform.position.x < player.position.x && !dirRight)
                flipRight();
            else if (transform.position.x > player.position.x && dirRight)
                flipLeft();
            //если босс передвигается влево, перемещаем его, если он достиг конечной точки меняем направление движения
            if (goingLeft)
            {
                if (Vector2.Distance(transform.position, point2.position) > 0.1f)
                    transform.position = Vector2.MoveTowards(transform.position, point2.position, speed * Time.deltaTime);
                else
                    goingLeft = false;
            }
            else //если босс передвигается вправо, перемещаем его, если он достиг конечной точки меняем направление движения
            {
                if (Vector2.Distance(transform.position, point1.position) > 0.1f)
                    transform.position = Vector2.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);
                else
                    goingLeft = true;
            } 
        }        
        if (recharged)//если босс перезаряжен то запускаем корутину стрельбы
            StartCoroutine(Shoot());
    }
    //корутина ожидания 1,5сек, затем разрешаем стрелять боссу
    private IEnumerator waitpls()
    {
        yield return new WaitForSeconds(1.5f);
        recharged = true;
    }
        //корутина стрельбы
        private IEnumerator Shoot() {
        recharged = false;//задаем состояние не перезаряжен
        animator.SetInteger("State", 4);//включаем анимацию стрельбы
        yield return new WaitForSeconds(0.2f);//ждем 0.2 секунд      
        //цикл совершения 5ти выстрелов
        for (int i = 0; i < 5; ++i) {
            bossFX.PlayOneShot(ShootSound);//включаем звук выстрела
            Instantiate(bullet, shotPoint.position, shotPoint.rotation);//спавним снаряд
            yield return new WaitForSeconds(ReloadTime);//ждем время перезарядки
        }
        canGo = false;//останавливаем врага
        canDamage = false;
        animator.SetInteger("State", 5);//включаем анимацию перехода к уязвимости босса
        yield return new WaitForSeconds(0.4f);//ждем 0.4 секунды
        animator.SetInteger("State", 10);//включаем анимацию уязвимости босса
        canTakeDamage = true;//разрешаем получать урон
        yield return new WaitForSeconds(TakingDamageTime);//ждем время получения урона
        canTakeDamage = false;//запрещаем получать урон
        animator.SetInteger("State", 6);//включаем анимацию включения неуязвимости босса
        yield return new WaitForSeconds(1.5f);//ждем 1.5 секунды, чтобы дать игроку уйти
        canDamage = true;
        canGo = true;//разрешаем двигаться
        recharged = true;//босс перезаряжен
    }
    //корутина бросания снаряда
    void ThrowingBullet()
    {   //вычисляем угол по оси z до игрока и спавним снаряд,который будет лететь в таком направлении
        difference = player.position - shotPoint.position;
        rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        shotPoint.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        bossFX.PlayOneShot(ThrowSound);//включаем звук бросания снаряда
        Instantiate(bullet, shotPoint.position, shotPoint.rotation);//спавним снаряд
    }
    //корутина подготовки к бегу
    private IEnumerator WaitToRush()
    {
        yield return new WaitForSeconds(2.2f);
        canRush = true;
    }
    //корутина смены стадии
    private IEnumerator ChangeStage(float blockTime, int StageToChange, GameObject wave)
    {
        foreach (Transform spawner in gasPoints)//для каждой точки спавна
            Instantiate(GasEffect, spawner.position, spawner.rotation); //создаем эффект выпуска газа
        yield return new WaitForSeconds(blockTime);//ждем время ожидания перехода
        player.GetComponent<PlayerController>().canControl = false;//выключаем игроку управление
        player.GetComponent<Animator>().SetInteger("State", 1);//включаем анимацию покоя игроку
        blackoutEffect.SetActive(true);//включаем эффект затемнения
        yield return new WaitForSeconds(0.5f);//ждем 0.5 секунды
        animator.SetInteger("State", 0);//включаем анимацию покоя
        player.position = PlayerPositionPoint.position;//меняем позицию игрока на исходную в комнате босса
        wave.SetActive(false);//выключаем объект данной стадии
        stage = StageToChange;//переключаем стадию на следующую
        if (stage == 4) {//если стадия 4 принудительно ставим нормальный размер игрока
            if (!player.GetComponent<PlayerController>().normalSize)
                player.GetComponent<PlayerController>().changeSize();
        }else if (player.GetComponent<PlayerController>().normalSize)//если стадия не 4 принудительно ставим маленький размер игрока
            player.GetComponent<PlayerController>().changeSize();
        yield return new WaitForSeconds(1.5f); //ожидание окончания анимации телепортации
        blackoutEffect.SetActive(false);//выключаем эффект затемнения 
        blocking = false;//выключаем блокировщик
        canTakeDamage = false;//выключаем получение урона
        player.GetComponent<PlayerController>().canControl = true;//возвращаем управление игроку
    }
    public void GetDamage(int damage)//функция получения урона
    {
        if (canTakeDamage)//если можно получать урон 
        {
            if (stage == 4)//на 4ой стадии получаем вдвое меньше урона
                GetComponent<HealthBar>().GetDamage(damage / 2);
            else if (stage == 1)//на первой стадии во время бега босса двойный урон
            {//иначе обычный
                if (x2)
                    GetComponent<HealthBar>().GetDamage(2 * damage);
                else
                    GetComponent<HealthBar>().GetDamage(damage);
            }
            else GetComponent<HealthBar>().GetDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
            player.gameObject.GetComponent<HealthBar>().GetDamage(110);
    }
}
