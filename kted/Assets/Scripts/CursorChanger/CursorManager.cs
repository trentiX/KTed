using DG.Tweening;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor textures")]
    [SerializeField] private Texture2D[] cursors; // Текстуры курсоров
    [SerializeField] private GameObject footStep; // Префаб следа

    private bool isOverUi = false;
    private Vector3 lastMousePosition; // Последняя позиция мыши в мировых координатах
    private bool isFirstStep = true; // Флаг для первого шага
    private AudioManager audioManager;
    public static CursorManager CursorManagerInstance { get; private set; }

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (CursorManagerInstance != null && CursorManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            CursorManagerInstance = this;
        }
    }

    private void Start()
    {
        SetCursor(0);

        // Обновляем lastMousePosition при запуске
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastMousePosition.z = 0f;
    }

    public void ToggleUiCursor(bool toggle)
    {
        isOverUi = toggle;
        if (isOverUi)
        {
            SetCursor(2); // Курсор для UI
        }
        else
        {
            SetCursor(0); // Обычный курсор
        }
    }

    private void SetCursor(int cursorIndex)
    {
        Cursor.SetCursor(cursors[cursorIndex], Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (!isOverUi)
        {
            if (Input.GetMouseButtonDown(0)) // Обрабатываем клик мыши
            {
                SetCursor(1); // Курсор для клика
                SpawnClickFX(); // Создаем эффект клика
                audioManager.ClickSound(); // Проигрываем звук
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SetCursor(0); // Возвращаем обычный курсор
            }
        }
    }

    private void SpawnClickFX()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Преобразуем позицию мыши в мировые координаты
        mousePosition.z = 0f; // Убедимся, что Z-координата равна 0

        if (footStep != null)
        {
            if (isFirstStep)
            {
                // Если это первый шаг, не используем направление
                lastMousePosition = mousePosition;
                isFirstStep = false;
            }

            // Вычисляем направление движения
            Vector3 direction = (mousePosition - lastMousePosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Вычисляем угол в градусах

            // Создаем след
            GameObject newFootStep = Instantiate(footStep, mousePosition, Quaternion.identity);

            // Устанавливаем угол поворота
            newFootStep.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Анимация исчезновения и уничтожения следа
            newFootStep.GetComponent<SpriteRenderer>().DOFade(0, 2f);
            Destroy(newFootStep, 2f);

            // Обновляем последнюю позицию мыши
            lastMousePosition = mousePosition;
        }
        else
        {
            Debug.LogWarning("Footstep prefab is not assigned in the Inspector.");
        }
    }
}
