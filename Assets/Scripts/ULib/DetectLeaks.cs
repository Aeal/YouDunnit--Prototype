using UnityEngine;
using System.Collections;

public class DetectLeaks : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.Label("All " + FindObjectsOfType(typeof(UnityEngine.Object)).Length);
        GUILayout.Label("Textures " + FindObjectsOfType(typeof(Texture)).Length);
        GUILayout.Label("HashID " + FindObjectsOfType(typeof(HashID)).Length);
        GUILayout.Label("AudioClips " + FindObjectsOfType(typeof(AudioClip)).Length);
        GUILayout.Label("Meshes " + FindObjectsOfType(typeof(Mesh)).Length);
        GUILayout.Label("Materials " + FindObjectsOfType(typeof(Material)).Length);
        GUILayout.Label("GameObjects " + FindObjectsOfType(typeof(GameObject)).Length);
        GUILayout.Label("Components " + FindObjectsOfType(typeof(Component)).Length);
    }
}