using System;
using System.Collections.Generic;
using DataSources;
using Game;
using Menus;
using UnityEngine;

namespace Navigation
{
    public class NavigationManager : MonoBehaviour
    {
        [Tooltip("First menu in the list is the default one :)")]
        [SerializeField] private List<MenuWithId> menusWithId;

        [SerializeField] private DataSource<GameManager> gameManagerDataSource;
        [SerializeField] private List<string> idsToTellGameManager = new();
        private int _currentMenuIndex = 0;

        private void Start()
        {
            foreach (var menu in menusWithId)
            {
                menu.Menu.Setup();
                menu.Menu.OnChangeMenu += HandleChangeMenu;
                menu.Menu.gameObject.SetActive(false);
            }

            if (menusWithId.Count > 0)
            {
                menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(true);
            }
        }

        private void HandleChangeMenu(string id)
        {
            if (idsToTellGameManager.Contains(id) && gameManagerDataSource != null && gameManagerDataSource.Value != null)
            {
                gameManagerDataSource.Value.HandleSpecialEvents(id);
            }
            for (var i = 0; i < menusWithId.Count; i++)
            {
                var menuWithId = menusWithId[i];
                if (menuWithId.ID == id)
                {
                    menusWithId[_currentMenuIndex].Menu.gameObject.SetActive(false);
                    menuWithId.Menu.gameObject.SetActive(true);
                    _currentMenuIndex = i;
                    break;
                }
            }
        }

        [Serializable]
        public struct MenuWithId
        {
            [field: SerializeField] public string ID { get; set; }
            [field: SerializeField] public Menu Menu { get; set; }
        }
    }
}
