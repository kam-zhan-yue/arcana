using System;
using UnityEngine;

public class TorchPair : MonoBehaviour
{
    [SerializeField] private float distanceBetween = 1.9f;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (transform.childCount >= 2)
        {
            Transform leftChild = transform.GetChild(0);
            leftChild.transform.localPosition = new Vector3(distanceBetween, 0f, 0f);
            
            Transform rightChild = transform.GetChild(1);
            rightChild.transform.localPosition = new Vector3(-distanceBetween, 0f, 0f);
        }
    }
#endif
}
