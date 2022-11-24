// https://learn.microsoft.com/en-us/dotnet/standard/security/decrypting-data

using System.Security.Cryptography;
using System.Text;

try
{
    using (FileStream fileStream = new("C:\\Users\\Lee\\Dev\\GSA\\DotnetCryptography\\Projects\\Data\\TestData.txt", FileMode.Open))
    {
        using (Aes aes = Aes.Create())
        {
            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            //byte[] key =
            //{
            //    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            //    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
            //};

            string decryptionKey = "0F33A8759FB79E80E4F6AE34AD15703817C69676AEB28D83";
            byte[] key = Encoding.ASCII.GetBytes(decryptionKey);

            using (CryptoStream cryptoStream = new(
               fileStream,
               aes.CreateDecryptor(key, iv),
               CryptoStreamMode.Read))
            {
                // By default, the StreamReader uses UTF-8 encoding.
                // To change the text encoding, pass the desired encoding as the second parameter.
                // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                using (StreamReader decryptReader = new(cryptoStream))
                {
                    string decryptedMessage = await decryptReader.ReadToEndAsync();
                    Console.WriteLine($"The decrypted original message: {decryptedMessage}");
                }
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"The decryption failed. {ex}");
}