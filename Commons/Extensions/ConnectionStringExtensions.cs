using System;
using System.Configuration;

namespace Commons.Extensions
{
    public static class ConnectionStringExtensions
    {
        public static string GetConnectionString(this ConnectionStringSettingsCollection collection)
        {

            //This is super ugly, gotta do this if i want to keep working on 2 computers. Or keep changing the connection string in app.config.
            return collection[Environment.MachineName switch
                    {
                        "DESKTOP-D40UEJC" => "Home",
                        _ => "Work"
                    }
            ].ConnectionString;
        }
    }
}