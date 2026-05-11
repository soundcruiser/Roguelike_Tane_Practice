using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP表示と戦闘ログを管理するHUDクラスです。
/// Canvas や UI 要素を動的に生成し、毎フレーム更新します。
/// </summary>
public class HudManager : MonoBehaviour
{
    // 戦闘ログに表示する最大行数
    private const int MaxLogLines = 5;

    private PlayerController player;
    private Text hpText;
    private Text logText;
    private readonly List<string> logLines = new List<string>();

    public void Initialize(PlayerController playerRef)
    {
        player = playerRef;
        BuildCanvas();
        UnitBase.OnCombatLog += AddLogLine;
    }

    private void OnDestroy()
    {
        UnitBase.OnCombatLog -= AddLogLine;
    }

    private void Update()
    {
        if (player == null || hpText == null)
        {
            return;
        }

        hpText.text = $"HP  {player.CurrentHp} / {player.MaxHp}";
    }

    private void AddLogLine(string message)
    {
        logLines.Add(message);

        // 古いログを捨てて最新だけ残す
        while (logLines.Count > MaxLogLines)
        {
            logLines.RemoveAt(0);
        }

        if (logText != null)
        {
            logText.text = string.Join("\n", logLines);
        }
    }

    // ----- UI構築 -----

    private void BuildCanvas()
    {
        // uGUI に必要な EventSystem がなければ生成する
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Screen Space Overlay の Canvas を作成
        GameObject canvasObj = new GameObject("HudCanvas");
        canvasObj.transform.SetParent(transform, false);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        hpText = CreateHpDisplay(canvasRect);
        logText = CreateLogDisplay(canvasRect);
    }

    /// <summary>
    /// 画面左上にHP表示を作成します。
    /// </summary>
    private Text CreateHpDisplay(RectTransform parent)
    {
        GameObject obj = new GameObject("HpText");
        obj.transform.SetParent(parent, false);

        Text text = obj.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        text.fontSize = 24;
        text.color = Color.white;
        text.alignment = TextAnchor.UpperLeft;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;

        // 左上アンカー
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(16f, -12f);
        rt.sizeDelta = new Vector2(300f, 40f);

        // 半透明の背景パネルで視認性を確保
        GameObject bg = new GameObject("HpBg");
        bg.transform.SetParent(parent, false);
        bg.transform.SetAsFirstSibling();
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.5f);

        RectTransform bgRt = bg.GetComponent<RectTransform>();
        bgRt.anchorMin = new Vector2(0f, 1f);
        bgRt.anchorMax = new Vector2(0f, 1f);
        bgRt.pivot = new Vector2(0f, 1f);
        bgRt.anchoredPosition = new Vector2(8f, -6f);
        bgRt.sizeDelta = new Vector2(200f, 44f);

        return text;
    }

    /// <summary>
    /// 画面下部に戦闘ログ表示を作成します。
    /// </summary>
    private Text CreateLogDisplay(RectTransform parent)
    {
        // 背景パネル
        GameObject bg = new GameObject("LogBg");
        bg.transform.SetParent(parent, false);
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.5f);

        RectTransform bgRt = bg.GetComponent<RectTransform>();
        bgRt.anchorMin = new Vector2(0f, 0f);
        bgRt.anchorMax = new Vector2(1f, 0f);
        bgRt.pivot = new Vector2(0.5f, 0f);
        bgRt.anchoredPosition = new Vector2(0f, 0f);
        bgRt.sizeDelta = new Vector2(0f, 110f);

        // テキスト
        GameObject obj = new GameObject("LogText");
        obj.transform.SetParent(parent, false);

        Text text = obj.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 18);
        text.fontSize = 18;
        text.color = new Color(1f, 1f, 0.8f, 1f);
        text.alignment = TextAnchor.LowerLeft;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(1f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(0f, 8f);
        rt.sizeDelta = new Vector2(-24f, 100f);

        return text;
    }
}
