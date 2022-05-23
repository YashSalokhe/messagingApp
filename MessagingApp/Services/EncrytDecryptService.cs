namespace MessagingApp.Services
{
    public class EncrytDecryptService
    {
        public string EncryptAsync(string message)
        {
            string textToEncrypt = message;
            string toReturn = string.Empty;
            string publicKey = "12345678";
            string IV = "87654321";


            byte[] secretkeyByte = System.Text.Encoding.UTF8.GetBytes(IV);
           
            byte[] publickeybyte = System.Text.Encoding.UTF8.GetBytes(publicKey);

            byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            DES des = DES.Create();
            
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                toReturn = Convert.ToBase64String(ms.ToArray());
            
            return toReturn;
            
        }


        public string DecryptAsync(string message)
        {
            string textToDecrypt = message;
            string toReturn = string.Empty;
            string publickey = "12345678";
            string IV = "87654321";
          
            byte[] privatekeyByte = System.Text.Encoding.UTF8.GetBytes(IV);

            byte[] publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);

            byte[] inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            DES des = DES.Create();
            
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                toReturn = encoding.GetString(ms.ToArray());
          
            return toReturn;
        }
    }
}
