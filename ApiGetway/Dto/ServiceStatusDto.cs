using ApiGetway.Enums;

namespace ApiGetway.Dto
{
    public class ServiceStatusDto
    {
        public string Name { get; set; }

        public StatusType Status { get; set; }

        public ServiceStatusDto(string name, StatusType status)
        {
            Name = name;
            Status = status;
        }
    }
}
