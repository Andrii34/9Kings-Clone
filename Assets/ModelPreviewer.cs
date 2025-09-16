using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ModelPreviewer : MonoBehaviour
{
    [Header("��������� ������")]
    [SerializeField] private Camera previewCamera;
    [SerializeField] private RawImage targetUI;
    [SerializeField] private int textureSize = 256;
    [SerializeField] private Color backgroundColor = new Color(0, 0, 0, 0);

    [Header("����������")]
    [SerializeField] private string savePath = "Assets/ModelPreviews";

    [Header("������ ��� ������")]
    [Required]
    [SerializeField] private GameObject modelToCapture;

    private RenderTexture renderTexture;

    [Button("Capture & Save")]
    private void CaptureAndSave()
    {
        if (modelToCapture == null)
        {
            Debug.LogWarning("������ �� ���������!");
            return;
        }

        string fileName = modelToCapture.name;
        Sprite sprite = CaptureModel(modelToCapture, fileName);

        if (sprite != null)
            Debug.Log($"�������� ������ {fileName} ������ � �������!");
    }

    public Sprite CaptureModel(GameObject model, string fileName = null)
    {
        if (previewCamera == null || model == null)
        {
            Debug.LogWarning("ModelPreviewer: ������ ��� ������ �� ���������.");
            return null;
        }

        if (renderTexture == null || renderTexture.width != textureSize || renderTexture.height != textureSize)
        {
            if (renderTexture != null) renderTexture.Release();
            renderTexture = new RenderTexture(textureSize, textureSize, 16);
        }

        previewCamera.targetTexture = renderTexture;
        previewCamera.clearFlags = CameraClearFlags.SolidColor;
        previewCamera.backgroundColor = backgroundColor;

        Vector3 originalPosition = model.transform.position;
        Quaternion originalRotation = model.transform.rotation;

        // ���������� ������
        model.transform.position = Vector3.zero;
        model.transform.rotation = Quaternion.identity;

        // ������
        previewCamera.Render();

        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        // ��������������� ������
        model.transform.position = originalPosition;
        model.transform.rotation = originalRotation;

        previewCamera.targetTexture = null;

        // ���������� � UI
        if (targetUI != null)
            targetUI.texture = tex;

        // ���������
        if (!string.IsNullOrEmpty(fileName))
            SaveTextureToFile(tex, fileName);

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    private void SaveTextureToFile(Texture2D tex, string fileName)
    {
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        string fullPath = Path.Combine(savePath, fileName + ".png");
        File.WriteAllBytes(fullPath, tex.EncodeToPNG());

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
        Debug.Log($"Saved model preview to {fullPath}");
    }

    private void OnDisable()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            renderTexture = null;
        }
    }
}

