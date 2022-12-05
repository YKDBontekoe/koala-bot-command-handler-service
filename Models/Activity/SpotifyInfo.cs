﻿namespace Koala.ActivityConsumerService.Models;

public class SpotifyInfo
{
    public string Album { get; set; }
    public IReadOnlyCollection<string> Artists { get; set; }
    public string Track { get; set; }
    public int? DurationInSeconds { get; set; }
}