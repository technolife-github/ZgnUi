using System.ComponentModel.DataAnnotations.Schema;

namespace ZgnWebApi.Core.Entities.DTOs
{
    [NotMapped]
    public class KeyValueResult
    {
        public string? Name { get; set; }
        public object? Value { get; set; }

        public KeyValueResult()
        {
        }
        public KeyValueResult(string? name, object? value)
        {
            Name = name;
            Value = value;
        }


    }
}
