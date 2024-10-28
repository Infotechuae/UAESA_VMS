using DAL;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecuLobbyVMS
{
  public partial class WebForm1 : System.Web.UI.Page
  {
    DBConnection ocon = new DBConnection(MyConnection.ReadConStr("Local"));
    private static readonly string key = "12345678901234567890123456789012";

    private static readonly string iv = "1234567890123456";

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {


      }





    }


    public static string EncryptQRCODE(string plainText)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
          }
        }
      }
    }

    public static string DecryptQRCODE(string cipherText)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = Encoding.UTF8.GetBytes(iv);

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
        {
          using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader sr = new StreamReader(cs))
            {
              return sr.ReadToEnd();
            }
          }
        }
      }
    }
  }
}
