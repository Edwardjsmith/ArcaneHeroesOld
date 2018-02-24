using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(tileMap))]
public class tileMapEditor : Editor
{

    int spriteReferenceValue;

    public tileMap map;

    tileBrush brush;
    Vector3 mouseHitPos;

    bool mouseOnMap
    { 
        get
        {
            return mouseHitPos.x > 0 && mouseHitPos.x < map.gridSize.x && mouseHitPos.y < 0 && mouseHitPos.y > -map.gridSize.y;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        var oldSize = map.mapSize;
        map.mapSize = EditorGUILayout.Vector2Field("Map size", map.mapSize);

        if(map.mapSize != oldSize)
        {
            UpdateCalculations();
        }

        var oldTexture = map.texture2D;
        map.texture2D = (Texture2D)EditorGUILayout.ObjectField("Texture2D: ", map.texture2D, typeof(Texture2D), false);

        if(oldTexture != map.texture2D)
        {
            UpdateCalculations();
            map.tileID = 1;
            createBrush();
        }

        if(map.texture2D == null)
        {
            EditorGUILayout.HelpBox("You have not selected a texture yet..", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.LabelField("Tile size: ", map.tileSize.x + "x" + map.tileSize.y);
            EditorGUILayout.LabelField("Grid size in units: ", map.gridSize.x + "x" + map.gridSize.y);
            EditorGUILayout.LabelField("Pixels to units: ", map.pixelsToUnits.ToString());
            updateBrush(map.currentTileBrush);

            if(GUILayout.Button("Clear Tiles"))
            {
                if (EditorUtility.DisplayDialog("Clear map tiles?", "Are you sure?", "Clear", "Do not clear"))
                {
                    clearMap();
                }
            }
        }

        EditorGUILayout.EndVertical();
    }
    void OnEnable()
    {
        map = target as tileMap;
        Tools.current = Tool.View;

        if(map.tiles == null)
        {
            var tilesGO = new GameObject("Tiles");
            tilesGO.transform.SetParent(map.transform);
            tilesGO.transform.position = Vector3.zero;

            map.tiles = tilesGO;
        }

        if(map.texture2D != null)
        {
            UpdateCalculations();
            newBrush();
        }
    }

    private void OnDisable()
    {
        destroyBrush();
    }

    private void OnSceneGUI()
    {
        if(brush != null)
        {
            updateHitPos();
            moveBrush();
            if(map.texture2D != null && mouseOnMap)
            {
                Event current = Event.current;
                if(current.shift)
                {
                    drawNoCol();
                }
                else if(current.control)
                {
                    drawCol();
                }
                else if(current.alt)
                {
                    removeTile();
                }
            }
        }
    }
    private void UpdateCalculations()
    {
        var path = AssetDatabase.GetAssetPath(map.texture2D);
        map.spriteReferences = AssetDatabase.LoadAllAssetsAtPath(path);

        if (map.spriteReferences[1] != null)
         {
             spriteReferenceValue = 1;
         }
         else if(map.spriteReferences[1] == null)
         {
             spriteReferenceValue = 0;
         }

        //for (int i = 0; i < map.spriteReferences.Length; i++)
        {
            var sprite = (Sprite)map.spriteReferences[spriteReferenceValue];

            var width = sprite.textureRect.width;
            var height = sprite.textureRect.height;

            map.tileSize = new Vector2(width, height);
            map.pixelsToUnits = (int)(sprite.rect.width / sprite.bounds.size.x);
            map.gridSize = new Vector2((width / map.pixelsToUnits) * map.mapSize.x, (height / map.pixelsToUnits) * map.mapSize.y);
        }
    }

    void createBrush()
    {
        var sprite = map.currentTileBrush;
        if(sprite != null)
        {
            GameObject brushGO = new GameObject("Brush");
            brushGO.transform.SetParent(map.transform);
            
            brush = brushGO.AddComponent<tileBrush>();
            brush.renderer2D = brushGO.AddComponent<SpriteRenderer>();
            brush.renderer2D.sortingOrder = 1000;

            var pixelsToUnits = map.pixelsToUnits;
            brush.brushSize = new Vector2(sprite.textureRect.width / pixelsToUnits, sprite.textureRect.height / pixelsToUnits);

            brush.updateBrush(sprite);
        }
    }

    void newBrush()
    {
        if(brush == null)
        {
            createBrush();
        }
    }
    
    void destroyBrush()
    {
        if(brush != null)
        {
            DestroyImmediate(brush.gameObject);
        }
    }

    public void updateBrush(Sprite sprite)
    {
        if(brush != null)
        {
            brush.updateBrush(sprite);
        }
    }

    void updateHitPos()
    {
        var p = new Plane(map.transform.TransformDirection(Vector3.forward), Vector3.zero);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        var hit = Vector3.zero;
        var dist = 0.0f;

        if(p.Raycast(ray, out dist))
        {
            hit = ray.origin + ray.direction.normalized * dist;
        }

        mouseHitPos = map.transform.InverseTransformPoint(hit);
    }

    void moveBrush()
    {
        var tileSize = map.tileSize.x / map.pixelsToUnits;

        var x = Mathf.Floor(mouseHitPos.x / tileSize) * tileSize;
        var y = Mathf.Floor(mouseHitPos.y / tileSize) * tileSize;

        var row = x / tileSize;
        var column = Mathf.Abs(y / tileSize) - 1;

        if (!mouseOnMap)
        {
            return;
        }

       

        var id = (int)((column * map.mapSize.x) + row);

        brush.tileID = id;

        x += map.transform.position.x + tileSize / 2;
        y += map.transform.position.y + tileSize / 2;

        brush.transform.position = new Vector3(x, y, map.transform.position.z);
    }

    void drawCol()
    {
        var id = brush.tileID.ToString();

        var posX = brush.transform.position.x;
        var posY = brush.transform.position.y;

        GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

        if (tile == null)
        {
            tile = new GameObject("tile_" + id);
            tile.transform.SetParent(map.tiles.transform);
            tile.transform.position = new Vector3(posX, posY, 0);
            tile.AddComponent<SpriteRenderer>();
            tile.AddComponent<BoxCollider2D>();
            if (map.pixelsToUnits != 1)
            {
                tile.GetComponent<BoxCollider2D>().size = map.tileSize / map.pixelsToUnits;
            }
            else
            {
                tile.GetComponent<BoxCollider2D>().size = map.tileSize;
            }
        }

        tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite;
    }
    void drawNoCol()
    {
        var id = brush.tileID.ToString();

        var posX = brush.transform.position.x;
        var posY = brush.transform.position.y;

        GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

        if(tile == null)
        {
            tile = new GameObject("tile_" + id);
            tile.transform.SetParent(map.tiles.transform);
            tile.transform.position = new Vector3(posX, posY, 0);
            tile.AddComponent<SpriteRenderer>();
        }

        tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite;
    }

    void removeTile()
    {
        var id = brush.tileID.ToString();
        GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

        if(tile != null)
        {
            DestroyImmediate(tile);
        }
    }

    void clearMap()
    {
        for(var i = 0; i < map.tiles.transform.childCount; i++)
        {
            Transform t = map.tiles.transform.GetChild(i);
            DestroyImmediate(t.gameObject);
            i--;

        }
    }
}
