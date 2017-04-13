using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/// <summary>
/// Test plugin in c++ : http://www.alanzucconi.com/2015/10/11/how-to-write-native-plugins-for-unity/
/// </summary>
public class TestNativeCPlugin : MonoBehaviour {

    // The imported function
    [DllImport("NativePluginC", EntryPoint = "TestSort")]
    public static extern void TestSort(int[] a, int length);

    public int[] a;

    void Start()
    {
        Debug.Log("SORT WITH PLUGIN");
        TestSort(a, a.Length);
    }
}
