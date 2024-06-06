using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    [RequireComponent(typeof(Button))]
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private string _id;
        private Button _button;
    
        public event Action<string> OnClick;

        private void Reset()
        {
            GameObject child;
            if (transform.childCount < 1)
            {
                child = new GameObject("Text (TMP)");
                child.transform.SetParent(transform);
            }
            else
                child = transform.GetChild(0).GameObject();
        
            if (!child.TryGetComponent<TMP_Text>(out text))
            {
                text = child.AddComponent<TextMeshProUGUI>();
            }
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            text ??= GetComponent<TMP_Text>();
            _button ??= GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }
    
        public void Setup(string label, string id, Action<string> onClick)
        {
            text.SetText(label);
            _id = id;
            OnClick = onClick;
        }

        private void HandleButtonClick()
        {
            OnClick?.Invoke(_id);
        }
    }
}
