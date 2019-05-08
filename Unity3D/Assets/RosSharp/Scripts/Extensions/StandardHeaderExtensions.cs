using UnityEngine;
namespace RosSharp.RosBridgeClient
{
    public static class HeaderExtensions
    {
        private static NtpTime ntp=null;
        public static void Update(this Messages.Standard.Header header)
        {
            /*
            float time = UnityEngine.Time.realtimeSinceStartup;
            uint secs = (uint)time;
            uint nsecs = (uint)(1e9 *(time-secs));
            */
            header.seq++;
            if(ntp is null) ntp = GameObject.Find("Ros").GetComponent<NtpTime>();
            header.stamp=ntp.Now();
        }
    }
}
