using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Menus
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private ButtonController buttonPrefab;
        [SerializeField] private List<string> ids = new();
        [SerializeField] private Transform buttonsParent;
        
        public event Action<string> OnChangeMenu;

        public void Setup()
        {
            foreach (var id in ids)
            {
                var newButton = Instantiate(buttonPrefab, buttonsParent);
                newButton.name = $"{id}_Btn";
                newButton.Setup(id, id, HandleButtonClick);
            }
        }

        private void HandleButtonClick(string id)
        {
            OnChangeMenu?.Invoke(id);
        }
    }
}
