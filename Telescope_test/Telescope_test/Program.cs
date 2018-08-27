// This implements a console application that can be used to test an ASCOM driver
//

// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.

#define Telescope
// remove this to bypass the code that uses the chooser to select the driver
#define UseChooser

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ASCOM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Uncomment the code that's required
#if UseChooser
            // choose the device
            string id = ASCOM.DriverAccess.Telescope.Choose("");
            if (string.IsNullOrEmpty(id))
                return;
            // create this device
            ASCOM.DriverAccess.Telescope device = new ASCOM.DriverAccess.Telescope(id);
#else
            // this can be replaced by this code, it avoids the chooser and creates the driver class directly.
            ASCOM.DriverAccess.Telescope device = new ASCOM.DriverAccess.Telescope("ASCOM.PushToGo.Telescope");
#endif
            // now run some tests, adding code to your driver so that the tests will pass.
            // these first tests are common to all drivers.
            Console.WriteLine("name " + device.Name);
            Console.WriteLine("description " + device.Description);
            Console.WriteLine("DriverInfo " + device.DriverInfo);
            Console.WriteLine("driverVersion " + device.DriverVersion);

            device.Connected = true;

            Console.WriteLine("CurrentPos: " + device.RightAscension + " " + device.Declination);
            Console.WriteLine("CurrentAltAzPos: " + device.Altitude + " " + device.Azimuth);

            Console.WriteLine("PierSide: " + device.SideOfPier.ToString());

            if (device.AtPark)
                device.Unpark();

            device.Tracking = true;

            Console.WriteLine("Test slewing");
            device.SlewToAltAz(15, 50);

            Console.WriteLine("CurrentAltAzPos: " + device.Altitude + " " + device.Azimuth);

            device.GuideRateDeclination = 0.00208903731;

            Console.WriteLine("GuideRate: >" + device.CommandString("speed guide", false) + "<");
            Console.WriteLine("GuideRate: >" + device.CommandString("speed guide", false) + "<");
            Console.WriteLine("GuideRate: >" + device.CommandString("speed guide", false) + "<");
            Console.WriteLine("GuideRate: >" + device.CommandString("speed guide", false) + "<");
            Console.WriteLine("GuideRate: " + device.GuideRateDeclination);

            //Thread.Sleep(500);
            //device.SetupDialog(); // show setup

            //Console.WriteLine("Test homing");
            //device.Park();

            device.Connected = false;
            Console.WriteLine("Press Enter to finish");
            Console.ReadLine();
        }
    }
}
