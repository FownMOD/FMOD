using RueI.Displays;
using RueI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Display = RueI.Displays.Display;

namespace FMOD.API.SSHint
{
    public class DisplayManager
    {
        private static Dictionary<ReferenceHub, Display> playerDisplays = new Dictionary<ReferenceHub, Display>();
        private static List<FMODTextElement> activeElements = new List<FMODTextElement>();
        /// <summary>
        /// 为指定玩家显示定时文本
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="yPosition">Y坐标（0-1000）</param>
        /// <param name="duration">持续时间（秒）</param>
        /// <param name="player">目标玩家（null表示本地玩家）</param>
        public static FMODTextElement ShowForPlayer(string content, float yPosition, float duration = 3f, ReferenceHub player = null)
        {
            Display display = GetOrCreateDisplay(player);
            var element = FMODTextElement.CreateAndShow(content, yPosition, duration, display);

            activeElements.Add(element);
            SetupAutoCleanup(element, duration);

            return element;
        }

        /// <summary>
        /// 为所有玩家显示定时文本
        /// </summary>
        public static void ShowForAll(string content, float yPosition, float duration = 3f)
        {
            foreach (ReferenceHub player in ReferenceHub.AllHubs)
            {
                ShowForPlayer(content, yPosition, duration, player);
            }
        }

        /// <summary>
        /// 清除所有活跃的定时元素
        /// </summary>
        public static void ClearAll()
        {
            foreach (var element in activeElements)
            {
                element.HideImmediately();
                element.Dispose();
            }
            activeElements.Clear();
        }

        /// <summary>
        /// 清除指定玩家的所有定时元素
        /// </summary>
        public static void ClearForPlayer(ReferenceHub player)
        {
            if (playerDisplays.TryGetValue(player, out var display))
            {
                // 移除该显示器的所有定时元素
                for (int i = display.Elements.Count - 1; i >= 0; i--)
                {
                    if (display.Elements[i] is FMODTextElement timedElement)
                    {
                        timedElement.HideImmediately();
                        timedElement.Dispose();
                        activeElements.Remove(timedElement);
                    }
                }
            }
        }
        private static void SetupAutoCleanup(FMODTextElement element, float duration)
        {
            CoroutineRunner.StartCoroutine(CleanupCoroutine(element, duration));
        }
        private static Display GetOrCreateDisplay(ReferenceHub player)
        {
            if (!playerDisplays.TryGetValue(player, out var display))
            {
                display = new Display(player);
                playerDisplays[player] = display;
            }
            return display;
        }

        private static System.Collections.IEnumerator CleanupCoroutine(FMODTextElement element, float duration)
        {
            yield return new WaitForSeconds(duration + 0.1f);
            activeElements.Remove(element);
            element.Dispose();
        }
    }
    internal class CoroutineRunner:MonoBehaviour
    {
        private static MonoBehaviour runner;

        public static new UnityEngine.Coroutine StartCoroutine(System.Collections.IEnumerator routine)
        {
            if (runner == null)
            {
                GameObject obj = new GameObject("TimedDisplayManager_CoroutineRunner");
                runner = obj.AddComponent<CoroutineRunnerBehaviour>();
                UnityEngine.Object.DontDestroyOnLoad(obj);
            }
            return runner.StartCoroutine(routine);
        }

        private class CoroutineRunnerBehaviour : MonoBehaviour { }
    }
}
