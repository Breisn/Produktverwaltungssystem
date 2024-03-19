using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

// Hilfsklasse zum Hashen von Passwörtern und Überprüfen von gehashten Passwörtern
public static class HashHelper
{
    // Hashen eines Passworts mit dem PBKDF2-Algorithmus und einem zufälligen Salt
    public static string HashPassword(string password)
    {
        // Generiere ein zufälliges Salt
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Berechne den Hash mit PBKDF2
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Kombiniere Salt und das gehashte Passwort
        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    // Überprüft ein Passwort gegen ein gehashtes Passwort mit Salt
    public static bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
    {
        // Teile das gehashte Passwort in Salt- und Hash-Teile
        var parts = hashedPasswordWithSalt.Split('.');
        if (parts.Length != 2)
        {
            return false;
        }

        // Extrahiere das Salt und das gehashte Passwort
        var salt = Convert.FromBase64String(parts[0]);
        var hashedPassword = parts[1];

        // Berechne den Hash des bereitgestellten Passworts mit dem extrahierten Salt
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: passwordToCheck,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Vergleiche den berechneten Hash mit dem gespeicherten Hash
        return hashed == hashedPassword;
    }
}

