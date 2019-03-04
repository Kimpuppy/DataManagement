//	Copyright (c) KimPuppy.
//	http://bakak112.tistory.com/

using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManagement
{
    private static readonly string KEY = "DataManagement_KEY";
    private static readonly string IV = "DataManagement_IV";

    public static void SaveData<T>(string dataname, T data)
    {
        // Convert data to JSON
        string info = JsonUtility.ToJson(data);

        // Encrypt JSON data
        string encrypt = Encryption.Encrypt(info, KEY);

        // Encrypt encrypted data to AES256
        encrypt = EncryptAES256(encrypt);

        File.WriteAllText(Application.persistentDataPath + "/" + dataname + ".json", encrypt);
    }

    public static bool LoadData<T>(string dataname, ref T data)
    {
        // Check if file exists
        if(File.Exists(Application.persistentDataPath + "/" + dataname + ".json"))
        {
            // Read data to string
            string info = File.ReadAllText(Application.persistentDataPath + "/" + dataname + ".json");

            // Encrypt data to AES256
            info = DecryptAES256(info);

            // Encrypt encrypted data
            string decrypt = Encryption.Decrypt(info, KEY);

            // Convert JSON data to properties
            data = JsonUtility.FromJson<T>(decrypt);

            return true;
        }
        Debug.LogError("File does not exist in " + Application.persistentDataPath);
        return false;
    }

    public static void DeleteData<T>(string dataname, ref T data)
    {
        if(File.Exists(Application.persistentDataPath + "/" + dataname + ".json"))
            File.Delete(Application.persistentDataPath + "/" + dataname + ".json");
    }

    public static Rfc2898DeriveBytes MakeKey(string password)
    {
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(password);
        byte[] saltBytes = SHA512.Create().ComputeHash(keyBytes);
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(keyBytes, saltBytes, 256);
        return result;
    }

    public static Rfc2898DeriveBytes MakeIV(string vector)
    {
        byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(vector);
        byte[] saltBytes = SHA512.Create().ComputeHash(ivBytes);
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(ivBytes, saltBytes, 256);
        return result;
    }

    private static string EncryptAES256(string data)
    {   
        // Convert data to byte
        byte[] plainBytes = Encoding.UTF8.GetBytes(data);

        // Create key, iv
        Rfc2898DeriveBytes key = MakeKey(KEY);
        Rfc2898DeriveBytes iv = MakeIV(IV);

        // Rijndael algorithm
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 128;
        rijndaelManaged.Key = key.GetBytes(32);
        rijndaelManaged.IV = iv.GetBytes(16);
        
        // Create Memorystream
        MemoryStream memoryStream = new MemoryStream();

        // Define key, iv value
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);

        // Create CryptoStream with key and iv value using MemoryStream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        // Write byte array to CryptoStream and Flush final block
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        // Copy encrypted bytes to MemoryStream
        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        // Close Stream
        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    private static string DecryptAES256(string data)
    {
        // Convert base64 to byte
        byte[] encryptBytes = Convert.FromBase64String(data);

        // Create key, iv
        Rfc2898DeriveBytes key = MakeKey(KEY);
        Rfc2898DeriveBytes iv = MakeIV(IV);

        // Rijndael algorithm
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.Padding = PaddingMode.PKCS7;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 128;
        rijndaelManaged.Key = key.GetBytes(32);
        rijndaelManaged.IV = iv.GetBytes(16);

        // Create MemoryStream
        MemoryStream memoryStream = new MemoryStream(encryptBytes);

        // Define key, iv value
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);

        // Create CryptoStream with key and iv value using MemoryStream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        // Create byte array to copy decrypted bytes
        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        // Convert decrypted byte array
        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        // Close Stream
        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }
}