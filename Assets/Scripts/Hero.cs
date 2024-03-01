using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
	[SerializeField] private float speed = 5f;            // �������� �����
	[SerializeField] private float jumpForce = 15f;       // ������ ������ �����
	private bool isGrounded = false;

	private Rigidbody2D rb;   // ������ �����
	private Animator anim;      // �������� �����
	private SpriteRenderer sprite;          // �������� �����


	public bool isAttacking = false;        // ��������� �����
	public bool isRecharged = true;			// ����� �� ���������

	public Transform attackPos;             // ������� �����
	public float attcakRange;               // ���������� �����
	public LayerMask enemy;					// ���� � �������

	// ���������� Singleton
	public static Hero Instance { get;  set; }

	// ��������� � ������ ���������� ��� ��������� �������� �����
	private States State
	{
		get { return (States)anim.GetInteger("state"); }
		set { anim.SetInteger("state", (int)value); }
	}

	private void Start()
	{
		lives = 5;
	}
	// ��������� �������������(������ ������ �� ����������)
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


	// �������������� ���������� ������
	private void FixedUpdate()
	{
		CheckGround();
	}

	// �������� �� ������� ������
	private void Update()
	{
		if (isGrounded && !isAttacking) State = States.idle;

		if (!isAttacking && Input.GetButton("Horizontal")) Run();
		if (!isAttacking && isGrounded && Input.GetButtonDown("Jump")) Jump();
		if (Input.GetButtonDown("Fire1")) Attack();
	}

	// ����� ���� �����
	private void Run()
	{
		if (isGrounded) State = States.run;

		Vector3 dir = transform.right * Input.GetAxis("Horizontal");
		transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime); // �������� �� �����.
		sprite.flipX = dir.x < 0.0f;   // ��������� ��������� �� �����
	}

	// ����� ������ �����
	private void Jump()
	{
		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
	}

	// �������� ���� ����� �� �����
	private void CheckGround()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
		isGrounded = colliders.Length > 1;

		if (!isGrounded) State = States.jump;
	}

}


// ��������� �����
public enum States {
	idle,
	run,
	jump,
	attack
}

