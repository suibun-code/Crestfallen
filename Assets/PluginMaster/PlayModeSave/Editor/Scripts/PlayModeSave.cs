/*
Copyright (c) 2020 Omar Duarte
Unauthorized copying of this file, via any medium is strictly prohibited.
Writen by Omar Duarte, 2020.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System.Linq;
using UnityEngine;

namespace PluginMaster
{
    public class PlayModeSave : UnityEditor.EditorWindow
    {
        #region CONTEXT
        private const string TOOL_NAME = "Play Mode Save";
        [UnityEditor.MenuItem("CONTEXT/Component/Save Now", true, 1201)]
        private static bool ValidateSaveNowMenu(UnityEditor.MenuCommand command)
            => !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(command.context) && Application.IsPlaying(command.context);
        [UnityEditor.MenuItem("CONTEXT/Component/Save Now", false, 1201)]
        private static void SaveNowMenu(UnityEditor.MenuCommand command)
            => Add(command.context as Component, SaveCommand.SAVE_NOW, false, true);

        [UnityEditor.MenuItem("CONTEXT/Component/Save When Exiting Play Mode", true, 1202)]
        private static bool ValidateSaveOnExtiMenu(UnityEditor.MenuCommand command) => ValidateSaveNowMenu(command);
        [UnityEditor.MenuItem("CONTEXT/Component/Save When Exiting Play Mode", false, 1202)]
        private static void SaveOnExitMenu(UnityEditor.MenuCommand command)
            => Add(command.context as Component, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, false, true);

        [UnityEditor.MenuItem("CONTEXT/Component/Always Save When Exiting Play Mode", true, 1203)]
        private static bool ValidateAlwaysSaveOnExitMenu(UnityEditor.MenuCommand command)
           => (UnityEditor.SceneManagement.EditorSceneManager.IsPreviewScene((command.context as Component).gameObject.scene)
                || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(command.context)) ? false
            : !PMSData.Contains(new ComponentSaveDataKey(command.context as Component), out ComponentSaveDataKey foundKey);

        [UnityEditor.MenuItem("CONTEXT/Component/Always Save When Exiting Play Mode", false, 1203)]
        private static void AlwaysSaveOnExitMenu(UnityEditor.MenuCommand command)
            => Add(command.context as Component, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, true, true);

        [UnityEditor.MenuItem("CONTEXT/Component/Remove From Save List", true, 1204)]
        private static bool ValidateRemoveFromSaveList(UnityEditor.MenuCommand command)
            => UnityEditor.PrefabUtility.IsPartOfPrefabAsset(command.context) ? false
            : PMSData.Contains(new ComponentSaveDataKey(command.context as Component), out ComponentSaveDataKey foundKey);
        [UnityEditor.MenuItem("CONTEXT/Component/Remove From Save List", false, 1204)]
        private static void RemoveFromSaveList(UnityEditor.MenuCommand command)
        {
            var component = command.context as Component;
            var key = new ComponentSaveDataKey(component);
            PMSData.Remove(key);
            CompDataRemoveKey(key);
        }

        [UnityEditor.MenuItem("CONTEXT/Component/Apply Play Mode Changes", true, 1210)]
        private static bool ValidateApplyMenu(UnityEditor.MenuCommand command)
        {
            var key = GetKey(command.context);
            return !Application.isPlaying && CompDataContainsKey(ref key);
        }
        [UnityEditor.MenuItem("CONTEXT/Component/Apply Play Mode Changes", false, 1210)]
        private static void ApplyMenu(UnityEditor.MenuCommand command)
        {
            var key = GetKey(command.context);
            Apply(key);
            if (!PMSData.Contains(key, out ComponentSaveDataKey foundKey)) CompDataRemoveKey(key);
        }

        [UnityEditor.MenuItem("CONTEXT/Component/", false, 1300)]
        private static void Separator(UnityEditor.MenuCommand command) { }

        [UnityEditor.MenuItem("CONTEXT/ScriptableObject/Save Now", false, 1211)]
        private static void SaveScriptableObject(UnityEditor.MenuCommand command)
        {
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.SetDirty(command.context);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        #endregion

        #region WINDOW
        [UnityEditor.MenuItem("Tools/Plugin Master/" + TOOL_NAME, false, int.MaxValue)]
        private static void ShowWindow() => GetWindow<PlayModeSave>(TOOL_NAME);

        private const string SAVE_ENTIRE_HIERARCHY_PREF = "PLAY_MODE_SAVE_saveEntireHierarchy";
        private const string AUTO_APPLY_PREF = "PLAY_MODE_SAVE_autoApply";
        private const string INCLUDE_CHILDREN_PREF = "PLAY_MODE_SAVE_includeChildren";
        private static bool _showAplyButtons = true;

        private static void LoadPrefs()
        {
            _saveEntireHierarchy = UnityEditor.EditorPrefs.GetBool(SAVE_ENTIRE_HIERARCHY_PREF, false);
            _autoApply = UnityEditor.EditorPrefs.GetBool(AUTO_APPLY_PREF, true);
            _includeChildren = UnityEditor.EditorPrefs.GetBool(INCLUDE_CHILDREN_PREF, true);
        }

        private void OnEnable() => LoadPrefs();

        private void OnDisable()
        {
            UnityEditor.EditorPrefs.SetBool(SAVE_ENTIRE_HIERARCHY_PREF, _saveEntireHierarchy);
            UnityEditor.EditorPrefs.SetBool(AUTO_APPLY_PREF, _autoApply);
            UnityEditor.EditorPrefs.SetBool(INCLUDE_CHILDREN_PREF, _includeChildren);
        }

        private void OnSelectionChange() => Repaint();

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                using (new UnityEditor.EditorGUILayout.VerticalScope(UnityEditor.EditorStyles.helpBox))
                {
                    using (var check = new UnityEditor.EditorGUI.ChangeCheckScope())
                    {
                        var label = "Auto-Apply All Changes When Exiting Play Mode";
                        _autoApply = UnityEditor.EditorGUILayout.ToggleLeft(label, _autoApply);
                        if (check.changed) UnityEditor.EditorPrefs.SetBool(AUTO_APPLY_PREF, _autoApply);
                    }
                    if (!_autoApply)
                    {
                        if (_compData.Count == 0 && !_showAplyButtons)
                            UnityEditor.EditorGUILayout.LabelField("Nothing to apply");
                        else if (_compData.Count > 0 && _showAplyButtons)
                        {
                            using (new UnityEditor.EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button("Apply All Changes"))
                                {
                                    ApplyAll();
                                    _showAplyButtons = false;
                                }
                                GUILayout.FlexibleSpace();
                            }
                        }
                    }
                }
            }
            var selection = UnityEditor.Selection.GetFiltered<GameObject>(UnityEditor.SelectionMode.Editable
                | UnityEditor.SelectionMode.ExcludePrefab | UnityEditor.SelectionMode.TopLevel);
            if (selection.Length > 0)
            {
                void SaveSelection(SaveCommand cmd, bool always)
                {
                    foreach (var obj in selection)
                    {
                        var components = _includeChildren ? obj.GetComponentsInChildren<Component>()
                            : obj.GetComponents<Component>();
                        foreach (var comp in components) Add(comp, cmd, always, true);
                        if (cmd == SaveCommand.SAVE_ON_EXITING_PLAY_MODE)
                        {
                            var objKey = new ObjectDataKey(obj);
                            AddFullObjectData(objKey, always);
                            if (always) PMSData.AlwaysSaveFull(objKey);
                        }
                    }
                }

                using (new UnityEditor.EditorGUILayout.VerticalScope(UnityEditor.EditorStyles.helpBox))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Execute for all selected objects: ");
                        GUILayout.FlexibleSpace();
                        _includeChildren = UnityEditor.EditorGUILayout.ToggleLeft("Include children",
                            _includeChildren, GUILayout.Width(109));
                    }
                    if (Application.isPlaying)
                    {
                        if (GUILayout.Button("Save all components now"))
                            SaveSelection(SaveCommand.SAVE_NOW, false);
                        if (GUILayout.Button("Save all components when exiting play mode"))
                            SaveSelection(SaveCommand.SAVE_ON_EXITING_PLAY_MODE, false);
                        else maxSize = minSize = new Vector2(376, 180);
                    }
                    else maxSize = minSize = new Vector2(376, _autoApply ? 162 : 182);
                    if (GUILayout.Button("Always save all components when exiting play mode"))
                        SaveSelection(SaveCommand.SAVE_ON_EXITING_PLAY_MODE, true);
                    if (GUILayout.Button("Remove all components from save list"))
                    {
                        foreach (var obj in selection)
                        {
                            var components = _includeChildren ? obj.GetComponentsInChildren<Component>()
                           : obj.GetComponents<Component>();
                            foreach (var comp in components)
                            {
                                var key = new ComponentSaveDataKey(comp);
                                PMSData.Remove(key);
                                CompDataRemoveKey(key);
                            }
                            var objKey = new ObjectDataKey(obj);
                            RemoveFullObjectData(objKey);
                            PMSData.RemoveFull(objKey);
                            if (_includeChildren)
                            {
                                for (int i = 0; i < obj.transform.childCount; ++i)
                                {
                                    var child = obj.transform.GetChild(i);
                                    objKey = new ObjectDataKey(child.gameObject);
                                    RemoveFullObjectData(objKey);
                                    PMSData.RemoveFull(objKey);
                                }
                            }
                        }
                    }
                }
            }
            else maxSize = minSize = new Vector2(376, _autoApply ? 100 : 120);
            using (new UnityEditor.EditorGUILayout.VerticalScope(UnityEditor.EditorStyles.helpBox))
            {
                using (var check = new UnityEditor.EditorGUI.ChangeCheckScope())
                {
                    _saveEntireHierarchy = UnityEditor.EditorGUILayout.ToggleLeft
                        ("Save The Entire Hierarchy When Exiting Play Mode", _saveEntireHierarchy);
                    if (check.changed) UnityEditor.EditorPrefs.SetBool(SAVE_ENTIRE_HIERARCHY_PREF, _saveEntireHierarchy);
                    if (_saveEntireHierarchy)
                    {
                        UnityEditor.EditorGUILayout.HelpBox("NOT RECOMMENDED. Enabling this option in large scenes " +
                            "can cause a long delay when saving and applying changes.", UnityEditor.MessageType.Warning);
                    }
                }
            }
        }
        #endregion

        #region SAVE
        private static bool _autoApply = true;
        private static bool _saveEntireHierarchy = false;
        private static bool _includeChildren = true;
        private static System.Collections.Generic.List<string> _scenesToOpen = new System.Collections.Generic.List<string>();
        private static System.Collections.Generic.List<ComponentSaveDataKey> _componentsToBeDeleted
            = new System.Collections.Generic.List<ComponentSaveDataKey>();
        private static System.Collections.Generic.Dictionary<ObjectDataKey, SaveObject> _objData
            = new System.Collections.Generic.Dictionary<ObjectDataKey, SaveObject>();

        private static System.Collections.Generic.Dictionary<ComponentSaveDataKey, SaveDataValue> _compData
            = new System.Collections.Generic.Dictionary<ComponentSaveDataKey, SaveDataValue>();
        private static ComponentSaveDataKey GetKey(Object comp)
            => new ComponentSaveDataKey(comp as Component);
        private static System.Collections.Generic.List<FullObjectData> _fullObjData
            = new System.Collections.Generic.List<FullObjectData>();
        private static System.Collections.Generic.List<ObjectDataKey> _objectsToBeDeleted
            = new System.Collections.Generic.List<ObjectDataKey>();

        private static bool _openingScenes = false;
        private enum SaveCommand { SAVE_NOW, SAVE_ON_EXITING_PLAY_MODE }

        private const int MAX_PROPERTY_COUNT = 1000;
        private static void CopyPropertiesFromComponent(Component comp, UnityEditor.SerializedObject data,
            ref System.Collections.Generic.Dictionary<string, ObjectDataKeyBase> refDict)
        {
            var serializedObj = new UnityEditor.SerializedObject(comp);
            var prop = serializedObj.GetIterator();
            int propCount = 0;
            refDict = new System.Collections.Generic.Dictionary<string, ObjectDataKeyBase>();
            while (prop.NextVisible(true))
            {
                if (prop.propertyType == UnityEditor.SerializedPropertyType.ObjectReference && prop.name != "m_Script")
                {
                    if (prop.objectReferenceValue != null)
                    {
                        var gameObj = prop.objectReferenceValue as GameObject;
                        if (gameObj == null)
                        {
                            var component = prop.objectReferenceValue as Component;
                            if (component != null) gameObj = component.gameObject;
                        }
                        if (gameObj != null)
                        {
                            var objKey = new ObjectDataKeyBase(gameObj);
                            refDict.Add(prop.propertyPath, objKey);
                        }
                    }
                }

                data.CopyFromSerializedProperty(prop);
                if (++propCount >= MAX_PROPERTY_COUNT) break;
            }
        }
        private static void CopyPropertiesFromData(Component comp, UnityEditor.SerializedObject data,
            System.Collections.Generic.Dictionary<string, ObjectDataKeyBase> refDict)
        {
            var serializedObj = new UnityEditor.SerializedObject(comp);
            var prop = data.GetIterator();
            int propCount = 0;
            while (prop.NextVisible(true))
            {
                var compProperty = serializedObj.FindProperty(prop.propertyPath);
                if (compProperty != null)
                {
                    if (prop.propertyType == UnityEditor.SerializedPropertyType.ObjectReference && prop.name != "m_Script"
                        && refDict != null && refDict.Count > 0 && refDict.ContainsKey(prop.propertyPath))
                    {
                        var objKey = refDict[prop.propertyPath];
                        var refComp = FindObject(objKey);
                        compProperty.objectReferenceValue = refComp;
                        compProperty.objectReferenceInstanceIDValue = refComp.GetInstanceID();
                    }
                    else serializedObj.CopyFromSerializedProperty(prop);

                }
                if (++propCount >= MAX_PROPERTY_COUNT) break;
            }
            serializedObj.ApplyModifiedProperties();
        }

        [System.Serializable]
        private class PMSData
        {
            private const string FILE_NAME = "PMSData";
            private const string RELATIVE_PATH = "/PluginMaster/PlayModeSave/Editor/Resources/";
            [SerializeField] private string _rootDirectory = null;
            [SerializeField]
            private System.Collections.Generic.List<ComponentSaveDataKey> _alwaysSave
                = new System.Collections.Generic.List<ComponentSaveDataKey>();
            [SerializeField]
            private System.Collections.Generic.List<ObjectDataKey> _alwaysSaveFull
                = new System.Collections.Generic.List<ObjectDataKey>();

            public static ComponentSaveDataKey[] alwaysSaveArray => instance._alwaysSave.ToArray();
            public static ObjectDataKey[] alwaysSaveFullArray => instance._alwaysSaveFull.ToArray();
            public static void AlwaysSave(ComponentSaveDataKey key)
            {
                if (instance._alwaysSave.Contains(key)) return;
                instance._alwaysSave.Add(key);
                Save();
            }
            public static void AlwaysSaveFull(ObjectDataKey data)
            {
                if (instance._alwaysSaveFull.Contains(data)) return;
                instance._alwaysSaveFull.Add(data);
                Save();
            }
            public static bool saveAfterLoading { get; set; }
            public static void Save(bool refreshDataBase = true)
            {
                if (string.IsNullOrEmpty(instance._rootDirectory)) instance._rootDirectory = Application.dataPath;
                var fullDirectoryPath = instance._rootDirectory + RELATIVE_PATH;
                var fullFilePath = fullDirectoryPath + FILE_NAME + ".txt";
                if (!System.IO.File.Exists(fullFilePath))
                {
                    var directories = System.IO.Directory.GetDirectories(Application.dataPath,
                        "PluginMaster", System.IO.SearchOption.AllDirectories);
                    if (directories.Length == 0) System.IO.Directory.CreateDirectory(fullDirectoryPath);
                    else
                    {
                        instance._rootDirectory = System.IO.Directory.GetParent(directories[0]).FullName;
                        fullDirectoryPath = instance._rootDirectory + RELATIVE_PATH;
                        fullFilePath = fullDirectoryPath + FILE_NAME + ".txt";
                    }
                    if (!System.IO.Directory.Exists(fullDirectoryPath))
                        System.IO.Directory.CreateDirectory(fullDirectoryPath);
                }
                var jsonString = JsonUtility.ToJson(instance);
                System.IO.File.WriteAllText(fullFilePath, jsonString);
                if (refreshDataBase) UnityEditor.AssetDatabase.Refresh();
            }
            public static void Remove(ComponentSaveDataKey item)
            {
                if (Contains(item, out ComponentSaveDataKey foundKey))
                {
                    instance._alwaysSave.Remove(foundKey);
                    Save();
                }
            }

            public static void RemoveFull(ObjectDataKey item)
            {
                if (ContainsFull(item, out ObjectDataKey foundKey))
                {
                    instance._alwaysSaveFull.Remove(foundKey);
                    Save();
                }
            }

            public static void UpdateFull()
            {
                var loadedScenePaths = GetLoadedScenePaths();
                var alwaysSaveFull = alwaysSaveFullArray;
                var removed = false;
                foreach (var item in alwaysSaveFull)
                {
                    if (!loadedScenePaths.Contains(item.scenePath)) continue;
                    var obj = FindObject(item, false);
                    if (obj == null)
                    {
                        removed = true;
                        instance._alwaysSaveFull.Remove(item);
                    }
                }
                var alwaysSaveComponents = alwaysSaveArray;
                foreach (var item in alwaysSaveComponents)
                {
                    if (!loadedScenePaths.Contains(item.objKey.scenePath)) continue;
                    var obj = FindObject(item.objKey, false);
                    if (obj == null)
                    {
                        removed = true;
                        instance._alwaysSave.Remove(item);
                    }
                }
                if (removed) Save();
            }
            public static bool Load(bool refreshDataBase = true)
            {
                var jsonTextAsset = Resources.Load<TextAsset>(FILE_NAME);
                if (jsonTextAsset == null) return false;
                _instance = JsonUtility.FromJson<PMSData>(jsonTextAsset.text);
                _loaded = true;
                if (saveAfterLoading) Save(refreshDataBase);
                return true;
            }

            public static bool Contains(ComponentSaveDataKey key, out ComponentSaveDataKey foundKey)
            {
                foundKey = null;
                if (!_loaded) Load();
                foreach (var compKey in instance._alwaysSave)
                {
                    if (compKey == key)
                    {
                        foundKey = compKey;
                        return true;
                    }
                }
                return false;
            }

            public static bool ContainsFull(ObjectDataKey key, out ObjectDataKey foundKey)
            {
                foundKey = null;
                if (!_loaded) Load();
                foreach (var objKey in instance._alwaysSaveFull)
                {
                    if (objKey == key)
                    {
                        foundKey = objKey;
                        return true;
                    }
                }
                return false;

            }
            private static PMSData _instance = null;
            private static bool _loaded = false;
            private PMSData() { }
            private static PMSData instance
            {
                get
                {
                    if (_instance == null) _instance = new PMSData();
                    return _instance;
                }
            }
        }

        [System.Serializable]
        private class ObjectDataKeyBase : ISerializationCallbackReceiver, System.IEquatable<ObjectDataKeyBase>
        {
            [SerializeField] private int _objId = -1;
            [SerializeField] private string _globalObjId = null;
            [SerializeField] private string _scenePath = null;
            [SerializeField] private int[] _siblingPath = null;
            [SerializeField] private string _rootName = null;
            [SerializeField] private string _objName = null;


            public int objId { get => _objId; set => value = _objId; }
            public string globalObjId { get => _globalObjId; set => _globalObjId = value; }
            public string scenePath => _scenePath;
            public int[] siblingPath => _siblingPath;
            public string rootName => _rootName;
            public string objName => _objName;

            protected void NullInitialization()
            {
                _objId = -1;
                _globalObjId = null;
                _scenePath = null;
            }
            public ObjectDataKeyBase(GameObject gameObject)
            {
                if (gameObject == null)
                {
                    NullInitialization();
                    return;
                }
                _objId = gameObject.GetInstanceID();
                _globalObjId = UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(gameObject).ToString();
                _scenePath = gameObject.scene.path;
                var sibPath = new System.Collections.Generic.List<int>();
                var parent = gameObject.transform;
                do
                {
                    sibPath.Insert(0, parent.GetSiblingIndex());
                    if (parent.parent == null) _rootName = parent.name;
                    parent = parent.parent;
                } while (parent != null);
                _siblingPath = sibPath.ToArray();
                _objName = gameObject.name;
            }

            public ObjectDataKeyBase(int objId, string globalObjId, string scenePath)
            {
                _objId = objId;
                _globalObjId = globalObjId;
                _scenePath = scenePath;
            }

            public void UpdateSibPath()
            {
                var sibPath = new System.Collections.Generic.List<int>();
                var gameObject = FindObject(this);
                if (gameObject == null) return;
                var parent = gameObject.transform;
                do
                {
                    sibPath.Insert(0, parent.GetSiblingIndex());
                    if (parent.parent == null) _rootName = parent.name;
                    parent = parent.parent;
                } while (parent != null);
                _siblingPath = sibPath.ToArray();
                _objName = gameObject.name;
            }

            public bool isNull => _globalObjId == null;

            public override int GetHashCode() => _globalObjId.GetHashCode();
            public virtual bool Equals(ObjectDataKeyBase other)
            {
                if (other is null) return false;
                if (_objId == other._objId) return true;
                return _globalObjId == other._globalObjId;
            }
            public override bool Equals(object obj) => obj is ObjectDataKeyBase other && this.Equals(other);

            public virtual bool HierarchyEquals(ObjectDataKeyBase other)
            {
                if (other is null) return false;
                if (_objId == other._objId) return true;
                if (_globalObjId == other._globalObjId) return true;
                if (_scenePath != other._scenePath) return false;
                if (_rootName != other._rootName) return false;
                if (_objName != other._objName) return false;
                return Enumerable.SequenceEqual(_siblingPath, other._siblingPath);
            }
            public static bool operator ==(ObjectDataKeyBase lhs, ObjectDataKeyBase rhs) => lhs.Equals(rhs);
            public static bool operator !=(ObjectDataKeyBase lhs, ObjectDataKeyBase rhs) => !lhs.Equals(rhs);

            public void Copy(ObjectDataKeyBase other)
            {
                _objId = other._objId;
                _globalObjId = other._globalObjId;
                _scenePath = other._scenePath;
            }

            public void OnBeforeSerialize() { }

            public void OnAfterDeserialize()
            {
                if (UnityEditor.GlobalObjectId.TryParse(_globalObjId, out UnityEditor.GlobalObjectId id))
                {
                    var obj = UnityEditor.GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as GameObject;
                    if (obj == null) return;
                    _objId = obj.GetInstanceID();
                }
                PMSData.saveAfterLoading = true;
            }
        }


        [System.Serializable]
        private class ObjectDataKey : ObjectDataKeyBase
        {
            [SerializeField]
            private System.Collections.Generic.List<ObjectDataKeyBase> _parentKeys
                = new System.Collections.Generic.List<ObjectDataKeyBase>();
            public ObjectDataKey(GameObject gameObject) : base(gameObject)
            {
                if (gameObject == null)
                {
                    NullInitialization();
                    return;
                }
                var parent = gameObject.transform;
                do
                {
                    parent = parent.parent;
                    if (parent != null) _parentKeys.Add(new ObjectDataKeyBase(parent.gameObject));
                } while (parent != null);
            }

            public void UpdateParentKeys()
            {
                var obj = FindObject(this);
                _parentKeys.Clear();
                if (obj == null) return;
                var parent = obj.transform;
                do
                {
                    parent = parent.parent;
                    if (parent != null) _parentKeys.Add(new ObjectDataKeyBase(parent.gameObject));
                } while (parent != null);
                UpdateSibPath();
            }

            public bool ParentHasChanged(out Transform savedParent, out int savedSiblingIndex)
            {
                savedParent = null;
                savedSiblingIndex = 0;
                var obj = FindObject(this);
                var currentParent = obj == null ? null : obj.transform.parent;

                if (_parentKeys.Count > 0)
                {
                    var firstParentkey = _parentKeys[0];
                    var firstSavedParentObj = FindObject(firstParentkey);
                    savedParent = firstSavedParentObj == null ? null : firstSavedParentObj.transform;
                }
                else if (currentParent == null)
                    return false;
                savedSiblingIndex = (siblingPath != null && siblingPath.Length > 0) ? siblingPath.Last() : 0;
                foreach (var parentKey in _parentKeys)
                {
                    var savedParentObj = FindObject(parentKey);
                    var savedParentTransform = savedParentObj == null ? null : savedParentObj.transform;
                    if (savedParentTransform != currentParent) return true;
                    if (currentParent == null) break;
                    currentParent = currentParent.parent;
                }
                if (savedParent == null) return true;
                return false;
            }

            public override bool HierarchyEquals(ObjectDataKeyBase other)
            {
                var baseEquals = base.Equals(other);
                if (!baseEquals) return false;
                var otherKey = other as ObjectDataKey;
                if (otherKey == null) return baseEquals;
                return Enumerable.SequenceEqual(_parentKeys, otherKey._parentKeys);
            }
        }

        [System.Serializable]
        private class ComponentSaveDataKey : ISerializationCallbackReceiver, System.IEquatable<ComponentSaveDataKey>
        {
            [SerializeField] private ObjectDataKey _objkey = null;
            [SerializeField] private int _compId = -1;
            [SerializeField] private string _globalCompId = null;
            [SerializeField] private int _compIdx = -1;
            [SerializeField] private string _compTypeName = null;
            [SerializeField] private string _globalObjId = null;

            public ObjectDataKey objKey
            {
                get
                {
                    UpdateObjKey();
                    return _objkey;
                }
            }
            public int compId { get => _compId; set => _compId = value; }
            public string globalCompId { get => _globalCompId; set => _globalCompId = value; }
            public int compIdx => _compIdx;
            public System.Type compType => System.Type.GetType(_compTypeName, true);



            public ComponentSaveDataKey(Component component)
            {
                _objkey = new ObjectDataKey(component.gameObject);
                _compId = component.GetInstanceID();
                _globalCompId = UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(component).ToString();
                var compType = component.GetType();
                _compTypeName = compType.AssemblyQualifiedName;
                var comps = component.gameObject.GetComponents(compType);
                _compIdx = System.Array.IndexOf(comps, component);

            }

            public void UpdateObjKey()
            {
                var objKeyUninitialized = _objkey is null;
                if (!objKeyUninitialized)
                    if (string.IsNullOrEmpty(_objkey.globalObjId)) objKeyUninitialized = true;
                if (objKeyUninitialized && !string.IsNullOrEmpty(_globalObjId))
                {
                    if (UnityEditor.GlobalObjectId.TryParse(_globalObjId, out UnityEditor.GlobalObjectId id))
                    {
                        var obj = UnityEditor.GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as GameObject;
                        if (obj == null) return;
                        _objkey = new ObjectDataKey(obj);
                    }
                }
            }

            public void OnBeforeSerialize() { }
            public void OnAfterDeserialize()
            {
                UpdateObjKey();
                if (UnityEditor.GlobalObjectId.TryParse(_globalCompId, out UnityEditor.GlobalObjectId compId))
                {
                    var comp = UnityEditor.GlobalObjectId.GlobalObjectIdentifierToObjectSlow(compId) as Component;
                    if (comp == null) return;
                    _compId = comp.GetInstanceID();
                }
                PMSData.saveAfterLoading = true;
            }
            public override int GetHashCode() => (_objkey.globalObjId, _globalCompId).GetHashCode();
            public bool Equals(ComponentSaveDataKey other)
            {
                return (_objkey == other._objkey && (_globalCompId == other._globalCompId || _compId == other._compId));
            }

            public bool HierarchyEquals(ComponentSaveDataKey other)
            {
                return (_objkey.HierarchyEquals(other._objkey)
                    && (_globalCompId == other._globalCompId || _compId == other._compId));
            }

            public override bool Equals(object obj) => obj is ComponentSaveDataKey other && this.Equals(other);
            public static bool operator ==(ComponentSaveDataKey lhs, ComponentSaveDataKey rhs) => lhs.Equals(rhs);
            public static bool operator !=(ComponentSaveDataKey lhs, ComponentSaveDataKey rhs) => !lhs.Equals(rhs);
        }

        private class SaveDataValue
        {
            public UnityEditor.SerializedObject serializedObj;
            public SaveCommand saveCmd;
            public System.Type componentType;
            public int compId;
            public string globalCompId;
            public System.Collections.Generic.Dictionary<string, ObjectDataKeyBase> objectReferences = null;

            public SaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd, Component component)
            {
                this.serializedObj = serializedObj;

                this.saveCmd = saveCmd;
                this.componentType = component.GetType();
                compId = component.GetInstanceID();
                globalCompId = UnityEditor.GlobalObjectId.GetGlobalObjectIdSlow(component).ToString();
            }
            public virtual void Update(int componentId)
            {
                if (serializedObj.targetObject == null) return;
                serializedObj.Update();
            }
        }

        private class SpriteRendererSaveDataValue : SaveDataValue
        {
            public int sortingOrder;
            public int sortingLayerID;
            public SpriteRendererSaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd,
                Component component, int sortingOrder, int sortingLayerID) : base(serializedObj, saveCmd, component)
                => (this.sortingOrder, this.sortingLayerID) = (sortingOrder, sortingLayerID);

            public override void Update(int componentId)
            {
                base.Update(componentId);
                var spriteRenderer = UnityEditor.EditorUtility.InstanceIDToObject(componentId) as SpriteRenderer;
                sortingOrder = spriteRenderer.sortingOrder;
                sortingLayerID = spriteRenderer.sortingLayerID;
            }
        }

        private class SpriteShapeRendererSaveDataValue : SaveDataValue
        {
            public int sortingOrder;
            public int sortingLayerID;
            public SpriteShapeRendererSaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd,
                Component component, int sortingOrder, int sortingLayerID) : base(serializedObj, saveCmd, component)
                => (this.sortingOrder, this.sortingLayerID) = (sortingOrder, sortingLayerID);

            public override void Update(int componentId)
            {
                base.Update(componentId);
                var spriteRenderer = UnityEditor.EditorUtility.InstanceIDToObject(componentId)
                    as UnityEngine.U2D.SpriteShapeRenderer;
                sortingOrder = spriteRenderer.sortingOrder;
                sortingLayerID = spriteRenderer.sortingLayerID;
            }
        }

        private class ParticleSystemSaveDataValue : SaveDataValue
        {
            public int sortingOrder;
            public int sortingLayerID;
            public ParticleSystemSaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd,
                Component component, int sortingOrder, int sortingLayerID) : base(serializedObj, saveCmd, component)
                => (this.sortingOrder, this.sortingLayerID) = (sortingOrder, sortingLayerID);

            public override void Update(int componentId)
            {
                base.Update(componentId);
                var particleSystem = UnityEditor.EditorUtility.InstanceIDToObject(componentId) as ParticleSystem;
                var spriteRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                sortingOrder = spriteRenderer.sortingOrder;
                sortingLayerID = spriteRenderer.sortingLayerID;
            }
        }

        private class ClothSaveDataValue : SaveDataValue
        {
            public ClothSkinningCoefficient[] coefficients;
            public ClothSaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd, Component component,
                ClothSkinningCoefficient[] coefficients) : base(serializedObj, saveCmd, component)
                => (this.coefficients) = (coefficients);

            public override void Update(int componentId)
            {
                base.Update(componentId);
                var cloth = UnityEditor.EditorUtility.InstanceIDToObject(componentId) as Cloth;
                coefficients = cloth.coefficients.ToArray();
            }
        }

        private class SortingGroupSaveDataValue : SaveDataValue
        {
            public int sortingOrder;
            public int sortingLayerID;
            public SortingGroupSaveDataValue(UnityEditor.SerializedObject serializedObj, SaveCommand saveCmd,
                Component component, int sortingOrder, int sortingLayerID) : base(serializedObj, saveCmd, component)
                => (this.sortingOrder, this.sortingLayerID) = (sortingOrder, sortingLayerID);

            public override void Update(int componentId)
            {
                base.Update(componentId);
                var sortingGroup = UnityEditor.EditorUtility.InstanceIDToObject(componentId)
                    as UnityEngine.Rendering.SortingGroup;
                sortingOrder = sortingGroup.sortingOrder;
                sortingLayerID = sortingGroup.sortingLayerID;
            }
        }

        private class SaveObject
        {
            public SaveCommand saveCmd;
            public string name = null;
            public string tag = null;
            public int layer = 0;
            public bool isStatic = false;
            public bool always = false;
            public bool includeChildren = false;
            public System.Collections.Generic.List<SaveObject> saveChildren = null;
            public System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<SaveDataValue>>
                compDataDictionary
                = new System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<SaveDataValue>>();
            public bool isRoot = true;
            public ObjectDataKey parentKey;
            public string prefabPath = null;
            public int siblingIndex = -1;
            private GameObject _obj = null;
            public ObjectDataKey objKey;
            public bool unloadedScene = false;

            public bool objIsNull => _obj == null;
            public SaveObject(Transform transform, SaveCommand cmd, bool always, bool includeChildren)
            {
                this.always = always;
                this.includeChildren = includeChildren;
                _obj = transform.gameObject;
                var prefabRoot = UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(_obj);
                if (prefabRoot == _obj)
                    prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(_obj);
                name = _obj.name;
                tag = _obj.tag;
                layer = _obj.layer;
                isStatic = _obj.isStatic;
                isRoot = transform.parent == null;
                var parent = isRoot ? null : transform.parent.gameObject;
                parentKey = new ObjectDataKey(parent);
                objKey = new ObjectDataKey(transform.gameObject);
                siblingIndex = _obj.transform.GetSiblingIndex();
                saveCmd = cmd;
            }

            public System.Type[] types => compDataDictionary.Keys.ToArray();

            public void Update()
            {
                if (_obj == null) return;
                var components = _obj.GetComponents<Component>();
                foreach (var comp in components)
                {
                    var data = new UnityEditor.SerializedObject(comp);
                    var type = comp.GetType();
                    SaveDataValue saveData = null;
                    if (!compDataDictionary.ContainsKey(type))
                        compDataDictionary.Add(type, new System.Collections.Generic.List<SaveDataValue>());
                    if (comp is SpriteRenderer)
                    {
                        var renderer = comp as SpriteRenderer;
                        saveData = new SpriteRendererSaveDataValue(data, saveCmd, renderer,
                            renderer.sortingOrder, renderer.sortingLayerID);
                        compDataDictionary[type].Add(saveData);
                    }
                    else if (comp is UnityEngine.U2D.SpriteShapeRenderer)
                    {
                        var renderer = comp as UnityEngine.U2D.SpriteShapeRenderer;
                        saveData = new SpriteShapeRendererSaveDataValue(
                            data, saveCmd, renderer, renderer.sortingOrder, renderer.sortingLayerID);
                        compDataDictionary[type].Add(saveData);
                    }
                    else if (comp is ParticleSystem)
                    {
                        var particleSystem = comp as ParticleSystem;
                        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                        saveData = new ParticleSystemSaveDataValue(
                            data, saveCmd, renderer, renderer.sortingOrder, renderer.sortingLayerID);
                        compDataDictionary[type].Add(saveData);
                    }
                    else if (comp is Cloth)
                    {
                        saveData = new ClothSaveDataValue(
                            data, saveCmd, comp, (comp as Cloth).coefficients);
                        compDataDictionary[type].Add(saveData);
                    }
                    else if (comp is UnityEngine.Rendering.SortingGroup)
                    {
                        var sortingGroup = comp as UnityEngine.Rendering.SortingGroup;
                        saveData = new SortingGroupSaveDataValue(data, saveCmd, sortingGroup,
                            sortingGroup.sortingOrder, sortingGroup.sortingLayerID);
                        compDataDictionary[type].Add(saveData);
                    }
                    else
                    {
                        saveData = new SaveDataValue(data, saveCmd, comp);
                        compDataDictionary[type].Add(saveData);
                    }

                    CopyPropertiesFromComponent(comp, data, ref saveData.objectReferences);
                }
                var childCount = _obj.transform.childCount;
                if (childCount == 0) return;
                var children = new System.Collections.Generic.List<Transform>();
                for (int i = 0; i < childCount; ++i) children.Add(_obj.transform.GetChild(i));
                saveChildren = new System.Collections.Generic.List<SaveObject>();
                foreach (var child in children)
                {
                    if (child == _obj.transform) continue;
                    var saveChild = new SaveObject(child, saveCmd, always, includeChildren);
                    saveChildren.Add(saveChild);
                }
            }
        }

        private class FullObjectData
        {
            public ObjectDataKey key = null;
            public bool always = false;
            public FullObjectData(ObjectDataKey key, bool always) => (this.key, this.always) = (key, always);
        }

        private static bool CompDataContainsKey(ref ComponentSaveDataKey key, bool update = false)
        {
            foreach (var compKey in _compData.Keys)
            {
                if (compKey == key)
                {
                    if (update) UpdateCompKey(compKey, key);
                    key = compKey;
                    return true;
                }
            }
            return false;
        }

        private static void CompDataRemoveKey(ComponentSaveDataKey key)
        {
            if (CompDataContainsKey(ref key))
            {
                var dataClone = new System.Collections.Generic.Dictionary<ComponentSaveDataKey, SaveDataValue>();
                foreach (var compKey in _compData.Keys)
                {
                    if (compKey == key) continue;
                    if (dataClone.ContainsKey(compKey)) continue;
                    dataClone.Add(compKey, _compData[compKey]);
                }
                _compData = dataClone;
            }
        }

        private static void UpdateCompKey(ComponentSaveDataKey oldKey, ComponentSaveDataKey newKey)
        {
            oldKey.objKey.objId = newKey.objKey.objId;
            oldKey.objKey.globalObjId = newKey.objKey.globalObjId;
            oldKey.compId = newKey.compId;
            oldKey.globalCompId = newKey.globalCompId;
        }

        private static bool ObjectDataContainsKey(ref ObjectDataKey key, bool update = false)
        {
            foreach (var objKey in _objData.Keys)
            {
                if (objKey == key)
                {
                    if (update) UpdateObjectDataKey(objKey, key);
                    key = objKey;
                    return true;
                }
            }
            return false;
        }

        private static void UpdateObjectDataKey(ObjectDataKey oldKey, ObjectDataKey newKey)
        {
            oldKey.objId = newKey.objId;
            oldKey.globalObjId = newKey.globalObjId;
        }

        private static bool FullObjectDataContains(ObjectDataKey objKey, out FullObjectData foundItem)
        {
            foundItem = null;
            foreach (var item in _fullObjData)
            {
                if (objKey == item.key)
                {
                    foundItem = item;
                    return true;
                }
            }
            return false;
        }

        private static void AddFullObjectData(ObjectDataKey objKey, bool always)
        {
            if (FullObjectDataContains(objKey, out FullObjectData foundItem))
            {
                foundItem.key.Copy(objKey);
                foundItem.always = always;
                return;
            }
            _fullObjData.Add(new FullObjectData(objKey, always));
        }

        private static void RemoveFullObjectData(ObjectDataKey objKey)
        {
            if (FullObjectDataContains(objKey, out FullObjectData foundItem)) _fullObjData.Remove(foundItem);
        }

        private static bool WillBeDeleted(ObjectDataKey objKey, out ObjectDataKey foundKey)
        {
            foundKey = null;
            foreach (var toBeDeleted in _objectsToBeDeleted)
            {
                if (toBeDeleted == objKey)
                {
                    foundKey = toBeDeleted;
                    return true;
                }
            }
            return false;
        }

        private static void ToBeDeleted(ObjectDataKey objKey)
        {
            if (WillBeDeleted(objKey, out ObjectDataKey foundKey))
            {
                foundKey.Copy(objKey);
            }
            else _objectsToBeDeleted.Add(objKey);
        }
        private static void Add(Component component, SaveCommand cmd, bool always, bool serialize)
        {
            if (IsCombinedOrInstance(component)) return;
            var scenePath = component.gameObject.scene.path;
            if (!_scenesToOpen.Contains(scenePath)) _scenesToOpen.Add(scenePath);
            var key = new ComponentSaveDataKey(component);
            if (always) PMSData.AlwaysSave(key);
            var data = new UnityEditor.SerializedObject(component);
            SaveDataValue GetValue()
            {
                if (!serialize) return new SaveDataValue(null, cmd, component);
                if (component is SpriteRenderer)
                {
                    var renderer = component as SpriteRenderer;
                    return new SpriteRendererSaveDataValue(data, cmd, component, renderer.sortingOrder,
                        renderer.sortingLayerID);
                }
                else if (component is UnityEngine.U2D.SpriteShapeRenderer)
                {
                    var renderer = component as UnityEngine.U2D.SpriteShapeRenderer;
                    return new SpriteShapeRendererSaveDataValue(data, cmd, component, renderer.sortingOrder,
                        renderer.sortingLayerID);
                }
                else if (component is ParticleSystem)
                {
                    var particleSystem = component as ParticleSystem;
                    var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                    return new ParticleSystemSaveDataValue(data, cmd, component, renderer.sortingOrder,
                        renderer.sortingLayerID);
                }
                else if (component is Cloth)
                {
                    var cloth = component as Cloth;
                    return new ClothSaveDataValue(data, cmd, component, cloth.coefficients.ToArray());
                }
                else if (component is UnityEngine.Rendering.SortingGroup)
                {
                    var sortingGroup = component as UnityEngine.Rendering.SortingGroup;
                    return new SortingGroupSaveDataValue(data, cmd, component,
                        sortingGroup.sortingOrder, sortingGroup.sortingLayerID);
                }
                else return new SaveDataValue(data, cmd, component);
            }
            var saveObj = new SaveObject(component.transform, cmd, always, _includeChildren);
            saveObj.Update();

            var saveData = GetValue();
            if (CompDataContainsKey(ref key, true))
                _compData[key] = saveData;
            else
                _compData.Add(key, saveData);
            CopyPropertiesFromComponent(component, data, ref saveData.objectReferences);
            var objKey = saveObj.objKey;

            if (ObjectDataContainsKey(ref objKey, true))
            {
                _objData[objKey] = saveObj;
            }
            else _objData.Add(objKey, saveObj);
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }

        private static bool IsCombinedOrInstance(Component comp)
        {
            if (comp is MeshFilter)
            {
                var meshFilter = comp as MeshFilter;
                if (meshFilter.sharedMesh != null)
                    if (meshFilter.sharedMesh.name.Contains("Combined Mesh (root: scene)")
                        || meshFilter.sharedMesh.name.ToLower().Contains("instance")) return true;
            }
            else if (comp is Renderer)
            {
                var renderer = comp as Renderer;
                var materials = renderer.sharedMaterials;
                if (materials != null)
                    foreach (var mat in materials)
                        if (mat.name.ToLower().Contains("instance")) return true;
            }
            return false;
        }

        private static void AddAll()
        {
            var components = GameObject.FindObjectsOfType<Component>();
            foreach (var comp in components) Add(comp, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, false, true);
        }

        private static void Apply(ComponentSaveDataKey key)
        {
            if (CompDataContainsKey(ref key))
            {
                GameObject obj;
                var comp = FindComponent(key, out obj);
                if (obj != null && key.objKey.ParentHasChanged(out Transform savedParent, out int savedSiblingIndex))
                {
                    obj.transform.parent = savedParent;
                    obj.transform.SetSiblingIndex(savedSiblingIndex);
                }
                if (WillBeDeleted(key.objKey, out ObjectDataKey delFoundKey)) return;
                if (comp == null && obj != null && !_componentsToBeDeleted.Contains(key))
                {
                    comp = obj.AddComponent(_compData[key].componentType);
                    var objKey = key.objKey;
                    if (PMSData.ContainsFull(objKey, out ObjectDataKey objFoundKey)) PMSData.AlwaysSave(key);
                }
                if (comp == null) return;
                var value = _compData[key];
                var data = value.serializedObj;
                if (data == null) return;
                var objDatakeys = _objData.Keys.ToArray();
                foreach (var objKey in objDatakeys)
                    if (key.objKey == objKey) _objData.Remove(objKey);
                CopyPropertiesFromData(comp, data, value.objectReferences);
                if (value is SpriteRendererSaveDataValue)
                {
                    var rendererData = value as SpriteRendererSaveDataValue;
                    var renderer = comp as SpriteRenderer;
                    renderer.sortingOrder = rendererData.sortingOrder;
                    renderer.sortingLayerID = rendererData.sortingLayerID;
                }
                else if (value is SpriteShapeRendererSaveDataValue)
                {
                    var rendererData = value as SpriteShapeRendererSaveDataValue;
                    var renderer = comp as UnityEngine.U2D.SpriteShapeRenderer;
                    renderer.sortingOrder = rendererData.sortingOrder;
                    renderer.sortingLayerID = rendererData.sortingLayerID;
                }
                else if (value is ParticleSystemSaveDataValue)
                {
                    var rendererData = value as ParticleSystemSaveDataValue;
                    var particleSystem = comp as ParticleSystem;
                    var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                    renderer.sortingOrder = rendererData.sortingOrder;
                    renderer.sortingLayerID = rendererData.sortingLayerID;
                }
                else if (value is ClothSaveDataValue)
                {
                    var clothData = value as ClothSaveDataValue;
                    var cloth = comp as Cloth;
                    cloth.coefficients = clothData.coefficients;
                }
                else if (value is SortingGroupSaveDataValue)
                {
                    var sortingGroupData = value as SortingGroupSaveDataValue;
                    var sortingGroup = comp as UnityEngine.Rendering.SortingGroup;
                    sortingGroup.sortingOrder = sortingGroupData.sortingOrder;
                    sortingGroup.sortingLayerID = sortingGroupData.sortingLayerID;
                }
            }
        }

        private static void ApplyAll()
        {
            var comIds = _compData.Keys.ToArray();
            foreach (var id in comIds) Apply(id);
            if (Create())
                foreach (var id in comIds) Apply(id);
            foreach (var id in comIds)
                if (!PMSData.Contains(id, out ComponentSaveDataKey foundKey)) CompDataRemoveKey(id);
            Delete();
            _objectsToBeDeleted.Clear();
            _componentsToBeDeleted.Clear();
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }

        private static string[] GetLoadedScenePaths()
        {
            var countLoaded = UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount;
            var loadedScenePaths = new string[countLoaded];
            for (int i = 0; i < countLoaded; i++)
                loadedScenePaths[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path;
            return loadedScenePaths;
        }

        private static GameObject FindObject(ObjectDataKeyBase key, bool findInHierarchy = true)
        {
            var obj = UnityEditor.EditorUtility.InstanceIDToObject(key.objId) as GameObject;
            if (obj != null) return obj;
            if (string.IsNullOrEmpty(key.globalObjId)) return null;
            if (UnityEditor.GlobalObjectId.TryParse(key.globalObjId, out UnityEditor.GlobalObjectId id))
            {
                obj = UnityEditor.GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as GameObject;
                if (obj != null)
                {
                    key.objId = obj.GetInstanceID();
                    return obj;
                }
            }
            if (!findInHierarchy) return null;
            var sibPath = key.siblingPath;

            GameObject FindInScene(UnityEngine.SceneManagement.Scene targetScene)
            {
                GameObject obInScene = null;
                var rootObjs = targetScene.GetRootGameObjects();
                if (rootObjs.Length <= sibPath[0]) return null;
                var rootObj = rootObjs[sibPath[0]];
                var childTrans = rootObj.transform;
                if (childTrans.name != key.rootName) return null;
                if (sibPath.Length == 1) obInScene = childTrans.gameObject;
                else
                {
                    for (var depth = 1; depth < sibPath.Length; ++depth)
                    {
                        if (sibPath[depth] >= childTrans.childCount) return null;
                        childTrans = childTrans.GetChild(sibPath[depth]);
                        if (childTrans == null) return null;
                    }
                    obInScene = childTrans.gameObject;
                }
                return obInScene;
            }

            if (key.scenePath == "DontDestroyOnLoad")
            {
                for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; ++i)
                {
                    var loadedScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                    var ddol_obj = FindInScene(loadedScene);
                    if (ddol_obj != null) return ddol_obj;
                }
                return null;
            }
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(key.scenePath);
            if (!scene.IsValid()) return null;
            return FindInScene(scene);
        }

        private static Component FindComponent(ComponentSaveDataKey key, out GameObject obj)
        {
            if (string.IsNullOrEmpty(key.objKey.globalObjId)) key.UpdateObjKey();
            obj = FindObject(key.objKey);
            if (obj == null) return null;
            var comp = UnityEditor.EditorUtility.InstanceIDToObject(key.compId) as Component;
            if (comp == null)
            {
                if (UnityEditor.GlobalObjectId.TryParse(key.globalCompId, out UnityEditor.GlobalObjectId id))
                {
                    comp = UnityEditor.GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id) as Component;
                    if (comp != null)
                        if (comp.GetType() != key.compType) comp = null;
                        else key.compId = comp.GetInstanceID();
                }
            }
            if (comp == null)
            {
                var type = key.compType;
                var comps = obj.GetComponents(type);
                if (comps.Length <= key.compIdx) return null;
                comp = comps[key.compIdx];
                if (comp.name != key.objKey.objName) return null;
            }
            return comp;
        }

        private static bool Create()
        {
            GameObject CreateObj(SaveObject saveObj, Transform parent, string scenePath)
            {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(scenePath);
                if (scene.IsValid()) UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                else return null;
                GameObject obj = saveObj.prefabPath != null
                        ? (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab
                        (UnityEditor.AssetDatabase.LoadMainAssetAtPath(saveObj.prefabPath))
                        : new GameObject(saveObj.name);
                obj.name = saveObj.name;
                UnityEditor.Undo.RegisterCreatedObjectUndo(obj, "Save Object Created In Play Mode");
                obj.transform.parent = parent;
                obj.isStatic = saveObj.isStatic;
                obj.tag = saveObj.tag;
                obj.layer = saveObj.layer;
                obj.transform.SetSiblingIndex(saveObj.siblingIndex);

                var compToRemove = obj.GetComponents<Component>().ToList();
                foreach (var type in saveObj.compDataDictionary.Keys)
                {
                    var compDataList = saveObj.compDataDictionary[type];
                    var components = obj.GetComponents(type);
                    for (int i = 0; i < compDataList.Count; ++i)
                    {
                        var compData = compDataList[i];
                        var comp = obj.GetComponent(type);
                        if (comp == null || components.Length < i + 1) comp = obj.AddComponent(type);
                        if (components.Length > i) comp = components[i];
                        if (compToRemove.Contains(comp)) compToRemove.Remove(comp);
                        CopyPropertiesFromData(comp, compData.serializedObj, compData.objectReferences);
                        if (compData is ClothSaveDataValue)
                        {
                            var clothData = compData as ClothSaveDataValue;
                            var cloth = comp as Cloth;
                            cloth.coefficients = clothData.coefficients.ToArray();
                        }
                    }
                }
                if (saveObj.compDataDictionary.Count > 0)
                    foreach (var comp in compToRemove) DestroyImmediate(comp);
                if (saveObj.always)
                {
                    var createdComponents = obj.GetComponentsInChildren<Component>();
                    foreach (var comp in createdComponents) Add(comp, SaveCommand.SAVE_NOW, true, saveObj.includeChildren);
                }

                if (!saveObj.includeChildren || saveObj.saveChildren == null) return obj;
                foreach (var child in saveObj.saveChildren)
                {
                    var prefabChild = obj.transform.Find(child.name);
                    if (_objData.ContainsKey(child.objKey)) continue;
                    if (prefabChild != null && prefabChild.transform.GetSiblingIndex() == child.siblingIndex) continue;
                    CreateObj(child, obj.transform, scenePath);
                }
                return obj;
            }

            var objDataClone = new System.Collections.Generic.Dictionary<ObjectDataKey, SaveObject>();
            foreach (var key in _objData.Keys)
            {
                if (objDataClone.ContainsKey(key)) continue;
                objDataClone.Add(key, _objData[key]);
            }
            bool objCreated = false;

            bool IsANestedPrefab(SaveObject saveObj)
            {
                if (saveObj.prefabPath == null) return false;
                if (saveObj.parentKey.isNull) return false;
                if (!objDataClone.ContainsKey(saveObj.parentKey)) return false;
                var parent = objDataClone[saveObj.parentKey];
                return parent.prefabPath != null;
            }

            foreach (var key in objDataClone.Keys)
            {
                if (WillBeDeleted(key, out ObjectDataKey TBDFoundKey)) continue;
                var root = FindObject(key, false);
                var data = objDataClone[key];
                if (root != null) continue;
                Transform rootParent = null;
                if (!data.isRoot)
                {
                    var rootParentObj = FindObject(data.parentKey);
                    if (rootParentObj == null) continue;
                    rootParent = rootParentObj.transform;
                }
                if (!IsANestedPrefab(data))
                    CreateObj(data, rootParent, key.scenePath);
                objCreated = true;
            }
            _objData.Clear();
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
            return objCreated;
        }

        private static void Delete()
        {
            foreach (var objItem in _objectsToBeDeleted)
            {
                var obj = FindObject(objItem);
                if (obj == null) return;
                UnityEditor.Undo.DestroyObjectImmediate(obj);
            }
            foreach (var item in _componentsToBeDeleted)
            {
                GameObject obj = null;
                var comp = FindComponent(item, out obj);
                if (comp == null) continue;
                UnityEditor.Undo.DestroyObjectImmediate(comp);
            }
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }

        private static void UpdateFullObjects()
        {
            PMSData.Load();
            var alwaysSaveFull = PMSData.alwaysSaveFullArray;
            foreach (var item in alwaysSaveFull)
            {
                var obj = FindObject(item);
                if (obj == null)
                {
                    PMSData.RemoveFull(item);
                    continue;
                }
                var objKey = new ObjectDataKey(obj);
                var components = _includeChildren ? obj.GetComponentsInChildren<Component>() : obj.GetComponents<Component>();
                foreach (var comp in components) Add(comp, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, true, true);
                AddFullObjectData(objKey, true);
            }
        }

        private static void LoadData()
        {
            if (!PMSData.Load()) return;
            foreach (var key in PMSData.alwaysSaveArray)
            {
                GameObject obj = null;
                var comp = FindComponent(key, out obj);
                if (obj == null || comp == null)
                {
                    if (string.IsNullOrEmpty(UnityEditor.AssetDatabase.AssetPathToGUID(key.objKey.scenePath)))
                        PMSData.Remove(key);
                    else
                    {
                        var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                        if (activeScene.path == key.objKey.scenePath) PMSData.Remove(key);
                    }
                    continue;
                }
                Add(comp, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, true, Application.isPlaying);
            }
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }

        private static void UpdateObjKeys()
        {
            if (!PMSData.Load(false)) return;
            foreach (var key in PMSData.alwaysSaveArray) key.UpdateObjKey();
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
        }

        [UnityEditor.InitializeOnLoad]
        private static class ApplicationEventHandler
        {
            private static GameObject autoApplyFlag = null;
            private const string AUTO_APPLY_OBJECT_NAME = "PlayModeSave_AutoApply";
            private static Texture2D _icon = Resources.Load<Texture2D>("Save");
            private static string _currentScenePath = null;
            private static bool _loadData = true;
            private static bool _refreshDataBase = true;

            static ApplicationEventHandler()
            {
                UnityEditor.EditorApplication.playModeStateChanged += OnStateChanged;
                UnityEditor.EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCallback;
                UnityEditor.EditorApplication.hierarchyChanged += OnHierarchyChanged;
                LoadPrefs();
                UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpened;
                UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
                UpdateObjKeys();
                UnityEditor.EditorApplication.RepaintHierarchyWindow();
            }

            static void OnHierarchyChanged()
            {
                var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                string activeScenePath = null;
                if (activeScene != null) activeScenePath = activeScene.path;
                if (!_loadData || _currentScenePath == activeScenePath) return;
                if (_currentScenePath != activeScenePath) _currentScenePath = activeScenePath;
                LoadData();
            }

            static private void OnSceneOpened(UnityEngine.SceneManagement.Scene scene,
                UnityEditor.SceneManagement.OpenSceneMode mode)
            {
                if (!_openingScenes)
                {
                    _scenesToOpen.Clear();
                    return;
                }
                _scenesToOpen.Remove(scene.path);
                if (_scenesToOpen.Count > 0) return;
                _openingScenes = false;
                if (_autoApply) PlayModeSave.ApplyAll();
            }

            static private void OnSceneUnloaded(UnityEngine.SceneManagement.Scene scene)
            {
                if (!Application.isPlaying) return;
                foreach (var data in _objData.Values)
                    if (data.objKey.scenePath == scene.path) data.unloadedScene = true;
            }
            private static void OnStateChanged(UnityEditor.PlayModeStateChange state)
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    PMSData.UpdateFull();
                    if (_autoApply)
                    {
                        autoApplyFlag = new GameObject(AUTO_APPLY_OBJECT_NAME);
                        autoApplyFlag.hideFlags = HideFlags.HideAndDontSave;
                    }
                    return;
                }
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                {
                    if (_saveEntireHierarchy) AddAll();
                    foreach (var data in _compData)
                    {
                        data.Key.objKey.UpdateParentKeys();
                        if (data.Value.saveCmd == SaveCommand.SAVE_NOW) continue;
                        if (data.Value.serializedObj.targetObject == null) _componentsToBeDeleted.Add(data.Key);
                        else data.Value.Update(data.Key.compId);
                    }

                    var objDataClone = _objData.ToDictionary(entry => entry.Key, entry => entry.Value);

                    foreach (var key in objDataClone.Keys)
                    {
                        key.UpdateParentKeys();
                        var data = objDataClone[key];
                        if (data.objIsNull)
                        {
                            if (!data.unloadedScene) ToBeDeleted(key);
                            continue;
                        }
                        if (data.saveCmd == SaveCommand.SAVE_NOW) continue;

                        if (FullObjectDataContains(key, out FullObjectData foundItem))
                        {
                            var obj = FindObject(key);
                            if (obj == null)
                            {
                                var objKey = foundItem;
                                ToBeDeleted(key);
                                RemoveFullObjectData(key);
                                continue;
                            }
                            var components = obj.GetComponents<Component>();

                            foreach (var comp in components)
                                Add(comp, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, data.always, true);
                            if (!data.always) RemoveFullObjectData(key);
                            if (_includeChildren)
                            {
                                var children = obj.GetComponentsInChildren<Transform>();
                                foreach (var child in children)
                                {
                                    var childObj = child.gameObject;
                                    if (childObj == obj) continue;
                                    var childComps = childObj.GetComponentsInChildren<Component>();
                                    foreach (var childComp in childComps)
                                    {
                                        Add(childComp, SaveCommand.SAVE_ON_EXITING_PLAY_MODE, data.always, true);
                                    }
                                    var objKey = new ObjectDataKey(childObj);
                                    AddFullObjectData(objKey, true);
                                    PMSData.AlwaysSaveFull(objKey);
                                }
                            }
                        }
                    }
                    return;
                }
                if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode)
                {
                    _scenesToOpen.Clear();
                    _componentsToBeDeleted.Clear();
                    UpdateFullObjects();
                    return;
                }
                if (state == UnityEditor.PlayModeStateChange.EnteredEditMode)
                {
                    _showAplyButtons = true;
                    var openedSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
                    var openedScenes = new System.Collections.Generic.List<string>();
                    for (int i = 0; i < openedSceneCount; ++i)
                        openedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path);
                    bool applyAll = true;
                    _openingScenes = false;
                    var scenesToOpen = _scenesToOpen.Where(
                        scenePath => openedScenes.Contains(scenePath) || scenePath == "DontDestroyOnLoad").ToArray();
                    scenesToOpen = _scenesToOpen.ToArray();
                    foreach (var scenePath in scenesToOpen)
                    {
                        applyAll = false;
                        _openingScenes = true;
                        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath,
                            UnityEditor.SceneManagement.OpenSceneMode.Additive);
                    }
                    autoApplyFlag = GameObject.Find(AUTO_APPLY_OBJECT_NAME);
                    _autoApply = autoApplyFlag != null;
                    if (_autoApply)
                    {
                        DestroyImmediate(autoApplyFlag);
                        if (applyAll) PlayModeSave.ApplyAll();
                    }
                    _loadData = false;
                }
            }

            private static void HierarchyItemCallback(int instanceID, Rect selectionRect)
            {
                if (_refreshDataBase)
                {
                    UnityEditor.AssetDatabase.Refresh();
                    _refreshDataBase = false;
                }
                var data = _compData;
                var keys = _compData.Keys.Where(k => k.objKey.objId == instanceID).ToArray();
                if (keys.Length == 0) return;
                if (_icon == null) _icon = Resources.Load<Texture2D>("Save");
                var rect = new Rect(selectionRect.xMax - 10, selectionRect.y + 2, 11, 11);
                GUI.Box(rect, _icon, GUIStyle.none);
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0
                    && rect.Contains(Event.current.mousePosition))
                {
                    var obj = UnityEditor.EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                    var compNames = obj.name + " components to save: ";
                    foreach (var key in keys)
                    {
                        if (key.objKey.objId != instanceID) continue;
                        var comp = UnityEditor.EditorUtility.InstanceIDToObject(key.compId) as Component;
                        if (comp == null) continue;
                        compNames += comp.GetType().Name + ", ";
                    }
                    compNames = compNames.Remove(compNames.Length - 2);
                    Debug.Log(compNames);
                }
                UnityEditor.EditorApplication.RepaintHierarchyWindow();
            }
        }
        #endregion
    }
}