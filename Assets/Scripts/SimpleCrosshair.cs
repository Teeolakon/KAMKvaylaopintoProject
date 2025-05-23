using UnityEngine;

public class SimpleCrosshair : MonoBehaviour
{
    public Color crosshairColor = Color.white;
    public int crosshairSize = 20;
    public int crosshairThickness = 2;

    void Start()
    {
        // Hide mouse cursor
        Cursor.visible = false;
    }

    void OnGUI()
    {
        // Calculate center of screen
        float centerX = Screen.width / 2f;
        float centerY = Screen.height / 2f;

        // Create crosshair texture if it doesn't exist
        Texture2D crosshairTexture = new Texture2D(1, 1);
        crosshairTexture.SetPixel(0, 0, crosshairColor);
        crosshairTexture.Apply();

        // Draw horizontal line
        GUI.DrawTexture(
            new Rect(centerX - crosshairSize / 2, centerY - crosshairThickness / 2, crosshairSize, crosshairThickness),
            crosshairTexture
        );

        // Draw vertical line
        GUI.DrawTexture(
            new Rect(centerX - crosshairThickness / 2, centerY - crosshairSize / 2, crosshairThickness, crosshairSize),
            crosshairTexture
        );
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false;
        }
    }
}