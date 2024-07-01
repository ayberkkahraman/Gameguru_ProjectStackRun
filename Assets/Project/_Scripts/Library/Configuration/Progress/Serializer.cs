using Unity.Plastic.Newtonsoft.Json;

namespace Project._Scripts.Library.Configuration.Progress
{
  public static class Serializer
  {
    public static string Serialize<T>(T toSerialize) => JsonConvert.SerializeObject(toSerialize, Formatting.Indented);
    public static T DeSerialize<T>(string toDeSerialize) => JsonConvert.DeserializeObject<T>(toDeSerialize);
  }

}
