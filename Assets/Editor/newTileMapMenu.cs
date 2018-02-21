using System.Collections;

using UnityEngine;
using UnityEditor;

public class newTileMapMenu : MonoBehaviour {

    [MenuItem("GameObject/Tile Map")]
	public static void createTileMap()
    {
        GameObject tileMap = new GameObject("Tile Map");
        tileMap.AddComponent<tileMap>();
    }

}
