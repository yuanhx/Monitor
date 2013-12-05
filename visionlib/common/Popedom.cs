using System;
using System.Collections.Generic;
using System.Text;
using Config;

namespace Popedom
{
    public enum ACOpts : ushort
    {
        None = 0,               //无   :0000000000000000
        View = 1,               //查看 :0000000000000001
        Manager = 14,
        Manager_Add = 2,        //管理 :0000000000000010  14:  0000000000001110
        Manager_Modify = 4,     //管理 :0000000000000100
        Manager_Delete = 8,     //管理 :0000000000001000
        Exec = 240,
        Exec_Init = 16,         //执行 :0000000000010000  240: 0000000011110000  
        Exec_Start = 32,        //执行 :0000000000100000
        Exec_Stop = 64,         //执行 :0000000001000000
        Exec_Cleanup = 128,     //执行 :0000000010000000        
    }

    public interface IPopedom
    {
        bool Verify(ACOpts acopt, bool isQuiet);
        bool Verify(ACOpts acopt);
    }

    public abstract class CPopedom : IPopedom
    {
        public abstract bool Verify(ACOpts acopt, bool isQuiet);

        public bool Verify(ACOpts acopt)
        {
            return Verify(acopt, false);
        }
    }
}
