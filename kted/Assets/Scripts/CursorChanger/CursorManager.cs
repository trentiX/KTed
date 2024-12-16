using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
	[Header("Cursor Textures")]
	[SerializeField] private Texture2D[] cursors; // Текстуры курсоров
	[SerializeField] private GameObject footStep; // Префаб следа
	[SerializeField] private GameObject player; // Главный персонаж (если есть)

	private bool isOverUi = false;
	private Vector3 lastMousePosition; // Последняя позиция мыши в мировых координатах
	private bool isFirstStep = true; // Флаг для первого шага
	private bool stepRight = true; // Для чередования левого и правого шага
	
	private void Start()
	{
		SetCursor(0);

		// Обновляем lastMousePosition при запуске
		lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMousePosition.z = 0f; // Убираем компонент Z для 2D
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

	private void Update()
	{
		if (!isOverUi && Input.GetMouseButtonDown(0)) // Обрабатываем клик мыши
		{
			if (CheckPlayerClick()) // Проверка клика по главному персонажу
			{
				ReactToPlayerClick();
			}
			else
			{
				SetCursor(1); // Курсор для клика
				SpawnClickFX(); // Создаём эффект клика
			}
		}
		else if (!isOverUi && Input.GetMouseButtonUp(0))
		{
			SetCursor(0); // Возвращаем обычный курсор
		}
	}

	private bool CheckPlayerClick()
	{
		// Преобразуем позицию мыши в мировые координаты
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0;

		// Выполняем Raycast на позиции мыши
		RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

		// Проверяем, попали ли в объект игрока
		if (hit.collider != null && hit.collider.gameObject == player)
		{
			return true;
		}

		return false;
	}

	private void ReactToPlayerClick()
	{
		Debug.Log("Player clicked!");

		// Пример реакции: изменяем цвет персонажа на короткое время
		SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
		if (playerSprite != null)
		{
			Color originalColor = playerSprite.color;
			playerSprite.color = Color.red;

			// Возврат к оригинальному цвету через 0.5 секунды
			DOTween.To(() => playerSprite.color, x => playerSprite.color = x, originalColor, 0.5f);
		}

		// Дополнительно можно добавить любую другую анимацию:
		player.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
		{
			player.transform.DOScale(1f, 0.2f);
		});
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
			angle -= 90f; // Корректируем угол для правильной ориентации

			// Создаем след
			GameObject newFootStep = Instantiate(footStep, mousePosition, Quaternion.identity);

			newFootStep.transform.rotation = Quaternion.Euler(0, 0, angle); // Правый шаг

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

	private void SetCursor(int cursorIndex)
	{
		Cursor.SetCursor(cursors[cursorIndex], Vector2.zero, CursorMode.Auto);
	}
}
