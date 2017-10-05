using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionIndicatorsManager : Singleton<PositionIndicatorsManager>
{
    public Transform MarkObjsRoot;
    public Transform OriginModel;

    private Matrix4x4 originMatrix;
    private Matrix4x4 targetMatrix;
    private Matrix4x4 transMatrix;
    //transMatrix*originMatrix=targetMatrix

    private bool[] indicatorsDoneState = new bool[4];
    private bool allIndicatorsSetDone//TODO:Whether this will work
    {
        get
        {
            bool allDone = true;
            for (int index = 0; index != indicatorsDoneState.Length; ++index)
                allDone &= indicatorsDoneState[index];
            return allDone;
        }
    }
    private void Start()
    {
        QRCodeDetector.Instance.OnDetectedQRCode += EnableMapping;
        InitMatrix(out originMatrix);
    }

    int currentMarkIndex = -1;
    private void EnableMapping(string QRResult)
    {
        if (int.TryParse(QRResult, out currentMarkIndex))
        {
            currentMarkIndex = Mathf.Clamp(currentMarkIndex, 1, MarkObjsRoot.childCount) - 1;
            SpatialMapping.Instance.MappingEnabled = true;
            GestureManager.Instance.OnDoubleClick += SetMarkObjPosition;
        }
        else
            Debug.Log("Exception: error QRResult " + currentMarkIndex);
    }

    private void SetMarkObjPosition()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 30, SpatialMapping.PhysicsRaycastMask))
        {
            MarkObjsRoot.GetChild(currentMarkIndex).position = hitInfo.point;
            SpatialMapping.Instance.MappingEnabled = false;

            indicatorsDoneState[currentMarkIndex] = true;
            if (allIndicatorsSetDone)//TODO:make this in indicatorsDoneState's Set
            {
                InitMatrix(out targetMatrix);
                transMatrix = targetMatrix * originMatrix.inverse;
                SetModalTransform();
            }
        }

        GestureManager.Instance.OnDoubleClick -= SetMarkObjPosition;
    }


    private void SetModalTransform()
    {
        //Scale
        float originDistance = Vector3.Distance(originMatrix.GetColumn(0), originMatrix.GetColumn(1));
        float targetDistance = Vector3.Distance(targetMatrix.GetColumn(0), targetMatrix.GetColumn(1));
        float ScaleTimes = targetDistance / originDistance;
        OriginModel.localScale=Vector3.Scale(OriginModel.localScale, new Vector3(ScaleTimes, ScaleTimes, ScaleTimes));

        //Rotation
        Vector4 forwardDirection = transMatrix.GetColumn(2).normalized;
        Vector4 upDirection = transMatrix.GetColumn(1).normalized;
        OriginModel.rotation = Quaternion.LookRotation(forwardDirection, upDirection) * OriginModel.rotation;

        //Position
        Vector4 targetPosition = OriginModel.position;
        targetPosition[3] = 1;
        targetPosition = transMatrix * targetPosition;
        OriginModel.position = targetPosition;
    }

    private void InitMatrix(out Matrix4x4 matrixToSet, bool test = false)
    {
        if (MarkObjsRoot == null || MarkObjsRoot.childCount != 4)
            Debug.LogError("Wrong Mark Obj State");
        Vector4[] positions = new Vector4[4];
        for (int index = 0; index != MarkObjsRoot.childCount; ++index)
        {
            positions[index] = MarkObjsRoot.GetChild(index).position;
            positions[index][3] = 1;
        }
        matrixToSet = new Matrix4x4(positions[0], positions[1], positions[2], positions[3]);
    }

    private void OnDestroy()
    {
        QRCodeDetector.Instance.OnDetectedQRCode -= EnableMapping;
    }
}
