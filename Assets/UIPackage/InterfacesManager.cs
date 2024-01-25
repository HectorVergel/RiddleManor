using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InterfacesManager : MonoBehaviour
{
    [Serializable]
    public struct _interface
    {
        public string actionName;
        public Menu interfaceObject;
    }
    public _interface[] _interfaces;
    public static InterfacesManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start() {
        Cursor.visible = false;
    }
    private void OnEnable() {
        foreach (_interface _interface in _interfaces)
        {
            InputManager.GetAction(_interface.actionName).action += ((GameObject) => Open(_interface.interfaceObject));
        }
    }
    private void OnDisable() {
        foreach (_interface _interface in _interfaces)
        {
            InputManager.GetAction(_interface.actionName).action -= ((GameObject) => Open(_interface.interfaceObject));
        }
    }
    void Open(Menu interfaceToOpen)
    {
        interfaceToOpen.OpenMenu();
    }


    _interface GetInterface(string actionName)
    {
        foreach (_interface _interface in _interfaces)
        {
            if (actionName == _interface.actionName) return _interface;
        }
        return new _interface();
    }
}
