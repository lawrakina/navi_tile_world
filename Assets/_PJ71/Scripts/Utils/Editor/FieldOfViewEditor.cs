using UnityEditor;
using UnityEngine;

namespace NavySpade._PJ71.Utils.Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewNewEditor : UnityEditor.Editor
    {
        void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView) target;
            Vector3 objPosition = fow.transform.position;
            Handles.color = fow.RadiusColor;
            Handles.DrawWireArc(objPosition, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
            Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle / 2, false);
            Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2, false);

            Handles.color = fow.FieldOfViewColor;
            Handles.DrawLine(objPosition, objPosition + viewAngleA * fow.ViewRadius);
            Handles.DrawLine(objPosition, objPosition + viewAngleB * fow.ViewRadius);

            Handles.color = fow.TargetColor;
            if (fow.VisibleTargets != null)
            {
                foreach (Collider visibleTarget in fow.VisibleTargets)
                {
                    Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
                }
            }
        }
    }
}