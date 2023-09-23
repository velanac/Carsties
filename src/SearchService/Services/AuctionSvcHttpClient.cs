using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;


public class AuctionSvcHttpClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public AuctionSvcHttpClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdate = await DB.Find<Item, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await _http.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdate);
    }
}