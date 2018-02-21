using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class tileSelection : EditorWindow
{

    public enum Scale
    {
        x1,
        x2,
        x3,
        x4,
        x5
    }

    Scale scale;

    Vector2 currentSelection = Vector2.zero;

    public Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/Tile Selection")]
    public static void openTileSelectionWindow()
    { 

        var window = EditorWindow.GetWindow(typeof(tileSelection));
        var title = new GUIContent();
        title.text = "Tile selection";
        window.titleContent = title;
    }

    void OnGUI()
    {
        if (Selection.activeGameObject == null)
        {
            return;
        }

        var selection = Selection.activeGameObject.GetComponent<tileMap>();

        if(selection != null)
        {
            var texture2D = selection.texture2D;
            if(texture2D != null)
            {
                scale = (Scale)EditorGUILayout.EnumPopup("Zoom", scale);
                var newScale = ((int)scale) + 1;
                var newTextureSize = new Vector2(texture2D.width, texture2D.height) * newScale;
                var offset = new Vector2(10, 25);
                var viewport = new Rect(0, 0, position.width - 5, position.height - 5);
                var contentSize = new Rect(0, 0, newTextureSize.x + offset.x, newTextureSize.y + offset.y);
                scrollPos = GUI.BeginScrollView(viewport, scrollPos, contentSize);
                GUI.DrawTexture(new Rect(offset.x, offset.y, newTextureSize.x, newTextureSize.y), texture2D);

                var tile = selection.tileSize * newScale;
                var grid = new Vector2(newTextureSize.x / tile.x, newTextureSize.y / tile.y);

                var selectionPos = new Vector2(tile.x * currentSelection.x + offset.x, tile.y * currentSelection.y + offset.y);

                var boxTexture = new Texture2D(1, 1);
                boxTexture.SetPixel(0, 0, new Color(0, 0.5f, 1.0f, 0.4f));
                boxTexture.Apply();

                var style = new GUIStyle(GUI.skin.customStyles[0]);
                style.normal.background = boxTexture;

                GUI.Box(new Rect(selectionPos.x, selectionPos.y, tile.x, tile.y), "", style);

                var cEvent = Event.current;
                Vector2 mousePos = new Vector2(cEvent.mousePosition.x, cEvent.mousePosition.y);
                if(cEvent.type == EventType.mouseDown && cEvent.button == 0)
                {
                    currentSelection.x = Mathf.Floor((mousePos.x + scrollPos.x) / tile.x);
                    currentSelection.y = Mathf.Floor((mousePos.y + scrollPos.y) / tile.y);

                    if(currentSelection.x > grid.x - 1)
                    {
                        currentSelection.x = grid.x - 1;
                    }
                    if (currentSelection.y > grid.y - 1)
                    {
                        currentSelection.y = grid.y - 1;
                    }

                    selection.tileID = (int)(currentSelection.x + (currentSelection.y * grid.x) + 1);

                    Repaint();
                }


                GUI.EndScrollView();
            }
        }
        
    }



}
