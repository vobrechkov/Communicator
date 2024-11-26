namespace Communicator.Shared.Helpers;

using NanoidDotNet;

public static class NanoIdHelper
{
    public static string GenerateNanoId()
    {
        return Nanoid.Generate();
    }

    public static Task<string> GenerateNanoIdAsync()
    {
        return Nanoid.GenerateAsync();
    }
}