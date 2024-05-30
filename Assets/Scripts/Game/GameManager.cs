using System;
using DataSources;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string playId = "Play";
        [SerializeField] private string exitId = "Exit";
        [SerializeField] private DataSource<GameManager> gameManagerDataSource;

        private void OnEnable()
        {
            if(gameManagerDataSource != null )
                gameManagerDataSource.Value = this;
        }

        private void OnDisable()
        {
            if (gameManagerDataSource != null && gameManagerDataSource.Value == this)
            {
                gameManagerDataSource.Value = null;
            }
        }

        public void HandleSpecialEvents(string id)
        {
            if (id == playId)
            {
                Debug.Log($"Player selected to play the game :D");
            }
            else if (id == exitId)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }
    }
}
