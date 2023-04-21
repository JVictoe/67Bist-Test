#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MTAssets.MobileInputControls
{
    /*
     This class is responsible for the functioning of the "Touch Area" component, and all its functions.
    */
    /*
     * The Mobile Input Controls was developed by Marcos Tomaz in 2019.
     * Need help? Contact me (mtassets@windsoft.xyz)
     */

    public class DragArea : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        [HideInInspector]
        public float speedMultiplier = 1;
        [HideInInspector]
        public bool pressed = false;
        [HideInInspector]
        public int fingerCount = 0;
        [HideInInspector]
        public int currentFingerId = -1;
        [HideInInspector]
        public Vector2 positionDeltaVector;
        [HideInInspector]
        public UnityEvent onTouchFirstTime;
        [HideInInspector]
        public Vector2 currentFingerPosition;

#if UNITY_EDITOR
        //Public variables of Interface
        private bool gizmosOfThisComponentIsDisabled = false;

        //The UI of this component
        #region INTERFACE_CODE
        [UnityEditor.CustomEditor(typeof(DragArea))]
        public class CustomInspector : UnityEditor.Editor
        {
            public bool DisableGizmosInSceneView(string scriptClassNameToDisable, bool isGizmosDisabled)
            {
                /*
                *  This method disables Gizmos in scene view, for this component
                */

                if (isGizmosDisabled == true)
                    return true;

                //Try to disable
                try
                {
                    //Get all data of Unity Gizmos manager window
                    var Annotation = System.Type.GetType("UnityEditor.Annotation, UnityEditor");
                    var ClassId = Annotation.GetField("classID");
                    var ScriptClass = Annotation.GetField("scriptClass");
                    var Flags = Annotation.GetField("flags");
                    var IconEnabled = Annotation.GetField("iconEnabled");

                    System.Type AnnotationUtility = System.Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
                    var GetAnnotations = AnnotationUtility.GetMethod("GetAnnotations", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    var SetIconEnabled = AnnotationUtility.GetMethod("SetIconEnabled", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    //Scann all Gizmos of Unity, of this project
                    System.Array annotations = (System.Array)GetAnnotations.Invoke(null, null);
                    foreach (var a in annotations)
                    {
                        int classId = (int)ClassId.GetValue(a);
                        string scriptClass = (string)ScriptClass.GetValue(a);
                        int flags = (int)Flags.GetValue(a);
                        int iconEnabled = (int)IconEnabled.GetValue(a);

                        // this is done to ignore any built in types
                        if (string.IsNullOrEmpty(scriptClass))
                        {
                            continue;
                        }

                        const int HasIcon = 1;
                        bool hasIconFlag = (flags & HasIcon) == HasIcon;

                        //If the current gizmo is of the class desired, disable the gizmo in scene
                        if (scriptClass == scriptClassNameToDisable)
                        {
                            if (hasIconFlag && (iconEnabled != 0))
                            {
                                /*UnityEngine.Debug.LogWarning(string.Format("Script:'{0}' is not ment to show its icon in the scene view and will auto hide now. " +
                                    "Icon auto hide is checked on script recompile, if you'd like to change this please remove it from the config", scriptClass));*/
                                SetIconEnabled.Invoke(null, new object[] { classId, scriptClass, 0 });
                            }
                        }
                    }

                    return true;
                }
                //Catch any error
                catch (System.Exception exception)
                {
                    string exceptionOcurred = "";
                    exceptionOcurred = exception.Message;
                    if (exceptionOcurred != null)
                        exceptionOcurred = "";
                    return false;
                }
            }

            public Rect GetInspectorWindowSize()
            {
                //Returns the current size of inspector window
                return EditorGUILayout.GetControlRect(true, 0f);
            }

            public override void OnInspectorGUI()
            {
                //Start the undo event support, draw default inspector and monitor of changes
                DragArea script = (DragArea)target;
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(target, "Undo Event");
                script.gizmosOfThisComponentIsDisabled = DisableGizmosInSceneView("DragArea", script.gizmosOfThisComponentIsDisabled);

                //Support reminder
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Remember to read the Mobile Input Controls documentation to understand how to use it.\nGet support at: mtassets@windsoft.xyz", MessageType.None);

                GUILayout.Space(10);

                //Verify if exists the sprites, if not exists, create them
                Image touchImage = script.GetComponent<Image>();
                if (touchImage == null)
                {
                    Image image = script.gameObject.AddComponent<Image>();
                    image.color = Color.clear;
                    UnityEditorInternal.ComponentUtility.MoveComponentDown(script);
                    UnityEditorInternal.ComponentUtility.MoveComponentDown(script);
                }

                //Start of settings
                EditorGUILayout.LabelField("Drag Area Settings", EditorStyles.boldLabel);
                GUILayout.Space(10);

                script.speedMultiplier = EditorGUILayout.Slider(new GUIContent("Speed Multiplier",
                                "The Drag Area component will multiply the moves made by the player by this number. The higher the values here, the faster the Drag Area will interpret the player's moves."),
                                script.speedMultiplier, 0.01f, 100f);

                GUILayout.Space(10);
                EditorGUILayout.LabelField("Drag Area Events", EditorStyles.boldLabel);
                GUILayout.Space(10);
                DrawDefaultInspector();

                GUILayout.Space(10);
                EditorGUILayout.LabelField("Drag Area Debug", EditorStyles.boldLabel);
                GUILayout.Space(10);

                script.positionDeltaVector.x = EditorGUILayout.FloatField(new GUIContent("Position Delta Vector X",
                    "This is the current X-axis (Horizontal) value of delta position in X."),
                    script.positionDeltaVector.x);

                script.positionDeltaVector.y = EditorGUILayout.FloatField(new GUIContent("Position Delta Vector Y",
                    "This is the current Y-axis (Vertical) value of delta position in Y."),
                    script.positionDeltaVector.y);

                script.fingerCount = EditorGUILayout.IntField(new GUIContent("Finger Count",
                    "Number of fingers touching, in this Drag Area."),
                    script.fingerCount);

                script.currentFingerId = EditorGUILayout.IntField(new GUIContent("Current Finger ID",
                   "ID of the finger on this Drag Area"),
                   script.currentFingerId);

                script.pressed = EditorGUILayout.Toggle(new GUIContent("Drag Area Pressed",
                        "Is the Drag Area pressed now?"),
                        script.pressed);

                script.currentFingerPosition.x = EditorGUILayout.FloatField(new GUIContent("Current Finger Pos. X",
                    "This is the current X-axis (Horizontal) value of finger position in X."),
                    script.currentFingerPosition.x);

                script.currentFingerPosition.y = EditorGUILayout.FloatField(new GUIContent("Current Finger Pos. Y",
                    "This is the current Y-axis (Vertical) value of finger position in Y."),
                    script.currentFingerPosition.y);

                //Final space
                GUILayout.Space(10);

                //Apply changes on script, case is not playing in editor
                if (GUI.changed == true && Application.isPlaying == false)
                {
                    EditorUtility.SetDirty(script);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
                }
                if (EditorGUI.EndChangeCheck() == true)
                {

                }
            }
        }
        #endregion
#endif

        public void Update()
        {
            //If has more than 1 finger in drag, clear the vector
            if (fingerCount > 1)
            {
                positionDeltaVector = Vector2.zero;
                currentFingerPosition = Vector2.zero;
                return;
            }

            //Detect if the finger is stopped drag for clear the vector
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.fingerId == currentFingerId)
                    {
                        if (touch.phase == TouchPhase.Stationary)
                        {
                            positionDeltaVector = Vector2.zero;
                        }
                    }
                }
            }
        }

        public virtual void OnDrag(PointerEventData pointerEventData)
        {
            //On drag the finger
            pressed = true;
            if (fingerCount == 1)
            {
                positionDeltaVector.x = pointerEventData.delta.x * speedMultiplier;
                positionDeltaVector.y = pointerEventData.delta.y * speedMultiplier;
                currentFingerPosition = pointerEventData.position;
            }
        }

        public virtual void OnPointerDown(PointerEventData pointerEventData)
        {
            //On pointer down
            fingerCount += 1;
            currentFingerId = pointerEventData.pointerId;
            currentFingerPosition = pointerEventData.position;
            OnDrag(pointerEventData);

            //Call event
            if (Application.isPlaying == true && onTouchFirstTime != null)
                onTouchFirstTime.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData pointerEventData)
        {
            //On pointer up
            pressed = false;
            positionDeltaVector = Vector2.zero;
            currentFingerPosition = Vector2.zero;
            fingerCount = 0;
            currentFingerId = -1;
        }
    }
}