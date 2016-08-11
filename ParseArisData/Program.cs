using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ParseArisData
{

    class Program
    {

        const int MOT_NUM = 19;
        const int FOR_NUM = 7;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct ArisData
        {
            /* Motor Control */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] target_pos;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] feedback_pos;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] target_vel;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] feedback_vel;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] target_cur;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = MOT_NUM)]
            public int[] feedback_cur;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = MOT_NUM)]
            public byte[] cmd;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = MOT_NUM)]
            public byte[] mode;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U2, SizeConst = MOT_NUM)]
            public UInt16[] statusword;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = MOT_NUM)]
            public Int16[] ret;
            /* Force Sensor */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] Fx;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] Fy;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] Fz;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] Mx;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] My;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = FOR_NUM)]
            public float[] Mz;

            /* IMU */
            [MarshalAs(UnmanagedType.R4)]
            public float yaw;
            [MarshalAs(UnmanagedType.R4)]
            public float pitch;
            [MarshalAs(UnmanagedType.R4)]
            public float roll;
            [MarshalAs(UnmanagedType.I4)]
            public int count;


        };
        


        [DllImport("ArisUDPClient.dll", EntryPoint = "StartUDPListener", CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void StartUDPListener();

        [DllImport("ArisUDPClient.dll", EntryPoint = "CloseUDPListener", CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void CloseUDPListener();

        [DllImport("ArisUDPClient.dll", EntryPoint = "GetExchangeData", CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern void GetExchangeData(ref ArisData data);



        static void Main(string[] args)
        {
            System.Threading.Thread udplistener = new System.Threading.Thread(StartUDPListener);
            //StartUDPListener();
            udplistener.Start();
            bool isEnd = false;
            ArisData data = new ArisData();
            while (!isEnd)
            {
                Console.WriteLine("UDP started");
                //string a = Console.ReadKey().Key.ToString();
                //Console.Write(a);
                //a = a.Substring(0,a.Length-1);
                //Console.Write(a);
                GetExchangeData(ref data);

                Console.WriteLine("C#:"+data.cmd[0].ToString());

                //if (string.Compare("E",a)==0)
                //{
                //    Console.WriteLine("exit");
                //    isEnd = true;
                //    udplistener.Abort();
                //    break;
                //}
                
            }
            Console.WriteLine("exit");

        }
    }
}
