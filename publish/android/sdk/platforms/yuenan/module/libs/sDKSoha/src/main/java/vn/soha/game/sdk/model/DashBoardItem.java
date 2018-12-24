package vn.soha.game.sdk.model;

import java.util.ArrayList;
import java.util.List;

public class DashBoardItem {

	String title;
	String type;
	String icon,pageId;
	String url;
	int tab;
	int active;
	int autoOpen ;
	String messageActive;
	int notify;
	Boolean isLoaded = false;
	List<DashBoardItem> listSubTab = new ArrayList<DashBoardItem>();

	public DashBoardItem() {
		super();
	}

	public int getAutoOpen() {
		return autoOpen;
	}

	public void setAutoOpen(int autoOpen) {
		this.autoOpen = autoOpen;
	}

	public DashBoardItem(String title, String type, String icon, String url, int tab, int active) {
		super();
		this.title = title;
		this.type = type;
		this.icon = icon;
		this.url = url;
		this.tab = tab;
		this.active = active;
	}
	
	

	public String getPageId() {
		return pageId;
	}

	public void setPageId(String pageId) {
		this.pageId = pageId;
	}

	public int getActive() {
		return active;
	}

	public void setActive(int active) {
		this.active = active;
	}

	public String getMessageActive() {
		return messageActive;
	}

	public void setMessageActive(String messageActive) {
		this.messageActive = messageActive;
	}

	public int getTab() {
		return tab;
	}

	public void setTab(int tab) {
		this.tab = tab;
	}

	public int getNotify() {
		return notify;
	}

	public void setNotify(int notify) {
		this.notify = notify;
	}

	public List<DashBoardItem> getListSubTab() {
		return listSubTab;
	}

	public void setListSubTab(List<DashBoardItem> listSubTab) {
		this.listSubTab = listSubTab;
	}

	public Boolean getIsLoaded() {
		return isLoaded;
	}

	public void setIsLoaded(Boolean isLoaded) {
		this.isLoaded = isLoaded;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public String getIcon() {
		return icon;
	}

	public void setIcon(String icon) {
		this.icon = icon;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

}
