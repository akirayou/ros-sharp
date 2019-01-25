base_frame="map"
unity_frame="Unity"

tf_prefix="T"
pose_to_tf=["/U/Cam"]

pose_prefix="/RU/"
tf_to_pose=["roomba_1/base_link"]


#delta of base_frame to Unity correction
# 
pose_offset="/R/offset" 

import rospy
import tf2_ros
import tf2_kdl


from geometry_msgs.msg import PoseStamped,TransformStamped

rospy.init_node("vrbridge_p")
rosRate = rospy.Rate(20) 
br = tf2_ros.TransformBroadcaster()
tfBuffer = tf2_ros.Buffer()
tfl= tf2_ros.TransformListener(tfBuffer)


pose_pub = [ rospy.Publisher(pose_prefix+ p , PoseStamped, queue_size=10) for p in tf_to_pose]

unity_tf=TransformStamped()
unity_tf.header.frame_id=base_frame
unity_tf.child_frame_id=unity_frame
unity_tf.header.seq=1
unity_tf.header.stamp=rospy.Time.now();
unity_tf.transform.rotation.x=0
unity_tf.transform.rotation.y=0
unity_tf.transform.rotation.z=0
unity_tf.transform.rotation.w=1
unity_tf.transform.translation.x=0
unity_tf.transform.translation.y=0
unity_tf.transform.translation.z=0


def sub_pose(pose_topic):
    def f(pose):
        trans=TransformStamped()
        trans.header.stamp=pose.header.stamp
        trans.header.frame_id=unity_frame
        trans.child_frame_id=tf_prefix+pose_topic
        trans.transform.translation=pose.pose.position
        trans.transform.rotation=pose.pose.orientation
        br.sendTransform(trans)
    return f        
        
        
for pose_topic  in pose_to_tf:
    rospy.Subscriber(pose_topic,PoseStamped,sub_pose(pose_topic))




import PyKDL

def sub_pose_offset(pose):
    global unity_tf 
    p=pose.pose.position
    r=pose.pose.orientation
    offset=PyKDL.Frame(PyKDL.Rotation.Quaternion(r.x,r.y,r.z,r.w),
                       PyKDL.Vector(p.x,p.y,p.z))
                       
    p=unity_tf.transform.translation
    r=unity_tf.transform.rotation
                       
    org_tf=PyKDL.Frame(PyKDL.Rotation.Quaternion(r.x,r.y,r.z,r.w),
                       PyKDL.Vector(p.x,p.y,p.z))
                
    new=org_tf*offset
    #new=offset*org_tf
    r=new.M.GetQuaternion()
    p=new.p
    unity_tf.transform.rotation.x=r[0]
    unity_tf.transform.rotation.y=r[1]
    unity_tf.transform.rotation.z=r[2]
    unity_tf.transform.rotation.w=r[3]
    unity_tf.transform.translation.x=p[0]
    unity_tf.transform.translation.y=p[1]
    unity_tf.transform.translation.z=p[2]
    print("newtf",unity_tf)
    
    
rospy.Subscriber(pose_offset,PoseStamped,sub_pose_offset)





trans=1
while not rospy.is_shutdown():
    unity_tf.header.seq+=1
    unity_tf.header.stamp=rospy.Time.now()
    br.sendTransform(unity_tf)

    for target,pub in zip(tf_to_pose,pose_pub):
        try:
            trans = tfBuffer.lookup_transform(unity_frame, target, rospy.Time(0))
            pose=PoseStamped()
            pose.header=trans.header
            
            pose.pose.position=trans.transform.translation
            pose.pose.orientation=trans.transform.rotation
            pub.publish(pose)            
            
        except (tf2_ros.LookupException, tf2_ros.ConnectivityException, tf2_ros.ExtrapolationException):
            print("lookup erro",target)            
            continue
    
    
    rosRate.sleep()





