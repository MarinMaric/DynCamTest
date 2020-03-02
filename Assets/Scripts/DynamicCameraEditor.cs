using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(DynamicCameraControl))]
[CanEditMultipleObjects]
public class DynamicCameraEditor : Editor
{
    SerializedProperty stateDrivenCamera;
    SerializedProperty camerasArray;
    SerializedProperty cameraProperties;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        if(cameraProperties == null)
        {
            cameraProperties = serializedObject.FindProperty("cameraProperties");
        }

        for(int i=0; i< cameraProperties.arraySize; i++)
        {
            //if (EditorGUILayout.Foldout(true, new GUIContent("Camera" + (i + 1)))){
            //    EditorGUILayout.PropertyField(cameraProperties.GetArrayElementAtIndex(i).FindPropertyRelative("camGO"));
            //    EditorGUILayout.PropertyField(cameraProperties.GetArrayElementAtIndex(i).FindPropertyRelative("changeCollider"));
            //    EditorGUILayout.PropertyField(cameraProperties.GetArrayElementAtIndex(i).FindPropertyRelative("triggerList"));
            //}
            EditorGUILayout.PropertyField(cameraProperties.GetArrayElementAtIndex(i));
        }

        serializedObject.ApplyModifiedProperties();

        //Adding camera
        if (GUILayout.Button("Create New Camera", EditorStyles.miniButton))
        {
            if (stateDrivenCamera == null)
            {
                stateDrivenCamera = serializedObject.FindProperty("stateDrivenCamera");
            }
            camerasArray = serializedObject.FindProperty("cameras");
            cameraProperties = serializedObject.FindProperty("cameraProperties");
            var cameraParent = new GameObject();
            var childCamera = new GameObject();
            //childCamera.name = "CM vcam" + (((CinemachineStateDrivenCamera)stateDrivenCamera.objectReferenceValue).ChildCameras.Length + 1);
            if (DynamicCameraControl.Instance.cameraProperties.Count != 0)
            {
                int parsed = int.Parse((DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameras.Count - 1].camGO.name.Substring(
                    DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameras.Count - 1].camGO.name.Length - 1, 1)));
                parsed++;
                childCamera.name = "CM vcam" + parsed;
                cameraParent.name = "Camera Auxiliaries " + parsed;
            }
            else
            {
                childCamera.name = "CM vcam1";
                cameraParent.name = "Camera Auxiliaries 1";
            }
            childCamera.AddComponent<CinemachineVirtualCamera>();
            childCamera.transform.parent = ((CinemachineStateDrivenCamera)stateDrivenCamera.objectReferenceValue).transform;
            var vcam=childCamera.GetComponent<CinemachineVirtualCamera>();
            //vcam.AddCinemachineComponent<CinemachineTransposer>();
            vcam.AddCinemachineComponent<CinemachineComposer>();
            vcam.m_Lens.FieldOfView = 60;

            childCamera.AddComponent<BezierTravel>();
            var zoomScript = childCamera.AddComponent<DynCamZoom>();

            var collidersParent = new GameObject();
            collidersParent.name = childCamera.name + "_SpeedColliders";

            AddToCameraList(childCamera, collidersParent, cameraParent);
            Repaint();
        }

        //Removing camera
        if (GUILayout.Button("Remove Cameras", EditorStyles.miniButton))
        {
            if (stateDrivenCamera == null)
            {
                stateDrivenCamera = serializedObject.FindProperty("stateDrivenCamera");
            }
            //check all cameras that are marked to be deleted and delete them
            camerasArray = serializedObject.FindProperty("cameras");
            cameraProperties = serializedObject.FindProperty("cameraProperties");

            RemoveFromList();
        }
    }

    public void AddToCameraList(GameObject ccam, GameObject collidersParent, GameObject cameraParent)
    {
        camerasArray.Next(true); //generic field
        camerasArray.Next(true); //length
        int arrayLength = camerasArray.intValue;

        camerasArray.intValue = ++arrayLength;
        int lastIndex = arrayLength - 1;

        camerasArray.Next(true);

        for(int i=0; i<arrayLength; i++)
        {
            if (i < lastIndex)
                camerasArray.Next(false);
            else
            {
                camerasArray.objectReferenceValue = ccam;
            }
        }

        cameraProperties.arraySize++;
        var newProperty = cameraProperties.GetArrayElementAtIndex(cameraProperties.arraySize - 1);
        newProperty.FindPropertyRelative("camGO").objectReferenceValue = ccam;
        newProperty.FindPropertyRelative("positionOffset").vector3Value = Vector3.zero;
        newProperty.FindPropertyRelative("zoomMin").floatValue = 40f;
        newProperty.FindPropertyRelative("zoomMax").floatValue = 60f;
        newProperty.FindPropertyRelative("zoomSpeedFactor").floatValue = 1f;
        newProperty.FindPropertyRelative("zoomAmount").floatValue = 60f;
        newProperty.FindPropertyRelative("frequency").intValue = 15;
        newProperty.FindPropertyRelative("speedCollidersParent").objectReferenceValue = collidersParent.transform;
        newProperty.FindPropertyRelative("cameraParent").objectReferenceValue = cameraParent.transform;
        ccam.transform.parent = DynamicCameraControl.Instance.transform;
        collidersParent.transform.parent = cameraParent.transform;
        cameraParent.transform.parent = DynamicCameraControl.Instance.transform;
        if (DynamicCameraControl.Instance.cameraProperties.Count > 0) {
            //if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camID > DynamicCameraControl.idGenerator)
                DynamicCameraControl.idGenerator = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camID + 1;
        }
        newProperty.FindPropertyRelative("camID").intValue = DynamicCameraControl.idGenerator;
        #region buttons obsolete
        //newProperty.FindPropertyRelative("buttonAdd").FindPropertyRelative("text").stringValue = "Add Curve";
        //newProperty.FindPropertyRelative("buttonAdd").FindPropertyRelative("index").intValue = cameraProperties.arraySize -1;
        //newProperty.FindPropertyRelative("buttonClear").FindPropertyRelative("text").stringValue = "Clear Curve";
        //newProperty.FindPropertyRelative("buttonClear").FindPropertyRelative("index").intValue = cameraProperties.arraySize - 1; 
        #endregion
        camerasArray.serializedObject.ApplyModifiedProperties();
    }

    public void RemoveFromList()
    {
        List<string> namesToDelete = new List<string>();

        for(int i=0; i < cameraProperties.arraySize; i++)
        {
            var cam = cameraProperties.GetArrayElementAtIndex(i);

            if (cam.FindPropertyRelative("delete").boolValue)
            {
                namesToDelete.Add(cam.FindPropertyRelative("camGO").objectReferenceValue.name);
                var pathGO = GameObject.Find(namesToDelete[namesToDelete.Count - 1] + "_Path");
                if (pathGO != null)
                    DestroyImmediate(pathGO);
                var colliderGO = GameObject.Find(namesToDelete[namesToDelete.Count - 1] + "_ChangeCollider");
                if (colliderGO != null)
                    DestroyImmediate(colliderGO);
                var speedCollidersParent = GameObject.Find(namesToDelete[namesToDelete.Count - 1] + "_SpeedColliders");
                if (speedCollidersParent != null)
                    DestroyImmediate(speedCollidersParent);
                //foreach (DynCamera dynCam in DynamicCameraControl.Instance.cameraProperties)
                //{
                //    if (dynCam.camID == cam.FindPropertyRelative("camID").intValue)
                //    {
                //        foreach(BoxCollider speedCol in dynCam.speedColliders)
                //        {
                //            if(speedCol!=null)
                //                DestroyImmediate(speedCol.gameObject);
                //        }
                //    }
                //}
            }
        }

        int finalSize = cameraProperties.arraySize-namesToDelete.Count;
        int index = 0, lastIndex = cameraProperties.arraySize - 1;

        while (cameraProperties.arraySize > finalSize)
        {
            if (cameraProperties.GetArrayElementAtIndex(index).FindPropertyRelative("delete").boolValue)
            {
                //if the index is smaller than last the elements in front of it will be moved back. to avoid skipping the element ahead index should stay the same while last
                //is decreased.
                if (index < lastIndex)
                {
                    var parent = DynamicCameraControl.Instance.GetDynCamByID(cameraProperties.GetArrayElementAtIndex(index).FindPropertyRelative("camID").intValue).cameraParent;
                    DestroyImmediate(parent.gameObject);
                    cameraProperties.DeleteArrayElementAtIndex(index);
                    lastIndex--;
                }
                //if the index is last then just delete and stop
                else
                {
                    var parent = DynamicCameraControl.Instance.GetDynCamByID(cameraProperties.GetArrayElementAtIndex(index).FindPropertyRelative("camID").intValue).cameraParent;
                    DestroyImmediate(parent.gameObject);
                    cameraProperties.DeleteArrayElementAtIndex(index);
                    lastIndex--;
                    break;
                }
            }
            else
            {
                index++;
            }
        }

        foreach (string n in namesToDelete)
        {
            for (int i = 0; i < camerasArray.arraySize; i++)
            {
                var cam = camerasArray.GetArrayElementAtIndex(i).objectReferenceValue;
                if (cam != null)
                {
                    if (camerasArray.GetArrayElementAtIndex(i).objectReferenceValue.name == n)
                    {
                        DestroyImmediate(camerasArray.GetArrayElementAtIndex(i).objectReferenceValue);
                        camerasArray.DeleteArrayElementAtIndex(i);
                    }
                }
            }
        }

        int nullCounter = 0;

        for(int i=0; i < camerasArray.arraySize; i++)
        {
            if (camerasArray.GetArrayElementAtIndex(i).objectReferenceValue == null)
                nullCounter++;
        }

        int nulledSize = camerasArray.arraySize - nullCounter;

        while(camerasArray.arraySize > nulledSize)
        {
            for(int i = 0; i< camerasArray.arraySize; i++)
            {
                if (camerasArray.GetArrayElementAtIndex(i).objectReferenceValue == null)
                    camerasArray.DeleteArrayElementAtIndex(i);
            }
        }

        camerasArray.serializedObject.ApplyModifiedProperties();
    }
}