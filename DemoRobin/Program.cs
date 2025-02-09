using System.Security.Cryptography;
using ToolBox.Security.Rsa;

RSACryptoServiceProvider rsa = new(4096);
EncryptionService s1 = new(rsa);
EncryptionService s2 = new(rsa);

string messageFromS1 = s1.Encrypt("Coucou", s2.PublicKey);
Console.WriteLine(messageFromS1);
Console.WriteLine(s2.Decrypt(messageFromS1));

string messageFromS2 = s2.Encrypt("Comment ca va ☺?", s1.PublicKey);
Console.WriteLine(messageFromS2);
Console.WriteLine(s1.Decrypt(messageFromS2));