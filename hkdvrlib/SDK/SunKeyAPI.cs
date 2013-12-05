using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SunKeySDK
{
    public class SunKeyAPI
    {
        private const String SDKDll = "SUNKEY1.dll";

        #region 错误代码

        //SUCCESS
        public const uint KEY_OPP_SUCCESS = 0;
        //No device
        public const uint KEY_ERROR_DEVICE=0xffffffff;
        //Pin was blocked
        public const uint KEY_PIN_LOCKED = 0xA0100001;
        //Pin error
        public const uint KEY_PIN_ERROR = 0xA0100002;
        //EEPROM not enough
        public const uint KEY_NO_SPACE = 0xA0100003;
        //Not init
        public const uint KEY_NOT_INIT = 0xA0100004;
        //address error
        public const uint KEY_ADDRESS_INVALID = 0xA0100009;
        //Not support
        public const uint KEY_NOT_SURPORT = 0xA010000C;
        //Pin type error
        public const uint KEY_ERROR_PIN_TYPE = 0xA010000D;
        //Error Status (eg.Not Verify The User/Manager Pin then Read/Write the Data )
        public const uint KEY_STATUS_ERROR = 0xA010000E;

        #endregion

        #region Pin type
        
        //Unblock Pin
        public const int SUPER_MANAGER_PIN=0x00;
        //MANAGER PIN
        public const int MANAGER_PIN=0x01;
        //user Pin
        public const int USER_PIN = 0x02;

        #endregion

        //Open Device
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool Dev_SunKeyOpen(ref int hSunKey);

        //Close Device
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool Dev_SunKeyClose(int hSunKey);

        //Read EEPROM
	    [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyRead(int hSunKey, short wOffset, short len, byte[] pbDataBuf);

        //Write EEPROM
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyWrite(int hSunKey, short wOffset, short len, byte[] pbDataBuf);

        //Verify pin
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyVerifyPIN(int hSunKey, byte PinType, byte[] pbPin, short wLen, ref short pwPinCount);

        //Change pin(default pin is "888888")
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyChangePIN(int hSunKey, byte PinType, byte[] pbOld, short wOldLen, byte[] pbNew, short wNewLen);

        //UnBlock pin
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyUnBlockPIN(int hSunKey, byte PinType, byte[] pbUnBlockPin, short wLen);

        //GetSunKeyLastErr
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static int GetUkeyLastErr();

        //Dev_SunKeyGetGuid
        [DllImport(SDKDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = true)]
        public extern static bool Dev_SunKeyGetGuid(int hSunKey, byte[] pGuid, ref short wLen);
    }
}
