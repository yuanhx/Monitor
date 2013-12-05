using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDP.Common
{
    public interface ISerializable
    {
        String Type { get; }
	
        String ToStr();
        String ToXml();	
    }
}
