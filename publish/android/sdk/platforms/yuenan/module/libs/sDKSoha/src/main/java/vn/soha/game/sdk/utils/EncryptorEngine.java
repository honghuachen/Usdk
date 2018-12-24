package vn.soha.game.sdk.utils;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.security.InvalidKeyException;
import java.security.KeyFactory;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.PublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.util.ArrayList;
import java.util.Arrays;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;

import android.annotation.SuppressLint;
import android.util.Base64;
import android.util.Log;

/**
 * Created by PC0182 on 9/6/2016.
 */
public class EncryptorEngine {
	private static final String RSA = "RSA/ECB/PKCS1Padding";
	//	private static final String RSA = "AES/CBC/PKCS7Padding";
	// private static final String RSA = "RSA";
	private static final int KEY_LENGTH = 2048;

	public static KeyPair generateRSAKey() {
		KeyPairGenerator kpg = null;
		try {
			// get an RSA key generator
			kpg = KeyPairGenerator.getInstance(RSA);
		} catch (NoSuchAlgorithmException e) {
			Log.e(EncryptorEngine.class.getName(), e.getMessage(), e);
			throw new RuntimeException(e);
		}
		// initialize the key to 2048 bits
		kpg.initialize(KEY_LENGTH);
		// return the generated key pair
		return kpg.genKeyPair();
	}

	@SuppressLint("NewApi") public static String encryptData(String text, String pub_key) {
		byte[] data = new byte[0];
		try {
			data = text.getBytes("utf-8");
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		PublicKey publicKey = null;
		try {
			publicKey = getPublicKey(Base64.decode(pub_key.getBytes("utf-8"),
					Base64.DEFAULT));
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (InvalidKeySpecException e) {
			e.printStackTrace();
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		Cipher cipher = null;
		try {
			cipher = Cipher.getInstance(RSA);
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (NoSuchPaddingException e) {
			e.printStackTrace();
		}
		try {
			cipher.init(Cipher.ENCRYPT_MODE, publicKey);
		} catch (InvalidKeyException e) {
			e.printStackTrace();
		}
		String result = "";
		if (data.length > 116) {
			int parts = data.length / 116;
			ArrayList<byte[]> list = new ArrayList<byte[]>();
			// byte[] byteArray = new byte[parts + 1];
			for (int i = 0; i < parts + 1; i++) {
				if (116 * (i + 1) <= data.length) {
					byte[] splitbyte = Arrays.copyOfRange(data, 116 * i,
							116 * (i + 1));
					// byteArray[i] = data.substring(i * 100, 100 * (i + 1));
					list.add(splitbyte);
				} else {
					byte[] splitbyte = Arrays.copyOfRange(data, 116 * i,
							data.length);
					list.add(splitbyte);
				}

				try {
					if (i != 0) {
						String temp =  Base64.encodeToString(
								cipher.doFinal(list.get(i)), Base64.DEFAULT);
						result = result
								+ "_"
								+ URLEncoder.encode(temp, "UTF-8");
					} else {
						String temp =  Base64.encodeToString(
								cipher.doFinal(list.get(i)), Base64.DEFAULT);

						//URLEncoder.encode(temp, "UTF-8");
						result =URLEncoder.encode(temp, "UTF-8");

						/*Base64.encodeToString(
									cipher.doFinal(list.get(i)), Base64.DEFAULT);*/


					}
				} catch (Exception e) {
					e.printStackTrace();
				} 
			}
		} else {
			try {
				data = text.getBytes("utf-8");
				result = result
						+ Base64.encodeToString(cipher.doFinal(data),
								Base64.DEFAULT);
				Log.e("encrypted data", result);
			} catch (UnsupportedEncodingException e) {
				e.printStackTrace();
			} catch (BadPaddingException e) {
				e.printStackTrace();
			} catch (IllegalBlockSizeException e) {
				e.printStackTrace();
			} catch (IllegalStateException e) {
				e.printStackTrace();
			}
		}
		// if (text.length() > 100) {
		// int parts = text.length() / 100;
		// Log.e("PART", " is " + parts);
		// String[] textArray = new String[parts + 1];
		// for (int i = 0; i < parts + 1; i++) {
		// if (100 * (i + 1) <= text.length()) {
		// textArray[i] = text.substring(i * 100, 100 * (i + 1));
		// } else {
		// textArray[i] = text.substring((i * 100), text.length());
		// }
		//
		// try {
		// data = textArray[i].getBytes("utf-8");
		// } catch (UnsupportedEncodingException e) {
		// e.printStackTrace();
		// }
		// try {
		// if(i != 0){
		// result = result + "_" + Base64.encodeToString(cipher.doFinal(data),
		// Base64.DEFAULT);
		// }else{
		// result = Base64.encodeToString(cipher.doFinal(data), Base64.DEFAULT);
		// }
		// } catch (IllegalBlockSizeException e) {
		// e.printStackTrace();
		// } catch (BadPaddingException e) {
		// Log.e("LOL", e.toString());
		// e.printStackTrace();
		// } catch (IllegalStateException e) {
		// e.printStackTrace();
		// }catch(RuntimeException e){
		// e.printStackTrace();
		// }
		// }
		// } else {
		// try {
		// data = text.getBytes("utf-8");
		// result = result + Base64.encodeToString(cipher.doFinal(data),
		// Base64.DEFAULT);
		// Log.e("encrypted data" , result);
		// } catch (UnsupportedEncodingException e) {
		// e.printStackTrace();
		// } catch (BadPaddingException e) {
		// e.printStackTrace();
		// } catch (IllegalBlockSizeException e) {
		//
		// } catch (IllegalStateException e) {
		// e.printStackTrace();
		// }
		// }

		return result;

	}

	@SuppressLint("NewApi") public static String encryptDataNoURLEn(String text, String pub_key) {
		byte[] data = new byte[0];
		try {
			data = text.getBytes("utf-8");
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		PublicKey publicKey = null;
		try {
			publicKey = getPublicKey(Base64.decode(pub_key.getBytes("utf-8"),
					Base64.DEFAULT));
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (InvalidKeySpecException e) {
			e.printStackTrace();
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		Cipher cipher = null;
		try {
			cipher = Cipher.getInstance(RSA);
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		} catch (NoSuchPaddingException e) {
			e.printStackTrace();
		}
		try {
			cipher.init(Cipher.ENCRYPT_MODE, publicKey);
		} catch (InvalidKeyException e) {
			e.printStackTrace();
		}
		String result = "";
		if (data.length > 116) {
			int parts = data.length / 116;
			ArrayList<byte[]> list = new ArrayList<byte[]>();
			// byte[] byteArray = new byte[parts + 1];
			for (int i = 0; i < parts + 1; i++) {
				if (116 * (i + 1) <= data.length) {
					byte[] splitbyte = Arrays.copyOfRange(data, 116 * i,
							116 * (i + 1));
					// byteArray[i] = data.substring(i * 100, 100 * (i + 1));
					list.add(splitbyte);
				} else {
					byte[] splitbyte = Arrays.copyOfRange(data, 116 * i,
							data.length);
					list.add(splitbyte);
				}

				try {
					if (i != 0) {
						String temp =  Base64.encodeToString(
								cipher.doFinal(list.get(i)), Base64.DEFAULT);
						result = result
								+ "_"
								+ temp;
					} else {
						String temp =  Base64.encodeToString(
								cipher.doFinal(list.get(i)), Base64.DEFAULT);

						//URLEncoder.encode(temp, "UTF-8");
						result = temp;

						/*Base64.encodeToString(
									cipher.doFinal(list.get(i)), Base64.DEFAULT);*/


					}
				} catch (Exception e) {
					e.printStackTrace();
				} 
			}
		} else {
			try {
				data = text.getBytes("utf-8");
				result = result
						+ Base64.encodeToString(cipher.doFinal(data),
								Base64.DEFAULT);
				Log.e("encrypted data", result);
			} catch (UnsupportedEncodingException e) {
				e.printStackTrace();
			} catch (BadPaddingException e) {
				e.printStackTrace();
			} catch (IllegalBlockSizeException e) {
				e.printStackTrace();
			} catch (IllegalStateException e) {
				e.printStackTrace();
			}
		}
		// if (text.length() > 100) {
		// int parts = text.length() / 100;
		// Log.e("PART", " is " + parts);
		// String[] textArray = new String[parts + 1];
		// for (int i = 0; i < parts + 1; i++) {
		// if (100 * (i + 1) <= text.length()) {
		// textArray[i] = text.substring(i * 100, 100 * (i + 1));
		// } else {
		// textArray[i] = text.substring((i * 100), text.length());
		// }
		//
		// try {
		// data = textArray[i].getBytes("utf-8");
		// } catch (UnsupportedEncodingException e) {
		// e.printStackTrace();
		// }
		// try {
		// if(i != 0){
		// result = result + "_" + Base64.encodeToString(cipher.doFinal(data),
		// Base64.DEFAULT);
		// }else{
		// result = Base64.encodeToString(cipher.doFinal(data), Base64.DEFAULT);
		// }
		// } catch (IllegalBlockSizeException e) {
		// e.printStackTrace();
		// } catch (BadPaddingException e) {
		// Log.e("LOL", e.toString());
		// e.printStackTrace();
		// } catch (IllegalStateException e) {
		// e.printStackTrace();
		// }catch(RuntimeException e){
		// e.printStackTrace();
		// }
		// }
		// } else {
		// try {
		// data = text.getBytes("utf-8");
		// result = result + Base64.encodeToString(cipher.doFinal(data),
		// Base64.DEFAULT);
		// Log.e("encrypted data" , result);
		// } catch (UnsupportedEncodingException e) {
		// e.printStackTrace();
		// } catch (BadPaddingException e) {
		// e.printStackTrace();
		// } catch (IllegalBlockSizeException e) {
		//
		// } catch (IllegalStateException e) {
		// e.printStackTrace();
		// }
		// }

		return result;

	}

	public static String decryptRSA(String text, String private_key)
			throws NoSuchPaddingException, NoSuchAlgorithmException,
			UnsupportedEncodingException, InvalidKeySpecException,
			InvalidKeyException, BadPaddingException, IllegalBlockSizeException {
		Cipher cipher = Cipher.getInstance(RSA);
		PrivateKey privateKey;
		String result = "";
		byte[] data = Base64.decode(text, Base64.DEFAULT);
		privateKey = getPrivateKey(Base64.decode(private_key.getBytes("utf-8"),
				Base64.DEFAULT));
		cipher.init(Cipher.DECRYPT_MODE, privateKey);
		result = new String(cipher.doFinal(data));

		return result;
	}

	public static PublicKey getPublicKey(byte[] keyBytes)
			throws NoSuchAlgorithmException, InvalidKeySpecException {
		X509EncodedKeySpec keySpec = new X509EncodedKeySpec(keyBytes);
		KeyFactory keyFactory = KeyFactory.getInstance("RSA");
		return keyFactory.generatePublic(keySpec);
	}

	public static PrivateKey getPrivateKey(byte[] keyBytes)
			throws NoSuchAlgorithmException, InvalidKeySpecException {
		PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(keyBytes);
		KeyFactory fact = KeyFactory.getInstance("RSA");
		return fact.generatePrivate(keySpec);
	}


}
