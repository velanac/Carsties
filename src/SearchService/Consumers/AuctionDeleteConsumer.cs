using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeleteConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine("--> Consuming auction delete: " + context.Message.Id);

        var reslut = await DB.DeleteAsync<Item>(context.Message.Id);

        if (!reslut.IsAcknowledged) throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
    }
}