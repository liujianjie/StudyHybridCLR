using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{

    void Start()
    {
        // 先补充元数据
        LoadMetadataForAOTAssemblies();

        // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
        // Editor下无需加载，直接查找获得HotUpdate程序集
        Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        Type type = hotUpdateAss.GetType("Hello");
        MethodInfo method = type.GetMethod("Run");
        method.Invoke(null, null);
    }
    /// <summary>
    /// 补充AOT所需的元数据
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        List<string> aotDllList = new List<string>() {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
        };
        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = File.ReadAllBytes($"{Application.streamingAssetsPath}/{aotDllName}.bytes");
            var err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
            Debug.LogError($"LoadMetadataForAOTAssembly {aotDllName} load code: {err}");
        }
    }
}
