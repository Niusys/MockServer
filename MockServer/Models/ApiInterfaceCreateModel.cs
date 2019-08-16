using MockServer.Entities;

namespace MockServer.Models
{
    public class ApiInterfaceCreateModel
    {
        public string Category { get; set; }
        public string RequestPath { get; set; }
        public string ResponseResult { get; set; }
        public string Remark { get; set; }
    }

    public class ApiInterfaceModifyModel : ApiInterfaceCreateModel
    {
        public string InterfaceId { get; set; }
    }

    public class ApiInterfaceTestModel : ApiInterfaceModifyModel
    {

    }
}
