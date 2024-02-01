using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class Hello
{
    public static void Run()
    {
        Debug.Log("Hello, World liujianjie");

        InstantiateNewPrefab();
    }
    // 2. 从Ab包里加载Prefab，实例化，然后添加新的脚本
    public static void InstantiateNewPrefab()
    {
        // 从Ab包里加载Prefab
        var handle = Addressables.LoadAssetAsync<GameObject>("2UseMonoBehaviour");
        handle.Completed += (obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("2UseMonoBehaviour 资源加载成功1");
                // 实例化
                GameObject go = GameObject.Instantiate(obj.Result);
                // 添加新的脚本
                //go.AddComponent<HotMono>();
                Debug.Log("2UseMonoBehaviour 资源挂载hotmono成功2");
            }
            else
            {
                Debug.LogError("2UseMonoBehaviour 资源加载失败");
            }
        };
    }
}