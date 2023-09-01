using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shuffler : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Window/Shuffler")]
    static void Init()
    {
        Shuffler window = (Shuffler)EditorWindow.GetWindow(typeof(Shuffler));
        window.Show();
    }

    void OnGUI()
    {
        int amount = Selection.transforms.Length;
        GUILayout.Label("\n\nAmount of selected objects: " + amount);
        if (amount > 1)
        {
            if (GUILayout.Button("Shuffle"))
            {
                List<Transform> selectedTransforms = new List<Transform>();

                List<Transform> ghosts = new List<Transform>();

                foreach (Transform selectedTransform in Selection.transforms)
                {
                    selectedTransforms.Add(selectedTransform);
                    Transform temp = new GameObject("Temp&&&&&&").transform;
                    temp.position = selectedTransform.position;
                    ghosts.Add(temp);
                }

                for (int i = 0; i < selectedTransforms.Count; i++)
                {
                    Transform temp = ghosts[Random.Range(0, ghosts.Count)];
                    selectedTransforms[i].position = temp.position;
                    DestroyImmediate(temp.gameObject);
                    ghosts.Remove(temp);
                }
            }
        }
    } 
#endif
}
