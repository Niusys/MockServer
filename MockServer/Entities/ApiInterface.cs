using MockServer.MongoStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockServer.Entities
{
    public class ApiInterface : MongoEntityBase
    {
        public string Category { get; set; }
        public string RequestPath { get; set; }
        public string ResponseResult { get; set; }
        public string Remark { get; set; }
    }
}
