using UnityEngine;

public class LegacyCharacterController : MonoBehaviour
{
    private Animation anim;
    private bool isDead = false;

    void Start()
    {
        // Получаем старый компонент Animation
        anim = GetComponent<Animation>();
        
        // Включаем зацикливание для ходьбы и покоя, чтобы они не останавливались
        if (anim["Walk"]) anim["Walk"].wrapMode = WrapMode.Loop;
        if (anim["Idle"]) anim["Idle"].wrapMode = WrapMode.Loop;
        
        // Для атаки и урона ставим режим "один раз", чтобы они возвращались в Idle
        if (anim["Swing"]) anim["Swing"].wrapMode = WrapMode.Once;
        if (anim["Hurt"]) anim["Hurt"].wrapMode = WrapMode.Once;
        
        // При смерти анимация должна застыть на последнем кадре
        if (anim["Die"]) anim["Die"].wrapMode = WrapMode.ClampForever;
    }

    void Update()
    {
        if (isDead) return; // Если мертв, кнопки больше не работают

        // 1. Управление ходьбой (удержание кнопки W)
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Плавно переключаемся на ходьбу за 0.2 секунды
            anim.CrossFade("Walk", 0.2f); 
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            // Плавно возвращаемся в покой
            anim.CrossFade("Idle", 0.2f);
        }

        // 2. Атака (ЛКМ) — проигрывается поверх, если персонаж не занят другой важной анимацией
        if (Input.GetMouseButtonDown(0))
        {
            anim.CrossFade("Swing", 0.1f);
        }

        // 3. Получение урона (Кнопка H)
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.CrossFade("Hurt", 0.1f);
        }

        // 4. Смерть (Кнопка X)
        if (Input.GetKeyDown(KeyCode.X))
        {
            isDead = true;
            anim.CrossFade("Die", 0.2f);
        }
    }
}