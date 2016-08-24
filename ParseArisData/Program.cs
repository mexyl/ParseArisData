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
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = MOT_NUM)]
            public Int16[] target_cur;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = MOT_NUM)]
            public Int16[] feedback_cur;
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

        static ArisData data;

        static void Main(string[] args)
        {
            System.Threading.Thread udplistener = new System.Threading.Thread(StartUDPListener);
            //StartUDPListener();
            udplistener.Start();
            bool isEnd = false;
            //ArisData data = new ArisData();
            //Console.SetWindowSize(1920, 1080);
            Console.WriteLine("UDP started");
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            while (!isEnd)
            {
                
                //string a = Console.ReadKey().Key.ToString();
                //Console.Write(a);
                //a = a.Substring(0,a.Length-1);
                //Console.Write(a);
                
                GetExchangeData(ref data);

                Console.SetCursorPosition(0,0);
                Console.WriteLine(" Time :" + data.count.ToString().PadLeft(10,' ')+" ");
                Console.Write("cmdpos".PadLeft(20,' ')+" ");
                Console.Write("fdkpos".PadLeft(20, ' ')+" ");
                Console.Write("fdkcur".PadLeft(6, ' ')+" ");
                Console.Write("stawrd".PadLeft(6, ' ')+" ");
                Console.WriteLine("cmdwrd".PadLeft(6, ' '));

                for (int i=0;i<MOT_NUM;i++)
                {
                    Console.Write(data.target_pos[i].ToString().PadLeft(20, ' ') + " ");
                    Console.Write(data.feedback_pos[i].ToString().PadLeft(20, ' ') + " ");
                    Console.Write(data.feedback_cur[i].ToString().PadLeft(6, ' ') + " ");
                    Console.Write(data.statusword[i].ToString().PadLeft(6, ' ') + " ");
                    Console.Write(data.cmd[i].ToString().PadLeft(6, ' ') );
                    Console.WriteLine();
                }
                
                

                Console.Write("fsr_fx :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.Fx[i].ToString("0.00").PadLeft(10,' ') +" ");
                }
                Console.WriteLine();

                Console.Write("fsr_fy :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.Fy[i].ToString("0.00").PadLeft(10, ' ') + " ");
                }
                Console.WriteLine();

                Console.Write("fsr_fz :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.Fz[i].ToString("0.00").PadLeft(10, ' ') + " ");
                }
                Console.WriteLine();

                Console.Write("fsr_mx :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.Mx[i].ToString("0.00").PadLeft(10, ' ') + " ");
                }
                Console.WriteLine();

                Console.Write("fsr_my :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.My[i].ToString("0.00").PadLeft(10, ' ') + " ");
                }
                Console.WriteLine();

                Console.Write("fsr_mz :");
                for (int i = 0; i < FOR_NUM; i++)
                {
                    Console.Write(data.Mz[i].ToString("0.00").PadLeft(10, ' ') + " ");
                }
                Console.WriteLine();

                Console.WriteLine("  IMU :"+data.roll.ToString("0.00").PadLeft(10, ' ') 
                    + "\t"+data.pitch.ToString("0.00").PadLeft(10, ' ') 
                    + "\t"+data.yaw.ToString("0.00").PadLeft(10, ' '));

                System.Threading.Thread.Sleep(100);
                
            }
            Console.WriteLine("exit");

        }
    }
}
