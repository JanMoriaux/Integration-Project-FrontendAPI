using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndAPI.Utility
{
    public class UUIDGenerator
    {
        private static readonly string _baseURL = "http://ec2-52-29-5-250.eu-central-1.compute.amazonaws.com:3000/api";
        private static readonly HttpClient _client = new HttpClient();


        public static string GenerateUserUUID(string email)
        {
            object body =  new { Email = email };
            var jsonBody = JsonConvert.SerializeObject(body);

            var postTask = _client.PostAsync(String.Format("{0}/user/uuid",_baseURL),new StringContent(jsonBody,Encoding.UTF8,"application/json"));
            var response = postTask.Result.Content.ReadAsStringAsync().Result;
            var responseObject = JObject.Parse(response);
            
            return responseObject["uuid"].ToString();
        }

        public static string GenerateEventUUID(string name)
        {
            object body = new { Name = name};
            var jsonBody = JsonConvert.SerializeObject(body);

            var postTask = _client.PostAsync(String.Format("{0}/event/uuid", _baseURL), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var response = postTask.Result.Content.ReadAsStringAsync().Result;
            var responseObject = JObject.Parse(response);

            return responseObject["uuid"].ToString();
        }

        public static string GenerateActivityUUID(string name,string eventUUID)
        {
            object body = new { Name = name,EventUUID = eventUUID };
            var jsonBody = JsonConvert.SerializeObject(body);

            var postTask = _client.PostAsync(String.Format("{0}/activity/uuid", _baseURL), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var response = postTask.Result.Content.ReadAsStringAsync().Result;
            var responseObject = JObject.Parse(response);

            return responseObject["uuid"].ToString();
        }

        public static string GenerateReservationUUID(string activityUUID, string visitorUUID)
        {
            object body = new { ActivityUUID = activityUUID, UserUUID= visitorUUID};
            var jsonBody = JsonConvert.SerializeObject(body);

            var postTask = _client.PostAsync(String.Format("{0}/reservation/uuid", _baseURL), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
            var response = postTask.Result.Content.ReadAsStringAsync().Result;
            var responseObject = JObject.Parse(response);

            return responseObject["uuid"].ToString();
        }
    }
}
