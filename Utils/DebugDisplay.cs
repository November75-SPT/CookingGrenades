using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookingGrenades.Utils
{
    // made by Grok AI
    public class DebugDisplay : MonoBehaviour
    {
        private static DebugDisplay _instance;

        public static DebugDisplay Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("DebugDisplay");
                    _instance = go.AddComponent<DebugDisplay>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        public bool Enable;
        private Rect _windowRect = new Rect(20, 20, 300, 50); // Initial height smaller, will adjust dynamically
        private Vector2 _scrollPosition = Vector2.zero;
        private List<DisplayItem> _displayItems = new List<DisplayItem>();

        private class DisplayItem
        {
            public string Label;
            public Func<object> ValueProvider;

            public DisplayItem(string label, Func<object> valueProvider)
            {
                Label = label;
                ValueProvider = valueProvider;
            }

            public object GetValue()
            {
                try
                {
                    return ValueProvider();
                }
                catch (System.Exception)
                {
                    return "System.Exception";
                }
            }
        }

        public void InsertDisplayObject(string label, Func<object> valueProvider, bool checkDuplicate = true)
        {
            if (!checkDuplicate)
            {
                _displayItems.Add(new DisplayItem(label, valueProvider));
                return;
            }
            if (_displayItems.Find(x => x.Label == label) == null)
            {
                _displayItems.Add(new DisplayItem(label, valueProvider));
            }
        }

        public void RemoveDisplayObject(string label)
        {
            _displayItems.RemoveAll(item => item.Label == label);
        }

        public void ClearDisplayObjects()
        {
            _displayItems.Clear();
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnGUI()
        {
            if (!Config.ConfigManager.DebugGUI.Value)
            {
                return;
            }
            _windowRect = GUI.Window(0, _windowRect, DrawWindow, "Debug Window");
        }

        private void DrawWindow(int windowId)
        {
            GUILayout.BeginVertical();

            // Calculate content height based on items
            float contentHeight = CalculateContentHeight();
            float windowHeight = Mathf.Clamp(contentHeight + 60f, 100f, 400f); // Min 100, Max 400, +60 for button and padding
            _windowRect.height = windowHeight;

            // ScrollView only if content exceeds max height
            if (contentHeight > 340f) // 400 - 60 (button and padding)
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(280), GUILayout.Height(windowHeight - 60f));
            }

            if (_displayItems.Count == 0)
            {
                GUILayout.Label("No objects to display.");
            }
            else
            {
                foreach (var item in _displayItems)
                {
                    object value = item.GetValue();
                    string displayText = FormatDisplayText(item.Label, value);
                    GUILayout.Label(displayText);
                }
            }

            if (contentHeight > 340f)
            {
                GUILayout.EndScrollView();
            }

            if (GUILayout.Button("Clear All"))
            {
                ClearDisplayObjects();
            }

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private float CalculateContentHeight()
        {
            float lineHeight = GUI.skin.label.CalcHeight(new GUIContent("A"), 280f); // Single line height
            int itemCount = _displayItems.Count > 0 ? _displayItems.Count : 1; // At least 1 for "No objects"
            return lineHeight * itemCount + 10f; // +10 for padding
        }

        private string FormatDisplayText(string label, object value)
        {
            if (value == null)
            {
                return $"{label}: Null";
            }
            return $"{label}: {value}";
        }
    }
}