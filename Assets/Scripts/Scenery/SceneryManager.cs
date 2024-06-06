using System;
using System.Collections;
using DataSources;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenery
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private DataSource<SceneryManager> sceneryManagerDataSource;
        [SerializeField] private Level defaultLevel;
        private Level _currentLevel;
        public event Action onLoading = delegate { };
        /// <summary>
        /// The float given is always between 0 and 1
        /// </summary>
        public event Action<float> onLoadPercentage = delegate { };
        public event Action onLoaded = delegate { };

        private void OnEnable()
        {
            if(sceneryManagerDataSource != null )
                sceneryManagerDataSource.Value = this;
        }

        private void Start()
        {
            //Load default level
            StartCoroutine(LoadFirstLevel(defaultLevel));
        }

        private void OnDisable()
        {
            if (sceneryManagerDataSource != null && sceneryManagerDataSource.Value == this)
            {
                sceneryManagerDataSource.Value = null;
            }
        }

        public void ChangeLevel(Level level)
        {
            StartCoroutine(ChangeLevel(_currentLevel, level));
        }

        private IEnumerator ChangeLevel(Level currentLevel, Level newLevel)
        {
            onLoading();
            onLoadPercentage(0);
            var unloadCount = currentLevel.SceneNames.Count;
            var loadCount = newLevel.SceneNames.Count;
            var total = unloadCount + loadCount;
            yield return new WaitForSeconds(2);
            yield return Unload(currentLevel,
                currentIndex => onLoadPercentage((float)currentIndex / total));
            yield return new WaitForSeconds(2);
            yield return Load(newLevel,
                currentIndex => onLoadPercentage((float)(currentIndex + unloadCount) / total));
            yield return new WaitForSeconds(2);
            
            _currentLevel = newLevel;
            onLoaded();
        }

        private IEnumerator LoadFirstLevel(Level level)
        {
            //This is a cheating value, do not use in production!
            var addedWeight = 5;
            
            onLoading();
            onLoadPercentage(0);
            var total = level.SceneNames.Count + addedWeight;
            var current = 0;
            yield return Load(level,
                currentIndex => onLoadPercentage((float)currentIndex / total));

            //This is cheating so the screen is shown over a lot of time :)
            for (; current <= total; current++)
            {
                yield return new WaitForSeconds(1);
                onLoadPercentage((float)current / total);
            }
            _currentLevel = level;
            onLoaded();
        }

        private IEnumerator Load(Level level, Action<int> onLoadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onLoadedSceneQtyChanged(current);
            }
        }

        private IEnumerator Unload(Level level, Action<int> onUnloadedSceneQtyChanged)
        {
            var current = 0;
            foreach (var sceneName in level.SceneNames)
            {
                var loadOp = SceneManager.UnloadSceneAsync(sceneName);
                yield return new WaitUntil(() => loadOp.isDone);
                current++;
                onUnloadedSceneQtyChanged(current);
            }
        }
    }
}
