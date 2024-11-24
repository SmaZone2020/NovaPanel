using Newtonsoft.Json;
namespace N0v4P4n31.Data.Languages
{
    public static class OriginWord
    {
        public static string DefLanguage { get; set; }

        public static Def Load(string langS)
        {

                return JsonConvert.DeserializeObject<Def>(
                         JsonConvert.SerializeObject(new En(), Formatting.Indented)
                         );

        }
    }
}
