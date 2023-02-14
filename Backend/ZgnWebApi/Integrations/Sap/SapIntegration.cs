using Newtonsoft.Json;
using System.Text;
using ZgnWebApi.Integrations.Sap.Models;

namespace ZgnWebApi.Integrations.Sap
{
    public class SapGroupDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class SapConfig
    {
        public string ApiUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<SapGroupDto> Groups { get; set; }
    }
    public interface ISapIntegration
    {
        public SapConfig Config { get; }
        SapList GetData(string groupName);
    }
    public class SapIntegration : ISapIntegration
    {
        public SapConfig Config { get; }
        private readonly IConfiguration Configuration;
        private HttpClient HttpClient;
        public SapIntegration(IConfiguration configuration)
        {
            this.Configuration = configuration;
            Config = Configuration.GetSection("Sap").Get<SapConfig>();
        }
        public SapList? GetData(string groupName)
        {
            HttpClient = new HttpClient();
            // Set the basic authentication header
            var byteArray = Encoding.ASCII.GetBytes($"{Config.UserName}:{Config.Password}");
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Make the GET request
            var response = HttpClient.GetAsync($"{Config.ApiUrl.Replace("{Path}", groupName)}");

            // Read the response
            var responseString = response.Result.Content.ReadAsStringAsync();
            responseString.Wait();
            return JsonConvert.DeserializeObject<SapList>(responseString.Result);
            //return JsonConvert.DeserializeObject<SapList>(GetSampleData().Replace("'","\""));

        }
        private string GetSampleData()
        {
            return @"{
  'ITAB': [
    { 'MATNR': '000000010000047972', 'MAKTX': 'STRAFOR_FAN_ICIN' },
    {
      'MATNR': '000000010000047550',
      'MAKTX': 'STRAFOR_DAMLAMA_TAVASI_HAVA_CIKIS_IZOLAS'
    },
    { 'MATNR': '000000010000047132', 'MAKTX': 'RAF_DESTEK_STRAFORU_600-800' },
    { 'MATNR': '000000010000046337', 'MAKTX': 'STRAFOR_KESME_1200*500*30' },
    { 'MATNR': '000000010000046331', 'MAKTX': 'STRAFOR_KESME_30x50x70  ' },
    {
      'MATNR': '000000010000046327',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046325',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046323',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046321',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046318',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046316',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    {
      'MATNR': '000000010000046314',
      'MAKTX': 'KESME_STRAFOR_SC505_SC600_CEZAYIR_SKD_KA'
    },
    { 'MATNR': '000000010000046306', 'MAKTX': 'KESME_STRAFOR_205x590x20' },
    { 'MATNR': '000000010000046304', 'MAKTX': 'KESME_STRAFOR_1030x630x20' },
    { 'MATNR': '000000010000046267', 'MAKTX': 'STRAFOR_KESME_40x15x90' },
    {
      'MATNR': '000000010000046228',
      'MAKTX': 'STRAFOR_DESTEK_AMBALAJ_RA_AYDINLATMA_COO'
    },
    {
      'MATNR': '000000010000046227',
      'MAKTX': 'STRAFOR_DESTEK_AMBALAJ_RA_AYDINLATMA_SC6'
    },
    { 'MATNR': '000000010000046205', 'MAKTX': 'STRAFOR_KESME_150x20x20' },
    {
      'MATNR': '000000010000046191',
      'MAKTX': 'STRAFOR_45CM_CANOPY_ 989*215*50'
    },
    { 'MATNR': '000000010000046021', 'MAKTX': 'STRAFOR_UST_YALITIM_SC505' },
    {
      'MATNR': '000000010000046020',
      'MAKTX': 'STRAFOR_SAG_SOL_PLS_CRC_DFSG_FF/FC'
    },
    {
      'MATNR': '000000010000046014',
      'MAKTX': 'STRAFOR TABAN D 372 WIC ST MERLONÝ'
    },
    {
      'MATNR': '000000010000046012',
      'MAKTX': 'STRAFOR ARKA DESTEK D 372 WIC ST MERLONI'
    },
    {
      'MATNR': '000000010000046010',
      'MAKTX': 'STRAFOR ÜST YAN D 372 WIC ST MERLONÝ'
    },
    {
      'MATNR': '000000010000046006',
      'MAKTX': 'STRAFOR S 355 OFC EVAP ÜSTÜ 610*310*17'
    },
    { 'MATNR': '000000010000045978', 'MAKTX': 'STRAFOR CDD DESTEK 40*40*60' },
    { 'MATNR': '000000010000045976', 'MAKTX': 'STRAFOR CDD DESTEK 20*20*60' },
    { 'MATNR': '000000010000045974', 'MAKTX': 'STRAFOR DIK AYDINLATMA DESTEK' },
    {
      'MATNR': '000000010000045958',
      'MAKTX': 'STRAFOR_D_372_SC_CAM_DESTEK_580*100*20'
    },
    { 'MATNR': '000000010000045799', 'MAKTX': 'STRAFOR (SERBETLIK)' },
    {
      'MATNR': '000000010000045798',
      'MAKTX': 'D372WICST CAM DESTEK STR.(AVANTI)'
    },
    { 'MATNR': '000000010000045795', 'MAKTX': 'ARKA UST STRAPOR SC' },
    { 'MATNR': '000000010000045793', 'MAKTX': 'ON UST STRAPOR SC' },
    { 'MATNR': '000000010000045791', 'MAKTX': 'ON ALT STRAFOR SC' },
    {
      'MATNR': '000000010000045784',
      'MAKTX': 'STRAFOR DF KOSE DIKMELER ARCELIK'
    },
    {
      'MATNR': '000000010000045756',
      'MAKTX': 'S86SC UST AMBALAJ KOPUGU SOL (MIL)'
    },
    {
      'MATNR': '000000010000045739',
      'MAKTX': 'STRAFOR DESTEK D210-314-400-500 DFSG AC'
    },
    {
      'MATNR': '000000010000044150',
      'MAKTX': 'STRAFOR_RAF_AMBALAJ_OF_185_109mm'
    },
    { 'MATNR': '000000010000044131', 'MAKTX': 'STRAFOR_KESME_535X250X95' },
    {
      'MATNR': '000000010000044127',
      'MAKTX': 'STRAFOR_D372SC_M4C_YUKSEK_KANOPI_DESTEK'
    },
    {
      'MATNR': '000000010000044123',
      'MAKTX': 'STRAFOR_ORTA_KOPRU_D372SC_M4C_TD_ORTADAN'
    },
    { 'MATNR': '000000010000030709', 'MAKTX': 'fatihp' },
    { 'MATNR': '000000010000044117', 'MAKTX': 'KESME STRAFOR 110*110*240 mm' },
    {
      'MATNR': '000000010000044113',
      'MAKTX': 'STRAFOR_KESME_KANOPI-KAPI_ARASI_G48_300*'
    },
    {
      'MATNR': '000000010000044111',
      'MAKTX': 'STRAFOR_RAF_UST_AMBALAJ_OF_1100'
    },
    {
      'MATNR': '000000010000044086',
      'MAKTX': 'STRAFOR ARKA SOL1335X160X20 MM'
    },
    { 'MATNR': '000000010000044052', 'MAKTX': 'STRAFOR 550X200X5 mm' },
    { 'MATNR': '000000010000044050', 'MAKTX': '25X59X1.5 CM STRAPOR' },
    { 'MATNR': '000000010000060736', 'MAKTX': 'STRAFOR TASIMA DESTEK (SC)' },
    { 'MATNR': '000000020000108337', 'MAKTX': 'GEKAP_TEST' },
    {
      'MATNR': '000000010000029123',
      'MAKTX': 'STRAFOR_KESME_HIGH_KANOPI_500*100*210'
    },
    {
      'MATNR': '000000010000029119',
      'MAKTX': 'STRAFOR_CAM_DESTEK_28_DENSITY_535*100*45'
    },
    {
      'MATNR': '000000010000029118',
      'MAKTX': 'STRAFOR_KESME_28_DENSITY_630*580*20'
    },
    {
      'MATNR': '000000010000029117',
      'MAKTX': 'STRAFOR_KESME_28_DENSITY_630*100*45'
    },
    {
      'MATNR': '000000010000029116',
      'MAKTX': 'KESME_STRAFOR_GOVDE_UST_D372SC_CIFT_KAT_'
    },
    {
      'MATNR': '000000010000029115',
      'MAKTX': 'STRAFOR_KESME_1100*150*50_DENSTY_20KG_M3'
    },
    {
      'MATNR': '000000010000029114',
      'MAKTX': 'STRAFOR_KESME_880*50*50_MM_(DENSITY 20KG'
    },
    {
      'MATNR': '000000010000029113',
      'MAKTX': 'STRAFOR_KESME_880*820*20_MM_(DENSITY 20K'
    },
    {
      'MATNR': '000000010000029111',
      'MAKTX': 'KESME_STRAFOR_LUNA_KAPI_DESTEK_S400_LUNA'
    },
    {
      'MATNR': '000000010000029110',
      'MAKTX': 'KESME_STRAFOR_LUNA_KAPI_DESTEK_S520_WOC'
    },
    {
      'MATNR': '000000010000029106',
      'MAKTX': 'STRAFOR_AMBALAJ_UST_580*100*60'
    },
    {
      'MATNR': '000000010000029103',
      'MAKTX': 'STRAFOR_KESME_ON_770_GUC_AMB_700*100*45'
    },
    {
      'MATNR': '000000010000029102',
      'MAKTX': 'STRAFOR_KESME_UST_770_GUC_AMB_800*630*20'
    },
    {
      'MATNR': '000000010000029101',
      'MAKTX': 'STRAFOR_KESME_880*715*20_MM_(DENSITY 20K'
    }
  ]
}
";
        }

        public class SapException : Exception
        {
            public SapException() : base("Unknown Error!")
            {

            }
            public SapException(string message) : base(message)
            {

            }
        }
    }
}
