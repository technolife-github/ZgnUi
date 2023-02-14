namespace ZgnWebApi.Integrations.Sap.Models
{
    public class SapList
    {
        public List<SapData> ITAB {get;set;}
    }
    public class SapData
    {
        public string MATNR { get; set; }
        public string MAKTX { get; set; }
    }
}
