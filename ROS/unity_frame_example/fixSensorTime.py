#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Created on Tue Jan 22 17:32:56 2019

@author: La-noda
"""

import rospy
import tf2_ros

from sensor_msgs.msg import LaserScan


rospy.init_node("fixSensorTime")
rosRate = rospy.Rate(20) 
pub=rospy.Publisher("/roomba_1/scan" , LaserScan, queue_size=10)

def sub_sens(msg):
    msg.header.stamp=rospy.Time.now()
    pub.publish(msg)    
    

rospy.Subscriber("/roomba_1/scan_s",LaserScan, sub_sens)
while not rospy.is_shutdown():
    rosRate.sleep()