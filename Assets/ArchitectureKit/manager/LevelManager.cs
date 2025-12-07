using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Namespace_Level_Event;

namespace Namespace_Level
{
    internal sealed class LevelManager : ILevelManager
    {
        private IEventBus _bus;
        private bool _isLoading;
        private readonly HashSet<string> _contentScenes = new();
        private string _persistentSceneName = "";

        public LevelManager(IEventBus temp_bus)
        {
            _bus = temp_bus;
            _bus.ISubscribe<LevelRequest>(OnSwitch);
            _bus.ISubscribe<LevelLoad>(OnLoadLegacy);
            IndexCurrentContentScenes();
        }

        private void OnLoadLegacy(LevelLoad e) => OnSwitch(new LevelRequest(e.level));

        private async void OnSwitch(LevelRequest e)
        {
            if (_isLoading) return;
            _isLoading = true;

            var loadOp = SceneManager.LoadSceneAsync(e.level, LoadSceneMode.Additive);
            while (!loadOp.isDone) await Task.Yield();

            var newScene = SceneManager.GetSceneByName(e.level);
            if (newScene.IsValid()) SceneManager.SetActiveScene(newScene);

            var toUnload = FindUnloadTargets(except: e.level);
            foreach (var s in toUnload)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(s);
                while (unloadOp != null && !unloadOp.isDone) await Task.Yield();
                _contentScenes.Remove(s);
            }

            _contentScenes.Add(e.level);

            _bus.IPublish(new LevelLoad(e.level));
            _isLoading = false;

            GameObject[] root = newScene.GetRootGameObjects();
            _bus.IPublish(new TriggerRegister(root));
        }

        private IEnumerable<string> FindUnloadTargets(string except)
        {
            var currentScenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                currentScenes.Add(SceneManager.GetSceneAt(i));
            }

            foreach (var sc in currentScenes)
            {
                if (!sc.isLoaded) continue;

                var name = sc.name;

                if (name == except) continue;
                if (!string.IsNullOrEmpty(_persistentSceneName) && name == _persistentSceneName) continue;
                if (string.IsNullOrEmpty(_persistentSceneName) && currentScenes.IndexOf(sc) == 0) continue;

                yield return name;
            }
        }

        private void IndexCurrentContentScenes()
        {
            _contentScenes.Clear();
            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                var sc = SceneManager.GetSceneAt(i);
                if (!sc.isLoaded) continue;
                var name = sc.name;
                if (!string.IsNullOrEmpty(_persistentSceneName) && name == _persistentSceneName) continue;
                if (string.IsNullOrEmpty(_persistentSceneName) && i == 0) continue;
                _contentScenes.Add(name);
            }
        }
    }
}