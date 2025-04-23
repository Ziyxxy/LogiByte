using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    public Color highlightColor = Color.green;
    public int gridX;
    public int gridY;
    
    public GameObject currentComponent; // 🧠 Track placed component

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Highlight()
    {
        sr.color = highlightColor;
    }

    public void ResetHighlight()
    {
        sr.color = originalColor;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
