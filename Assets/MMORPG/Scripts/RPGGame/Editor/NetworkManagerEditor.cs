using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Mirror;
[CustomEditor(typeof(NetworkManager), true)]
[CanEditMultipleObjects]
public class NetworkManagerEditor : Editor
{
    SerializedProperty spawnListProperty;

    ReorderableList spawnList;

    protected NetworkManager networkManager;

    protected void Init()
    {
        if (this.spawnList == null)
        {

            this.networkManager = target as NetworkManager;

            this.spawnListProperty = base.serializedObject.FindProperty("spawnPrefabs");

            this.spawnList = new ReorderableList(base.serializedObject, this.spawnListProperty)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawChild,
                onReorderCallback = Changed,
                onRemoveCallback = RemoveButton,
                onChangedCallback = Changed,
                onAddCallback = AddButton,
                // this uses a 16x16 icon. other sizes make it stretch.
                elementHeight = 16
            };
        }
    }

    public override void OnInspectorGUI()
    {
        this.Init();
        base.DrawDefaultInspector();
        EditorGUI.BeginChangeCheck();
        this.spawnList.DoLayoutList();
        if (EditorGUI.EndChangeCheck())
        {
            this.serializedObject.ApplyModifiedProperties();
        }
    }

    static void DrawHeader(Rect headerRect)
    {
        GUI.Label(headerRect, "Registered Spawnable Prefabs:");
    }

    internal void DrawChild(Rect r, int index, bool isActive, bool isFocused)
    {
        SerializedProperty prefab = spawnListProperty.GetArrayElementAtIndex(index);
        GameObject go = (GameObject)prefab.objectReferenceValue;

        GUIContent label;
        if (go == null)
        {
            label = new GUIContent("Empty", "Drag a prefab with a NetworkIdentity here");
        }
        else
        {
            label = new GUIContent(go.name, "AssetId: [0] - No Network Identity");
        }

        GameObject newGameObject = (GameObject)EditorGUI.ObjectField(r, label, go, typeof(GameObject), false);

        if (newGameObject != go)
        {
            // if (newGameObject != null && !newGameObject.GetComponent<NetworkIdentity>())
            // {
            //     Debug.LogError("Prefab " + newGameObject + " cannot be added as spawnable as it doesn't have a NetworkIdentity.");
            //     return;
            // }
            prefab.objectReferenceValue = newGameObject;
        }
    }

    internal void Changed(ReorderableList list)
    {
        EditorUtility.SetDirty(target);
    }

    internal void AddButton(ReorderableList list)
    {
        this.spawnListProperty.arraySize += 1;
        list.index = spawnListProperty.arraySize - 1;

        SerializedProperty obj = this.spawnListProperty.GetArrayElementAtIndex(spawnListProperty.arraySize - 1);
        obj.objectReferenceValue = null;

        this.spawnList.index = spawnList.count - 1;

        this.Changed(list);
    }

    internal void RemoveButton(ReorderableList list)
    {
        this.spawnListProperty.DeleteArrayElementAtIndex(spawnList.index);
        if (list.index >= this.spawnListProperty.arraySize)
        {
            list.index = this.spawnListProperty.arraySize - 1;
        }
    }
}

