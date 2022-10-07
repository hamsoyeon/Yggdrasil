using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    //프리펩 형식만 불러와진다 단순 이미지는 안 불러와짐.
    //사용할 프리펩들은 에디터에서 Addressable 체크 하세요
    //리소스 매니저 인스펙터창에서 사용할 프리펩들을 선택하세요.


    [SerializeField] AssetReference assetReference; //어드레스 저장
    [SerializeField] AssetReferenceGameObject assetRefObj;

    public void AssetLoad()
    {
        Debug.Log("생성 : " + assetReference.InstantiateAsync(new Vector3(0, 0, 0), Quaternion.identity));
        
    }

    public void AssetUnLoad(GameObject obj)
    {
        assetReference.ReleaseInstance(obj);
    }

    public void AssetUnLoadTest()
    {
        Debug.Log("함수 실행 됨, gameObject : " + gameObject);
        //AssetUnLoad(assetReference.);
    }
}
