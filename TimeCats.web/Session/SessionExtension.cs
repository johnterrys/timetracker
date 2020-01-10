using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TimeCats.Session
{
    public static class SessionExtension
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void DestroySession<T>(this ISession session, string username)
        {
            session.SetObjectAsJson(username, default(T));
        }
    }
}