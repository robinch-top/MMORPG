using System;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mirror
{
    /// 服务器和客户端之间的时间同步
    public static class NetworkTime
    {
        
        static readonly Stopwatch stopwatch = new Stopwatch();
        public static double offset = 0;

        static NetworkTime()
        {
            stopwatch.Start();
        }

        static double LocalTime()
        {
            return stopwatch.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// 服务器已运行时间,需要减去通信延时才精准
        /// </summary>
        public static double time => LocalTime() - offset;

        
    }
}