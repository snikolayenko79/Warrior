using UnityEngine;
using UnityEngine.UI;

public class UITrianglePointer : MaskableGraphic
{
    [Header("Настройки формы")]
    [SerializeField] private float baseWidth = 40f; // Ширина основания треугольника у панели UI

    private Vector2 _startPoint; // Точка А (У панели диалога)
    private Vector2 _endPoint;   // Точка Б (Над головой персонажа на экране)

    // Метод для обновления позиций из менеджера диалогов
    public void SetPositions(Vector2 startScreenPos, Vector2 endScreenPos)
    {
        _startPoint = startScreenPos;
        _endPoint = endScreenPos;
        SetVerticesDirty(); // Принудительно перерисовываем UI-меш
    }

    // Генерируем геометрию треугольника
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (_startPoint == Vector2.zero && _endPoint == Vector2.zero) return;

        // 1. Находим вектор направления от панели к персонажу
        Vector2 direction = (_endPoint - _startPoint).normalized;
    
        // 2. Находим ПРАВИЛЬНЫЙ перпендикуляр, который всегда смотрит вбок относительно экрана
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (baseWidth / 2f);

        // 3. Вычисляем экранные координаты
        Vector2 screenVertexA = _startPoint - perpendicular; // Левое основание
        Vector2 screenVertexB = _startPoint + perpendicular; // Правое основание
        Vector2 screenVertexC = _endPoint;                  // Вершина у персонажа

        // 4. Конвертируем в локальные координаты UI
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenVertexA, canvas.worldCamera, out Vector2 localA);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenVertexB, canvas.worldCamera, out Vector2 localB);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenVertexC, canvas.worldCamera, out Vector2 localC);

        // 5. Записываем вершины
        UIVertex v = UIVertex.simpleVert;
        v.color = color;

        v.position = localA; vh.AddVert(v); // Индекс 0
        v.position = localB; vh.AddVert(v); // Индекс 1
        v.position = localC; vh.AddVert(v); // Индекс 2

        // 6. ВСЕГДА рендерим в правильном порядке обхода (две стороны для защиты от инверсии)
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 1, 0); // Дублируем триугольник "спиной", чтобы он гарантированно не исчезал при разворотах
    }
}