using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(SlideShow))]
public class SlideShowEditor : Editor 
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        SlideShow current = target as SlideShow;
        EditorGUILayout.BeginVertical("box");


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Scene To Load:");
        current.SceneToLoad = EditorGUILayout.TextField(current.SceneToLoad);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Start On Scene Load:");
        current.StartOnStart = EditorGUILayout.Toggle(current.StartOnStart);
        EditorGUILayout.EndHorizontal();
       
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Allow Player Skip:  ");
        current.allowSkip    = EditorGUILayout.Toggle( current.allowSkip   );
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Pause For User:    ");
        current.pauseForUser = EditorGUILayout.Toggle(current.pauseForUser);
        EditorGUILayout.EndHorizontal();

        current.ScaleMode = (ScaleMode) EditorGUILayout.EnumPopup("Scale Mode: ",current.ScaleMode);

        if(GUILayout.Button("Add Slide"))
        {
            current.Slides.Add(new Slide());
        }
        EditorGUILayout.BeginVertical();

        for (int index = 0; index < current.Slides.Count; index++)
        {
            Slide slide = current.Slides[index];
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            slide.picture =
                (Texture2D) EditorGUILayout.ObjectField("Slide Picture: ", slide.picture, typeof (Texture2D), false);
            slide.subtitle = (string)EditorGUILayout.TextField("Subtitle: ", slide.subtitle); 
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            slide.sound = (AudioClip) EditorGUILayout.ObjectField("Sound: ", slide.sound, typeof (AudioClip), false);
            slide.fadeTime = EditorGUILayout.FloatField("Fade Time", slide.fadeTime);
            if (GUILayout.Button("Remove"))
            {
                current.Slides.Remove(slide);
                index--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}
