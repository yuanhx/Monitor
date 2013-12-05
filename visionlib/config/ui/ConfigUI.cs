using System;
using System.Collections.Generic;
using System.Text;

namespace Config
{
    public interface IConfigForm
    {
        bool IsOK { get; }

        IConfig Config { get; }

        void ShowEditDialog(IConfig config);
        void ShowAddDialog(IConfigManager manager);
        void ShowAddDialog(IConfigType type, IConfigManager manager);
    }

    public interface IConfigControl
    {
        IMonitorSystemContext SystemContext { get; }
        IConfigType Type { get; }

        IConfig Config { get; }

        bool IsOK { get; }

        bool NewConfig();
        void EditConfig(IConfig config);

        void ResetUI();
    }
}
