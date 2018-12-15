package com.usdk.util;

import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

/**
 * 方法�?
 * @author syh
 *
 */

public class ReflectionUtils {

	/**
	 * 循环向上转型, 获取对象�? DeclaredMethod
	 * @param object : 子类对象
	 * @param methodName : 父类中的方法�?
	 * @param parameterTypes : 父类中的方法参数类型
	 * @return 父类中的方法对象
	 */
	
	public static Method getDeclaredMethod(Object object, String methodName, Class<?> ... parameterTypes){
		Method method = null ;
		
		for(Class<?> clazz = object.getClass() ; clazz != Object.class ; clazz = clazz.getSuperclass()) {
			try {
				method = clazz.getDeclaredMethod(methodName, parameterTypes) ;
				return method ;
			} catch (Exception e) {
				//这里甚么都不要做！并且这里的异常必须这样写，不能抛出去�??
				//如果这里的异常打印或者往外抛，则就不会执行clazz = clazz.getSuperclass(),�?后就不会进入到父类中�?
			
			}
		}
		
		return null;
	}
	
	/**
	 * 直接调用对象方法, 而忽略修饰符(private, protected, default)
	 * @param object : 子类对象
	 * @param methodName : 父类中的方法�?
	 * @param parameterTypes : 父类中的方法参数类型
	 * @param parameters : 父类中的方法参数
	 * @return 父类中方法的执行结果
	 */
	
	public static Object invokeMethod(Object object, String methodName, Class<?> [] parameterTypes,
			Object... parameters) {
		//根据 对象、方法名和对应的方法参数 通过反射 调用上面的方法获�? Method 对象
		Method method = getDeclaredMethod(object, methodName, parameterTypes) ;
		
		//抑制Java对方法进行检�?,主要是针对私有方法�?�言
		method.setAccessible(true) ;
		
			try {
				if(null != method) {
					
					//调用object �? method �?代表的方法，其方法的参数�? parameters
					return method.invoke(object, parameters) ;
				}
			} catch (IllegalArgumentException e) {
				e.printStackTrace();
			} catch (IllegalAccessException e) {
				e.printStackTrace();
			} catch (InvocationTargetException e) {
				e.printStackTrace();
			}
		
		return null;
	}

	/**
	 * 循环向上转型, 获取对象�? DeclaredField
	 * @param object : 子类对象
	 * @param fieldName : 父类中的属�?�名
	 * @return 父类中的属�?�对�?
	 */
	
	public static Field getDeclaredField(Object object, String fieldName){
		Field field = null ;
		
		Class<?> clazz = object.getClass() ;
		
		for(; clazz != Object.class ; clazz = clazz.getSuperclass()) {
			try {
				field = clazz.getDeclaredField(fieldName) ;
				return field ;
			} catch (Exception e) {
				//这里甚么都不要做！并且这里的异常必须这样写，不能抛出去�??
				//如果这里的异常打印或者往外抛，则就不会执行clazz = clazz.getSuperclass(),�?后就不会进入到父类中�?
				
			} 
		}
	
		return null;
	}	
	
	/**
	 * 直接设置对象属�?��??, 忽略 private/protected 修饰�?, 也不经过 setter
	 * @param object : 子类对象
	 * @param fieldName : 父类中的属�?�名
	 * @param value : 将要设置的�??
	 */
	
	public static void setFieldValue(Object object, String fieldName, Object value){
	
		//根据 对象和属性名通过反射 调用上面的方法获�? Field对象
		Field field = getDeclaredField(object, fieldName) ;
		
		//抑制Java对其的检�?
		field.setAccessible(true) ;
		
		try {
			//�? object �? field �?代表的�?? 设置�? value
			 field.set(object, value) ;
		} catch (IllegalArgumentException e) {
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			e.printStackTrace();
		}
		
	}
	
	/**
	 * 直接读取对象的属性�??, 忽略 private/protected 修饰�?, 也不经过 getter
	 * @param object : 子类对象
	 * @param fieldName : 父类中的属�?�名
	 * @return : 父类中的属�?��??
	 */
	
	public static Object getFieldValue(Object object, String fieldName){
		
		//根据 对象和属性名通过反射 调用上面的方法获�? Field对象
		Field field = getDeclaredField(object, fieldName) ;
		
		//抑制Java对其的检�?
		field.setAccessible(true) ;
		
		try {
			//获取 object �? field �?代表的属性�??
			return field.get(object) ;
			
		} catch(Exception e) {
			e.printStackTrace() ;
		}
		
		return null;
	}
}

