using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ResetFrame : Publisher<Messages.Geometry.PoseStamped>
    {
        public Transform ReferenceTransform; //i.e. Camera position, set the camera on REAL robot and run Update
        public Transform TargetTransform; //i.e. robot in AR(=Unity)  it is miss matched one
        public string FrameId = "Unity";
        private Messages.Geometry.PoseStamped message;

        protected override void Start()
        {
            base.Start();
            message = new Messages.Geometry.PoseStamped
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };
        }
        //called by some event
        //Set the AR camera on real robot and do ResetFrame(), "Unity"TF will fixed  with the offset of transform as PoseStamped message (in ROS side script)
        public void RsetFrame()
        {
            Matrix4x4 v=  TargetTransform.localToWorldMatrix  * ReferenceTransform.worldToLocalMatrix;
            Vector3 p = v.GetColumn(3);  
            message.header.Update();
            message.pose.position = GetGeometryPoint(p.Unity2Ros());
            message.pose.orientation = GetGeometryQuaternion(v.rotation.Unity2Ros());
            //Just check code for development  
            /*
            if (false)
            {
                GameObject o=new GameObject();
                GameObject r = new GameObject();
                o.transform.position = p;//GetPosition(message).Ros2Unity();
                o.transform.rotation = v.rotation;//GetRotation(message).Ros2Unity();
                r.transform.position = TargetTransform.position;
                r.transform.rotation= TargetTransform.rotation;
                r.transform.parent = o.transform;
                Debug.Log("Positio");
                Debug.Log(r.transform.localPosition);
                Debug.Log(ReferenceTransform.position);
                Debug.Log("Rot");
                Debug.Log(r.transform.localRotation);
                Debug.Log(ReferenceTransform.rotation);
                Destroy(o);
                Destroy(r);
            }*/
            Publish(message);
        }
        private Messages.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            Messages.Geometry.Point geometryPoint = new Messages.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }

        private Vector3 GetPosition(Messages.Geometry.PoseStamped message)
        {
            return new Vector3(
                message.pose.position.x,
                message.pose.position.y,
                message.pose.position.z);
        }

        private Quaternion GetRotation(Messages.Geometry.PoseStamped message)
        {
            return new Quaternion(
                message.pose.orientation.x,
                message.pose.orientation.y,
                message.pose.orientation.z,
                message.pose.orientation.w);
        }
    }
}
