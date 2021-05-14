using UnityEngine;
using System.Collections;

//скрипт управления персонажем
public class PlayerController : MonoBehaviour
{
    [Header("Move Patameters")]
    [SerializeField] private float speed         = 3f;      //скорость движения
    [SerializeField] private float verticalSpeed = 5f;      //скорость движения по лестнице
    [SerializeField] private float jumpForce     = 15f;     //высота прыжка
    private float                  gravityScale  = 5f;      //гравитация
    private bool                   isGrounded    = false;   //проверка нахождения на земле
    private bool                   onRight       = true;    //проверка движения вправо
    private bool                   inCollWLadder = false;   //проверка соприкосновенря с лестницей
    public Transform               groundCheck;             //пустой объект проверяющий землю под ногами
    private Vector2                moveVector;              //вектор движения

    [Header("Attack Patameters")]
    public Transform               attackPos;               //центр области атаки
    public LayerMask               Enemies;                 //слой врагов
    public LayerMask               Ground;                  //слой земли
    private bool                   isAttacking   = false;   //проверка атаки
    public float                   attackRange;             //радиус атаки
    public int                     damage;                  //урон
    public float                   attackForce;             //сила отбрасывания при атаке
    public GameObject              shuriken;                //сюрикен

    [Header("Inventory")]
    private int                    potionsCount  = 3;       //счетчик зелий лечения
    public Animator                potionAnim;              //анимация зелий лечения для UI
    private int                    weaponsCount  = 1;       //счетчик оружия
    public Animator                weaponAnim;              //анимация количества оружия для UI
    public bool                    hasKey        = false;   //проверка наличия ключа
    public Animator                keyAnim;                 //анимация наличия ключа для UI

    private Rigidbody2D            rb;                      //тело игрока
    private HealthBar              healthbar;               //полоска хп
    private SpriteRenderer         sprite;                  //спрайт
    private Animator               animator;                //аниматор, включающий все анимации персонажа
    public GameObject              teleportationEffect;     //эффект телепортации
    public GameObject              effectBonus;             //эффект бонуса
    public GameObject              effectHealing;           //эффект лечения
    public gameUI gameUI;                                   //пользовательский интерфейс


    //метод вызывается в начале работы скрипта
    //объявляет тело игрока, спрайт, аниматор, полоску хп
    //убирает курсор в игре
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthbar = GetComponent<HealthBar>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    //метод вызывается каждые 0.02 секунды
    //если нажаты кнопки горизонтального бега и игрок не атакует
    //вызывается метод бега
    void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") && !isAttacking)
            Run();
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    //метод вызывается каждый фрейм
    void Update()
    {
        if (gameUI.gameIsPaused) //если игра на паузе ничего не делает
            return;
        CheckGround(); //проверяет землю под ногами
        //если нажата кнопка атаки, в данный момент не проходит анимация атаки и игрок на земле
        if (Input.GetButtonDown("Fire1") && !isAttacking && isGrounded)
            Attack(); //метод атаки
        if (isGrounded && !isAttacking) //если игрок на земле и не атакует
        {
            animator.SetInteger("State", 1); //переключение аниматора на анимацию покоя персонажа
            if (Input.GetButton("Horizontal")) //если нажата кнопка бега (влево, вправо)
                animator.SetInteger("State", 2); //переключение аниматора на анимацию бега
            if (Input.GetButtonDown("Jump") && isGrounded) //если нажата кнопка прыжка и персонаж находитсяна земле
                Jump(); //метод прыжка
        }
        //если нажата кнопка лечения (у нас Q), еще есть зелья и хп < максимума
        if (Input.GetKeyDown(KeyCode.Q) && potionsCount > 0 && healthbar.GetHP()< healthbar.maxHP) { 
            potionAnim.SetInteger("Count", --potionsCount); //переключаем картинку на UI и уменьшаем количество зелий
            Instantiate(effectHealing, transform.position, Quaternion.identity); //создаем эффект лечения
            healthbar.GetHeal(12); //восстанавливаем 12 хп
        }
        //если нажата кнопка броска сюрикена и их количество > 0
        if (Input.GetButtonDown("Fire2") && weaponsCount > 0)
        {
            weaponAnim.SetInteger("weaponsCount", --weaponsCount); //переключаем картинку на UI и уменьшаем количество сюрикенов
            Instantiate(shuriken, attackPos.position, transform.rotation); //создаем эффект броска
        }

    }


    //метод бега
    //изменяет координату игрока и поворачивает модельку(если необходимо)
    private void Run()
    {
        moveVector.x = Input.GetAxis("Horizontal"); //если нажата кнопка бега влево или вправо
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y); //перемещаем игрока
        if (moveVector.x < 0 && onRight) //проверяем нужно ли развернуть модельку
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0); //разворачиваем модельку
            onRight = false;
        }
        else if (moveVector.x > 0 && !onRight)  //проверяем нужно ли развернуть модельку
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0); //разворачиваем модельку
            onRight = true;
        }
    }

    //метод проверки земли под ногами
    //создается массив, в который попадают все элементы на слое Ground,
    //находящиеся в круге с центром в groundCheck и радиуса 0.1f
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f,Ground);
        isGrounded = (colliders.Length > 0) && !inCollWLadder;
        if (!isGrounded && !inCollWLadder) //если нет земли под ногами и игрок не на лестнице
            animator.SetInteger("State", 3); //переключение на анимацию падения
    }

    //метод прыжка
    //толкает лето ирока в направлении transform и силой jumpForce (ForceMode2D.Impulse - тип действующей силы)
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    //метод атаки
    private void Attack()
    {
        isAttacking = true;
        animator.SetInteger("State", 5); //переключение на анимацию ататки

        StartCoroutine(AttackTime()); //капуск корутины
    }

    //метод вызывается в аниматоре на 0.10 анимации атаки
    //создается массив, в который попадают все элементы на слое Enemies,
    //находящиеся в круге с центром в attackPos и радиуса attackRange
    //наносится урон каждому элементу в массиве
    private void onAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemies);
        for (int i = 0; i < colliders.Length; ++i)
            if(!colliders[i].isTrigger)
            colliders[i].GetComponent<EnemyPatrol>().GetDamage(damage);
    }

    //метод разрешает атаковать снова через 0.2 секунды
    //так создается некий кулдаун атаки
    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    //метод рисует круги, для радиуса атаки и проверки земли под ногами
    //создано для отладки, в игре не задействуется
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }

    //метод работает в течение соприкосновения объектов
    //создан для взаимодействия с лестницами и порталом
    private void OnTriggerStay2D(Collider2D collision)
    {
        //если игрок соприкасается с лестницей
        if (collision.gameObject.CompareTag("Ladder") || collision.gameObject.CompareTag("Ladder_l"))
        {
            rb.gravityScale = 0; //отключается гравитация (чтобы игрок не сползал вниз)
            inCollWLadder = true; //проверка на соприкосновение с лестницей
            animator.SetInteger("State", 4); //переключение на анимацию подъема по лестнице
            moveVector.y = Input.GetAxisRaw("Vertical"); //изменение координаты в зависимости от нажатой кнопки
            //переключение между анимациями движения по лестнице и покоя
            animator.SetFloat("moveY", Mathf.Abs(moveVector.y)); 
            rb.velocity = new Vector2(rb.velocity.x, moveVector.y * verticalSpeed); //изменение координаты
        }
        else if (collision.CompareTag("Portal")) //если игрок соприкасается с порталом
        {
            if (hasKey && Input.GetKeyDown(KeyCode.E)) { //если имеет ключ и нажата кнопка взимодействия (Е)
                hasKey = false;
                keyAnim.SetBool("hasKey", false); //переключаем анимацию UI на отсутствие ключа
                //переход в анимацию открытия портала
                collision.GetComponent<Animator>().SetBool("isOpened", true);
                //если портал открыт и нажата кнопка взаимодействия (Е)
            } else if (collision.GetComponent<Animator>().GetBool("isOpened")&&Input.GetKeyDown(KeyCode.E))
            {
                //активация эффекта телепортации
                teleportationEffect.SetActive(true);
                //запуск корутины
                StartCoroutine(Teleportation());
            }
        }
    }

    //метод телепортации
    //через 0.5 секунд переносит игрока на указанные координаты (в комнату с боссом)
    private IEnumerator Teleportation()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector2(-166.4f, -151.6f);
        yield return new WaitForSeconds(1f); //ожидание окончания анимации телепортации
        teleportationEffect.SetActive(false);
    }

    //метод вызывается вначале контакта объектов
    //отвечает за сбор бонусов
    private void OnTriggerEnter2D(Collider2D collision) {
        //если игрок соприкасается с зельем лечения и имеет их меньше трех
        if (collision.CompareTag("HealthPotion") && potionsCount < 3)
        {

            potionAnim.SetInteger("Count", ++potionsCount); //переключение анимации UI и увеличение счетчика зелий
            Instantiate(effectBonus, transform.position, Quaternion.identity); //вызов эффекта подбора бонуса на месте игрока
            Destroy(collision.transform.parent.gameObject); //уничтожение бонуса
        }
        //если игрок соприкасается с сюрикеном и имеет их меньше трех
        else if (collision.CompareTag("Weapon") && weaponsCount < 3)
        {
            weaponAnim.SetInteger("weaponsCount", ++weaponsCount); //переключение анимации UI и увеличение счетчика зелий
            Instantiate(effectBonus, transform.position, Quaternion.identity); //вызов эффекта подбора бонуса на месте игрока
            Destroy(collision.transform.parent.gameObject); //уничтожение бонуса
        }else if (collision.CompareTag("Key")) //если соприкасается с ключем
        {
            hasKey = true; //помещаем ключ в инвентарь
            keyAnim.SetBool("hasKey", true); //переключение анимации UI для ключа
            Instantiate(effectBonus, transform.position, Quaternion.identity); //вызов эффекта подбора бонуса на месте игрока
            Destroy(collision.transform.parent.gameObject); //уничтожение ключа
        }
    }

    //метод вызывается при выходе из контакта с объектом
    private void OnTriggerExit2D(Collider2D collision)
    {
        rb.gravityScale = gravityScale; //возврат гравитации в нормальное состояние
        inCollWLadder = false; //переменная соприкосновения с лестницей
    }
}