using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Used for the tools menu of NavTile at the top of the screen.
    /// Generally used to create NavTiles.
    /// </summary>
    public class CreateNavTiles
    {
        /// <summary>
        /// Enables and disables the NavTiles from sprites button based on selection.
        /// </summary>
        /// <returns></returns>
        [MenuItem("Tools/NavTiles/Create NavTiles from Sprites", true)]
        static bool ValidateNavTilesFromSprites()
        {
            // Check if there is a sprite in the selection.
            foreach (Object obj in Selection.objects)
            {
                if (obj.GetType() == typeof(Sprite))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates NavTiles from selected sprites on selected save location.
        /// </summary>
        [MenuItem("Tools/NavTiles/Create NavTiles from Sprites")]
        static void CreateNavTilesFromSprites()
        {
            Object[] objects = Selection.objects;

            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < objects.Length; i++)
            {
                // Only use sprites, no subclasses.
                if (objects[i].GetType() == typeof(Sprite))
                {
                    sprites.Add((Sprite)objects[i]);
                }
            }

            if (sprites.Count == 0)
            {
                Debug.LogWarning("No sprites selected to create NavTiles for.");
                return;
            }

            string path = EditorUtility.OpenFolderPanel("Choose a save location for the NavTiles", Application.dataPath, "");
            path = path.Substring(path.IndexOf("Assets"));

            if (string.IsNullOrEmpty(path))
                return;

            for (int i = 0; i < sprites.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Creating NavTile(s)", $"Creating from {sprites[i].name}", (float)i / (sprites.Count - 1));
                NavTile tile = ScriptableObject.CreateInstance<NavTile>();
                tile.sprite = sprites[i];

                if (AssetDatabase.LoadAssetAtPath($"{path}/{sprites[i].name}.asset", typeof(NavTile)))
                {
                    // Create dialog to ask if User is sure they want to overwrite existing prefab.
                    if (EditorUtility.DisplayDialog("Are you sure?",
                            "A NavTile with the name '" + sprites[i].name + "' already exists. Do you want to overwrite it?",
                            "Yes",
                            "No"))
                    // If the user presses the yes button, create the Prefab.
                    {
                        AssetDatabase.CreateAsset(tile, $"{path}/{sprites[i].name}.asset");
                    }
                }
                else
                {
                    AssetDatabase.CreateAsset(tile, $"{path}/{sprites[i].name}.asset");
                }
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// Enables and disables the Tiles to NavTiles button based on selection.
        /// </summary>
        [MenuItem("Tools/NavTiles/Convert Tiles to NavTiles", true)]
        static bool ValidateConvertTiles()
        {
            // Check if there is a tile in the selection.
            foreach (Object obj in Selection.objects)
            {
                if (obj.GetType() == typeof(Tile))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts all selected tiles from regular Tiles to NavTiles, preserving their properties.
        /// </summary>
        [MenuItem("Tools/NavTiles/Convert Tiles to NavTiles")]
        static void ConvertTiles()
        {
            Object[] objects = Selection.objects;

            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < objects.Length; i++)
            {
                // Only use tiles, no subclasses.
                if (objects[i].GetType() == typeof(Tile))
                {
                    tiles.Add((Tile)objects[i]);
                }
            }

            Object[] newTiles = new Object[tiles.Count];

            for (int i = 0; i < tiles.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Converting to NavTile(s)", $"Converting {tiles[i].name}", (float)i / (tiles.Count - 1));

                NavTile nTile = ScriptableObject.CreateInstance<NavTile>();

                nTile.CreateFromTile(tiles[i]);

                string path = AssetDatabase.GetAssetPath(tiles[i]);

                Tilemap[] maps = Resources.FindObjectsOfTypeAll<Tilemap>();

                for (int x = 0; x < maps.Length; x++)
                {
                    if (maps[x].ContainsTile(tiles[i]))
                    {
                        maps[x].SwapTile(tiles[i], nTile);
                    }
                }

                AssetDatabase.CreateAsset(nTile, path);

                newTiles[i] = nTile;
            }

            EditorUtility.ClearProgressBar();

            Selection.objects = newTiles;
        }

        /// <summary>
        /// Enables and disables the revert NavTiles button based on selection.
        /// </summary>
        [MenuItem("Tools/NavTiles/Revert NavTiles", true)]
        static bool ValidateRevertTiles()
        {
            // Check if there is a tile in the selection.
            foreach (Object obj in Selection.objects)
            {
                if (obj is NavTile)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reverts all NavTiles to regular tiles, preserving their properties.
        /// </summary>
        [MenuItem("Tools/NavTiles/Revert NavTiles")]
        static void RevertTiles()
        {
            Object[] objects = Selection.objects;

            List<NavTile> tiles = new List<NavTile>();

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is NavTile)
                {
                    tiles.Add((NavTile)objects[i]);
                }
            }

            Object[] newTiles = new Object[tiles.Count];

            for (int i = 0; i < tiles.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Reverting NavTile(s)", $"Reverting {tiles[i].name}", (float)i / (tiles.Count - 1));

                Tile aTile = ScriptableObject.CreateInstance<Tile>();

                // Copy all properties.
                aTile.sprite = tiles[i].sprite;
                aTile.color = tiles[i].color;
                aTile.colliderType = tiles[i].colliderType;
                aTile.flags = tiles[i].flags;
                aTile.name = tiles[i].name;

                string path = AssetDatabase.GetAssetPath(tiles[i]);

                // Swap all existing tiles placed on the tilemap.
                Tilemap[] maps = Resources.FindObjectsOfTypeAll<Tilemap>();

                for (int x = 0; x < maps.Length; x++)
                {
                    if (maps[x].ContainsTile(tiles[i]))
                    {
                        maps[x].SwapTile(tiles[i], aTile);
                    }
                }

                AssetDatabase.CreateAsset(aTile, path);

                newTiles[i] = aTile;
            }

            EditorUtility.ClearProgressBar();

            Selection.objects = newTiles;
        }

        /// <summary>
        /// Enables and disables the NavLinks for Tiles button based on selection.
        /// </summary>
        [MenuItem("Tools/NavTiles/Create NavLinks for Tiles", true)]
        static bool ValidateNavLinkCreation()
        {
            // Check if there is a tile in the selection.
            foreach (Object obj in Selection.objects)
            {
                if (obj is TileBase && !(obj is NavTile))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates NavLinks for all selected tiles and places them in the resources folder.
        /// </summary>
        [MenuItem("Tools/NavTiles/Create NavLinks for Tiles")]
        static void CreateNavLinkForTile()
        {
            List<TileBase> tiles = new List<TileBase>();

            foreach (Object obj in Selection.objects)
            {
                if (obj is TileBase && !(obj is NavTile))
                {
                    tiles.Add((TileBase)obj);
                }
            }

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Resources/NavLinks"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "NavLinks");
            }

            Object[] newLinks = new Object[tiles.Count];

            for (int i = 0; i < tiles.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Creating NavLink(s)", $"Linking {tiles[i].name}", (float)i / (tiles.Count - 1));

                if (NavTileManager.Instance.LinkManager.ContainsTileLink(tiles[i]))
                {
                    Debug.LogWarning("Tried to create a NavLink for a already linked tile.", tiles[i]);
                    continue;
                }

                NavLink aLink = ScriptableObject.CreateInstance<NavLink>();

                aLink.LinkedTile = tiles[i];

                AssetDatabase.CreateAsset(aLink, "Assets/Resources/NavLinks/" + tiles[i].name + "Link.asset");

                aLink.OnTileChanged(null, aLink.LinkedTile);

                newLinks[i] = aLink;
            }

            EditorUtility.ClearProgressBar();

            Selection.objects = newLinks;
        }
    }
}
