using UnityEngine;

// シーン起動時の初期セットアップを行うクラスです。
// RuntimeInitializeOnLoadMethod でシーンに配置しなくても自動起動します。
public class GameBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoBoot()
    {
        // シーンに手動配置されていなければ自動生成する
        if (FindObjectOfType<GameBootstrap>() == null)
        {
            new GameObject("GameBootstrap").AddComponent<GameBootstrap>();
        }
    }

    private void Awake()
    {
        // カメラ・マネージャー・ゲーム本体の順に準備します。
        SetupCamera();

        TurnManager turnManager = FindObjectOfType<TurnManager>();
        if (turnManager == null)
        {
            turnManager = new GameObject("TurnManager").AddComponent<TurnManager>();
        }

        GridGameController game = FindObjectOfType<GridGameController>();
        if (game == null)
        {
            game = new GameObject("GridGameController").AddComponent<GridGameController>();
        }

        // Resources/Data/ から各ユニットのステータス定義を読み込みます。
        UnitStats playerStats = Resources.Load<UnitStats>("Data/PlayerStats");
        UnitStats enemyStats = Resources.Load<UnitStats>("Data/EnemyStats");

        game.Initialize(playerStats, enemyStats);
        turnManager.Register(game.Player, game.Enemy);

        // HUD を生成してプレイヤー情報を渡します。
        HudManager hud = new GameObject("HudManager").AddComponent<HudManager>();
        hud.Initialize(game.Player);
    }

    private void SetupCamera()
    {
        // Main Camera がなければ自動生成します。
        Camera cam = Camera.main;
        if (cam == null)
        {
            cam = new GameObject("Main Camera").AddComponent<Camera>();
            cam.tag = "MainCamera";
        }

        // 2D向け設定（正投影）にします。
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.transform.position = new Vector3(5.5f, 4.5f, -10f);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}

// テスト用の 1x1 白スプライトを使い回すための補助クラスです。
public static class SpriteFactory
{
    private static Sprite sharedSquare;

    public static Sprite SharedSquare
    {
        get
        {
            if (sharedSquare == null)
            {
                // 実アセットがまだ無い段階でも、見た目確認できるように動的生成します。
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                sharedSquare = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            }

            return sharedSquare;
        }
    }
}
