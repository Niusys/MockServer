using MockServer.Entities;

namespace MockServer.Models
{
    public class ApiInterfaceListItem
    {
        public string InterfaceId { get; set; }
        public string RequestPath { get; set; }
        public string Category { get; set; }
        public string Remark { get; set; }
    }
}
