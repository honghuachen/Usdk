package vn.sgame.sdk.view;

import android.content.Context;
import android.graphics.Canvas;
import android.util.AttributeSet;

public class CustomListView extends HorizontalListView{
	
	private android.view.ViewGroup.LayoutParams params;
    private int oldCount = 0;

	public CustomListView(Context context, AttributeSet attrs) {
		super(context, attrs);
		// TODO Auto-generated constructor stub
	}

	 @Override
	    protected void onDraw(Canvas canvas)
	    {
	        if (getCount() != oldCount) 
	        {
	            int width = getChildAt(0).getWidth() + 1 ;
	            oldCount = getCount();
	            params = getLayoutParams();
	            params.width = getCount() * width;
	            setLayoutParams(params);
	        }

	        super.onDraw(canvas);
	    }
}
