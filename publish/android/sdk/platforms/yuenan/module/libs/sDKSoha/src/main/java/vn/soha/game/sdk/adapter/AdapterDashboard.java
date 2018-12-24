package vn.soha.game.sdk.adapter;

import java.util.List;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.model.DashBoardItem;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

public class AdapterDashboard extends BaseAdapter {

	List<DashBoardItem> listOption;
	Context mContext;
	LayoutInflater inflater;

	public AdapterDashboard(Context context,List<DashBoardItem> data) {
		this.listOption = data;
		this.mContext = context;
		inflater = (LayoutInflater) mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return listOption.size();
	}

	@Override
	public Object getItem(int arg0) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public long getItemId(int arg0) {
		// TODO Auto-generated method stub
		return listOption.size();
	}

	@Override
	public View getView(int position, View convertView, ViewGroup root) {
		// TODO Auto-generated method stub
		DashBoardItem data = listOption.get(position);
		if (convertView == null) {
			convertView = (View) inflater.inflate(R.layout.item_dashboard,root, false);
			Holder hoder = new Holder();
			hoder.imgIcon = (ImageView) convertView.findViewById(R.id.imgItemsDb);
			hoder.tvTitle = (TextView) convertView.findViewById(R.id.tvItemsDb);
			hoder.tvNotfiDb = (TextView) convertView.findViewById(R.id.tvNotiDb);
			convertView.setTag(hoder);
		}
		Holder hoder = (Holder) convertView.getTag();
		Picasso.with(mContext).load(data.getIcon()).into(hoder.imgIcon);
		hoder.tvTitle.setText(data.getTitle());
		if(data.getNotify()>0){
			hoder.tvNotfiDb.setVisibility(View.VISIBLE);
			hoder.tvNotfiDb.setText(""+data.getNotify());
		}else{
			hoder.tvNotfiDb.setVisibility(View.GONE);
		}
		
		return convertView;
	}

	class Holder {
		ImageView imgIcon;
		TextView tvTitle, tvNotfiDb;
	}
}
