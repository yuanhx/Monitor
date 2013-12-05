using System;
using System.Collections.Generic;
using System.Text;

namespace UICtrls
{
    public interface ILinkObject
    {
        object LinkObj { get; set; }
    }

    public interface IExtObject
    {
        object ExtConfigObj { get; }
        object ExtObj { get; }
    }
}
