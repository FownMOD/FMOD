using MEC;
using RueI.Displays;
using RueI.Elements;
using RueI.Parsing.Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FMOD.API.SSHint
{
    public class FMODTextElement:Element
    {
        private string content;
        private float duration;
        private Coroutine displayCoroutine;
        private MonoBehaviour coroutineRunner;
        private RueI.Displays.Display display;
        /// <summary>
        /// 创建一个新的定时元素
        /// </summary>
        /// <param name="content">显示内容</param>
        /// <param name="position">屏幕Y坐标（0-1000）</param>
        /// <param name="duration">显示时长（秒）</param>
        /// <param name="display">要添加到的显示器</param>
        public FMODTextElement(string content, float position, float duration = 3f, RueI.Displays.Display display = null)
            : base(position)
        {
            this.content = content;
            this.duration = duration;
            this.display = display;

            // 初始不启用
            Enabled = false;

            // 创建协程运行器
            CreateCoroutineRunner();
        }

        /// <summary>
        /// 元素内容
        /// </summary>
        public string Content
        {
            get => content;
            set
            {
                content = value;
                // 触发更新
                if (display != null) display.Update();
            }
        }

        /// <summary>
        /// 显示时长（秒）
        /// </summary>
        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        /// <summary>
        /// 关联的显示器
        /// </summary>
        public RueI.Displays.Display Display
        {
            get => display;
            set => display = value;
        }

        private void CreateCoroutineRunner()
        {
            if (coroutineRunner == null)
            {
                GameObject runnerObj = new GameObject("TimedElement_CoroutineRunner");
                coroutineRunner = runnerObj.AddComponent<CoroutineRunner>();
                Object.DontDestroyOnLoad(runnerObj);
            }
        }

        /// <summary>
        /// 将元素添加到显示器
        /// </summary>
        /// <param name="targetDisplay">目标显示器</param>
        public void AddToDisplay(RueI.Displays.Display targetDisplay = null)
        {
            if (targetDisplay != null)
            {
                display = targetDisplay;
            }

            if (display != null && !display.Elements.Contains(this))
            {
                display.Elements.Add(this);
            }
        }

        /// <summary>
        /// 从显示器中移除元素
        /// </summary>
        public void RemoveFromDisplay()
        {
            if (display != null && display.Elements.Contains(this))
            {
                display.Elements.Remove(this);
                display.Update();
            }
        }

        /// <summary>
        /// 显示元素指定时间
        /// </summary>
        public void ShowForDuration()
        {
            if (displayCoroutine != null)
            {
                coroutineRunner.StopCoroutine(displayCoroutine);
            }

            Enabled = true;
            if (display != null) display.Update();

            displayCoroutine = coroutineRunner.StartCoroutine(HideAfterDelay());
        }

        /// <summary>
        /// 显示元素指定时间
        /// </summary>
        /// <param name="customDuration">自定义显示时长</param>
        public void ShowForDuration(float customDuration)
        {
            duration = customDuration;
            ShowForDuration();
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(duration);
            Enabled = false;
            if (display != null) display.Update();
            displayCoroutine = null;
        }

        /// <summary>
        /// 立即隐藏元素
        /// </summary>
        public void HideImmediately()
        {
            Enabled = false;
            if (display != null) display.Update();

            if (displayCoroutine != null)
            {
                coroutineRunner.StopCoroutine(displayCoroutine);
                displayCoroutine = null;
            }
        }

        /// <summary>
        /// 获取解析后的数据
        /// </summary>
        protected override ParsedData GetParsedData(DisplayCore core)
        {
            return Parser.Parse(content);
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            HideImmediately();
            RemoveFromDisplay();

            if (coroutineRunner != null && coroutineRunner.gameObject != null)
            {
                Object.Destroy(coroutineRunner.gameObject);
            }
        }

        /// <summary>
        /// 创建并立即显示定时元素
        /// </summary>
        public static FMODTextElement CreateAndShow(string content, float yPosition, float duration = 3f, RueI.Displays.Display display = null)
        {
            var element = new FMODTextElement(content, yPosition, duration, display);

            // 添加到显示器
            element.AddToDisplay(display);

            element.ShowForDuration();
            return element;
        }
    }
}
