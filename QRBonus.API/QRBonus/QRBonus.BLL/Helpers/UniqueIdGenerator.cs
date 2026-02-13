using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace QRBonus.BLL.Helpers;

public static class UniqueIdGenerator
{
    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    public static string Generate16CharId()
    {
        // Get timestamp in milliseconds (8 bytes = 64 bits)
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestampBytes); // Convert to big-endian for consistency

        // Get 4 random bytes = 32 bits
        byte[] randomBytes = new byte[4];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomBytes);

        // Combine timestamp and randomness into 12 bytes (96 bits)
        byte[] combined = new byte[12];
        Buffer.BlockCopy(timestampBytes, 0, combined, 0, 8);
        Buffer.BlockCopy(randomBytes, 0, combined, 8, 4);

        // Convert to BigInteger and encode to Base62
        var id = new BigInteger(combined.Concat(new byte[] { 0 }).ToArray()); // prevent sign issues
        return Base62Encode(id).PadLeft(16, '0').Substring(0, 16);
    }

    private static string Base62Encode(BigInteger value)
    {
        var sb = new StringBuilder();
        while (value > 0)
        {
            value = BigInteger.DivRem(value, 62, out var remainder);
            sb.Insert(0, Base62Chars[(int)remainder]);
        }
        return sb.ToString();
    }
}