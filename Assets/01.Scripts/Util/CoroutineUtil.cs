using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class CoroutineUtil : MonoBehaviour
    {
        private static GameObject _coroutineObj;
        private static CoroutineExecutor _coroutineExecutor;

        static CoroutineUtil()
        {
            _coroutineObj = new GameObject("CoroutineObj");
            _coroutineExecutor = _coroutineObj.AddComponent<CoroutineExecutor>();
        }

        public static void CallWaitForOneFrame(Action action)
        {
            _coroutineExecutor.StartCoroutine(DoCallWaitForOneFrame(action));
        }

        public static void CallWaitForSeconds(float seconds, Action afterAction)
        {
            _coroutineExecutor.StartCoroutine(DoCallWaitForSeconds(seconds, afterAction));
        }

        private static IEnumerator DoCallWaitForOneFrame(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        private static IEnumerator DoCallWaitForSeconds(float seconds, Action afterAction)
        {
            yield return new WaitForSeconds(seconds);
            afterAction?.Invoke();
        }

        public static void Callback<T>(Action<T> action)
        {
            CallWaitForOneFrame(() => action(default));
        }

        private class CoroutineExecutor : MonoBehaviour { }
    }
}