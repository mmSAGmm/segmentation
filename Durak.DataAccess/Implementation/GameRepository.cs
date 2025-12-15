using Common.DataAccess.Abstraction;
using Common.DataAccess.Options;
using Dapper;
using Durak.DataAccess.Abtractions;
using Durak.DomainModels;
using Durak.DomainModels.GameEngine.Abtractions;
using Durak.DomainModels.GameEngine.Implementation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;

namespace Durak.DataAccess.Implementation
{
    public class GameRepository(
        IConnectionProvider connectionProvider,
        IOptions<QueryOption> option) : IGameRepository
    {
        DbConnection Connection => connectionProvider.Get();

        public async Task CreateGame(MultiplayerGame game)
        {
            await Connection.ExecuteAsync("INSERT INTO Games VALUES(@Id, @Json)",
                new { game.Id, Json = JsonConvert.SerializeObject(game) },
                commandTimeout: option.Value.TimeoutSeconds);
        }

        public async Task DeleteGame(Guid id)
        {
            await Connection.ExecuteAsync("DELETE FROM Games WHERE Id = @Id",
               new { Id = id },
               commandTimeout: option.Value.TimeoutSeconds);
        }

        public async Task<MultiplayerGame> GetGame(Guid id)
        {
            var gameData = await Connection.QueryFirstOrDefaultAsync<string>(
                "SELECT Json FROM Games WHERE Id = @Id",
                new { Id = id }, commandTimeout: option.Value.TimeoutSeconds);

            var game = JsonConvert.DeserializeObject<MultiplayerGame>(gameData);
            return game;
        }

        public async Task Init()
        {
            await Connection.ExecuteAsync(
             @"
CREATE TABLE Games 
(
    Id VARCHAR(20) PRIMARY KEY,
    Json TEXT NOT NULL
);");
        }

        public async Task UpdateGame(MultiplayerGame game)
        {
            await Connection.ExecuteAsync("UPDATE Games SET Json=@Json WHERE Id = @Id",
               new { game.Id, Json = JsonConvert.SerializeObject(game) },
               commandTimeout: option.Value.TimeoutSeconds);
        }
    }
}
