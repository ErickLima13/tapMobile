using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

public class ScreenLimits : MonoBehaviour
{
    [SerializeField] private Vector2 screenBounds;

    private Camera main;

    public GameObject _testArea;

    private void Awake()
    {
        main = Camera.main;

        // SEGREDO PARA WEBGL: Força o cálculo baseado na proporção fixa de pé (9:16)
        // em vez de ler a largura esticada do monitor do PC.
        float targetAspect = 9f / 16f;

        // Calcula a altura do mundo com base na câmera ortográfica
        float orthoHeight = main.orthographicSize;
        // Multiplica a altura pela proporção alvo para achar a largura real do mundo 2D
        float orthoWidth = orthoHeight * targetAspect;

        // Define os limites exatos do cenário vertical
        screenBounds = new Vector2(orthoWidth, orthoHeight);

        _ = Delay();
    }

    private async Task Delay()
    {
        await UniTask.WaitForEndOfFrame();

        // Mantém a sua lógica original de escala, mas agora com os limites travados em 9:16
        float x = screenBounds.x * 2 - 1f;
        _testArea.transform.localScale = new Vector3(x, screenBounds.y, 1f);
    }

    public Vector2 GetLimits()
    {
        return screenBounds;
    }
}
