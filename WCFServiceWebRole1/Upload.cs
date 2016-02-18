using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFServiceWebRole1
{
    [DataContract]
    public class Upload
    {
        public Upload()
        {
        }

        [DataMember(Name = "blob")]
        public String blob { get; set; }

        [DataMember(Name = "nameFile")]
        public String nameFile { get; set; }

        [DataMember(Name = "pathFile")]
        public String pathFile { get; set; }
    }
}