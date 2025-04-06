using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookingGrenades.Utils;

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
    private Rect _windowRect = new Rect(20, 20, 300, 200);
    private Vector2 _scrollPosition = Vector2.zero;
    private List<DisplayItem> _displayItems = new List<DisplayItem>();

    // 디스플레이 항목 클래스: 값 또는 함수 참조를 저장
    private class DisplayItem
    {
        public string Label; // 표시 이름
        public Func<object> ValueProvider; // 동적 값 제공 함수

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
                return $"System.Exception";
            }
        }
    }

    // 동적 값 추가 (함수 참조)
    public void InsertDisplayObject(string label, Func<object> valueProvider, bool checkDuplicate = true)
    {
        if (!checkDuplicate)
        {
            _displayItems.Add(new DisplayItem(label, valueProvider));
            return;  
        }
        if( _displayItems.Find(x=>x.Label == label) == null)
        {
            _displayItems.Add(new DisplayItem(label, valueProvider));
        } 
    }

    // 항목 제거
    public void RemoveDisplayObject(string label)
    {
        _displayItems.RemoveAll(item => item.Label == label);
    }

    // 모든 항목 초기화
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
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(280), GUILayout.Height(160));

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

        GUILayout.EndScrollView();

        if (GUILayout.Button("Clear All"))
        {
            ClearDisplayObjects();
        }

        GUI.DragWindow();
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
