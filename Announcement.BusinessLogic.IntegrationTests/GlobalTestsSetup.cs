namespace Announcement.BusinessLogic.Tests;

/// <summary>
/// Provides global setup for all tests in the test assembly.
/// </summary>
[SetUpFixture]
public class GlobalTestsSetup
{
    /// <summary>
    /// Performs one-time setup before any tests are run.
    /// Resets and seeds the database to ensure a consistent test environment.
    /// </summary>
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        AnnouncementDbContextFactory.Reset();
        AnnouncementDbContextFactory.Seed();
    }
}