using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private float maxRotation = 30f;
    [SerializeField] private float stabilizationSpeed = 5f;

    private Vector3 targetPosition;
    private Vector3 dragOffset;
    private Vector3 velocity = Vector3.zero;
    private bool isDragging = false;

    private float currentRotationZ = 0f;
    private Vector3 lastMousePosition;
    private RectTransform panelRect;
    private RectTransform canvasRect;

    private void Start()
    {
        panelRect = GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
        targetPosition = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition; 

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            eventData.position,
            canvas.worldCamera,
            out localPoint))
        {
            dragOffset = transform.position - canvas.transform.TransformPoint(localPoint);
        }
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out localPoint))
        {
            targetPosition = canvas.transform.TransformPoint(localPoint) + dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Update()
    {
        SmoothFollow();
        AdjustRotation();
    }

    private void SmoothFollow()
    {
        // Двигаем панель к позиции
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);
        ClampToBounds(); // Корректируем выход за границы
    }

    private void AdjustRotation()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        lastMousePosition = Input.mousePosition;

        if (isDragging)
        {
            float rotationChange = -mouseDelta.x * 0.2f;
            currentRotationZ += rotationChange;
            currentRotationZ = Mathf.Clamp(currentRotationZ, -maxRotation, maxRotation);
        }

        currentRotationZ = Mathf.Lerp(currentRotationZ, 0, Time.deltaTime * stabilizationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
    }

    private void ClampToBounds()
    {
        Vector3 minBounds = canvasRect.position - (Vector3)canvasRect.rect.size * 0.5f;
        Vector3 maxBounds = canvasRect.position + (Vector3)canvasRect.rect.size * 0.5f;

        Vector3 panelSize = panelRect.rect.size * panelRect.lossyScale; // Учитываем масштаб

        float leftEdge = transform.position.x - panelSize.x * 0.5f;
        float rightEdge = transform.position.x + panelSize.x * 0.5f;
        float bottomEdge = transform.position.y - panelSize.y * 0.5f;
        float topEdge = transform.position.y + panelSize.y * 0.5f;

        Vector3 correctedPosition = transform.position;

        if (leftEdge < minBounds.x)
            correctedPosition.x += minBounds.x - leftEdge;
        if (rightEdge > maxBounds.x)
            correctedPosition.x -= rightEdge - maxBounds.x;
        if (bottomEdge < minBounds.y)
            correctedPosition.y += minBounds.y - bottomEdge;
        if (topEdge > maxBounds.y)
            correctedPosition.y -= topEdge - maxBounds.y;

        transform.position = correctedPosition;
    }
}
