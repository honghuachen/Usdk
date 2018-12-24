package vn.soha.game.sdk.utils;

import vn.sgame.sdk.R;
import android.content.Context;
import android.widget.ImageView;

import com.squareup.picasso.Picasso;

/**
 * @since Friday, April 24, 2015
 * @author hoangcaomobile
 *
 */

public class PhotoDownloader {

	public static void vLoadPhotoCenterInside(Context context, String photoUrl, ImageView iv) {
		Picasso.with(context)
		.load(photoUrl) //
		.placeholder(R.drawable.ic_photo_loading) //
		.error(R.drawable.ic_photo_corrupt) //
		.fit() //
		.centerInside()
		.tag(context) //
		.into(iv);
	}

	public static void vLoadAvatarCenterInside(Context context, String photoUrl, ImageView iv) {
		Picasso.with(context)
		.load(photoUrl) //
		.placeholder(R.drawable.avatar_default) //
		.error(R.drawable.avatar_default) //
		.fit() //
		.centerInside()
		.tag(context) //
		.into(iv);
	}

}
