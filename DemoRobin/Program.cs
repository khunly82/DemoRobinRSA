using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Cryptography;
using ToolBox.Security.Rsa;

Dictionary<string, ConnectionInfos> connectedUsers = new Dictionary<string, ConnectionInfos>();

// création du service de crypto
RSACryptoServiceProvider rsa = new(4096);
EncryptionService service = new(rsa);

// création de la connection signalR
HubConnection connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5148/ws/message").Build();

// démarrer le connection
await connection.StartAsync();

connection.On<Dictionary<string, ConnectionInfos>>("connectionsChanged", personnes => {
    connectedUsers = personnes;
    Console.WriteLine($"Connected users : {string.Join(",", personnes.Keys)}");
});

connection.On<string>("messageReceived", message =>
{
    try
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Message recu: {service.Decrypt(message)}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Message recu: {message}");
        Console.ResetColor();
    }
    
});

await connection.InvokeAsync("Connect", new { Nom = Console.ReadLine() ?? string.Empty, PubKey = service.PublicKey });

while (true)
{
    string dest = Console.ReadLine() ?? string.Empty;
    if(!connectedUsers.TryGetValue(dest, out ConnectionInfos? infos))
    {
        Console.WriteLine("...");
        continue;
    }

    string message = Console.ReadLine() ?? string.Empty;
    await connection.InvokeAsync("SendMessage", new { Nom = dest, EncodedMessage = service.Encrypt(message, infos!.PublicKey) });
}

public class ConnectionInfos
{
    public string ConnectionId { get; set; } = null!;
    public string PublicKey { get; set; } = null!;
}