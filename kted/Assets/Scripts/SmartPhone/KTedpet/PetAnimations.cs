using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimations : MonoBehaviour, IDataPersistence
{
    // Serialization
    [SerializeField] private Animator petAnimator;

    // Singleton instance
    public static PetAnimations instance;

    // State variables
    private PetSprite currentSprite = PetSprite.idle;
    private List<PetAppearance> currentAppearance = new List<PetAppearance> { PetAppearance.normal };
    private string currentState;

    // Animation state control
    private bool isAnimating = false;

    // Animation constants
    private readonly Dictionary<(string, PetSprite), string> animationMap = new();

    private void Start()
    {
        instance = this;
        InitializeAnimationMap();
    }

    private void InitializeAnimationMap()
    {
        // Normal
        animationMap.Add(("normal", PetSprite.idle), "Pet_IDLE");
        animationMap.Add(("normal", PetSprite.lookingAround), "Pet_LookingAround");
        animationMap.Add(("normal", PetSprite.yawning), "Pet_Yawning");

        // Cool
        animationMap.Add(("cool,normal", PetSprite.idle), "Pet_Cool_IDLE");
        animationMap.Add(("cool,normal", PetSprite.lookingAround), "Pet_Cool_LookingAround");
        animationMap.Add(("cool,normal", PetSprite.yawning), "Pet_Cool_Yawning");

        // Hat
        animationMap.Add(("hat,normal", PetSprite.idle), "Pet_Hat_IDLE");
        animationMap.Add(("hat,normal", PetSprite.lookingAround), "Pet_Hat_LookingAround");
        animationMap.Add(("hat,normal", PetSprite.yawning), "Pet_Hat_Yawning");

        // Cool + Hat
        animationMap.Add(("cool,hat,normal", PetSprite.idle), "Pet_CoolHat_IDLE");
        animationMap.Add(("cool,hat,normal", PetSprite.lookingAround), "Pet_CoolHat_LookingAround");
        animationMap.Add(("cool,hat,normal", PetSprite.yawning), "Pet_CoolHat_Yawning");
    }

    private void StartAnimationCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationsCoroutine());
    }

    private IEnumerator AnimationsCoroutine()
    {
        while (true)
        {
            if (isAnimating) yield return null;

            yield return new WaitForSeconds(UnityEngine.Random.Range(7, 12));

            ChangePetSprite((PetSprite)UnityEngine.Random.Range(1, 3));
            isAnimating = true;

            yield return new WaitForSeconds(GetAnimationClipLength(currentState));

            ChangePetSprite(PetSprite.idle);
            isAnimating = false;
        }
    }

    public void ChangePetSprite(PetSprite petSprite)
    {
        if (currentSprite == petSprite) return;
        currentSprite = petSprite;
        UpdateAnimation();
    }

    public void ChangePetAppearance(PetAppearance appearance)
    {
        if (!currentAppearance.Contains(appearance))
            currentAppearance.Add(appearance);

        UpdateAnimation();
    }

    public void RemovePetAppearance(PetAppearance appearance)
    {
        if (currentAppearance.Contains(appearance))
            currentAppearance.Remove(appearance);

        if (currentAppearance.Count == 0)
            currentAppearance.Add(PetAppearance.normal);

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        string animationToPlay = GetAnimationNameForCurrentState();
        PlayAnimations(animationToPlay);
    }

    private string GetAnimationNameForCurrentState()
    {
        List<string> appearanceNames = currentAppearance.ConvertAll(a => a.ToString());
        appearanceNames.Sort();
        string key = string.Join(",", appearanceNames);

        if (animationMap.TryGetValue((key, currentSprite), out string animation))
        {
            return animation;
        }

        Debug.LogWarning($"Animation not found for: {key} + {currentSprite}");
        return "Pet_IDLE";
    }

    private void PlayAnimations(string newState)
    {
        if (currentState == newState || petAnimator == null) return;
        petAnimator.Play(newState);
        currentState = newState;
    }

    private float GetAnimationClipLength(string animationName)
    {
        foreach (AnimationClip clip in petAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName) return clip.length;
        }
        return 0f;
    }

    // Data persistence
    public void LoadData(GameData data)
    {
        Debug.Log($"Loading data... Data is null: {data == null}");
        Debug.Log($"Data appearance is null: {data?.petAppearance == null}");
        
        // Проверяем, есть ли элементы в списке
        if (data.petAppearance == null || data.petAppearance.Count == 0)
        {
            Debug.LogWarning("Appearance list is null or empty. Setting to default (normal).");
            currentAppearance = new List<PetAppearance> { PetAppearance.normal };
        }
        else
        {
            currentAppearance = new List<PetAppearance>(data.petAppearance);
        }

        Debug.Log($"Current appearance count after fix: {currentAppearance.Count}");
        
        ChangePetSprite(PetSprite.idle);
        UpdateAnimation();
        StartAnimationCoroutine();
    }

    public void SaveData(ref GameData data)
    {
        data.petAppearance = new List<PetAppearance>(currentAppearance);
    }
}

// Enum for pet sprite states
public enum PetSprite
{
    idle,
    yawning,
    lookingAround,
}

// Enum for pet appearance states
public enum PetAppearance
{
    normal,
    cool,
    hat
}
