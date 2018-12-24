package vn.soha.game.sdk.utils;

import java.security.Key;
import java.security.KeyFactory;
import java.security.spec.KeySpec;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;

import javax.crypto.Cipher;

import android.annotation.SuppressLint;
import android.util.Base64;

public class RSAUtils {


	@SuppressLint("TrulyRandom") public static String encryptRSAToString(String clearText, String publicKey) {
		String encryptedBase64 = "";
		try {
			KeyFactory keyFac = KeyFactory.getInstance("RSA");
			KeySpec keySpec = new X509EncodedKeySpec(Base64.decode(publicKey.trim().getBytes(), Base64.DEFAULT));
			Key key = keyFac.generatePublic(keySpec);

			// get an RSA cipher object and print the provider
			final Cipher cipher = Cipher.getInstance("RSA/ECB/OAEPWITHSHA-256ANDMGF1PADDING");
			// encrypt the plain text using the public key
			cipher.init(Cipher.ENCRYPT_MODE, key);

			byte[] encryptedBytes = cipher.doFinal(clearText.getBytes("UTF-8"));
			encryptedBase64 = new String(Base64.encode(encryptedBytes, Base64.DEFAULT));
		} catch (Exception e) {
			e.printStackTrace();
		}

		return encryptedBase64.replaceAll("(\\r|\\n)", "");
	}

	public static String decryptRSAToString(String encryptedBase64, String privateKey) {

		String decryptedString = "";
		try {
			KeyFactory keyFac = KeyFactory.getInstance("RSA");
			KeySpec keySpec = new PKCS8EncodedKeySpec(Base64.decode(privateKey.trim().getBytes(), Base64.DEFAULT));
			Key key = keyFac.generatePrivate(keySpec);

			// get an RSA cipher object and print the provider
			final Cipher cipher = Cipher.getInstance("RSA/ECB/OAEPWITHSHA-256ANDMGF1PADDING");
			// encrypt the plain text using the public key
			cipher.init(Cipher.DECRYPT_MODE, key);

			byte[] encryptedBytes = Base64.decode(encryptedBase64, Base64.DEFAULT);
			byte[] decryptedBytes = cipher.doFinal(encryptedBytes);
			decryptedString = new String(decryptedBytes);
		} catch (Exception e) {
			e.printStackTrace();
		}

		return decryptedString;
	}

}
