using System;

namespace RegisterSQLServers.SSMS.Model
{
    public class DatabaseInstance
    {
        public string Name { get; set; }

        public string DescriptiveName
        {
            get
            {
                return Name + " on " + FullyQualifiedInstanceName;
            }
        }

        public string Description { get; set; }

        public string ServerName { get; set; }

        public string InstanceName { get; set; }

        public string FullyQualifiedInstanceName
        {
            get
            {
                if (string.Equals(ServerName, InstanceName, StringComparison.InvariantCultureIgnoreCase))
                    return ServerName;
                else
                    return ServerName + "\\" + InstanceName;
            }
        }

        public bool UseCustomConnectionColor { get; set; }

        public int CustomConnectionColorARGB { get; set; }
    }
}
