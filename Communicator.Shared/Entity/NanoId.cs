namespace Communicator.Shared.Entity;

public record NanoId(string Value)
{
    public static NanoId NewId(int size = 12)
    {
        return new NanoId(NanoidDotNet.Nanoid.Generate(size: size));
    }

    public static async Task<NanoId> NewIdAsync(int size = 12)
    {
        return new NanoId(await NanoidDotNet.Nanoid.GenerateAsync(size: size));
    }

    public static implicit operator string(NanoId nanoId)
    {
        return nanoId.Value;
    }

    public static implicit operator NanoId(string value)
    {
        return new NanoId(value);
    }
    
    public static bool TryParse(string input, IFormatProvider formatProvider, out NanoId result)
    {
        if (!string.IsNullOrEmpty(input))
        {
            result = new NanoId(input);
            return true;
        }

        result = null;
        return false;
    }
};