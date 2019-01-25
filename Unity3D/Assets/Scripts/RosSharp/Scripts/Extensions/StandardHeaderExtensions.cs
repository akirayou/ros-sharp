using UnityEngine;
namespace RosSharp.RosBridgeClient
{
    public static class HeaderExtensions
    {
        private static NtpTime ntp=null;
        public static void Update(this Messages.Standard.Header header)
        {
            header.seq++;
            if(ntp is null) ntp = GameObject.Find("Ros").GetComponent<NtpTime>();
            header.stamp=ntp.Now();
        }
    }
}
