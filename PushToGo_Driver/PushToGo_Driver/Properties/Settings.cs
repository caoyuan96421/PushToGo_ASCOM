using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ASCOM.PushToGo.Properties
{
    [DeviceId("ASCOM.PushToGo.Telescope", DeviceName = "PushToGo Mount")]
    [SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings
    {
    }
}
