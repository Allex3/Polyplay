using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PolyplayAPI.Models.Chats;

namespace PolyplayAPI.Services;

public class GeneralChatService
{
    private readonly IMongoCollection<GeneralChatMessage> _generalChatCollection;

    public GeneralChatService(
        IOptions<ChatDatabaseSettings> chatDatabaseSettings)
    {
        // Reads the server instance for running database operations, connected with the connection string
        var mongoClient = new MongoClient(
            chatDatabaseSettings.Value.ConnectionString);

        // gets the database from the server that interests me (will be PolyPlayChat)
        var mongoDatabase = mongoClient.GetDatabase(
            chatDatabaseSettings.Value.DatabaseName);

        // gets a specific collection ("table") from the database, for this specific one is GeneralChat (not cat)
        _generalChatCollection = mongoDatabase.GetCollection<GeneralChatMessage>("GeneralChat");
    }

    public async Task<List<GeneralChatMessage>> GetAsync() =>
        await _generalChatCollection.Find(_ => true).ToListAsync();

    public async Task<GeneralChatMessage?> GetAsync(string id) =>
        await _generalChatCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(GeneralChatMessage newMessage) =>
        await _generalChatCollection.InsertOneAsync(newMessage);

    public async Task UpdateAsync(string id, GeneralChatMessage updatedMessage) =>
        await _generalChatCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);

    public async Task RemoveAsync(string id) =>
        await _generalChatCollection.DeleteOneAsync(x => x.Id == id);
}