using System.Security.Cryptography;
using System.Text;

RSACryptoServiceProvider rsa1 = new(4096);
RSACryptoServiceProvider rsa2 = new(4096);


// création des clés
string privateKey1 = rsa1.ExportPkcs8PrivateKeyPem();
string pubKey1 = rsa1.ExportRSAPublicKeyPem();

string privateKey2 = rsa2.ExportPkcs8PrivateKeyPem();
string pubKey2 = rsa2.ExportRSAPublicKeyPem();

// échange des clés
rsa2.ImportFromPem(pubKey1);
rsa1.ImportFromPem(pubKey2);

byte[] value = rsa2.Encrypt(Encoding.UTF8.GetBytes("Hello World!!!"), true);
string encodedValue = Convert.ToBase64String(value);
Console.WriteLine(encodedValue);

rsa1.ImportFromPem(privateKey1);
byte[] result = rsa1.Decrypt(Convert.FromBase64String(encodedValue), true);
Console.WriteLine(Encoding.UTF8.GetString(result));

//byte[] value2 = rsa1.Encrypt(Encoding.UTF8.GetBytes("Comment ca va"), true);
//string encodedValue2 = Convert.ToBase64String(value);
//Console.WriteLine(encodedValue);

//byte[] result2 = rsa2.Decrypt(Convert.FromBase64String(encodedValue2), true);
//Console.WriteLine(Encoding.UTF8.GetString(result2));