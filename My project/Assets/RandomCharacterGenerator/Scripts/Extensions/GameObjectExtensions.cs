using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class GameObjectExtensions
{
    /// <summary>
    /// Add a copy of the given component to the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T AddComponent <T>(this GameObject go, T component) where T : Component
    {
        return go.AddComponent<T>().CopyComponent(component) as T;
    }

    /// <summary>
    /// Add a copy of the given component to the object
    /// </summary>
    /// <param name="go"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Component AddComponent(this GameObject go, Component component)
    {
        return go.AddComponent(component.GetType()).CopyComponent(component);
    }

    /// <summary>
    /// Add a copy of the given component to the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T AddComponent<T>(this Component go, T component) where T : Component
    {
        return go.gameObject.AddComponent<T>().CopyComponent(component) as T;
    }

    /// <summary>
    /// Add a copy of the given component to the object
    /// </summary>
    /// <param name="go"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Component AddComponent(this Component go, Component component)
    {
        return go.gameObject.AddComponent(component.GetType()).CopyComponent(component);
    }

    /// <summary>
    /// Copy the values of a given component to another
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T CopyComponent<T>(this Component component, T source) where T : Component
    {
        System.Type type = component.GetType();

        if (type != component.GetType())
            return null;

        System.Reflection.BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
            BindingFlags.Default;

        var pInfos = from property in type.GetProperties(flags) where !property.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ObsoleteAttribute)) select property;

        foreach (var pInfo in pInfos)
        {
            if (pInfo.CanWrite && !pInfo.PropertyType.IsValueType)
            {
                try
                {
                    pInfo.SetValue(component, pInfo.GetValue(source, null), null);
                }

                catch
                {

                }
            }
        }

        FieldInfo[] fInfos = type.GetFields(flags);

        foreach (var fInfo in fInfos)
        {
            fInfo.SetValue(component, fInfo.GetValue(source));
        }

        return component as T;
    }

    /// <summary>
    /// Copy the values of a given component to another
    /// </summary>
    /// <param name="component"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Component CopyComponent (this Component component, Component source)
    {
        System.Type type = component.GetType();

        if (type != component.GetType())
            return null;

        System.Reflection.BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        var pInfos = from property in type.GetProperties(flags) where !property.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ObsoleteAttribute)) select property;

        foreach (var pInfo in pInfos)
        {
            if (pInfo.CanWrite)
            {
                try
                {
                    pInfo.SetValue(component, pInfo.GetValue(source, null), null);
                }

                catch
                {

                }
            }
        }

        FieldInfo[] fInfos = type.GetFields(flags);

        foreach (var fInfo in fInfos)
        {
            fInfo.SetValue(component, fInfo.GetValue(source));
        }

        return component;
    }




}
