using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private bool playerMertv = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerMertv) return; // Если погиб, кнопки не работают

        // Ходьба: зажали W — идет, отпустили — останавливается
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // Атака на ЛКМ
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
        
        // Заклинание на ПКМ
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Spell");
        }

        // Получение урона на H
        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("TakeDamage");
        }

        // Смерть на X
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerMertv = true;
            animator.SetTrigger("IsDead");
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 r = transform.localRotation.eulerAngles;
            r.y += 200 * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(r);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 r = transform.localRotation.eulerAngles;
            r.y -= 200 * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(r);
        }
    }
}