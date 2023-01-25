using System.Threading.Tasks;
using RestSharp;
using System;

namespace FrontEndClient.Models
{
  public class ApiHelper
  {
    public static async Task<string> GetAll(string route)
    {
      RestClient client = new RestClient("https://localhost:7210/");
      RestRequest request = new RestRequest($"api/{route}", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }
    public static async Task<string> Get(string route, int id)
    {
      RestClient client = new RestClient("https://localhost:7210/");
      RestRequest request = new RestRequest($"api/{route}/{id}", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }
    public static async Task<string> Search(string route, string search)
    {
      RestClient client = new RestClient("https://localhost:7210/");
      RestRequest request = new RestRequest($"api/{route}?search={search}", Method.Get);
      RestResponse response = await client.GetAsync(request);
      return response.Content;
    }
  }
}
