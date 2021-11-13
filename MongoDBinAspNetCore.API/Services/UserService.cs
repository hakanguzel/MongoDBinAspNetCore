using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBinAspNetCore.API.Configurations;
using MongoDBinAspNetCore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBinAspNetCore.API.Services
{
    public class UserService
    {
        private readonly IMongoCollection<Users> _user;
        private readonly MongoDatabaseConfiguration _settings;
        public UserService(IOptions<MongoDatabaseConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _user = database.GetCollection<Users>(_settings.CollectionName);
        }
        public async Task<List<Users>> GetAllAsync()
        {
            return await _user.Find(c => true).ToListAsync();
        }
        public async Task<Users> GetByIdAsync(string id)
        {
            return await _user.Find<Users>(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Users> CreateAsync(Users user)
        {
            await _user.InsertOneAsync(user);
            return user;
        }
        public async Task UpdateAsync(string id, Users user)
        {
            await _user.ReplaceOneAsync(c => c.Id == id, user);
        }
        public async Task DeleteAsync(string id)
        {
            await _user.DeleteOneAsync(c => c.Id == id);
        }
    }
}
