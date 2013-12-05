using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDP.Common
{
    public interface IProperty
    {
        string SetProperty(object value);
        void SetProperty(string name, object value);
        void SetProperty(string name, bool value);
        void SetProperty(string name, int value);
        void SetProperty(string name, long value);
        void SetProperty(string name, float value);
        void SetProperty(string name, double value);
        void SetProperty(string name, DateTime value);
        object GetProperty(string name);
        string GetStrProperty(string name);
        string GetStrProperty(string name, string defvalue);
        bool GetBoolProperty(string name);
        bool GetBoolProperty(string name, bool defvalue);
        int GetIntProperty(string name);
        int GetIntProperty(string name, int defvalue);
        long GetLongProperty(string name);
        long GetLongProperty(string name, long defvalue);
        float GetFloatProperty(string name);
        float GetFloatProperty(string name, float defvalue);
        double GetDoubleProperty(string name);
        double GetDoubleProperty(string name, double defvalue);
        DateTime GetDateProperty(string name);
        DateTime GetDateProperty(string name, DateTime defvalue);
        void RemoveProperty(string name);
        void ClearProperty();
    }
}
