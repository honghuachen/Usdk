package vn.soha.game.sdk.utils;

import vn.sgame.sdk.R;
import android.content.Context;
import android.widget.EditText;

/**
 * @since March 11, 2015
 * @author hoangcaomobile
 *
 */
public class ColorUtils {

	public static void vSetHintColor(Context context, EditText editext) {
		editext.setHintTextColor(context.getResources().getColor(R.color.gray));
	}

}
