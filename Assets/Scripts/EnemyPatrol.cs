using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private SpriteRenderer sprite;      //спрайт врага
    private Animator animator;          //аниматор содержащий анимации атаки и ходьбы
    private Transform player;           //координаты игрока
    public Transform patrolPoint;       //точка к которой привязан объект
    public Transform groundCheck;       //точка для проверки есть ли обрыв перед врагом
    public LayerMask Ground;            //слой по которому враг может ходить

    public float speed;                 //скорость врага
    public float patrolDistance;        //дистанция патрулирования врага  
    public float stopDistance;          //дистанция обнаружения игрока
    public float cooldownAttack=0.6f;   //время перезарядки атаки
    private float currentSpeed;         //текущая скорость врага
    private bool moveRight = true;      //проверка движения вправо
    private bool isAttacking = false;   //проверка атакует ли враг
    private bool patrol = false;        //проверка патрулирует ли враг
    private bool angry = false;         //проверка спровоцирован ли враг
    private bool goBack = false;        //проверка идет ли враг обратно патрулировать
    private bool isGround;               //проверка есть ли земля перед врагом
    private bool isWaiting = false;     //проверка ожидает ли враг



    void MoveRight()//функция для поворота врага вправо, при этом меняем соответствующую булевую переменную
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        moveRight = true;
    }
    void MoveLeft()//функция для поворота врага налево, при этом меняем соответствующую булевую переменную
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
        moveRight = false;
    }



    //метод рисует круги, для радиуса атаки и проверки земли под ногами
    //создано для отладки, в игре не задействуется
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }

    //метод вызывается в начале работы скрипта
    //объявляет спрайт врага, аниматор, и объявление переменной позиции игрока
    //устанавливает текущую скорость как максимальную
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        currentSpeed = speed;
    }
    //функция для определения находится ли игрок на видимой дистанции по высоте, чтобы спровоцировать врага
    bool agrHeight()
    {
        return (-0.3f < (transform.position.y - player.position.y) && (transform.position.y - player.position.y) < 1.2f);
    }
    //метод вызывается каждый фрейм
    void Update()
    {
        //проверка есть ли обрыв перед врагом
        //создается массив, в который попадают все элементы на слое Ground,
        //находящиеся в круге с центром в groundCheck и радиуса 0.1f
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f, Ground);                                                                                         
        isGround = colliders.Length > 0;//земля есть, если кол-во элементов в массиве больше нуля
        if (cooldownAttack > 0)//если атака не перезарядилась, уменьшаем время 
            cooldownAttack -= Time.deltaTime;
        //проверка находится ли враг в пределах дистанции патрулирования от точки патрулирования
        //если он при этом не спровоцирован, задаем ему состояние патрулирование
        if (Mathf.Abs(transform.position.x - patrolPoint.position.x) < patrolDistance && angry == false)
        {
            patrol = true;
        }
        //проверка находится ли враг в пределах дистанции обнаружения игрока и перед ним есть земля
        //задаем ему состояние агрессии 
        if ( Mathf.Abs(transform.position.x-player.position.x) < stopDistance&& agrHeight()&& isGround)
        {
            angry = true;
            patrol = false;
            goBack = false;
        }
        //проверка находится ли враг за пределами дистанции обнаружения игрока 
        //задаем ему состояние возвращения к патрулированию 
        if (Mathf.Abs(transform.position.x - player.position.x) > stopDistance || !agrHeight())
        {
            goBack = true;
            angry = false;
        }

        //в зависимости от состояния вызвываем соответствующую функцию
        if (patrol == true && !isAttacking)
            Patrol();
        else if (angry == true && !isAttacking)
            Agr();
        else if (goBack == true && !isAttacking)
            GoBack();
    }
    //функция патрулирования
    void Patrol()
    {   //разворачиваем врага, если перед ним пропасть и если он выходит за границу патрулирования
        if (transform.position.x > patrolPoint.position.x + patrolDistance || (!isGround && moveRight))
            MoveLeft();
        else if (transform.position.x < patrolPoint.position.x - patrolDistance ||(!isGround && !moveRight))
            MoveRight();
        //если враг находится в состоянии ожидания, выходим из него
        if (isWaiting)
        {
            isWaiting = false;
            animator.SetBool("isWaiting", false);
        }
        //перемещаем врага, с заданной скоростью
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }

    //функция преследования игрока
    void Agr()
    {
        //если перед врагом есть земля 
        if (isGround)
        {
            //разворачиваем врага, если игрок подошел ему со спины
            if (player.position.x < transform.position.x && moveRight == true)
                MoveLeft();
            if (player.position.x > transform.position.x && moveRight == false)
                MoveRight();
            //если враг находится в состоянии ожидания, выходим из него 
            if (isWaiting)
            {
                isWaiting = false;
                animator.SetBool("isWaiting", false);
            }
            //перемещаем врага, с заданной скоростью
            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        }
        //если перед врагом нет земли переходим в ожидание
        //создано для ситуации, когда игрок находится на лестнице,
        //тогда враг будет ждать пока игрок залезет на один уровень с ним
        else
        {
            isWaiting = true;
            animator.SetBool("isWaiting", true);           
        }
    }
    //функция возвращения к точке патрулирования
    void GoBack()
    {
        //разворачиваем врага, чтобы вернуться к точке патрулирования
        if (patrolPoint.position.x < transform.position.x && moveRight == true)
            MoveLeft();
        if (patrolPoint.position.x > transform.position.x && moveRight == false)
            MoveRight();
        // если враг находится в состоянии ожидания, выходим из него
        if (isWaiting)
        {
            isWaiting = false;
            animator.SetBool("isWaiting", false);
        }
        //перемещаем врага, с заданной скоростью
        transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
    }
    //функция получения урона, при этом враг замедляется на 0.5 секунд с помощью корутины
    public void GetDamage(int damage)
    {
        currentSpeed = 0.40f;
        GetComponent<HealthBar>().GetDamage(damage);
        StartCoroutine(StopTime());//запуск корутины
    }
    //корутина возвращающая скорость врагу через 0.5 секунд
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.5f);
        currentSpeed = speed;
    }
    //метод работает в течение соприкосновения объектов
    //создан для атаки и разворота в случае соприкосновения со стеной перед собой
    private void OnTriggerStay2D(Collider2D collision) {
        //если перед врагом не атакует, атака перезаряжена, и перед ним враг, то атакует
        if (collision.gameObject.CompareTag("Player")) {
            if (!isAttacking && cooldownAttack <= 0)
            {
                isAttacking = true;
                //рандом одной из двух видов анимации атаки
                animator.SetFloat("animChance", Random.Range(0.0f, 1.0f));
                animator.SetBool("isAttacking", isAttacking);

                StartCoroutine(AttackTime());//запуск корутины атаки
            }
        }
        else if (!collision.isTrigger)//если перед врагом стена, разворот
        {
            if (moveRight == true)
                MoveLeft();
            else
                MoveRight();
        }
    }
    //функция нанесения рандомного значения урона(6-11) игроку при ударе, вызывающаяся на 0.1 сек анимации
    private void onAttack()
    {
        player.gameObject.GetComponent<HealthBar>().GetDamage(Random.Range(6,11));
    }
    //корутина атаки, выключаем анимацию атаки, задаем кулдаун после удара
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        cooldownAttack = 0.5f;
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
}
