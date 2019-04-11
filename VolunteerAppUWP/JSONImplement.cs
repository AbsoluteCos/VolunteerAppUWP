using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolunteerAppUWP
{
    public static class JSONImplement
    {
        public static async Task<List<VOpp>> getOppurtunities(CancellationToken ct)
        {
            using (HttpClient hc = new HttpClient())
            using (HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Get, "https://api.betterplace.org/de/api_v4/projects.json?facets=completed%3Afalse"))
            using (var response = await hc.SendAsync(hrm, ct))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<VOpp>>(content);
            }
        }
    }
}
