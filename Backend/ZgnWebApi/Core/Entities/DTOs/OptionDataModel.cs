using System.ComponentModel.DataAnnotations.Schema;

namespace ZgnWebApi.Core.Entities.DTOs
{
    [NotMapped]
    public class OptionDataModel
    {
        public object id { get; set; }
        public object text { get; set; }
        public object obj { get; set; }
    }
}
