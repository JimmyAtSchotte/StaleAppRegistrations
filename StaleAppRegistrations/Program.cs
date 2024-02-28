using System.Text;
using System.Text.Json;
using WojciechMikołajewicz.CsvReader;

var signInFile = "..\\..\\..\\..\\data\\SignIns.json";
var appRegistrationFile = "..\\..\\..\\..\\data\\AppRegistrationList.csv";

var signIns = JsonSerializer.Deserialize<List<SignIn>>(File.ReadAllText(signInFile));

using var textReader = new StreamReader(appRegistrationFile, Encoding.UTF8, true);
using var csvDeserializer = new CsvDeserializer<AppRegistration>(textReader);
var appRegistrations = await csvDeserializer.ReadAllToListAsync();

Console.WriteLine($"signIns count:\t\t{signIns.Count}");
Console.WriteLine($"appRegistrations count:\t{appRegistrations.Count}");

Console.WriteLine("");

var activeAppRegistrations = appRegistrations.Where(appRegistration => signIns.Any(signIn => signIn.appId == appRegistration.appID));
var staleAppRegistrations = appRegistrations.Where(appRegistration => signIns.All(signIn => signIn.appId != appRegistration.appID));

Console.WriteLine("Active app registrations");
Console.WriteLine("========================");
Console.WriteLine("App id\t\t\t\t\tName");
foreach (var appRegistration in activeAppRegistrations)
    Console.WriteLine($"{appRegistration.appID}\t{appRegistration.displayName}");

Console.WriteLine("");
Console.WriteLine("Stale app registrations");
Console.WriteLine("=======================");
Console.WriteLine("App id\t\t\t\t\tName");
foreach (var appRegistration in staleAppRegistrations)
    Console.WriteLine($"{appRegistration.appID}\t{appRegistration.displayName}");

public class SignIn
{
    public string appId { get; set; }
}

public class AppRegistration
{
    public string displayName { get; set; }
    public string appID { get; set; }
}


