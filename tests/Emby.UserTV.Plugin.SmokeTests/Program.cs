using System;
using System.Collections.Generic;
using System.Linq;
using Emby.UserTV.Plugin.Configuration;
using Emby.UserTV.Plugin.Domain;
using Emby.UserTV.Plugin.Services;

var results = new List<(string Name, bool Success, string Detail)>();

Run("embedded web resources", () =>
{
    var assembly = typeof(Emby.UserTV.Plugin.Plugin).Assembly;
    var resources = assembly.GetManifestResourceNames();
    AssertContains(resources, "Emby.UserTV.Plugin.Web.index.html");
    AssertContains(resources, "Emby.UserTV.Plugin.Web.index.js");
    AssertContains(resources, "Emby.UserTV.Plugin.Web.styles.css");
    return $"{resources.Length} resources found";
});

Run("planner with fictive users and media", () =>
{
    var planner = new UserTvPlanner();
    var plans = planner.BuildPlans(FictiveFixtures.CreateFictiveUsers(), new PluginConfiguration
    {
        PlaylistPrefix = "UserTV",
        ChannelNameTemplate = "{prefix} - {user} Channel",
        MinimumItemsPerUser = 3,
        MaximumItemsPerUser = 10
    });

    var eligible = plans.Single(plan => plan.UserId == "viewer-alpha");
    var ineligible = plans.Single(plan => plan.UserId == "viewer-beta");

    AssertTrue(eligible.IsEligible, "viewer-alpha should be eligible");
    AssertEqual("UserTV - viewer-alpha", eligible.PlaylistName);
    AssertEqual("UserTV - viewer-alpha Channel", eligible.ChannelName);
    AssertEqual(3, eligible.Items.Count);
    AssertTrue(!ineligible.IsEligible, "viewer-beta should be ineligible");
    AssertTrue(ineligible.Reason.StartsWith("minimum_items_not_met:", StringComparison.Ordinal), ineligible.Reason);

    return $"{plans.Count} users planned";
});

Run("runtime dry-run summary", () =>
{
    var runtime = new UserTvRuntime(
        new FictiveSourceGateway(),
        new UserTvPlanner(),
        logger: null);

    var summary = runtime.Run(dryRun: true);
    AssertTrue(summary.DryRun, "summary must stay dry-run");
    AssertEqual(2, summary.UsersScanned);
    AssertEqual(1, summary.EligibleUsers);
    AssertEqual(3, summary.PlannedItems);
    AssertTrue(summary.Message.Contains("No Emby playlists", StringComparison.Ordinal), summary.Message);

    return summary.Mode;
});

foreach (var result in results)
{
    Console.WriteLine($"{(result.Success ? "PASS" : "FAIL")} {result.Name}: {result.Detail}");
}

if (results.Any(result => !result.Success))
{
    Environment.ExitCode = 1;
}

void Run(string name, Func<string> test)
{
    try
    {
        results.Add((name, true, test()));
    }
    catch (Exception ex)
    {
        results.Add((name, false, ex.Message));
    }
}

static void AssertContains(IEnumerable<string> values, string expected)
{
    if (!values.Contains(expected, StringComparer.Ordinal))
    {
        throw new InvalidOperationException($"Missing expected value: {expected}");
    }
}

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected {expected}, got {actual}.");
    }
}

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

sealed class FictiveSourceGateway : IUserTvSourceGateway
{
    public List<UserTvSourceUser> LoadUsersWithFavorites()
    {
        return FictiveFixtures.CreateFictiveUsers();
    }
}

static class FictiveFixtures
{
    public static List<UserTvSourceUser> CreateFictiveUsers()
    {
        return new List<UserTvSourceUser>
        {
            new UserTvSourceUser
            {
                UserId = "viewer-alpha",
                DisplayName = "viewer-alpha",
                FavoriteItems = new List<UserTvSourceItem>
                {
                    new UserTvSourceItem { ItemId = "item-001", Title = "Fictional Pilot", ItemType = "Movie", RuntimeTicks = 2400000000 },
                    new UserTvSourceItem { ItemId = "item-002", Title = "Signal Garden", ItemType = "Movie", RuntimeTicks = 2400000000 },
                    new UserTvSourceItem { ItemId = "item-003", Title = "Example Nights S01E01", ItemType = "Episode", RuntimeTicks = 2400000000 }
                }
            },
            new UserTvSourceUser
            {
                UserId = "viewer-beta",
                DisplayName = "viewer-beta",
                FavoriteItems = new List<UserTvSourceItem>
                {
                    new UserTvSourceItem { ItemId = "item-004", Title = "Example Nights S01E02", ItemType = "Episode", RuntimeTicks = 2400000000 }
                }
            }
        };
    }
}
