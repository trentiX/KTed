using DG.Tweening;
using UnityEngine;

public class YoyoAnim : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.Rotate(0, 0, Random.Range(-0.9f, -0.9f));
        gameObject.transform.DORotate(new Vector3(0, 0, Random.Range(0.9f, 0.9f)), Random.Range(3,4))
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine); // Optional ease for smoother animation
    }
}
