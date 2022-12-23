using System.Linq.Expressions;
using System.Text;
using Koala.CommandHandlerService.DTOs;
using Koala.CommandHandlerService.Models.Activity;
using Koala.CommandHandlerService.Repositories.Interfaces;
using Koala.CommandHandlerService.Services.Handlers.Commands.Interfaces;

namespace Koala.CommandHandlerService.Services.Handlers.Commands;

public class SpotifyInfoCommand : ICommand
{
    private readonly IReadOnlySet<CommandOptionDto> _spotifyCommands = new HashSet<CommandOptionDto>
    {
        new() { Name = "toptracks", Description = "Gets the most played tracks" },
        new() { Name = "topartists", Description = "Gets the most played artists" },
        new() { Name = "recentlyplayed", Description = "Gets the recently played tracks" },
        new() { Name = "topalbums", Description = "Gets the most played albums" },
    };
    
    private readonly IActivitiesRepository _activitiesRepository;

    public SpotifyInfoCommand(IActivitiesRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }

    public async Task<OptionDto<string>> ExecuteAsync(IReadOnlyDictionary<string, string> argsAndValues)
    {
        Expression<Func<Activity, bool>> spotifyExpression = x => x.Name == "Spotify";
        
        var spotifyOrderByExpression = GetGroupByExpression(argsAndValues);
        var spotifyActivities =
            await _activitiesRepository.GetActivitiesAsync(spotifyExpression, null, null, true);
        
        return new OptionDto<string>(FormatSpotifyList(spotifyActivities));
    }
    
    private static string FormatSpotifyList(IEnumerable<Activity> activities)
    {
        var sb = new StringBuilder();
        foreach (var activity in activities)
        {
            if (activity.SpotifyInfo is null)
            {
                if (!string.IsNullOrEmpty(activity.Name))
                {
                    sb.AppendLine($"{activity.Name} -");
                }
                continue;
            }
  
            sb.AppendLine($"{activity.SpotifyInfo.Track} - {activity.SpotifyInfo.Artists.Aggregate((a, b) => $"{a}, {b}")} - {activity.SpotifyInfo.Album}");
        }

        return sb.ToString();
    }

    public IReadOnlySet<CommandOptionDto> GetValidArgs()
    {
        return _spotifyCommands;
    }

    public string GetCommandName()
    {
        return "SpotifyInfo";
    }

    public string GetCommandDescription()
    {
        return "Get Spotify info";
    }

    private static Expression<Func<Activity, object>> GetGroupByExpression(IReadOnlyDictionary<string, string> argsAndValues)
    {
        var groupBy = "toptracks";
        var groupByExpression = groupBy switch
        {
            "toptracks" => (Expression<Func<Activity, object>>) (x => x.SpotifyInfo.Track),
            "topartists" => (Expression<Func<Activity, object>>) (x => x.SpotifyInfo.Artists),
            "topalbums" => (Expression<Func<Activity, object>>) (x => x.SpotifyInfo.Album),
            _ => throw new ArgumentException("Invalid argument for Spotify command")
        };

        return groupByExpression;
    }
}