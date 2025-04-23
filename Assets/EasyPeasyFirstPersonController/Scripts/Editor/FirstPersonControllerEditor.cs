using UnityEditor;
using UnityEngine;

namespace EasyPeasyFirstPersonController
{
    [CustomEditor(typeof(FirstPersonController))]
    public class FirstPersonControllerEditor : Editor
    {
        private SerializedProperty mouseSensitivity;
        private SerializedProperty snappiness;
        private SerializedProperty moveSpeed;
        private SerializedProperty walkSpeed;
        private SerializedProperty sprintSpeed;
        private SerializedProperty crouchSpeed;
        private SerializedProperty crouchHeight;
        private SerializedProperty crouchCameraHeight;
        private SerializedProperty slideSpeed;
        private SerializedProperty slideDuration;
        private SerializedProperty slideFovBoost;
        private SerializedProperty slideTiltAngle;
        private SerializedProperty gravity;
        private SerializedProperty jumpHeight;
        private SerializedProperty airControl;
        private SerializedProperty coyoteTimeEnabled;
        private SerializedProperty coyoteTimeDuration;
        private SerializedProperty normalFov;
        private SerializedProperty sprintFov;
        private SerializedProperty fovChangeSpeed;
        private SerializedProperty bobAmount;
        private SerializedProperty bobSpeed;
        private SerializedProperty canSlide;
        private SerializedProperty canJump;
        private SerializedProperty canSprint;
        private SerializedProperty canCrouch;
        private SerializedProperty groundCheck;
        private SerializedProperty groundDistance;
        private SerializedProperty groundMask;
        private SerializedProperty playerCamera;
        private SerializedProperty cameraParent;

        private bool showMovementSettings = true;
        private bool showAbilitySettings = true;
        private bool showCrouchSettings = true;
        private bool showSlideSettings = true;
        private bool showJumpSettings = true;
        private bool showHeadBobSettings = true;
        private bool showCameraSettings = true;
        private bool showPhysicsSettings = true;
        private bool showReferences = true;

        private static GUIStyle _headerStyle;
        private static GUIStyle HeaderStyle
        {
            get
            {
                if (_headerStyle == null)
                {
                    _headerStyle = new GUIStyle(EditorStyles.foldout)
                    {
                        fontStyle = FontStyle.Bold,
                        fontSize = 12,
                        padding = new RectOffset(15, 5, 2, 2),
                        margin = new RectOffset(5, 5, 5, 5)
                    };
                }
                return _headerStyle;
            }
        }

        private static GUIStyle _titleStyle;
        private static GUIStyle TitleStyle
        {
            get
            {
                if (_titleStyle == null)
                {
                    _titleStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = 13,
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleCenter,
                        margin = new RectOffset(0, 0, 5, 5)
                    };
                }
                return _titleStyle;
            }
        }

        private void OnEnable()
        {
            mouseSensitivity = serializedObject.FindProperty("mouseSensitivity");
            snappiness = serializedObject.FindProperty("snappiness");
            moveSpeed = serializedObject.FindProperty("moveSpeed");
            walkSpeed = serializedObject.FindProperty("walkSpeed");
            sprintSpeed = serializedObject.FindProperty("sprintSpeed");
            crouchSpeed = serializedObject.FindProperty("crouchSpeed");
            crouchHeight = serializedObject.FindProperty("crouchHeight");
            crouchCameraHeight = serializedObject.FindProperty("crouchCameraHeight");
            slideSpeed = serializedObject.FindProperty("slideSpeed");
            slideDuration = serializedObject.FindProperty("slideDuration");
            slideFovBoost = serializedObject.FindProperty("slideFovBoost");
            slideTiltAngle = serializedObject.FindProperty("slideTiltAngle");
            gravity = serializedObject.FindProperty("gravity");
            jumpHeight = serializedObject.FindProperty("jumpHeight");
            airControl = serializedObject.FindProperty("airControl");
            coyoteTimeEnabled = serializedObject.FindProperty("coyoteTimeEnabled");
            coyoteTimeDuration = serializedObject.FindProperty("coyoteTimeDuration");
            normalFov = serializedObject.FindProperty("normalFov");
            sprintFov = serializedObject.FindProperty("sprintFov");
            fovChangeSpeed = serializedObject.FindProperty("fovChangeSpeed");
            bobAmount = serializedObject.FindProperty("bobAmount");
            bobSpeed = serializedObject.FindProperty("bobSpeed");
            canSlide = serializedObject.FindProperty("canSlide");
            canJump = serializedObject.FindProperty("canJump");
            canSprint = serializedObject.FindProperty("canSprint");
            canCrouch = serializedObject.FindProperty("canCrouch");
            groundCheck = serializedObject.FindProperty("groundCheck");
            groundDistance = serializedObject.FindProperty("groundDistance");
            groundMask = serializedObject.FindProperty("groundMask");
            playerCamera = serializedObject.FindProperty("playerCamera");
            cameraParent = serializedObject.FindProperty("cameraParent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Title
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Easy First Person Controller", TitleStyle, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            // Movement Settings
            DrawSection("Movement Settings", ref showMovementSettings, () =>
            {
                EditorGUILayout.PropertyField(moveSpeed);
                EditorGUILayout.PropertyField(walkSpeed);
                EditorGUILayout.PropertyField(sprintSpeed);
                EditorGUILayout.PropertyField(crouchSpeed);
                EditorGUILayout.PropertyField(airControl);
            });

            // Ability Settings
            DrawSection("Ability Settings", ref showAbilitySettings, () =>
            {
                EditorGUILayout.PropertyField(canSlide);
                EditorGUILayout.PropertyField(canJump);
                EditorGUILayout.PropertyField(canSprint);
                EditorGUILayout.PropertyField(canCrouch);
            });

            // Crouch Settings
            DrawSection("Crouch Settings", ref showCrouchSettings, () =>
            {
                EditorGUILayout.PropertyField(crouchHeight);
                EditorGUILayout.PropertyField(crouchCameraHeight);
            });

            // Slide Settings
            DrawSection("Slide Settings", ref showSlideSettings, () =>
            {
                EditorGUILayout.PropertyField(slideSpeed);
                EditorGUILayout.PropertyField(slideDuration);
                EditorGUILayout.PropertyField(slideFovBoost);
                EditorGUILayout.PropertyField(slideTiltAngle);
            });

            // Jump Settings
            DrawSection("Jump Settings", ref showJumpSettings, () =>
            {
                EditorGUILayout.PropertyField(jumpHeight);
                EditorGUILayout.PropertyField(coyoteTimeEnabled);
                if (coyoteTimeEnabled.boolValue)
                {
                    EditorGUILayout.PropertyField(coyoteTimeDuration);
                }
            });

            // HeadBob Settings
            DrawSection("HeadBob Settings", ref showHeadBobSettings, () =>
            {
                EditorGUILayout.PropertyField(bobAmount);
                EditorGUILayout.PropertyField(bobSpeed);
            });

            // Camera Settings
            DrawSection("Camera Settings", ref showCameraSettings, () =>
            {
                EditorGUILayout.PropertyField(mouseSensitivity);
                EditorGUILayout.PropertyField(snappiness);
                EditorGUILayout.PropertyField(normalFov);
                EditorGUILayout.PropertyField(sprintFov);
                EditorGUILayout.PropertyField(fovChangeSpeed);
            });

            // Physics Settings
            DrawSection("Physics Settings", ref showPhysicsSettings, () =>
            {
                EditorGUILayout.PropertyField(gravity);
                EditorGUILayout.PropertyField(groundDistance);
                EditorGUILayout.PropertyField(groundMask);
            });

            // References
            DrawSection("References", ref showReferences, () =>
            {
                EditorGUILayout.PropertyField(groundCheck);
                EditorGUILayout.PropertyField(playerCamera);
                EditorGUILayout.PropertyField(cameraParent);
            });

            // Check references (no UI warnings)
            CheckReference(playerCamera, "Player Camera");
            CheckReference(cameraParent, "Camera Parent");
            CheckReference(groundCheck, "Ground Check");

            serializedObject.ApplyModifiedProperties();
        }

        private void CheckReference(SerializedProperty property, string name)
        {
            if (property == null)
            {
                return;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                return;
            }

            try
            {
                UnityEngine.Object reference = property.objectReferenceValue;
                string referenceInfo = reference != null ? $"{reference.GetType().Name} (InstanceID: {reference.GetInstanceID()})" : "null";
            }
            catch { }
        }

        private void DrawSection(string title, ref bool foldout, System.Action drawContent)
        {
            EditorGUILayout.BeginVertical();
            foldout = EditorGUILayout.Foldout(foldout, title, true, HeaderStyle);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                drawContent();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
    }
}