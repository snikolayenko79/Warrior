using UnityEngine;

public class CameraMoverFollowByPlayer : CameraMoverBase
{
    [Header("Настройки плавности")]
    [SerializeField, Min(0f)] protected float rotationSpeed = 5f; // Скорость поворота
    
    private readonly Quaternion _minusYToForwardCorrection = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _initialCameraRotation;

    protected new void Start()
    {
        base.Start();
        
        _initialCameraRotation = _cameraTransform.rotation;
    }

    protected new void LateUpdate()
    {
        // Быстрая C# проверка на null (работает мгновенно)
        if (_target == null ||  _target.IsDead)
            return;
        
        // --- НЕЗАВИСИМОСТЬ ОТ FPS ---
        float dt = Time.deltaTime;
        float moveLerpFactor = 1f - Mathf.Exp(-moveSpeed * dt);
        float rotLerpFactor = 1f - Mathf.Exp(-rotationSpeed * dt);
        
        // --- КОНВЕРТАЦИЯ ВЕКТОРА В КВАТЕРНИОН ---
        // Быстро переводим Vector3 углы персонажа в Кватернион один раз за кадр.
        // Функция Quaternion.Euler оптимизирована на уровне движка.
        Quaternion playerLookRotation = Quaternion.Euler(_target.Rotation) * Quaternion.Euler(90f, 0f, 0f);
        
        float playerYAngle = playerLookRotation.eulerAngles.y;
        Quaternion playerYRotation = Quaternion.Euler(0f, playerYAngle, 0f);
        
        // --- ЛОГИКА ПЕРЕМЕЩЕНИЯ ---
        // Умножаем полученный кватернион на локальный оффсет, чтобы камера знала, где "зад" персонажа
        Vector3 targetWorldPosition = _target.Position + (playerLookRotation  * _offset);
        
        // Плавно интерполируем позицию
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetWorldPosition, moveLerpFactor);
        
        // --- ЛОГИКА ПОВОРОТА ---
        // Плавно интерполируем вращение камеры вслед за кватернионом персонажа
        Quaternion targetCameraRotation = playerYRotation * _initialCameraRotation;
        _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation, targetCameraRotation, rotLerpFactor);
        
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