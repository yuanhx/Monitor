using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDP.Common
{
    public class SignConstant
    {
        public const String SegmentSign = "::";
        public const String SegmentReplace = "&#&";

        public const String ItemSign = ";";
        public const String ItemReplace = "&!&";

        public const String MapSign = "=";
        public const String MapReplace = "&:&";

        public const String ParamItemSign = ";";
        public const String ParamItemReplace = "&!&";

        public const String SqlSegSign = "<SqlSeg>";
        public const String SqlSegReplace = "&SqlSeg&";

        public const String FieldSign = "<F";
        public const String FieldReplace = "&f&";

        public const String RowSign = "<R>";
        public const String RowReplace = "&r&";
    }
}
