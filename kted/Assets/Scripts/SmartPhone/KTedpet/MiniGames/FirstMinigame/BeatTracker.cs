using System.Collections.Generic;
using UnityEngine;

public class BeatTracker : AudioSyncer
{
    public List<float> beatTimestamps = new List<float>(); // Список для хранения времени битов
    private float songStartTime;
    private bool firstBeat = true;

    public override void OnBeat()
    {
        if (firstBeat)
        {
            songStartTime = Time.time; // Запоминаем время старта трека
            firstBeat = false;
        }
        base.OnBeat(); // Вызываем базовый метод, который логирует "beat"

        float songTime = Time.time - songStartTime; // Вычисляем время относительно начала трека
        beatTimestamps.Add(songTime); // Записываем время дропа бита

        Debug.Log($"Бит зафиксирован в {songTime:F2} секундах.");
    }
    
    public override void OnUpdate()
	{
		base.OnUpdate();
		if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintBeatSummary();
        }
    }
	
    public void PrintBeatSummary()
    {
        Debug.Log("Сводка по битам:");
        foreach (float time in beatTimestamps)
        {
            Debug.Log($"Бит в {time:F2} секундах");
        }
    }
}
