using UnityEngine;
using UnityEngine.EventSystems;

public class PlayfulButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private bool isPointerOver = false;
    private Canvas canvas;

    [SerializeField] private float moveDistance = 30f; // Насколько далеко кнопка может убегать
    [SerializeField] private float returnSpeed = 5f;   // Скорость возврата к центру

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (isPointerOver)
        {
            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localMousePosition
            );

            // Вычисляем направление от курсора к кнопке (по X и Y)
            Vector3 direction = (rectTransform.localPosition - (Vector3)localMousePosition).normalized;

            // Смещаем кнопку в любом направлении
            Vector3 targetPosition = originalPosition + direction * moveDistance;

            rectTransform.localPosition = Vector3.Lerp(
                rectTransform.localPosition,
                targetPosition,
                Time.deltaTime * returnSpeed
            );
        }
        else
        {
            // Плавный возврат к начальному положению
            rectTransform.localPosition = Vector3.Lerp(
                rectTransform.localPosition,
                originalPosition,
                Time.deltaTime * returnSpeed
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }
}
