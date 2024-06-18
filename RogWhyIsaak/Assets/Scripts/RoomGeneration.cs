using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.WSA;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RoomGeneration : MonoBehaviour
{
    public Tilemap floorTilemap;
    public TileBase[] wallTiles;
    public TileBase[] bordTiles;
    public TileBase[] backgroundTiles;

    private int roomWidth;
    private int roomHeight;
    [SerializeField] private int howHatsThisRoom;

    public Vector2Int howRandRoom;
    public Vector2Int howRandHatsSize;
    public Vector2Int howRandRoomSize;
    private Vector2Int SETile;
    private int eventRandom;
    void Awake()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        // Визначаємо кількість шляхів від 1 до 3
        howHatsThisRoom = Random.Range(howRandRoom.x, howRandRoom.y);

        // Очищаємо попередню кімнату
        floorTilemap.ClearAllTiles();
        // Знаходимо всі дочірні об'єкти з компонентом BoxCollider2D, які належать до floorTilemap
        BoxCollider2D[] colliders = floorTilemap.GetComponentsInChildren<BoxCollider2D>();

        // Видаляємо кожен BoxCollider2D
        foreach (var collider in colliders)
        {
            DestroyImmediate(collider.gameObject);
        }

        // Початкові координати для першого шляху
        int startX = 0;
        int startY = 0;

        int rotate = Random.Range(1, 5);
       
        for (int i = 0; i < howHatsThisRoom; i++)
        {
            int size = Random.Range(howRandHatsSize.x, howRandHatsSize.y);
        
            roomHeight= Random.Range(howRandRoomSize.x, howRandRoomSize.y);
            if (rotate <= 2)
            {
                if (i == 0)
                startX = (size * -1) / 2;
                DrawHats(startX, startY, size, true);
               DrawRectangle(startX, startY, size,true, roomHeight);
                rotate = 4;
                startX = Random.Range(SETile.x, SETile.y);
            }
            else
            {
                if (i == 0)
                    startY = (size * -1) / 2;
                DrawHats(startX, startY, size, false);
               DrawRectangle(startX, startY, size, false, roomHeight);
                rotate = 0;
                startY = Random.Range(SETile.x, SETile.y);
            }

        }
        DrawDetals();
        DrawBords();
        DrawBackgroundGround();
    }
    void DrawHats(int startX, int startY, int size, bool rotH)
    {
        for (int i = 0; i < size; i++)
        {
            int t;
            if (rotH)
                t = startX;
            else t = startY;
            if (i == 0)
                SETile.x = t + i;
            else if(i==size-1)
            {
                SETile.y = t + i;
            }
            Vector3Int tilePosition;
            if (rotH)
                tilePosition = new Vector3Int(startX + i, startY, 0);
            else
                tilePosition = new Vector3Int(startX, startY + i, 0);
            
            floorTilemap.SetTile(tilePosition, wallTiles[0]);
        }
        
    }

    void DrawRectangle(int startX, int startY, int leanght, bool rotH,int size)
    {
        for (int i = 0; i < leanght; i++)
        {
            for (int j = (size/2)*-1; j < (size / 2); j++)
            {
                Vector3Int tilePosition;
                if (rotH)
                    tilePosition = new Vector3Int(startX + i, startY+j, 0);
                else
                    tilePosition = new Vector3Int(startX+j, startY + i, 0);
                floorTilemap.SetTile(tilePosition, wallTiles[0]);
                
            }

            
        }
    }
    void DrawDetals()
    {
        BoundsInt bounds = floorTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Отримуємо тайл за певною позицією
                TileBase tile = floorTilemap.GetTile(tilePosition);

                // Перевіряємо, чи тайл існує та чи це не той тайл, який ми малюємо
                if (tile != null )
                {
                    eventRandom = Random.Range(0, 24);
                    if (eventRandom < 3)
                    {
                        floorTilemap.SetTile(tilePosition, wallTiles[Random.Range(1, wallTiles.Length-1)]);
                    }
                }
            }
        }

        
    }

    void DrawBords()
    {
        BoundsInt bounds = floorTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Отримуємо тайл за певною позицією
                TileBase tile = floorTilemap.GetTile(tilePosition);

                // Перевіряємо, чи тайл існує
                if (tile != null)
                {
                    // Перевіряємо наявність сусідів
                    TileBase leftTile = floorTilemap.GetTile(tilePosition + Vector3Int.left);
                    TileBase rightTile = floorTilemap.GetTile(tilePosition + Vector3Int.right);
                    TileBase topTile = floorTilemap.GetTile(tilePosition + Vector3Int.up);
                    TileBase bottomTile = floorTilemap.GetTile(tilePosition + Vector3Int.down);

                    TileBase ltTile = floorTilemap.GetTile(tilePosition + Vector3Int.left + Vector3Int.up);
                    TileBase rtTile = floorTilemap.GetTile(tilePosition + Vector3Int.right + Vector3Int.up);
                    TileBase lbTile = floorTilemap.GetTile(tilePosition + Vector3Int.left + Vector3Int.down);
                    TileBase rbTile = floorTilemap.GetTile(tilePosition + Vector3Int.right + Vector3Int.down);

                    if (ltTile == null)
                        AddCollider(tilePosition, 8);
                    else if (rtTile == null)
                        AddCollider(tilePosition, 9);
                    else if (lbTile == null)
                        AddCollider(tilePosition, 10);
                    else if (rbTile == null)
                        AddCollider(tilePosition, 11);

                    if (leftTile == null)
                        AddCollider(tilePosition, 0);
                    else if (topTile == null)
                        AddCollider(tilePosition, 1);
                    else if (rightTile == null)
                        AddCollider(tilePosition, 2);
                    else if (bottomTile == null)
                        AddCollider(tilePosition, 3);

                    if (leftTile == null && topTile == null)
                        floorTilemap.SetTile(tilePosition, bordTiles[4]);
                    else if (rightTile == null && topTile == null)
                        floorTilemap.SetTile(tilePosition, bordTiles[5]);
                    else if (leftTile == null && bottomTile == null)
                        floorTilemap.SetTile(tilePosition, bordTiles[6]);
                    else if (rightTile == null && bottomTile == null)
                        floorTilemap.SetTile(tilePosition, bordTiles[7]);

                    
                }
            }
        }
    }

    void DrawBackgroundGround()
    {
        BoundsInt bounds = floorTilemap.cellBounds;
        Vector3Int bottomLeft = new Vector3Int(bounds.xMin, bounds.yMin, 0);
        Vector3Int topRight = new Vector3Int(bounds.xMax - 1, bounds.yMax - 1, 0);
        Vector3Int topLeft = new Vector3Int(bounds.xMin, bounds.yMax - 1, 0);
        Vector3Int bottomRight = new Vector3Int(bounds.xMax - 1, bounds.yMin, 0);

        float diagonal1 = Vector3.Distance(bottomLeft, topRight);
        float diagonal2 = Vector3.Distance(bottomRight, topLeft);

        float maxDiagonal = Mathf.Max(diagonal1, diagonal2);
        Vector3 centerPoint = (diagonal1 > diagonal2) ? (bottomLeft + topRight) / 2 : (bottomRight + topLeft) / 2;

        float halfSideLength = (maxDiagonal / 2) + 20;
        Vector3Int centerInt = Vector3Int.RoundToInt(centerPoint);

        for (int x = centerInt.x - Mathf.CeilToInt(halfSideLength); x <= centerInt.x + Mathf.CeilToInt(halfSideLength); x++)
        {
            for (int y = centerInt.y - Mathf.CeilToInt(halfSideLength); y <= centerInt.y + Mathf.CeilToInt(halfSideLength); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (floorTilemap.GetTile(tilePosition) == null)
                {
                    eventRandom = Random.Range(0, 24);
                    if (eventRandom < 3)
                        floorTilemap.SetTile(tilePosition, backgroundTiles[Random.Range(1, backgroundTiles.Length-1)]);
                    else
                        floorTilemap.SetTile(tilePosition, backgroundTiles[0]);
                }
            }
        }
    }

    void AddCollider( Vector3Int tilePosition, int i)
    {
        floorTilemap.SetTile(tilePosition, bordTiles[i]);

        // Створюємо новий GameObject
        GameObject tileGameObject = new GameObject("TileCollider");

        // Додаємо до нього компонент BoxCollider2D
        BoxCollider2D collider = tileGameObject.AddComponent<BoxCollider2D>();

        // Отримуємо глобальні координати тайла на основі його локальних координат
        Vector3 tileWorldPosition = floorTilemap.CellToWorld(tilePosition);

        // Встановлюємо позицію колайдера таку саму, як і тайла
        tileGameObject.transform.position = tileWorldPosition;

        // Призначаємо тайл в якості дитячого об'єкта для можливості видалення колайдера разом з тайлом
        tileGameObject.transform.SetParent(floorTilemap.transform);

        // Налаштовуємо розміри колайдера відповідно до розмірів тайла
        collider.size = floorTilemap.cellSize;
        // Додаємо тег "Bord" до колайдера
        collider.tag = "Bord";
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Room")] // Цей метод буде викликаний з контекстного меню в інспекторі Unity
    void GenerateRoomFromInspector()
    {
        GenerateRoom();
    }
    [ContextMenu("Clear Room")] // Цей метод буде викликаний з контекстного меню в інспекторі Unity
    void ClearRoomFromInspector()
    {
        floorTilemap.ClearAllTiles();
        // Знаходимо всі дочірні об'єкти з компонентом BoxCollider2D, які належать до floorTilemap
        BoxCollider2D[] colliders = floorTilemap.GetComponentsInChildren<BoxCollider2D>();

        // Видаляємо кожен BoxCollider2D
        foreach (var collider in colliders)
        {
            DestroyImmediate(collider.gameObject);
        }
    }
#endif
}
