package vn.soha.game.sdk.adapter;

import java.util.ArrayList;
import java.util.List;

import com.squareup.picasso.Picasso;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import vn.sgame.sdk.R;
import vn.soha.game.sdk.model.DashBoardItem;

public class DashBoardAdapter extends BaseAdapter {

	private final Context mContext;
	private List<DashBoardItem> mDashBoards = new ArrayList<DashBoardItem>();
	private boolean mIsMore = false;

	public DashBoardAdapter(Context context, List<DashBoardItem> dashBoards, boolean isMore) {
		this.mContext = context;
		this.mDashBoards = dashBoards;
		this.mIsMore = isMore;
	}

	@Override
	public int getCount() {
		return mDashBoards.size();
	}

	@Override
	public Object getItem(int position) {
		return mDashBoards.get(position);
	}

	@Override
	public long getItemId(int position) {
		return position;
	}

	public List<DashBoardItem> getListDashBoard() {
		return mDashBoards;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHolder viewHolder = null;
		GridView grid = (GridView) parent;
		int size = grid.getColumnWidth();
		if (convertView == null) {
			final LayoutInflater layoutInflater = LayoutInflater.from(mContext);
			convertView = layoutInflater.inflate(R.layout.dashboard_gridview_items, parent, false);
			convertView.setLayoutParams(new GridView.LayoutParams(size, size));
			viewHolder = new ViewHolder();
			viewHolder.image = (ImageView) convertView.findViewById(R.id.ivIcon);
			viewHolder.title = (TextView) convertView.findViewById(R.id.tvTitle);
			viewHolder.notify = (TextView) convertView.findViewById(R.id.tvNotify);

			convertView.setTag(viewHolder);
		} else {
			viewHolder = (ViewHolder) convertView.getTag();
		}

		DashBoardItem mDashBoard = mDashBoards.get(position);

		if (mDashBoard != null) {
			if (position == 4) {
				if (mIsMore) {
					convertView.setVisibility(View.VISIBLE);
					Picasso.with(mContext).load(R.drawable.back).into(viewHolder.image);
					viewHolder.title.setVisibility(View.GONE);
				} else {
					convertView.setVisibility(View.INVISIBLE);
				}
			} else {
				if (!mDashBoard.getTitle().equals("blank")) {
					if (mDashBoard.getTitle().equals(mContext.getString(R.string.textviewMore))) {
						Picasso.with(mContext).load(R.drawable.more).into(viewHolder.image);
						viewHolder.title.setVisibility(View.GONE);
					} else {
						if (mDashBoard.getIcon() != "")
							Picasso.with(mContext).load(mDashBoard.getIcon()).into(viewHolder.image);
						viewHolder.title.setText(mDashBoard.getTitle());
						if (mDashBoard.getNotify() > 0) {
							viewHolder.notify.setText(String.valueOf(mDashBoard.getNotify()));
							viewHolder.notify.setVisibility(View.VISIBLE);
						} else {
							viewHolder.notify.setVisibility(View.GONE);
						}
					}

				} else
					convertView.setVisibility(View.INVISIBLE);
			}
		}

		return convertView;
	}

	public class ViewHolder {
		ImageView image;
		TextView title;
		TextView notify;
	}

}
