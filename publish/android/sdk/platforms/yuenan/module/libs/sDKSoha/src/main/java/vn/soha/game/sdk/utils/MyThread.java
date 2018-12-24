package vn.soha.game.sdk.utils;

public class MyThread extends Thread {
	
	int myId;
	public MyThread(Runnable runable)
	{
		super(runable);
	}
		
	boolean isRuning =true;

	public boolean isRuning() {
		return isRuning;
	}

	public void setRuning(boolean isRuning) {
		this.isRuning = isRuning;
	}

	public int getMyId() {
		return myId;
	}

	public void setMyId(int myId) {
		this.myId = myId;
	}

	
}
