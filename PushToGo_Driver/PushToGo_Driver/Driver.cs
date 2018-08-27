//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Telescope driver for PushToGo
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Telescope interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code canbe deleted and this definition removed.
#define Telescope

using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using ASCOM.PushToGo.Properties;
using System.IO.Ports;
using System.Linq;

namespace ASCOM.PushToGo
{
    //
    // Your driver's DeviceID is ASCOM.PushToGo.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.PushToGo.Telescope
    // The ClassInterface/None addribute prevents an empty interface called
    // _PushToGo from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Telescope Driver for PushToGo.
    /// </summary>
    [Guid("50680c9d-f589-4d28-9b7d-f8fff69ec406")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ITelescopeV3
    {
        public const double sidereal_speed = 0.00417807462;
        private readonly char[] spchar = {' ', '\n', '\r', '\t'};

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.PushToGo.Telescope";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "PushToGo Mount Driver";

        //internal static string comPort; // Variables to hold the currrent device configuration

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;

        /// <summary>
        /// Transformation for various conversion between coordinate systems
        /// </summary>
        private Astrometry.Transform.Transform trans;

        /// <summary>
        /// Serial object for communication
        /// </summary>
        private SerialPort serial;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushToGo"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Telescope()
        {
            tl = new TraceLogger("", "PushToGo");
            ReadProfile(); // Read device configuration from the ASCOM Profile store
            tl.Enabled = Settings.Default.traceEnabled;

            tl.LogMessage("Telescope", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            trans = new Astrometry.Transform.Transform();
            serial = new SerialPort();

            //TODO: Implement your additional construction here

            tl.LogMessage("Telescope", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE ITelescopeV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
            {
                new SetupDialogForm(this).ShowDialog();
            }
            else
            {
                new SetupDialogForm(null).ShowDialog();
            }

            tl.Enabled = Settings.Default.traceEnabled;
        }

        public ArrayList SupportedActions
        {
            get
            {
                ArrayList actions = new ArrayList()
                {
                };
                tl.LogMessage("SupportedActions Get", actions.ToString());
                return actions;
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            switch (actionName)
            {
                default:
                    LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
                    throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
            }
            //return "";
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind" + " " + command);
            lock (serial)
            {
                serial.WriteLine(command + "\n");
            }
            //Console.WriteLine("]" + command + "[");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool" + " " + command);
            string ret = CommandString(command, raw);
            return ret == "true" ? true : false;
        }


        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString" + " " + command);
            // it's a good idea to put all the low level communication with the device here,
            // then all communication calls this function
            // you need something to ensure that only one command is in progress at a time
            string ret = "";
            lock (this)
            {
                lock (serial)
                {
                    serial.WriteLine(command + "\n");
                }
                //Console.WriteLine("]" + command + "[");

                string cmd = command.Split(' ')[0];
                string response = serial.ReadLine().TrimEnd(spchar);

                if (response == "Unknown command")
                {
                    return "";
                }

                do
                {
                    //Console.WriteLine(">" + response + "<");
                    string[] s = response.Split(' ');
                    if (s.Length != 0)
                    {
                        // Empty string
                        int retCode;
                        if (int.TryParse(s[0], out retCode))
                        {
                            if (s.Length >= 2 && s[1] == cmd)
                            {
                                if (raw)
                                {
                                    // Append to the output if in raw mode
                                    ret = ret + response + "\n";
                                }
                                break;
                            }
                        }

                        if(s[0] == cmd)
                        {
                            // Message addressed to this command
                            if (raw)
                            {
                                // Append to the output if in raw mode
                                ret = ret + response + "\n";
                            }
                            else
                            {
                                // Append the rest of the response to the output
                                ret = ret + String.Join(" ", s.Skip(1).ToArray()) + "\n";
                            }
                        }
                    }
                    response = serial.ReadLine().TrimEnd(spchar);
                } while (true);
               
            }
            ret = ret.TrimEnd(spchar);
            return ret;
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
            serial.Dispose();
            serial = null;
            trans.Dispose();
            trans = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", Settings.Default.comPort);

                    serial.PortName = Settings.Default.comPort;
                    serial.BaudRate = 115200;
                    serial.Parity = Parity.None;
                    serial.DataBits = 8;
                    serial.StopBits = StopBits.One;
                    serial.Handshake = Handshake.None;
                    serial.ReadTimeout = 300000;
                    serial.WriteTimeout = 500;
                    serial.NewLine = "\n";

                    serial.Open();
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from port {0}", Settings.Default.comPort);

                    if (serial.IsOpen)
                        serial.Close();
                }
            }
        }

        public string Description
        {
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "PushToGo Mount ASCOM Driver. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "PushToGo";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region Helper functions
        private bool getPointingEq(out double ra, out double dec)
        {
            ra = 0;
            dec = 0;
            string[] pos = CommandString("read", false).Split(' ');
            if (pos.Length != 2)
                return false;
            if (!Double.TryParse(pos[0], out ra))
                return false;
            if (!Double.TryParse(pos[1], out dec))
                return false;

            ra = astroUtilities.ConditionRA(ra / 15);

            return true;
        }

        private bool getPointingAltAz(out double alt, out double az)
        {
            alt = az = 0;

            if (!getPointingEq(out double ra, out double dec))
                return false;

            lock (trans)
            {
                var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                trans.SiteLatitude = SiteLatitude;
                trans.SiteLongitude = SiteLongitude;
                trans.SiteElevation = Settings.Default.elevation;
                trans.SiteTemperature = Settings.Default.temperature;
                trans.JulianDateUTC = jd;
                trans.SetJ2000(ra, dec);
                trans.Refraction = Settings.Default.refraction;
                trans.Refresh();
                alt = trans.ElevationTopocentric;
                az = trans.AzimuthTopocentric;
            }
            return true;
        }

        /// <summary>
        /// Low-level slew to ra/dec
        /// </summary>
        /// <param name="ra">Right ascension</param>
        /// <param name="dec">Declination</param>
        /// <param name="sync">Sync or Async</param>
        private void slewTo(double ra, double dec, bool sync)
        {
            ra = Math.IEEERemainder(ra * 15, 360.0);
            if (sync)
            {
                CommandString("goto " + ra + " " + dec, false);
            }
            else
            {
                CommandBlind("goto " + ra + " " + dec, false);
            }
        }
        #endregion

        #region ITelescope Implementation
        public void AbortSlew()
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("AbortSlew");
            }
            tl.LogMessage("AbortSlew", "");
            CommandBlind("stop", false);
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                AlignmentModes am = AlignmentModes.algGermanPolar;
                tl.LogMessage("AlignmentMode Get", am.ToString());
                return am;
            }
        }

        public double Altitude
        {
            get
            {
                getPointingAltAz(out double alt, out double az);
                tl.LogMessage("Altitude", "Get - " + utilities.DegreesToDMS(alt, ":", ":"));
                return alt;
            }
        }

        public double ApertureArea
        {
            get
            {
                tl.LogMessage("ApertureArea Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureArea", false);
            }
        }

        public double ApertureDiameter
        {
            get
            {
                tl.LogMessage("ApertureDiameter Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureDiameter", false);
            }
        }

        private bool atHome = false;

        public bool AtHome
        {
            get
            {
                tl.LogMessage("AtHome", "Get - " + atHome.ToString());
                return atHome;
            }
        }

        public bool AtPark
        {
            get
            {
                tl.LogMessage("AtPark", "Get - " + atHome.ToString());
                return atHome;
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            tl.LogMessage("AxisRates", "Get - " + Axis.ToString());
            return new AxisRates(Axis);
        }

        public double Azimuth
        {
            get
            {
                getPointingAltAz(out double alt, out double az);
                tl.LogMessage("Azimuth", "Get - " + utilities.DegreesToDMS(az, ":", ":"));
                return az;
            }
        }

        public bool CanFindHome
        {
            get
            {
                tl.LogMessage("CanFindHome", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            tl.LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: return false;
                case TelescopeAxes.axisSecondary: return false;
                case TelescopeAxes.axisTertiary: return false;
                default: throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
            }
        }

        public bool CanPark
        {
            get
            {
                tl.LogMessage("CanPark", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                tl.LogMessage("CanPulseGuide", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                tl.LogMessage("CanSetDeclinationRate", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                tl.LogMessage("CanSetGuideRates", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                tl.LogMessage("CanSetPierSide", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                tl.LogMessage("CanSetRightAscensionRate", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                tl.LogMessage("CanSetTracking", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlew
        {
            get
            {
                tl.LogMessage("CanSlew", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                tl.LogMessage("CanSlewAltAz", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                tl.LogMessage("CanSlewAltAzAsync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                tl.LogMessage("CanSlewAsync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSync
        {
            get
            {
                tl.LogMessage("CanSync", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                tl.LogMessage("CanSyncAltAz", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanUnpark
        {
            get
            {
                tl.LogMessage("CanUnpark", "Get - " + true.ToString());
                return true;
            }
        }

        public double Declination
        {
            get
            {
                getPointingEq(out double ra, out double dec);
                tl.LogMessage("Declination", "Get - " + utilities.DegreesToDMS(dec, ":", ":"));
                return dec;
            }
        }

        public double DeclinationRate
        {
            get
            {
                double declination = 0.0;
                if(!Double.TryParse(CommandString("speed slew", false), out declination))
                {
                    tl.LogMessage("DeclinationRate", "Get failed ");
                }
                else
                    tl.LogMessage("DeclinationRate", "Get - " + declination.ToString());
                return declination;
            }
            set
            {
                tl.LogMessage("DeclinationRate Set", value.ToString());
                CommandString("speed slew " + value.ToString(), false);
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            double ha = SiderealTime - RightAscension; // Hour angle
            ha = Math.IEEERemainder(ha, 24.0); // -12h ~ 12h

            PierSide side;
            if (ha <= 0)
            {
                side = PierSide.pierWest;
            }
            else
            {
                side = PierSide.pierEast;
            }

            tl.LogMessage("DestinationSideOfPier Get", side.ToString());
            return side;
        }

        public bool DoesRefraction
        {
            get
            {
                tl.LogMessage("DoesRefraction Get", Settings.Default.refraction.ToString());
                return Settings.Default.refraction;
            }
            set
            {
                tl.LogMessage("DoesRefraction Set", value.ToString());
                Settings.Default.refraction = value;
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                EquatorialCoordinateType equatorialSystem = EquatorialCoordinateType.equTopocentric;
                tl.LogMessage("DeclinationRate", "Get - " + equatorialSystem.ToString());
                return equatorialSystem;
            }
        }

        public void FindHome()
        {
            tl.LogMessage("FindHome", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("FindHome");
        }

        public double FocalLength
        {
            get
            {
                tl.LogMessage("FocalLength Get", Settings.Default.focalLength.ToString());
                return Settings.Default.focalLength;
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                double guideRate;
                if (!Double.TryParse(CommandString("speed guide", false), out guideRate))
                {
                    throw new ASCOM.DriverException("GuideRateDeclination");
                }
                guideRate *= sidereal_speed;
                tl.LogMessage("GuideRateDeclination Get", guideRate.ToString());
                return guideRate;
            }
            set
            {
                tl.LogMessage("GuideRateDeclination Set", value.ToString());
                double guideSidereal = value / sidereal_speed;
                CommandString("speed guide " + guideSidereal, false);
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                double guideRate;
                if (!Double.TryParse(CommandString("speed guide", false), out guideRate))
                {
                    throw new ASCOM.DriverException("GuideRateRightAscension");
                }
                guideRate *= sidereal_speed;
                tl.LogMessage("GuideRateRightAscension Get", guideRate.ToString());
                return guideRate;
            }
            set
            {
                tl.LogMessage("GuideRateRightAscension Set", value.ToString());
                double guideSidereal = value / sidereal_speed;
                CommandString("speed guide " + guideSidereal, false);
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                bool isPulseGuiding = CommandString("status", false).Contains("guiding");
                tl.LogMessage("IsPulseGuiding Get", isPulseGuiding.ToString());
                return isPulseGuiding;
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            tl.LogMessage("MoveAxis", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("MoveAxis");
        }

        public void Park()
        {
            tl.LogMessage("Park", "");
            CommandString("goto index", false);
            atHome = true;
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (IsPulseGuiding || !Tracking)
            {
                throw new ASCOM.DriverException("PulseGuide");
            }
            if (AtPark)
            {
                throw new ASCOM.ParkedException("PulseGuide");
            }
            string dir = "";
            switch (Direction)
            {
                case GuideDirections.guideNorth:
                    dir = "north";
                    break;
                case GuideDirections.guideSouth:
                    dir = "south";
                    break;
                case GuideDirections.guideEast:
                    dir = "east";
                    break;
                case GuideDirections.guideWest:
                    dir = "west";
                    break;
                default:
                    return;
            }
            tl.LogMessage("PulseGuide", dir + " " + Duration.ToString() + " ms");
            CommandString("guide " + dir + " " + Duration.ToString(), false);
        }

        public double RightAscension
        {
            get
            {
                getPointingEq(out double ra, out double dec);
                tl.LogMessage("RightAscension", "Get - " + utilities.HoursToHMS(ra));
                return ra;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                double ra = 0.0;
                if (!Double.TryParse(CommandString("speed slew", false), out ra))
                {
                    tl.LogMessage("RightAscensionRate", "Get failed ");
                }
                else
                    tl.LogMessage("RightAscensionRate", "Get - " + ra.ToString());
                return ra;
            }
            set
            {
                tl.LogMessage("RightAscensionRate Set", value.ToString());
                CommandString("speed slew " + value.ToString(), false);
            }
        }

        public void SetPark()
        {
            tl.LogMessage("SetPark", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SetPark");
        }

        public PierSide SideOfPier
        {
            get
            {
                PierSide ps = PierSide.pierUnknown;
                string ret = CommandString("read mount", false); // Read mount position
                tl.LogMessage("MOUNTPOS", ret);
                string[] pos = ret.Split(' ');
                if (pos.Length >= 2 && Double.TryParse(pos[1], out double dec)) // Mount dec pos
                {
                    dec = Math.IEEERemainder(dec, 360.0);
                    if (dec >= 0)
                    {
                        // Point to west half of sky
                        ps = PierSide.pierEast;
                    }
                    else
                    {
                        // Point to east half of sky
                        ps = PierSide.pierWest;
                    }
                }
                tl.LogMessage("SideOfPier Get", ps.ToString());
                return ps;
            }
            set
            {
                tl.LogMessage("SideOfPier Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SideOfPier", true);
            }
        }

        public double SiderealTime
        {
            get
            {
                // get greenwich sidereal time: https://en.wikipedia.org/wiki/Sidereal_time
                //double siderealTime = (18.697374558 + 24.065709824419081 * (utilities.DateUTCToJulian(DateTime.UtcNow) - 2451545.0));

                // alternative using NOVAS 3.1
                double siderealTime = 0.0;
                using (var novas = new ASCOM.Astrometry.NOVAS.NOVAS31())
                {
                    var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                    novas.SiderealTime(jd, 0, novas.DeltaT(jd),
                        ASCOM.Astrometry.GstType.GreenwichApparentSiderealTime,
                        ASCOM.Astrometry.Method.EquinoxBased,
                        ASCOM.Astrometry.Accuracy.Reduced, ref siderealTime);
                }
                // allow for the longitude
                siderealTime += SiteLongitude / 360.0 * 24.0;
                // reduce to the range 0 to 24 hours
                siderealTime = siderealTime % 24.0;
                tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                tl.LogMessage("SiteElevation Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteElevation", false);
            }
            set
            {
                tl.LogMessage("SiteElevation Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteElevation", true);
            }
        }

        public double SiteLatitude
        {
            get
            {
                if(!Double.TryParse(CommandString("config latitude", false), out double lat))
                {
                    throw new ASCOM.ValueNotSetException("SiteLatitude");
                }
                tl.LogMessage("SiteLatitude Get", lat.ToString());
                return lat;
            }
            set
            {
                if (value > 90 || value < -90)
                {
                    throw new ASCOM.InvalidValueException("SiteLatitude");
                }
                tl.LogMessage("SiteLatitude Set", value.ToString());
                CommandString("config latitude " + value.ToString(), false);
                CommandBlind("save", false);
            }
        }

        public double SiteLongitude
        {
            get
            {
                if (!Double.TryParse(CommandString("config longitude", false), out double lon))
                {
                    throw new ASCOM.ValueNotSetException("SiteLongitude");
                }
                tl.LogMessage("SiteLatitude Get", lon.ToString());
                return lon;
            }
            set
            {
                if (value > 180 || value < -180)
                {
                    throw new ASCOM.InvalidValueException("SiteLongitude");
                }
                tl.LogMessage("SiteLongitude Set", value.ToString());
                CommandString("config longitude " + value.ToString(), false);
                CommandBlind("save", false);
            }
        }

        public short SlewSettleTime
        {
            get
            {
                tl.LogMessage("SlewSettleTime Get", Settings.Default.slewSettleTime.ToString());
                return Settings.Default.slewSettleTime;
            }
            set
            {
                if (value < 0)
                {
                    throw new ASCOM.InvalidValueException("SlewSettleTime");
                }
                tl.LogMessage("SlewSettleTime Set", value.ToString());
                Settings.Default.slewSettleTime = value;
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToAltAz");
            }
            if(Azimuth > 360 || Azimuth < 0 || Altitude > 90 || Altitude < 0)
            {
                throw new ASCOM.InvalidValueException("SlewToAltAz");
            }
            tl.LogMessage("SlewToAltAz", "");
            double ra = 0, dec = 0;
            lock (trans)
            {
                var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                trans.SiteLatitude = SiteLatitude;
                trans.SiteLongitude = SiteLongitude;
                trans.SiteElevation = Settings.Default.elevation;
                trans.SiteTemperature = Settings.Default.temperature;
                trans.JulianDateUTC = jd;
                trans.Refraction = Settings.Default.refraction;
                trans.SetAzimuthElevation(Azimuth, Altitude);
                trans.Refresh();
                ra = trans.RAJ2000;
                dec = trans.DecJ2000;
            }
            slewTo(ra, dec, true);
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToAltAzAsync");
            }
            if (Azimuth > 360 || Azimuth < 0 || Altitude > 90 || Altitude < 0)
            {
                throw new ASCOM.InvalidValueException("SlewToAltAz");
            }
            tl.LogMessage("SlewToAltAzAsync", "");

            double ra = 0, dec = 0;
            lock (trans)
            {
                var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                trans.SiteLatitude = SiteLatitude;
                trans.SiteLongitude = SiteLongitude;
                trans.SiteElevation = Settings.Default.elevation;
                trans.SiteTemperature = Settings.Default.temperature;
                trans.JulianDateUTC = jd;
                trans.Refraction = Settings.Default.refraction;
                trans.SetAzimuthElevation(Azimuth, Altitude);
                trans.Refresh();
                ra = trans.RAJ2000;
                dec = trans.DecJ2000;
            }
            slewTo(ra, dec, false);
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToCoordinates");
            }
            if(RightAscension > 24 || RightAscension < 0 || Declination > 90 || Declination < -90)
            {
                throw new ASCOM.InvalidValueException("SlewToCoordinates");
            }
            tl.LogMessage("SlewToCoordinates", "");
            slewTo(RightAscension, Declination, true);
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToCoordinatesAsync");
            }
            if (RightAscension > 24 || RightAscension < 0 || Declination > 90 || Declination < -90)
            {
                throw new ASCOM.InvalidValueException("SlewToCoordinatesAsync");
            }
            tl.LogMessage("SlewToCoordinatesAsync", "");
            slewTo(RightAscension, Declination, false);
        }

        public void SlewToTarget()
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToTarget");
            }
            if (TargetRightAscension > 24 || TargetRightAscension < 0 || TargetDeclination > 90 || TargetDeclination < -90)
            {
                throw new ASCOM.InvalidValueException("SlewToTarget");
            }
            tl.LogMessage("SlewToTarget", "");
            SlewToCoordinates(TargetRightAscension, TargetDeclination);
        }

        public void SlewToTargetAsync()
        {
            if (AtPark)
            {
                throw new ASCOM.ParkedException("SlewToTargetAsync");
            }
            if (TargetRightAscension > 24 || TargetRightAscension < 0 || TargetDeclination > 90 || TargetDeclination < -90)
            {
                throw new ASCOM.InvalidValueException("SlewToTargetAsync");
            }
            tl.LogMessage("SlewToTargetAsync", "");
            SlewToCoordinatesAsync(TargetRightAscension, TargetDeclination);
        }

        public bool Slewing
        {
            get
            {
                bool slewing = CommandString("status", false) == "slewing";
                tl.LogMessage("Slewing Get", slewing.ToString());
                return slewing;
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            tl.LogMessage("SyncToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RightAscension, double Declination)
        {
            tl.LogMessage("SyncToCoordinates", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToCoordinates");
        }

        public void SyncToTarget()
        {
            tl.LogMessage("SyncToTarget", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToTarget");
        }

        private double targetDec = Double.NaN;
        private double targetRA = Double.NaN;

        public double TargetDeclination
        {
            get
            {
                if (Double.IsNaN(targetDec))
                {
                    throw new ASCOM.ValueNotSetException("TargetDeclination");
                }
                tl.LogMessage("TargetDeclination Get", targetDec.ToString());
                return targetDec;
            }
            set
            {
                if (value > 90 || value < -90)
                {
                    throw new ASCOM.InvalidValueException("TargetDeclination");
                }
                tl.LogMessage("TargetDeclination Set", value.ToString());
                targetDec = value;
            }
        }

        public double TargetRightAscension
        {
            get
            {
                if (Double.IsNaN(targetRA))
                {
                    throw new ASCOM.ValueNotSetException("TargetRightAscension");
                }
                tl.LogMessage("TargetRightAscension Get", targetRA.ToString());
                return targetRA;
            }
            set
            {
                if (value > 24 || value < 0)
                {
                    throw new ASCOM.InvalidValueException("TargetDeclination");
                }
                tl.LogMessage("TargetRightAscension Set", value.ToString());
                targetRA = value;
            }
        }

        public bool Tracking
        {
            get
            {
                string status = CommandString("status", false);
                bool tracking = status.Contains("tracking");
                tl.LogMessage("Tracking", "Get - " + tracking.ToString());
                return tracking;
            }
            set
            {
                tl.LogMessage("Tracking Set", value.ToString());
                if (value)
                {
                    CommandBlind("track", false);
                }
                else
                {
                    CommandBlind("stop track", false);
                }
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                DriveRates dr = DriveRates.driveSidereal;
                double rate;
                if(!Double.TryParse(CommandString("speed track", false), out rate)){
                    throw new ASCOM.DriverException("TrackingRate");
                }
                if (Math.Abs(rate - 0.9763) <= 1e-4)
                {
                    dr = DriveRates.driveLunar;
                }
                else if (Math.Abs(rate - 0.9973) <= 1e-4)
                {
                    dr = DriveRates.driveSolar;
                }
                else if (Math.Abs(rate - 0.9998) <= 1e-4)
                {
                    dr = DriveRates.driveKing;
                }
                tl.LogMessage("TrackingRate Get", dr.ToString());
                return dr;
            }
            set
            {
                tl.LogMessage("TrackingRate Set", value.ToString());
                switch (value)
                {
                    case DriveRates.driveSidereal:
                        CommandString("speed track 1.000000", false);
                        break;
                    case DriveRates.driveLunar:
                        CommandString("speed track 0.976331", false);
                        break;
                    case DriveRates.driveSolar:
                        CommandString("speed track 0.997274", false);
                        break;
                    case DriveRates.driveKing:
                        CommandString("speed track 0.999727", false);
                        break;
                    default:
                        break;
                }
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                ITrackingRates trackingRates = new TrackingRates();
                tl.LogMessage("TrackingRates", "Get - ");
                foreach (DriveRates driveRate in trackingRates)
                {
                    tl.LogMessage("TrackingRates", "Get - " + driveRate.ToString());
                }
                return trackingRates;
            }
        }

        public DateTime UTCDate
        {
            get
            {
                int unixTimestamp;
                if(!int.TryParse(CommandString("time stamp", false), out unixTimestamp))
                {
                    throw new ASCOM.DriverException("UTCDate");
                }
                DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTimestamp);
                tl.LogMessage("UTCDate", "Get - " + String.Format("MM/dd/yy HH:mm:ss", utcDate));
                return utcDate;
            }
            set
            {
                int unixTimestamp = (int)Math.Truncate((value - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                CommandBlind("settime " + unixTimestamp.ToString(), false);
                tl.LogMessage("UTCDate", "Set - " + String.Format("MM/dd/yy HH:mm:ss", value));
            }
        }

        public void Unpark()
        {
            tl.LogMessage("Unpark", "");
            atHome = false;
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Telescope";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // check that the driver hardware connection exists and is connected to the hardware
                if (!connectedState)
                    return false;
                if (!serial.IsOpen)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            Settings.Default.Reload();
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            Settings.Default.Save();
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
