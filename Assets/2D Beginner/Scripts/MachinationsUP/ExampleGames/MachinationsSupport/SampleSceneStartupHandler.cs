using MachinationsUP.Engines.Unity;
using UnityEngine;

namespace MachinationsUP.ExampleGames.MachinationsSupport
{
    /// <summary>
    /// Handles first-time initialization of a Scene.
    /// </summary>
    public class SampleSceneStartupHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            Debug.Log("Before first Scene loaded.");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoadRuntimeMethod()
        {
            Debug.Log("After first Scene loaded.");
            //Provide the MachinationsGameLayer with an IGameLifecycleProvider.
            //This will usually be the Game Engine.
            MachinationsGameLayer.Instance.GameLifecycleProvider = new SampleGameEngine();
        }

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {    
            Debug.Log("RuntimeMethodLoad: After first Scene loaded.");
        }
    }
}