using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject assetReferenceGameObject;

    //아직 쓰지 마세요
    public void AddressablesPrefab()
    {
        Addressables.InstantiateAsync(assetReferenceGameObject);
    }
}
