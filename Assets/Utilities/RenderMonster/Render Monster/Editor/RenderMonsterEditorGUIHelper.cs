using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AmazingAssets.RenderMonster
{    public static class RenderMonsterEditorGUIHelper
    {
        private static GUIStyle style;

        public static void Header(string title, Color backgroundColor)
        {
            if (style == null)
                style = "ShurikenModuleTitle";

            Rect rect = GUILayoutUtility.GetRect(16, 22f, style);

            using (new AmazingAssets.EditorGUIUtility.GUIBackgroundColor(backgroundColor))
            {
                GUI.Box(rect, title, style);
            }
        }
    }
}

