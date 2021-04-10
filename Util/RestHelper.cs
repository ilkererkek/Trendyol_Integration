using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trendyol_Integration.ViewModels;

namespace Trendyol_Integration.Util
{
    public class RestHelper
    {
        private const string username = "Bnib0D0RMditHE4NEiV8";
        private const string password = "rAsrd6PpPEDiahvsZEKy";
        private const string url = "https://api.trendyol.com/sapigw";
       
        RestClient client;
        public RestHelper()
        {
            client = new RestClient(url);
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            client.AddDefaultHeader("user-agent", "235333-PiaLab");
        }
        public string GetRequest(string url)
        {
            var request = new RestRequest(url);
            var response = client.Get(request);
            return response.Content;
        }
        public IRestResponse PostRequest(string url,string data)
        {
            var request = new RestRequest(url, Method.POST, DataFormat.Json);
            request.AddJsonBody(data);
            IRestResponse response = client.Post(request);
            return response;
        }
        
    }
}