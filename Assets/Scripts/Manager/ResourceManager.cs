using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] Image mIMG;
    AsyncOperationHandle Handle;

    public void _ClickLoad()
    {
        Addressables.LoadAssetAsync<Sprite>("Coin_02_Gold").Completed +=
            (AsyncOperationHandle<Sprite> Obj) =>
            {
                Handle = Obj;
                mIMG.sprite = Obj.Result;
            };
    }

    public void _ClickUnLoad()
    {
        Addressables.Release(Handle);
        mIMG.sprite = null;
    }
}
