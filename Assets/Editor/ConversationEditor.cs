using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Object = UnityEngine.Object;


public class ConversationEditor : EditorWindow
{

   

    private const int VERSION_NUMBER = 1;

    private const int maxWidth = 500,
                      RemoveButtonSize = 20;
    private GUILayoutOption[] GUILayoutOptions = new[] { GUILayout.ExpandHeight(true) },
                                ButtonOptions = new[] { GUILayout.ExpandWidth(false) };
    private string currentFile = "";
    private int actionToAdd;

    private string[] possibleActions;

    private List<Type> actionTypes;

    private static ConversationEditor instance;

    private static List<DialogItem> DialogItems;

    private static Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/Conversation Editor")]
    public static void ShowWindow()
    {
        if (instance == null)
            instance = CreateInstance<ConversationEditor>();
        instance.Show();
        if(DialogItems == null)
            DialogItems = new List<DialogItem>();

    }

    public void GenerateActionTypes()
    {
        possibleActions = Utility.GetSubClasses(typeof(NodeAction));
        actionTypes = Utility.GetClasses(typeof(NodeAction));
    }



    public void OnGUI()
    {
        EditorGUILayout.BeginVertical(new[] {GUILayout.MaxWidth(Screen.width)});
        DrawDialogControls();

        if (DialogItems == null) return;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical("box");
        for (int i = 0; i < DialogItems.Count; i++)
        {
            DialogItem dialog = DialogItems[i];

            EditorGUILayout.BeginVertical(new GUIStyle("box"));
            EditorGUILayout.BeginHorizontal();
            dialog.show = EditorGUILayout.Foldout(dialog.show,
                                                  dialog.subtitle.Substring(0, Mathf.Min(40, dialog.subtitle.Length)) +
                                                  "..");
            GUILayout.FlexibleSpace();
            DrawDialogOptionControls(dialog, i);
            EditorGUILayout.EndHorizontal();
            if (dialog.show)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();

                if (dialog.is_branch)
                {
                    DrawBranch(dialog, i);
                    DrawActionControls(ref dialog);
                }
                else
                {
                    DrawConversationOptions(dialog);
                    DrawActionControls(ref dialog);
                } // endif is_branch

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        } // endof loop over dialog elements

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawActionControls(ref DialogItem item)
    {
        item.skippable = EditorGUILayout.Toggle("Skipabble: ", item.skippable);
        actionToAdd = EditorGUILayout.Popup(actionToAdd, possibleActions);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Focused Action"))
        {
            NodeAction action = (NodeAction)actionTypes[actionToAdd].GetConstructor(Type.EmptyTypes).Invoke(null);
            item.onNodeFocusedActions.Add(action);
        }
        if(GUILayout.Button("Add Leave Action"))
        {
            NodeAction action = (NodeAction)actionTypes[actionToAdd].GetConstructor(Type.EmptyTypes).Invoke(null);
            item.onNodeLeaveActions.Add(action);
        }
        EditorGUILayout.EndHorizontal();

        DrawNodeActionControls(item.onNodeFocusedActions, "On Node Entered Actions");
        DrawNodeActionControls(item.onNodeLeaveActions,   "On Node Exit Actions");

      }

    void DrawNodeActionControls(List<NodeAction> nodeActions, string Title )
    {
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField(Title);
        for (int index = 0; index < nodeActions.Count; index++)
        {
            NodeAction VARIABLE = nodeActions[index];
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", ButtonOptions))
            {
                VARIABLE.OnEditorDestroy();
                nodeActions.Remove(VARIABLE);
                index--;
            }

            VARIABLE.IsShowing = EditorGUILayout.Foldout(VARIABLE.IsShowing, VARIABLE.GetType().ToString());

            if (VARIABLE.IsShowing)
            {
                VARIABLE.OnInspectorGUI();                
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
    }
    
    void DrawConversationOptions(DialogItem dialog)
    {
        EditorStyles.textField.wordWrap = true;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Speaker");
        dialog.speaker = (Speakers)EditorGUILayout.EnumPopup(dialog.speaker);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Audio Clip");
        dialog.spoken = (AudioClip)EditorGUILayout.ObjectField(dialog.spoken, typeof(AudioClip), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Subtitle");
        dialog.subtitle = EditorGUILayout.TextArea(dialog.subtitle, GUILayoutOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Wait type:");
        dialog.wait = (Waits)EditorGUILayout.EnumPopup(dialog.wait);
        EditorGUILayout.EndHorizontal();
        if(dialog.wait == Waits.Time)
        {
            dialog.waitTime = EditorGUILayout.FloatField("Wait time: ", dialog.waitTime);
        }
    }

    void DrawDialogOptionControls(DialogItem dialog, int i)
    {
        if (GUILayout.Button("up"))
        {
            if (i > 0)
            {
                DialogItems.Insert(i - 1, dialog);
                DialogItems.RemoveAt(i + 1);
            }
        }
        if (GUILayout.Button("down"))
        {
            if (i + 1 < DialogItems.Count)
            {
                DialogItems.Insert(i + 2, dialog);
                DialogItems.RemoveAt(i);
            }
        }
        if (GUILayout.Button("delete"))
        {
            DialogItems.RemoveAt(i);
        }
    }

    void DrawDialogControls()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Current File loaded: " + currentFile);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            SaveConvo(currentFile);
        }
        if (GUILayout.Button("Save As"))
        {
            SaveConvo();
        }
        if (GUILayout.Button("Load"))
        {
            LoadConvo();
        }
        if(GUILayout.Button("New"))
        {
            if(EditorUtility.DisplayDialog("Are you sure?", "You are about to loose all unsaved work. Are you sure?", "Yes", "No"))
            {
                DialogItems = new List<DialogItem>();
                currentFile = "";
            }
        }
        if(GUILayout.Button("Refresh"))
        {
            if(currentFile != "" )
            {
                SaveConvo(currentFile);
                LoadConvo(currentFile);
            }
            else
            {
               //Debug.Log("No file loaded");
            }
        }
        if(GUILayout.Button("Add Defaults"))
        {
            //ConversationActionWizard.CreateWizard(DialogItems);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add dialog element"))
        {
            if (DialogItems == null)
            {
                DialogItems = new List<DialogItem>();
            }
            DialogItems.Add(new DialogItem());
        }
        if (GUILayout.Button("Add branching choices"))
        {
            if (DialogItems == null)
            {
                DialogItems = new List<DialogItem>();
            }
            
            DialogItems.Add(new DialogItem {is_branch = true});
        }
        EditorGUILayout.EndHorizontal();

        
        EditorGUILayout.EndVertical();
    }

    void DrawBranch(DialogItem dialog, int i)
    {
        if (dialog.choice != null)
        {
            for (int j = 0; j < dialog.choice.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", ButtonOptions))
                {
                    DialogItems[i].choice.Remove(dialog.choice[j]);
                    j--;
                    continue;
                }
                dialog.choice[j] = EditorGUILayout.TextArea(dialog.choice[j], GUILayoutOptions);
                String[] option_names = new String[DialogItems.Count + 1];
                int[] option_numbers = new int[DialogItems.Count + 1];

                for (int k = 0; k < DialogItems.Count; k++)
                {
                    if (k < i)
                    {
                        option_names[k] =
                            DialogItems[k].subtitle.Substring(0, Mathf.Min(40, DialogItems[k].subtitle.Length)) + "..";
                        option_numbers[k] = k;
                    }
                    else if (k > i)
                    {
                        option_names[k - 1] =
                            DialogItems[k].subtitle.Substring(0, Mathf.Min(40, DialogItems[k].subtitle.Length)) + "..";
                        option_numbers[k - 1] = k;
                    }
                }
                dialog.jump[j] = EditorGUILayout.IntPopup(dialog.jump[j], option_names, option_numbers);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button("Add choice"))
        {
            if (dialog.choice == null)
            {
                dialog.choice = new List<String>();
                dialog.jump = new List<int>();
            }
            dialog.choice.Add("(enter text to display)");
            dialog.jump.Add(-1);
        }
    }

    void SaveConvo(string path = "")
    {
        string file = path;
        if (file == "" || file.EndsWith("txt"))
            file = EditorUtility.SaveFilePanel("Save Convo", Application.dataPath + "/Resources", "NewFile", "xml");
        if (file == "") return;
        

       //Debug.Log("Saving file: " + file);
        foreach (DialogItem dialogItem in DialogItems)
        {
            dialogItem.BuildForSerialize();
        }
       using(FileStream fs = new FileStream(file,FileMode.Create))
       {
           XmlSerializer serializer = new XmlSerializer(typeof(ConversationPackage), actionTypes.ToArray());
           serializer.Serialize(fs,new ConversationPackage(DialogItems,VERSION_NUMBER ));
       }
    }

    void LoadConvo(string file = "") 
    {
        if(file == "")
        {
            file = EditorUtility.OpenFilePanel("Load Convo", Application.dataPath + "/Resources", "xml");
        }
        if(file == "")return;
        currentFile = file;
       //Debug.Log("Loading File: " + currentFile);

        int versionNumber = 0; 
        
            try
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof (ConversationPackage), actionTypes.ToArray());
                    ConversationPackage package = serializer.Deserialize(fs) as ConversationPackage;
                    DialogItems = package.items;
                    versionNumber = package.versionNumber;
                }
            }
            catch (Exception)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                       //Debug.LogError("Error loading package, Trying legacy loader");
                        XmlSerializer serializer = new XmlSerializer(typeof (List<DialogItem>), actionTypes.ToArray());
                        DialogItems = serializer.Deserialize(fs) as List<DialogItem>;
                        if (DialogItems == null)
                        {
                           //Debug.LogError("Error in the legacy loader. Check for valid XML");
                        }
                    }
                }
                catch (Exception e)
                {
                   //Debug.LogError("Error in the legacy loader. " + e.Message);
                }
                

            }
            
        foreach (DialogItem dialogItem in DialogItems)
        {
            dialogItem.Initialize();
        }
        //Do specific version loading here

        
       
	}

    void OnEnable()
    {
        GenerateActionTypes();
        
    }

    
}

