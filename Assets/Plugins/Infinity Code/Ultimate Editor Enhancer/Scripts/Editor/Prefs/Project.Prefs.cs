/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static partial class Prefs
    {
        public static bool projectCreateFolder = true;
        public static bool projectCreateFolderByShortcut = true;
        public static bool projectCreateScript = true;
        public static bool projectCreateMaterial = true;

        public class ProjectManager : StandalonePrefManager<ProjectManager>, IHasShortcutPref
        {
            public override IEnumerable<string> keywords
            {
                get
                {
                    return new[]
                    {
                        "Create Folder By Shortcut",
                        "Create Script Button",
                    };
                }
            }

            public override void Draw()
            {
                projectCreateFolder = EditorGUILayout.ToggleLeft("Create Folder Button", projectCreateFolder);
                projectCreateFolderByShortcut = EditorGUILayout.ToggleLeft("Create Folder By Shortcut (F7)", projectCreateFolderByShortcut);
                projectCreateMaterial = EditorGUILayout.ToggleLeft("Create Material Button", projectCreateMaterial);
                projectCreateScript = EditorGUILayout.ToggleLeft("Create Script Button", projectCreateScript);
            }

            public IEnumerable<Shortcut> GetShortcuts()
            {
                if (projectCreateFolderByShortcut)
                {
                    return new[]
                    {
                        new Shortcut("Create Folder", "Project", KeyCode.F7),
                    };
                }

                return null;
            }
        }
    }
}