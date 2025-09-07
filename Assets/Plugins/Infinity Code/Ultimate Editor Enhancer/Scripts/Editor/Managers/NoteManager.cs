/*           INFINITY CODE          */
/*     https://infinity-code.com    */

using System.Collections.Generic;
using System.Linq;
using InfinityCode.UltimateEditorEnhancer.PostHeader;
using UnityEditor;
using UnityEngine;

namespace InfinityCode.UltimateEditorEnhancer
{
    public static class NoteManager
    {
        private static Dictionary<GameObject, NoteItem> cachedNotes = new Dictionary<GameObject, NoteItem>();
        
        public static void TryGetValue(GameObject gameObject, out NoteItem note)
        {
            if (cachedNotes.TryGetValue(gameObject, out note)) return;
            
            string gid = GlobalObjectId.GetGlobalObjectIdSlow(gameObject).ToString();
            note = ReferenceManager.notes.FirstOrDefault(n => n.gid == gid);
            if (note == null)
            {
                note = new NoteItem { gid = gid };
                cachedNotes[gameObject] = note;
            }
            else
            {
                cachedNotes[gameObject] = note;
            }
        }
    }
}