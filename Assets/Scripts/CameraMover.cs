using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("Настройки плавности")]
    [SerializeField, Min(0f)] private float _moveSpeed = 5f;     // Скорость перемещения
    [SerializeField, Min(0f)] private float _rotationSpeed = 5f; // Скорость поворота

    private Vector3 _offset = new Vector3(0f, 2f, -5f);
    private IMovable _target;
    private Transform _cameraTransform;

    void Start()
    {
        // Кэшируем собственный трансформ камеры для исключения нативных вызовов
        _cameraTransform = this.transform;
    }

    public void SetTarget(IMovable newTarget)
    {
        _target = newTarget;
        _offset = _cameraTransform.position - _target.Position;
    }

    void LateUpdate()
    {
        // Быстрая C# проверка на null (работает мгновенно)
        if (_target == null)
            return;

        float dt = Time.deltaTime;
        float moveLerpFactor = 1f - Mathf.Exp(-_moveSpeed * dt);
        
        Vector3 targetWorldPosition = _target.Position + _offset;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetWorldPosition, moveLerpFactor);
        
        // // --- НЕЗАВИСИМОСТЬ ОТ FPS ---
        // float dt = Time.deltaTime;
        // float moveLerpFactor = 1f - Mathf.Exp(-_moveSpeed * dt);
        // float rotLerpFactor = 1f - Mathf.Exp(-_rotationSpeed * dt);
        //
        // // --- КОНВЕРТАЦИЯ ВЕКТОРА В КВАТЕРНИОН ---
        // // Быстро переводим Vector3 углы персонажа в Кватернион один раз за кадр.
        // // Функция Quaternion.Euler оптимизирована на уровне движка.
        // Quaternion targetQuaternionRotation = Quaternion.Euler(_target.Rotation);
        //
        // // --- ЛОГИКА ПЕРЕМЕЩЕНИЯ ---
        // // Умножаем полученный кватернион на локальный оффсет, чтобы камера знала, где "зад" персонажа
        // Vector3 targetWorldPosition = _target.Position + (targetQuaternionRotation * _offset);
        //
        // // Плавно интерполируем позицию
        // _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetWorldPosition, moveLerpFactor);
        //
        // // --- ЛОГИКА ПОВОРОТА ---
        // // Плавно интерполируем вращение камеры вслед за кватернионом персонажа
        // _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation, targetQuaternionRotation, rotLerpFactor);
        
        // Вариант, если _target.Rotation - кватернион.
        // // --- ЛОГИКА ПЕРЕМЕЩЕНИЯ ---
        // // Умножаем кватернион поворота цели на локальный оффсет. 
        // // Это заставляет камеру автоматически залетать за спину персонажу при его разворотах.
        // Vector3 targetWorldPosition = _target.Position + (_target.Rotation * _offset);
        //
        // // Плавно интерполируем позицию
        // _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetWorldPosition, moveLerpFactor);
        //
        // // --- ЛОГИКА ПОВОРОТА ---
        // // Плавно интерполируем вращение камеры вслед за вращением персонажа
        // _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation, _target.Rotation, rotLerpFactor);
    }
}