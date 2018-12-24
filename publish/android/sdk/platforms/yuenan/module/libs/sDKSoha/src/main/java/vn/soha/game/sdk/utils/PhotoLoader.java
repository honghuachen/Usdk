package vn.soha.game.sdk.utils;

import vn.sgame.sdk.R;
import android.content.Context;
import android.graphics.Bitmap;
import android.view.View;
import android.widget.ImageView;

import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.listener.SimpleImageLoadingListener;

/**
 * @since Monday, June 02, 2014
 * @author hoangcaomobile
 *
 */
public class PhotoLoader {

	// display image option for photo loader
	@SuppressWarnings("deprecation")
	public static DisplayImageOptions displayImageOptions = new DisplayImageOptions.Builder()
	.showImageOnLoading(R.drawable.ic_photo_loading)
	.showImageForEmptyUri(R.drawable.ic_photo_corrupt)
	.showImageOnFail(R.drawable.ic_photo_corrupt)
	.cacheInMemory(true)
	.cacheOnDisc(true)
	.considerExifParams(true)
	.bitmapConfig(Bitmap.Config.RGB_565)
	.build();
	// --

	// load photo origin
	public static void loadPhotoOrigin(final String url, final ImageView iv, final Context context) {
		ImageLoader.getInstance().loadImage(url, displayImageOptions, new SimpleImageLoadingListener() {

			@Override
			public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
				iv.setImageBitmap(loadedImage);
				super.onLoadingComplete(imageUri, view, loadedImage);
			}

			@Override
			public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
				iv.setImageResource(R.drawable.ic_photo_corrupt);
				super.onLoadingFailed(imageUri, view, failReason);
			}

			@Override
			public void onLoadingStarted(String imageUri, View view) {
				iv.setImageResource(R.drawable.ic_photo_loading);
				super.onLoadingStarted(imageUri, view);
			}																														
		});
	}

	public static void loadPhotoOrigin2(final String url, final ImageView iv, final Context context) {
		ImageLoader.getInstance().loadImage(url, displayImageOptions, new SimpleImageLoadingListener() {

			@Override
			public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
				iv.setImageBitmap(loadedImage);
				super.onLoadingComplete(imageUri, view, loadedImage);
			}

			@Override
			public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
				iv.setImageResource(R.drawable.ic_photo_corrupt);
				super.onLoadingFailed(imageUri, view, failReason);
			}

			@Override
			public void onLoadingStarted(String imageUri, View view) {
				iv.setImageResource(R.drawable.default_sohagame_logo);
				super.onLoadingStarted(imageUri, view);
			}																														
		});
	}
	public static void loadPhotoOrigin(final String url, final ImageView iv, final Context context,
			final OnImageLoaderListener onImageLoaderListener) {
		ImageLoader.getInstance().loadImage(url, displayImageOptions, new SimpleImageLoadingListener() {

			@Override
			public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
				iv.setImageBitmap(loadedImage);
				onImageLoaderListener.onImageLoaded();
				super.onLoadingComplete(imageUri, view, loadedImage);
			}

			@Override
			public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
				iv.setImageResource(R.drawable.ic_photo_corrupt);
				super.onLoadingFailed(imageUri, view, failReason);
			}

			@Override
			public void onLoadingStarted(String imageUri, View view) {
				iv.setImageResource(R.drawable.ic_photo_loading);
				super.onLoadingStarted(imageUri, view);
			}																														
		});
	}

	public static void loadPhotoOrigin(final String url, final ImageView iv, final Context context,
			final OnImageLoaderBitmapListener onImageLoaderBitmapListener) {
		ImageLoader.getInstance().loadImage(url, displayImageOptions, new SimpleImageLoadingListener() {

			@Override
			public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
				iv.setImageBitmap(loadedImage);
				onImageLoaderBitmapListener.onImageLoaded(loadedImage);
				super.onLoadingComplete(imageUri, view, loadedImage);
			}

			@Override
			public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
				iv.setImageResource(R.drawable.ic_photo_corrupt);
				onImageLoaderBitmapListener.onImageLoadFailed();
				super.onLoadingFailed(imageUri, view, failReason);
			}

			@Override
			public void onLoadingStarted(String imageUri, View view) {
				iv.setImageResource(R.drawable.ic_photo_loading);
				onImageLoaderBitmapListener.onImageLoadFailed();
				super.onLoadingStarted(imageUri, view);
			}																														
		});
	}
	// --

	public interface OnImageLoaderListener {
		public void onImageLoaded();
	}

	public interface OnImageLoaderBitmapListener {
		public void onImageLoaded(Bitmap loadedBitmap);
		public void onImageLoadFailed();
	}
}
