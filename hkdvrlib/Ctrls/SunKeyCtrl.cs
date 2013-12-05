using System;
using System.Collections.Generic;
using System.Text;
using CommonException;
using SunKeySDK;

namespace SunKeyDevice
{
    public class SunKeyException : BaseException
    {
        //SUCCESS
        public const uint KEY_OPP_SUCCESS = 0;
        //No device
        public const uint KEY_ERROR_DEVICE = 0xffffffff;
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

        public SunKeyException()
            : base(0, "")
        {
            int code = SunKeyAPI.GetUkeyLastErr();
            SetCode(code);
        }

        public SunKeyException(int code)
            : base(code, "")
        {
            if (code == 0)
            {
                code = SunKeyAPI.GetUkeyLastErr();
                SetCode(code);
            }
        }

        public SunKeyException(string message)
            : base(0, message)
        {
            int code = SunKeyAPI.GetUkeyLastErr();
            SetCode(code);
        }

        public SunKeyException(int code, string message)
            : base(code, message)
        {
            if (code == 0)
            {
                code = SunKeyAPI.GetUkeyLastErr();
                SetCode(code);
            }
        }

        public override string Message
        {
            get
            {
                if (base.Message != "")
                    return GetMessage(this.Code) + base.Message;
                else return GetMessage(this.Code);
            }
        }

        public string FullMessage
        {
            get { return this.Code + ": " + Message; }
        }

        public static string GetMessage(int code)
        {
            switch ((uint)code)
            {
                case KEY_OPP_SUCCESS:
                    return "没有错误";
                case KEY_ERROR_DEVICE:
                    return "没有设备";
                case KEY_PIN_LOCKED:
                    return "识别码被锁定";
                case KEY_PIN_ERROR:
                    return "识别码错误";
                case KEY_NO_SPACE:
                    return "存储空间不足";
                case KEY_NOT_INIT:
                    return "没有初始化";
                case KEY_ADDRESS_INVALID:
                    return "地址错误";
                case KEY_NOT_SURPORT:
                    return "不支持";
                case KEY_ERROR_PIN_TYPE:
                    return "识别码类型错误";
                case KEY_STATUS_ERROR:
                    return "识别码状态错误";
                default:
                    return "其它未知错误";
            }
        }
    }

    public class SunKeyCtrl : IDisposable
    {
        private const short MAXPINCOUNT = 5;
        private const short GUIDSIZE = 8;

        public enum PinType
        {
            SUPER_MANAGER_PIN = 0x00,
            MANAGER_PIN = 0x01,
            USER_PIN = 0x02
        };

        private int mSunKeyHandle = -1;
        private short mPinCount = 0;

        private bool mIsOpen = false;
        private bool mIsVerifyPass = false;        

        ~SunKeyCtrl()
        {
            Dispose(false); 
        }

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected void Dispose(bool isDisposing)
        {
            Close();
        }

        public int Handle
        {
            get { return mSunKeyHandle; }
        }

        public bool IsOpen
        {
            get { return mIsOpen; }
        }

        public bool IsVerifyPass
        {
            get { return mIsVerifyPass; }
        }

        public short PinCount
        {
            get { return mPinCount; }
        }

        public bool Open()
        {
            if (!IsOpen)
            {
                if (SunKeyAPI.Dev_SunKeyOpen(ref mSunKeyHandle))
                {
                    mIsOpen = true;
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);                    
                }
                return false;
            }
            else return true;
        }

        public bool Close()
        {
            if (IsOpen)
            {
                if (SunKeyAPI.Dev_SunKeyClose(mSunKeyHandle))
                {
                    mPinCount = 0;
                    mIsVerifyPass = false;
                    mIsOpen = false;
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
                return false;
            }
            else return true;
        }

        public bool VerifyPIN(PinType PinType, string sPin)
        {
            if (IsOpen)
            {
                byte[] pbPin = System.Text.Encoding.Default.GetBytes(sPin);

                if (SunKeyAPI.Dev_SunKeyVerifyPIN(mSunKeyHandle, (byte)PinType, pbPin, (short)pbPin.Length, ref mPinCount))
                {
                    mIsVerifyPass = true;
                    return true;
                }
                else
                {
                    mIsVerifyPass = false;
                    mPinCount = 0;
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public bool ChangePIN(PinType PinType, string oldPin, string newPin)
        {
            if (IsOpen)
            {
                byte[] pbOld = System.Text.Encoding.Default.GetBytes(oldPin);
                byte[] pbNew = System.Text.Encoding.Default.GetBytes(newPin);

                if (SunKeyAPI.Dev_SunKeyChangePIN(mSunKeyHandle, (byte)PinType, pbOld, (short )pbOld.Length, pbNew, (short)pbNew.Length))
                {
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public bool UnBlockPIN(PinType PinType, string unBlockPin)
        {
            if (IsOpen)
            {
                byte[] pbUnBlockPin = System.Text.Encoding.Default.GetBytes(unBlockPin);

                if (SunKeyAPI.Dev_SunKeyUnBlockPIN(mSunKeyHandle, (byte)PinType, pbUnBlockPin, (short)pbUnBlockPin.Length))
                {
                    mPinCount = MAXPINCOUNT;
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public string Read(short wOffset, short len)
        {
            if (IsVerifyPass)
            {
                byte[] pbDataBuf = new byte[len];
                if (SunKeyAPI.Dev_SunKeyRead(mSunKeyHandle, wOffset, len, pbDataBuf))
                {
                    return System.Text.Encoding.Default.GetString(pbDataBuf);
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return "";
        }

        public bool Read(short wOffset, short len, byte[] pbDataBuf)
        {
            if (IsVerifyPass)
            {
                if (SunKeyAPI.Dev_SunKeyRead(mSunKeyHandle, wOffset, len, pbDataBuf))
                {
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public bool Write(short wOffset, string dataBuf)
        {
            if (IsVerifyPass)
            {
                byte[] pbDataBuf = System.Text.Encoding.Default.GetBytes(dataBuf);

                if (SunKeyAPI.Dev_SunKeyWrite(mSunKeyHandle, wOffset, (short)pbDataBuf.Length, pbDataBuf))
                {
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public bool Write(short wOffset, short len, byte[] pbDataBuf)
        {
            if (IsVerifyPass)
            {
                if (SunKeyAPI.Dev_SunKeyWrite(mSunKeyHandle, wOffset, len, pbDataBuf))
                {
                    return true;
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return false;
        }

        public string GetGuid()
        {
            if (IsOpen)
            {
                byte[] pGuid = new byte[GUIDSIZE];
                short wLen = GUIDSIZE;

                if (SunKeyAPI.Dev_SunKeyGetGuid(mSunKeyHandle, pGuid, ref wLen))
                {
                    return System.Text.Encoding.Default.GetString(pGuid);
                }
                else
                {
                    System.Console.Out.WriteLine("SunkeyLastErr = " + new SunKeyException().FullMessage);
                }
            }
            return "";
        }
    }
}
