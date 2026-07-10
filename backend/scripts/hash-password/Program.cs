using Microsoft.AspNetCore.Identity;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: dotnet run --project backend/scripts/hash-password -- <password>");
    return 1;
}

var password = args[0];
var hasher = new PasswordHasher<object>();
var hash = hasher.HashPassword(new object(), password);
Console.WriteLine(hash);
return 0;
