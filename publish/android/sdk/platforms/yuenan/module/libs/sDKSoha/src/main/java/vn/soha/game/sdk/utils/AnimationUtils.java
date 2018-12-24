package vn.soha.game.sdk.utils;

import android.view.View;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.view.animation.AnimationSet;
import android.view.animation.DecelerateInterpolator;

/**
 * @since January 27, 2015
 * @author hoangcaomobile
 *
 */
public class AnimationUtils {

	public static void vAnimationClick(View view) {
		Animation fadeIn = new AlphaAnimation(0, 1);
		fadeIn.setInterpolator(new DecelerateInterpolator()); //add this
		fadeIn.setDuration(200);

		AnimationSet animation = new AnimationSet(false); //change to false
		animation.addAnimation(fadeIn);

		view.setAnimation(animation);
		view.startAnimation(animation);
	}

}
