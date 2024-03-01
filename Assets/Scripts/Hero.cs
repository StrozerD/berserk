using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
	[SerializeField] private float speed = 5f;            // скорость перса
	[SerializeField] private float jumpForce = 15f;       // высота прыжка перса
	private bool isGrounded = false;

	private Rigidbody2D rb;   // Физика героя
	private Animator anim;      // Анимация героя
	private SpriteRenderer sprite;          // Картинка героя


	public bool isAttacking = false;        // Состояние атаки
	public bool isRecharged = true;			// Можно ли атаковать

	public Transform attackPos;             // Позиция атаки
	public float attcakRange;               // Расстояние атаки
	public LayerMask enemy;					// Слой с врагами

	// Реализация Singleton
	public static Hero Instance { get;  set; }

	// Установка и взятие переменных для состояния анимация героя
	private States State
	{
		get { return (States)anim.GetInteger("state"); }
		set { anim.SetInteger("state", (int)value); }
	}

	private void Start()
	{
		lives = 5;
	}
	// Первичная инициализация(взятие ссылок на компоненты)
	private void Awake()
	{
		Instance = this;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		isRecharged = true;
	}

	private IEnumerator AttackAnimation()
	{
		yield return new WaitForSeconds(0.4f);
		isAttacking = false;
	}

	private IEnumerator AttackCoolDown()
	{
		yield return new WaitForSeconds(0.5f);
		isRecharged = true;
	}

	private void Attack()
	{
		if (isGrounded && isRecharged) {
			State = States.attack;
			isAttacking = true;
			isRecharged= false;


			StartCoroutine(AttackAnimation());
			StartCoroutine(AttackCoolDown());
		}
	}

	private void onAttack()
	{
		print("Attack!!");
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attcakRange, enemy);
		print(colliders);
		for (int i = 0; i < colliders.Length; i++)
        {
			colliders[i].GetComponent<Entity>().GetDamage();
        }
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attcakRange);
	}


	// Фиксириованный постоянный апдейт
	private void FixedUpdate()
	{
		CheckGround();
	}

	// Проверка на нажатие кнопок
	private void Update()
	{
		if (isGrounded && !isAttacking) State = States.idle;

		if (!isAttacking && Input.GetButton("Horizontal")) Run();
		if (!isAttacking && isGrounded && Input.GetButtonDown("Jump")) Jump();
		if (Input.GetButtonDown("Fire1")) Attack();
	}

	// Метод бега героя
	private void Run()
	{
		if (isGrounded) State = States.run;

		Vector3 dir = transform.right * Input.GetAxis("Horizontal");
		transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime); // движение по гориз.
		sprite.flipX = dir.x < 0.0f;   // изменение положения по гориз
	}

	// Метод прыжка героя
	private void Jump()
	{
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
	}

	// Проверка если герой на земле
	private void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
		isGrounded = colliders.Length > 1;

		if (!isGrounded) State = States.jump;
	}

}


// Состояния героя
public enum States {
	idle,
	run,
	jump,
	attack
}

