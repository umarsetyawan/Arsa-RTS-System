using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using Arsa.CustomInspector.Field;

namespace Arsa.RTSSystem.Player.Controll
{
    public class CameraController : MonoBehaviour
    {
        [Header("Setup")]
        [Space(10)]
        [SerializeField] private InputActionAsset m_actionAsset;
        [Space(15)]
        [Header("Settings")]
        [Space(10)]
        [SerializeField] private MovementType m_cameraMovementType;
        [Space(2)]
        [SerializeField] private float m_cameraSpeedMultiplier;
        private float m_edgeDistance;

        private enum MovementType
        {
            None, Keyboard, EdgeScrolling
        }

        private Camera _camera;
        private InputAction _action;


        private void Start()
        {
            Init();
        }


        #region Init
        private void Init()
        {
            if (!_camera)
            {
                _camera = Camera.main;
                if (!_camera)
                {
                    Debug.LogWarning(string.Format("Cannot Find Camera in Scene"));
                    Debug.Break();
                    return;
                }
            }

            if (!m_actionAsset)
            {
                Debug.LogWarning(string.Format("Cannot Find Input Asset, Some Feature May Not Work Properly"));
                Debug.Break();
                return;
            }

            _action = m_actionAsset.FindAction("MoveCamera");

            m_actionAsset.Enable();

        }
        #endregion


        private void Update()
        {
            switch (m_cameraMovementType)
            {
                case MovementType.None:
                    break;
                case MovementType.Keyboard:
                    MoveCamera(_action.ReadValue<Vector2>());
                    break;
                case MovementType.EdgeScrolling:
                    EdgeScroll();
                    break;
                default:
                    break;
            }
        }

        private void EdgeScroll()
        {
            Vector2 c_screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 c_mousePosition = Mouse.current.position.ReadValue();

            //Horizontal
            if (c_mousePosition.x >= c_screenSize.x - m_edgeDistance)
            {
                MoveCamera(Vector2.right);
            }
            if (c_mousePosition.x <= m_edgeDistance)
            {
                MoveCamera(Vector2.left);
            }

            //Vertical
            if (c_mousePosition.y >= c_screenSize.y - m_edgeDistance)
            {
                MoveCamera(Vector2.up);
            }
            if (c_mousePosition.y <= m_edgeDistance)
            {
                MoveCamera(Vector2.down);
            }
        }

        private void MoveCamera(Vector2 MoveDirection)
        {
            Vector3 c_moveDirection = new Vector3(MoveDirection.x, MoveDirection.y);

            _camera.transform.Translate(c_moveDirection * m_cameraSpeedMultiplier * Time.deltaTime);
        }


        #region UNITY CUSTOM EDITOR
#if UNITY_EDITOR
        [CustomEditor(typeof(CameraController))]
        public class CameraControllerEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                CameraController cameraController = (CameraController)target;
                if (cameraController.m_cameraMovementType != MovementType.EdgeScrolling)
                {
                    return;
                }

                EditorGUILayout.Space(2);

                cameraController.m_edgeDistance = EditorGUILayout.FloatField("Edge Distance", cameraController.m_edgeDistance);
                
            }
        }
#endif
        #endregion
    }




}



