//using UnityEditor.UIElements;
//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEditor;

//[CustomEditor(typeof(Item))]
//[CanEditMultipleObjects]
//public class ItemEditor : Editor
//{
//    SerializedProperty itemIconProp;
//    SerializedProperty itemNameProp;
//    SerializedProperty itemInfoProp;
//    SerializedProperty itemIdProp;
//    SerializedProperty itemTypeProp;
//    ItemType it = ItemType.None;

//    void OnEnable()
//    {
//        // Setup the SerializedProperties.
//        itemIconProp = serializedObject.FindProperty("itemIcon");
//        itemNameProp = serializedObject.FindProperty("itemName");
//        itemInfoProp = serializedObject.FindProperty("itemInfo");
//        itemIdProp = serializedObject.FindProperty("itemId");
//        itemTypeProp = serializedObject.FindProperty("type");
//    }

//    public override void OnInspectorGUI()
//    {
//        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
//        serializedObject.Update();

//        // Show the custom GUI controls.
//        if (!itemNameProp.hasMultipleDifferentValues)
//            itemIdProp.intValue = EditorGUILayout.IntField("Item id:", itemIdProp.intValue);

//        if (!itemNameProp.hasMultipleDifferentValues)
//            itemNameProp.stringValue = EditorGUILayout.TextField("Item Name :", itemNameProp.stringValue);

//        if (!itemIconProp.hasMultipleDifferentValues)
//            EditorGUILayout.ObjectField(itemIconProp, typeof(Sprite));

//        if (!itemNameProp.hasMultipleDifferentValues)
//            itemInfoProp.stringValue = EditorGUILayout.TextField("Item description :", itemNameProp.stringValue);

//        if (!itemNameProp.hasMultipleDifferentValues)
//            it =  (ItemType)EditorGUILayout.EnumPopup("Item type:", it);


//        Debug.Log(itemTypeProp);

//        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
//        serializedObject.ApplyModifiedProperties();
        
//    }
//}
