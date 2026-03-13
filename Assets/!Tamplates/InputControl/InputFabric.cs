using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class InputFabric
{
    private static IInput input;

    public static IInput GetOrCreateInpit(bool dontDestroy = false, bool replace = false)
    {
        if (!replace && input != null)
            return input;

        GameObject goInput = new GameObject("IInput");

#if UNITY_EDITOR
        if (IsSimulatorView())
            input = goInput.AddComponent<TouchInput>();
        else
            input = goInput.AddComponent<MouseInput>();
#elif UNITY_ANDROID || UNITY_IOS
        input = goInput.AddComponent<TouchInput>();
#else
        input = goInput.AddComponent<MouseInput>();
#endif

        if (dontDestroy)
            GameObject.DontDestroyOnLoad(goInput);

        return input;
    }

    public static bool IsSimulatorView()
    {
#if UNITY_EDITOR
        // Ѕыстра€ проверка: если Input сообщает о поддержке тача и это не мобильна€ платформа
        if (Input.touchSupported && !Application.isMobilePlatform)
            return true;

        try
        {
            // 1) ЌадЄжный и простой способ: проверить все открытые EditorWindow
            //    и посмотреть, есть ли среди них окно симул€тора (по имени типа).
            var editorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var wnd in editorWindows)
            {
                var t = wnd.GetType();
                var fullName = t.FullName ?? t.Name;
                // »щем ключевые подстроки, которые встречаютс€ в именах типов пакета Device Simulator
                if (fullName.IndexOf("DeviceSimulator", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fullName.IndexOf("DeviceSimulation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fullName.IndexOf("SimulatorWindow", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    fullName.IndexOf("Simulator", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            // 2) ‘оллбек: если пр€мого экземпл€ра окна не найдено, пробуем обнаружить типы
            //    Device Simulator и проверить HasOpenInstances<T>() через отражение.
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                var candidateType = asm.GetType("UnityEditor.DeviceSimulation.DeviceSimulatorWindow")
                                    ?? asm.GetType("UnityEditor.DeviceSimulation.DeviceSimulator")
                                    ?? asm.GetType("UnityEditor.DeviceSimulation.SimulatorWindow")
                                    ?? asm.GetType("UnityEditor.DeviceSimulation.Simulator");

                if (candidateType == null)
                    continue;

                if (!typeof(UnityEngine.Object).IsAssignableFrom(candidateType))
                    continue;

                if (typeof(EditorWindow).IsAssignableFrom(candidateType))
                {
                    var hasOpenMethod = typeof(EditorWindow).GetMethod("HasOpenInstances", BindingFlags.Static | BindingFlags.Public);
                    if (hasOpenMethod != null && hasOpenMethod.IsGenericMethodDefinition)
                    {
                        try
                        {
                            var generic = hasOpenMethod.MakeGenericMethod(candidateType);
                            var result = generic.Invoke(null, null);
                            if (result is bool b && b)
                                return true;
                        }
                        catch
                        {
                            // продолжим провер€ть другие типы
                        }
                    }
                }

                // ещЄ одна попытка Ч найти объекты этого типа через Resources (если тип наследует UnityEngine.Object)
                try
                {
                    var found = Resources.FindObjectsOfTypeAll(candidateType);
                    if (found != null && found.Length > 0)
                        return true;
                }
                catch
                {
                    // пропустить тип, если FindObjectsOfTypeAll бросил
                }
            }
        }
        catch
        {
            // при любых ошибках считаем, что симул€тор не открыт
        }

        return false;
#else
        return false;
#endif
    }
}