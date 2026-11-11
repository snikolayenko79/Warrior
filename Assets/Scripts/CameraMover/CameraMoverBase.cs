using UnityEngine;

public class CameraMoverBase : MonoBehaviour
{
    [Header("Настройки плавности")]
    [SerializeField, Min(0f)] protected float moveSpeed = 5f;     // Скорость перемещения
    
    protected Vector3 _offset = new Vector3(0f, 0f, 0f);
    protected IMovable _target;
    protected Transform _cameraTransform;

    protected void Start()
    {
        // Кэшируем собственный трансформ камеры для исключения нативных вызовов
        _cameraTransform = this.transform;
    }

    public void SetTarget(IMovable newTarget)
    {
        _target = newTarget;
        _offset = _cameraTransform.position - _target.Position;
    }

    protected void LateUpdate()
    {
        // Быстрая C# проверка на null (работает мгновенно)
        if (_target == null ||  _target.IsDead)
            return;

        float dt = Time.deltaTime;
        float moveLerpFactor = 1f - Mathf.Exp(-moveSpeed * dt);
        
        Vector3 targetWorldPosition = _target.Position + _offset;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetWorldPosition, moveLerpFactor);
        
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