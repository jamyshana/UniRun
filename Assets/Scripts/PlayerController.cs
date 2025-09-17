using UnityEngine;

public class Playercontroller : MonoBehaviour
{
	public AudioClip deathClip;
	public float jumpForce = 700f;          // Amount of force added when the player jumps.

	private int jumpCount = 0;              // Number of jumps made (max 2)
	private bool isGrounded = false;            // Whether or not the player is grounded.
	private bool isDead = false;                // Whether or not the player is dead.

	private Rigidbody2D playerRigidbody;    // Reference to the player's rigidbody.
	private Animator animator;              // Reference to the Animator component.
	private AudioSource playerAudio;        // Reference to the AudioSource component.

	private void Start()
	{
		// Setting up references.
		playerRigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		playerAudio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (isDead)
		{   // 사망 시 처리를 더 이상 진행하지 않고 종료
			return;
		}

		// 마우스 왼쪽 버튼을 눌렀으며 && 최대 점프 횟수(2)를 넘지 않았을 때
		if (Input.GetMouseButtonDown(0) && jumpCount < 2)
		{
			// 점프 횟수 증가
			jumpCount++;
			// 점프 직전 속도를 0으로 설정
			playerRigidbody.linearVelocity = Vector2.zero;
			// 점프 힘을 가함
			playerRigidbody.AddForce(new Vector2(0, jumpForce));
			// 점프 사운드 재생
			playerAudio.Play();
		}
		else if (Input.GetMouseButtonUp(0) && playerRigidbody.linearVelocity.y > 0)
		{  // 마우스 왼쪽 버튼에서 손을 뗐으며 && 상승 중일 때
		   // 현재 속도를 절반으로 줄임
			playerRigidbody.linearVelocity = playerRigidbody.linearVelocity * 0.5f;
		}
		// 애니메이터의 Grounded 파라미터를 isGrounded 값으로 설정
		animator.SetBool("Grounded", isGrounded);
	}

	private void Die()
	{
		// 애니메이터의 Die 트리거 파라미터를 셋
		animator.SetTrigger("Die");

		// 오디오 소스에 사망 클립을 설정하고 재생
		playerAudio.clip = deathClip;
		// 사망 사운드 재생
		playerAudio.Play();

		//	속도를 제로로 설정
		playerRigidbody.linearVelocity = Vector2.zero;
		// 사망상태를 참으로 설정
		isDead = true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Dead" && !isDead)
		{
			Die();  // 플레이어가 아직 죽지 않았으면 Die() 메서드를 호출
		}
		// 트리거 콜라이더를 가진 장애물과의 충돌을 감지
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.contacts[0].normal.y > 0.7f)
		{
			isGrounded = true;  // 바닥에 닿았음을 감지
			jumpCount = 0;      // 점프 횟수를 초기화
		}
		// 바닥과의 충돌을 감지
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		// 바닥에서 벗어났음을 감지
		isGrounded = false;
	}
}

 
