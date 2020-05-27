package com.usdk.plugin.uuidkit;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.DESKeySpec;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

public class EncryptUtils {
    public static String encryptDES(String encryptString, String encryptKey) throws Exception {
        //返回实现指定转换的Cipher对象 "算法/模式/填充"
        Cipher cipher = Cipher.getInstance("DES/CBC/PKCS5Padding");
        //创建一个DESKeySpec对象，使用8个字节的key作为DES密钥的内容
        DESKeySpec desKeySpec = new DESKeySpec(encryptKey.getBytes("UTF-8"));
        //返回转换指定算法的秘密密钥的SecretKeyFactory对象
        SecretKeyFactory keyFactory = SecretKeyFactory.getInstance("DES");
        //根据提供的密钥生成SecretKey对象
        SecretKey secretKey = keyFactory.generateSecret(desKeySpec);
        //使用iv中的字节作为iv来构造一个iv ParameterSpec对象。复制该缓冲区的内容来防止后续修改
        IvParameterSpec iv = new IvParameterSpec(encryptKey.getBytes());
        //用密钥和一组算法参数初始化此 Cipher；Cipher：加密、解密、密钥包装或密钥解包，具体取决于 opmode 的值。
        cipher.init(Cipher.ENCRYPT_MODE, secretKey, iv);
        //加密同时解码成字符串返回
        return new String(BASE64.encode(cipher.doFinal(encryptString.getBytes("UTF-8"))));
    }
    public static String decryptDES(String decodeString, String decodeKey) throws Exception {
        //使用指定密钥构造IV
        IvParameterSpec iv = new IvParameterSpec(decodeKey.getBytes());
        //根据给定的字节数组和指定算法构造一个密钥。
        SecretKeySpec skeySpec = new SecretKeySpec(decodeKey.getBytes(), "DES");
        //返回实现指定转换的 Cipher 对象
        Cipher cipher = Cipher.getInstance("DES/CBC/PKCS5Padding");
        //解密初始化
        cipher.init(Cipher.DECRYPT_MODE, skeySpec, iv);
        //解码返回
        byte[] byteMi = BASE64.decode(decodeString.toCharArray());
        byte decryptedData[] = cipher.doFinal(byteMi);
        return new String(decryptedData);
    }
}
