using UnityEngine;
using UnityEditor;
using Snowcap.EditorPackage;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Editor class for the NavTile ScriptableObject.
    /// </summary>
    [CustomEditor(typeof(NavTile))]
    [CanEditMultipleObjects]
    public class NavTileEditor : Editor
    {
        private static ResizingBanner _navTileBanner = new ResizingBanner(NavTileGUIDs.NAV_TILE_BANNER);

        private const float k_PreviewWidth = 32;
        private const float k_PreviewHeight = 32;

        private SerializedProperty _spriteProperty;
        private SerializedProperty _colorProperty;
        private SerializedProperty _colliderTypeProperty;
        private SerializedProperty _areaIndexProperty;

        /// <summary>
        /// The NavTile being inspected.
        /// </summary>
        private NavTile Tile
        {
            get
            {
                return (NavTile)target;
            }
        }

        /// <summary>
        /// Initializes variables to be shown in the inspector.
        /// </summary>
        private void OnEnable()
        {
            _spriteProperty = serializedObject.FindProperty("m_Sprite");
            _colorProperty = serializedObject.FindProperty("m_Color");
            _colliderTypeProperty = serializedObject.FindProperty("m_ColliderType");
            _areaIndexProperty = serializedObject.FindProperty(nameof(NavTile.AreaIndex));
        }

        /// <summary>
        /// Draws the custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            _navTileBanner.Draw();

            DoTilePreview(Tile.sprite, Tile.color, Matrix4x4.identity);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_spriteProperty);

            using (new EditorGUI.DisabledGroupScope(_spriteProperty.objectReferenceValue == null))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Sprite Editor"))
                {
                    Selection.activeObject = _spriteProperty.objectReferenceValue;
                    EditorApplication.ExecuteMenuItem("Window/2D/Sprite Editor");
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(_colorProperty);
            EditorGUILayout.PropertyField(_colliderTypeProperty);
            EditorHelper.DrawCompressedPopup(_areaIndexProperty, NavTileManager.Instance.AreaManager.AllAreaNames);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Edit Areas"))
            {
                NavTileWindow.OpenAreasTab();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Helper function to draw the Tile preview.
        /// </summary>
        private void DoTilePreview(Sprite inSprite, Color inColor, Matrix4x4 inTransform)
        {
            if (inSprite == null)
                return;

            Rect guiRect = EditorGUILayout.GetControlRect(false, k_PreviewHeight);
            guiRect = EditorGUI.PrefixLabel(guiRect, new GUIContent("Preview", "Preview of tile with attributes set"));
            Rect previewRect = new Rect(guiRect.xMin, guiRect.yMin, k_PreviewWidth, k_PreviewHeight);
            Rect borderRect = new Rect(guiRect.xMin - 1, guiRect.yMin - 1, k_PreviewWidth + 2, k_PreviewHeight + 2);

            if (Event.current.type == EventType.Repaint)
                EditorStyles.textField.Draw(borderRect, false, false, false, false);

            DrawTexturePreview(previewRect, inSprite);
        }

        /// <summary>
        /// Helper function to draw a texture preview.
        /// </summary>
        private void DrawTexturePreview(Rect inPosition, Sprite inSprite)
        {
            Vector2 fullSize = new Vector2(inSprite.texture.width, inSprite.texture.height);
            Vector2 size = new Vector2(inSprite.textureRect.width, inSprite.textureRect.height);

            Rect coords = inSprite.textureRect;
            coords.x /= fullSize.x;
            coords.width /= fullSize.x;
            coords.y /= fullSize.y;
            coords.height /= fullSize.y;

            Vector2 ratio;
            ratio.x = inPosition.width / size.x;
            ratio.y = inPosition.height / size.y;
            float minRatio = Mathf.Min(ratio.x, ratio.y);

            Vector2 center = inPosition.center;
            inPosition.width = size.x * minRatio;
            inPosition.height = size.y * minRatio;
            inPosition.center = center;

            GUI.DrawTextureWithTexCoords(inPosition, inSprite.texture, coords);
        }
    }
}
