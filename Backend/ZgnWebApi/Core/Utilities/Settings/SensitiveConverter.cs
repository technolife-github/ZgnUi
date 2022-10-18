using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.IoC;

namespace ZgnWebApi.Core.Utilities.Settings
{
    public class SensitiveConverter : JsonConverter<string>
    {
        public int MaskStart { get; }
        public int MaskEnd { get; }
        public bool Mask { get; }
        public bool MaskType { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SensitiveConverter(int maskStart = 0, int maskEnd = 0, string claims = "*", bool maskType = false)
        {
            MaskStart = maskStart;
            MaskEnd = maskEnd;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            List<string> claimList = claims.Split(',').ToList();
            claimList.Add("Supervisor");
            Mask = claims == "*" ? true :
                (_httpContextAccessor.HttpContext.User.ClaimRoles().Where(r => !claimList.Contains(r)).Count() > 0 ?
                false : true);
            MaskType = maskType;

        }

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            if (Mask && !string.IsNullOrWhiteSpace(value) && (MaskStart > 0 || MaskEnd < 0))
            {
                var sb = new StringBuilder(value);
                for (var i = MaskStart; !MaskType && i < sb.Length - MaskEnd; sb[i++] = '*') ;
                for (var i = 0; MaskType && i < MaskStart; sb[i++] = '*') ;
                for (var i = sb.Length - MaskEnd; MaskType && i < sb.Length; sb[i++] = '*') ;
                value = sb.ToString();
            }

            writer.WriteStringValue(value);
        }
    }

}
