using Newtonsoft.Json;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics.Responses;

namespace ZgnWebApi.Integrations.BlueBotics.Models
{
    public class BlueBotics
    {
        public static class AppData
        {
            public static Login? Login { get; set; }
        }
        public interface IParameterValue
        {
            public string Payload { get; set; }
        }
        public class Parameter
        {
            public IParameterValue? Value { get; set; }
            public string? Desc { get; set; }
            public string? Type { get; set; }
            public string? Name { get; set; }
        }

        public class Responses
        {
            public class Response
            {
                public int RetCode { get; set; }
                public string? Error { get; set; }
            }
            public class Response<T>:Response
            {
                public T? Payload { get; set; }
            }
            public class Login
            {
                public User? User { get; set; }
                public string? SessionToken { get; set; }
                public string? Username { get; set; }
            }
            public class User
            {
                public string? FirstName { get; set; }
                public string? EmployeeNr { get; set; }
                public string? LastName { get; set; }
            }
            public class DataResponse<T>
            {
                public T? Data { get; set; }
            }
            public class FilterInfo<T>
            {
                public int Orientation { get; set; }
                public IList<int>? Offset { get; set; }
                public T? Data { get; set; }
                public string? Format { get; set; }
                public string? Group { get; set; }
            }
            public class Map
            {
                public int Id { get; set; }
                public int Level { get; set; }
                public List<Layer>? Layers { get; set; }
            }
            public class Layer
            {
                public string? Name { get; set; }
                public string? Desc { get; set; }
                public List<Symbol>? Symbols { get; set; }
            }
            public class Symbol
            {
                public string? SymbolId { get; set; }
                public string? Name { get; set; }
                public int Id { get; set; }
            }
            public class VehiclesData
            {
                public List<Vehicle>? Vehicles { get; set; }
            }
            public class Vehicle
            {
                public string? Name { get; set; }
                public int Operatingstate { get; set; }
                public string? MissionId { get; set; }
                public bool Isloaded { get; set; }
                public State State { get; set; }
                public DateTime Timestamp { get; set; }
            }
            public class State
            {
                [JsonProperty(PropertyName = "connection.ok")]
                public string[]? Connection { get; set; }
                [JsonProperty(PropertyName = "battery.info")]
                public string[]? BatteryInfo { get; set; }
            }
            public class AddMisson
            {
                public string[]? Rejectedmissions { get; set; }
                public string[]? Pendingmissions { get; set; }
                public string[]? Acceptedmissions { get; set; }
            }
            public class MissionData
            {
                public List<Mission>? Missions { get; set; }
            }
            public class Mission
            {
                public string? Missionid { get; set; }
                public int State { get; set; }
                public int Navigationstate { get; set; }
                public int Transportstate { get; set; }
                public string? Fromnode { get; set; }
                public string? Tonode { get; set; }
                public bool Isloaded { get; set; }
                public string? Payload { get; set; }
                public int Priority { get; set; }
                public string? Assignedto { get; set; }
                public DateTime? Deadline { get; set; }
                public string? Missiontype { get; set; }
                public string? Groupid { get; set; }
                public bool Istoday { get; set; }
                public int Schedulerstate { get; set; }
                public bool Askedforcancellation { get; set; }
            }
            public class CancelMission
            {
                public string? MissionId { get; set; }
                public bool? Cancelled { get; set; }
            }
            public class ListCancelMission
            {
                public string[]? Cancelled { get; set; }
            }
        }
    }
   
    
}
