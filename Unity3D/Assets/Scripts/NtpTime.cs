using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using GuerrillaNtp;

using System.Threading.Tasks;

public class NtpTime : MonoBehaviour
{
    public string Host = "172.31.20.203";
    public Int16 Port = 1123;
    public float NtpSpan = 10;
    private NtpClient ntp;
    private TimeSpan offset;
    public TimeSpan Offset() { return offset; }
    private bool synced=false;
    public bool Synced() { return synced; }

    public static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    public RosSharp.RosBridgeClient.Messages.Standard.Time Now()
    {
        TimeSpan unixEpoch = DateTime.Now.ToUniversalTime() - UNIX_EPOCH + offset;
        double  ds = unixEpoch.TotalMilliseconds;
        int sec = (int)(ds / 1000);
        RosSharp.RosBridgeClient.Messages.Standard.Time ret = new RosSharp.RosBridgeClient.Messages.Standard.Time
        {
            secs = sec,
            nsecs = (int)(  (ds/1000-sec) * 1e+9)
        };
        return ret;
    }

    void Awake()
    {
        ntp = new NtpClient(Dns.GetHostEntry(Host).AddressList[0],Port);
        offset=ntp.GetCorrectionOffset();
        synced = true;
    }

    // Update is called once per frame
    private float timeElapsed;
    async void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed < NtpSpan) return;
        timeElapsed = 0;
        TimeSpan tmp;
        await Task.Run(() =>
        {
            tmp = ntp.GetCorrectionOffset();
        });
        if (!synced)
            offset = tmp;
        else
            offset = TimeSpan.FromSeconds( 0.9 * offset.TotalSeconds + 0.1 * tmp.TotalSeconds) ;
    }
}
