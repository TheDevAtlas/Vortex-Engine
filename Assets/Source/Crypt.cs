using System;
using System.Security.Cryptography;

public class Crypt
{
    public static bool IsAccessControlBlock(uint blockIndex)
    {
        return (blockIndex % 4) != 3 ? false : true;
    }

    public static bool ShouldEncryptBlock(uint blockIndex)
    {
        if (blockIndex >= 8 && IsAccessControlBlock(blockIndex) == false)
        {
            return true;
        }
        return false;
    }

    public static void ComputeMD5(byte[] digest, byte[] bytesIn, uint inputLen)
    {
        using (MD5 md5 = MD5.Create())
        {
            md5.TransformBlock(bytesIn, 0, (int)inputLen, bytesIn, 0);
            md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            Array.Copy(md5.Hash, digest, 16);
        }
    }

    public static void ComputeEncryptionKey(byte[] keyOut, byte[] tagBlocks0and1, uint blockIndex)
    {
        byte[] hashConst = {
            0x20, 0x43, 0x6F, 0x70, 0x79, 0x72, 0x69, 0x67, 0x68, 0x74, 0x20, 0x28, 0x43, 0x29, 0x20, 0x32, // Copyright (C) 2
            0x30, 0x31, 0x30, 0x20, 0x41, 0x63, 0x74, 0x69, 0x76, 0x69, 0x73, 0x69, 0x6F, 0x6E, 0x2E, 0x20, // 010 Activision.
            0x41, 0x6C, 0x6C, 0x20, 0x52, 0x69, 0x67, 0x68, 0x74, 0x73, 0x20, 0x52, 0x65, 0x73, 0x65, 0x72, // All Rights Reser
            0x76, 0x65, 0x64, 0x2E, 0x20 // ved.
        };
        const int hashLen = 0x56;
        byte[] hashBuf = new byte[hashLen];
        Buffer.BlockCopy(tagBlocks0and1, 0, hashBuf, 0, 0x20);
        hashBuf[0x20] = (byte)blockIndex;
        Buffer.BlockCopy(hashConst, 0, hashBuf, 0x21, 0x35);

        ComputeMD5(keyOut, hashBuf, 0x56);
    }

    private const int KeyBits = 128;

    public static void EncryptAES128ECB(byte[] key, byte[] plainTextIn, byte[] cipherTextOut)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = KeyBits;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.Zeros;
            aes.Key = key;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptor.TransformBlock(plainTextIn, 0, 16, cipherTextOut, 0);
            }
        }
    }

    public static void DecryptAES128ECB(byte[] key, byte[] cipherTextIn, byte[] plainTextOut)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = KeyBits;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.Zeros;
            aes.Key = key;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                decryptor.TransformBlock(cipherTextIn, 0, 16, plainTextOut, 0);
            }
        }
    }

    public static void EncryptTagBlock(byte[] blockData, uint blockIndex, byte[] tagBlocks0and1)
    {
        if (ShouldEncryptBlock(blockIndex))
        {
            byte[] cipherText = new byte[16];
            byte[] aesKey = new byte[16];
            ComputeEncryptionKey(aesKey, tagBlocks0and1, blockIndex);
            EncryptAES128ECB(aesKey, blockData, cipherText);
            Buffer.BlockCopy(cipherText, 0, blockData, 0, 16);
        }
    }

    public static void DecryptTagBlock(byte[] blockData, uint blockIndex, byte[] tagBlocks0and1)
    {
        if (ShouldEncryptBlock(blockIndex))
        {
            byte[] plainText = new byte[16];
            byte[] aesKey = new byte[16];
            ComputeEncryptionKey(aesKey, tagBlocks0and1, blockIndex);
            DecryptAES128ECB(aesKey, blockData, plainText);
            Buffer.BlockCopy(plainText, 0, blockData, 0, 16);
        }
    }
}
