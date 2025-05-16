using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsLine : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private GameObject newsTextPrefab;
    [SerializeField] private float scrollSpeed = 100.0f; // Скорость прокрутки (положительное значение)
    [SerializeField] private float spawnPositionX = 1000f; // За правой границей экрана
    [SerializeField] private float despawnPositionX = -1000f; // За левой границей экрана
    
    [Header("Настройки размещения")]
    [SerializeField] private float spawnInterval = 2.0f; // Интервал времени между появлением новых текстов (в секундах)
    
    [Header("Контент")]
    [SerializeField] private string[] newsTexts; // Массив новостных текстов
    
    private List<GameObject> newsTextObjects = new List<GameObject>();
    private int currentTextIndex = 0;
    private float timeSinceLastSpawn = 0f;
    
    private void Start()
    {
        // Создаем первый текстовый объект сразу
        SpawnText();
    }
    
    private void Update()
    {   
        // Увеличиваем счетчик времени
        timeSinceLastSpawn += Time.deltaTime;
        
        // Создаем новый текст через регулярные интервалы
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnText();
            timeSinceLastSpawn = 0f;
        }
        
        // Временный список для объектов, которые нужно удалить
        List<GameObject> textObjectsToRemove = new List<GameObject>();
        
        // Перемещаем все тексты и проверяем, какие вышли за пределы экрана
        foreach (GameObject newsTextObject in newsTextObjects)
        {
            // Двигаем текст влево (отрицательное направление по оси X)
            newsTextObject.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
                    
            // Проверяем, вышел ли текст за пределы экрана слева
            if (newsTextObject.transform.position.x < despawnPositionX)
            {
                textObjectsToRemove.Add(newsTextObject);
            }
        }
        
        // Удаляем тексты, вышедшие за границу
        foreach (GameObject textToRemove in textObjectsToRemove)
        {
            newsTextObjects.Remove(textToRemove);
            Destroy(textToRemove);
        }
    }
    
    private void SpawnText()
    {
        // Создаем текстовый объект справа от экрана
        GameObject textObject = Instantiate(
            newsTextPrefab, 
            new Vector3(spawnPositionX, newsTextPrefab.transform.position.y, newsTextPrefab.transform.position.z), 
            Quaternion.identity,
            transform); // Делаем дочерним объектом для лучшей организации иерархии
            
        // Устанавливаем текст из массива, если он есть
        if (newsTexts != null && newsTexts.Length > 0)
        {
            SetTextContent(textObject, newsTexts[currentTextIndex]);
            
            // Переходим к следующему тексту для следующего спавна
            currentTextIndex = (currentTextIndex + 1) % newsTexts.Length;
        }
        
        textObject.SetActive(true); // Активируем объект
        newsTextObjects.Add(textObject);
    }
    
    private void SetTextContent(GameObject textObject, string content)
    {
        // Пробуем найти компонент TextMeshPro
        TMPro.TextMeshProUGUI tmpComponent = textObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpComponent != null)
        {
            tmpComponent.text = content;
            return;
        }
        
        // Пробуем найти компонент TextMeshPro - world space версию
        TMPro.TextMeshPro tmpWorldComponent = textObject.GetComponentInChildren<TMPro.TextMeshPro>();
        if (tmpWorldComponent != null)
        {
            tmpWorldComponent.text = content;
            return;
        }
        
        // Пробуем найти стандартный компонент UI Text
        UnityEngine.UI.Text textComponent = textObject.GetComponentInChildren<UnityEngine.UI.Text>();
        if (textComponent != null)
        {
            textComponent.text = content;
            return;
        }
        
        // Если текстовый компонент не найден, выводим предупреждение
        Debug.LogWarning("Текстовый компонент не найден в префабе новостей");
    }
}