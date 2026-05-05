using Bogus;
using PolyplayAPI.Models;

namespace PolyplayAPI
{
    public class FakeData
    {
        private Faker<Game> TestGames;
        public FakeData()
        {
            Randomizer.Seed = new Random(67);

            var tags = new[] { "incremental", "simulator", "action", "roguelike", "deckbuilder" };
            var usernames = new[] { "alex", "axolotl", "equinox", "gabi" };

            var gameId = 1;
            TestGames = new Faker<Game>()
                .StrictMode(true)
                .RuleFor(game => game.Id, f => gameId++)
                .RuleFor(game => game.Name, f => f.Name.JobTitle())
                .RuleFor(game => game.PostedDate, f => f.Date.Past(1))
                .RuleFor(game => game.MainTag, f => f.PickRandom(tags))
                .RuleFor(game => game.Description, f => f.Lorem.Paragraph())
                .RuleFor(game => game.Rating, f => f.Random.Float() * 5)
                .RuleFor(game => game.Developer, f => f.PickRandom(usernames))
                .RuleFor(game => game.IsPublished, f => f.Random.Bool())
                .RuleFor(game => game.ThumbnailPath, f => "../../assets/logo.png");
        }

        public List<Game> GenerateGames(int noGames)
        {
            return TestGames.Generate(noGames);
        }

    }
}
