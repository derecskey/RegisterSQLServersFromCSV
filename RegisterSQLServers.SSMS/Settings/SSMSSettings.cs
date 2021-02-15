using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace RegisterSQLServers.SSMS.Settings
{
    public class SSMSSettings
    {
        public string LocalServerGroupRoot { get; set; }

        public List<CustomColorMapping> CustomColorMappings { get; set; }
    }

    public class CustomColorMapping
    {
        public string EnvironmentTier { get; set; }

        public string ColorInHex { get; set; }

        public int ColorInArgb
        {
            get
            {
                if (!string.IsNullOrEmpty(ColorInHex))
                {
                    var colorInHex = ColorInHex;
                    string sanitizedHex;
                    
                    colorInHex = colorInHex.StartsWith("#") ? colorInHex : "#" + colorInHex;

                    sanitizedHex = colorInHex.Length == 7 ? colorInHex : null;

                    if (sanitizedHex == null)
                    {
                        throw new ArgumentException("Hexadecimal string is not in the correct format #FFFFFF.", nameof(ColorInHex));
                    }

                    var color = System.Drawing.ColorTranslator.FromHtml(sanitizedHex);
                    return color.ToArgb();

                    //var a = (byte) (packedValue >> 0);
                    //var r = (byte)(packedValue >> 24);
                    //var g = (byte)(packedValue >> 16);
                    //var b = (byte)(packedValue >> 8);

                    //return (int)packedValue;
                }

                return Color.White.ToArgb(); //white as a packed argb int
            }
        }
    }
}
