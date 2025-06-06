using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class JsonHelp
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(WrapArray(json));
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T> { items = array };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    private static string WrapArray(string rawJson)
    {
        return "{\"items\":" + rawJson + "}";
    }
}