using Bonitet.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Bonitet.Web.Classes
{
    public class SerializeHelper : DefaultContractResolver
    {
        IEnumerable<string> lstExclude;

        Dictionary<Type, string[]> TypeProperties;


        Type[] SerializableObjectTypes = new Type[]{
            typeof(String),
            typeof(string),
            typeof(int),
            typeof(ReportResponse),
            typeof(c_UserReportObj)
        };

        public SerializeHelper()
        {
            TypeProperties = new Dictionary<Type, string[]>();
        }

        public SerializeHelper(IEnumerable<string> excludedProperties)
        {
            lstExclude = excludedProperties;
            TypeProperties = new Dictionary<Type, string[]>();
        }

        public static JsonSerializerSettings GetDefaultSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            settings.Error = delegate(object sender2, Newtonsoft.Json.Serialization.ErrorEventArgs e2)
            {
                e2.ErrorContext.Handled = true;
            };

            settings.ContractResolver = new SerializeHelper(new string[] { 
                "ReportResponse",
                "c_UserReportObj"
            });

            settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            return settings;
        }
    }
}