//Generate Map 
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //variables
    public int maxHeight = 20;
    public int maxWidth = 12;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    [SerializeField] private GameObject mapObject;
    [SerializeField] private SpriteRenderer mapRend;

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    void CreateMap()
    {
        //create object and add SpriteRenderer
        mapObject = new GameObject("Map");
        mapRend = mapObject.AddComponent<SpriteRenderer>();

        //Creating new texture with the given width and heigh
        Texture2D txt = new Texture2D(maxWidth, maxHeight);
        
        //Looping through X and Y and coloring the map
        for (int x = 0; x < maxWidth; x++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                #region Visual Coloring
                if (x % 2 != 0)
                {
                    if (y % 2 != 0)
                    {
                        txt.SetPixel(x, y, color1);
                    }
                    else
                    {
                        txt.SetPixel(x, y, color2);
                    }
                }
                else
                {
                    if (y % 2 != 0)
                    {
                        txt.SetPixel(x, y, color2);
                    }
                    else
                    {
                        txt.SetPixel(x, y, color1);
                    }
                }
                #endregion
            }
        }

        //use filtering to sharpen the pixels
        txt.filterMode = FilterMode.Point;
        txt.Apply();
        
        //Create the sprite (Map)
        Rect rect = new Rect(0, 0, maxWidth, maxHeight);
        Sprite sprite = Sprite.Create(txt, rect, Vector2.one * .5f, 2, 0, SpriteMeshType.FullRect);
        mapRend.sprite = sprite;
    }
}
