using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void Awake()
    {
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

        game.Initialize();
        turnManager.Register(game.Player, game.Enemy);
    }

    private void SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            cam = new GameObject("Main Camera").AddComponent<Camera>();
            cam.tag = "MainCamera";
        }

        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.transform.position = new Vector3(5.5f, 4.5f, -10f);
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;
    }
}

public static class SpriteFactory
{
    private static Sprite sharedSquare;

    public static Sprite SharedSquare
    {
        get
        {
            if (sharedSquare == null)
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                sharedSquare = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            }

            return sharedSquare;
        }
    }
}
