   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Spatial.Services.News;

public static class LiveuaMapService
{
    public static async Task<LiveuaMapApiResults> GetAsync(string region, DateTime date)
    {
        var days = (date.Date - new DateTime(2017, 12, 01)).Days;

        long minutes = 1512073800 + days * 86400;

        var url = $"http://{region}.liveuamap.com/ajax/do?act=pts&curid=0&time={minutes}&last=0";

        var result = await IRI.Sta.Common.Helpers.NetHelper.HttpGetAsync<LiveuaMapApiResults>(url);
          
        return result?.Result;
    }
}
