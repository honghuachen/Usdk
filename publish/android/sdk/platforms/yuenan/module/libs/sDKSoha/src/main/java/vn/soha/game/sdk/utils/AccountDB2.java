package vn.soha.game.sdk.utils;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteException;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

/**
 * @since Sunday, February 22, 2015
 * @author hoangcaomobile
 *
 */
public class AccountDB2 extends SQLiteOpenHelper {

	private static final String TAG = AccountDB2.class.getSimpleName();
	private static final String DATABASE_NAME = "DBAccount2.db";
	private static final int DATABASE_VERSION = 1;	
	private static final String TABLE_NAME_ACCOUNT = "db_account2";
	private static final String ACCOUNT_ID = "accountId";
	private static final String ACCOUNT_EMAIL = "accountEmail";
	private static final String ACCOUNT_ACCESSTOKEN = "accountAccessToken";
	private static final String ACCOUNT_AVATAR = "accountAvatar";

	private static final String TABLE_ACCOUNT_CREATE
	= "CREATE TABLE " + TABLE_NAME_ACCOUNT
	+ " ("
	+ ACCOUNT_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, "
	+ ACCOUNT_EMAIL + " VARCHAR(255), "
	+ ACCOUNT_ACCESSTOKEN + " VARCHAR(255), "
	+ ACCOUNT_AVATAR + " VARCHAR(255));";

	private static final String TABLE_ACCOUNT_DROP = 
			"DROP TABLE IF EXISTS "
					+ TABLE_NAME_ACCOUNT;

	public AccountDB2(Context context) {
		super(context, DATABASE_NAME, null, DATABASE_VERSION);
	}

	@Override
	public void onCreate(SQLiteDatabase db) {
		db.execSQL(TABLE_ACCOUNT_CREATE);
	}

	@Override
	public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
		Log.w(TAG, "Upgrade der DB von V: "+ oldVersion + " zu V:" + newVersion + "; Alle Daten werden gel�scht!");
		db.execSQL(TABLE_ACCOUNT_DROP);
		onCreate(db);
	}

	// insert
	public void vInsert(String userEmail, String userAccessToken, String userAvatar) {
		long rowId = -1;
		try {
			SQLiteDatabase db = getWritableDatabase();
			ContentValues values = new ContentValues();
			values.put(ACCOUNT_EMAIL, userEmail);
			values.put(ACCOUNT_ACCESSTOKEN, userAccessToken);
			values.put(ACCOUNT_AVATAR, userAvatar);
			rowId = db.insert(TABLE_NAME_ACCOUNT, null, values);
		} catch (SQLiteException e){
			Log.e(TAG, "insert()", e);
		} finally {
			Log.i(TAG, "insert(): rowId=" + rowId + " | " + userEmail);
		}
	}
	// --


	// delete a item in history
	public void vDelete(String DBRowId) {
		/**
		 * delete if it existed
		 */
		String SELECT_QUERY = "SELECT * FROM " + TABLE_NAME_ACCOUNT + " WHERE " + ACCOUNT_ID + " LIKE " + DBRowId;
		SQLiteDatabase db1 = getWritableDatabase();
		Cursor dbCursor =  db1.rawQuery(SELECT_QUERY, null);

		if (dbCursor.getCount() > 0){
			int noOfScorer = 0;
			if (dbCursor != null) {
				dbCursor.moveToFirst();
			}
			while ((!dbCursor.isAfterLast()) && noOfScorer < dbCursor.getCount()) {
				noOfScorer++;
				db1.delete(TABLE_NAME_ACCOUNT, ACCOUNT_ID + "=" + dbCursor.getString(0), null);
				dbCursor.moveToNext();
			}
			Log.i(TAG, "deleted: " + DBRowId);
		} else {
			Log.i(TAG, "delete fail: " + DBRowId);
		}

		// --
	}
	// --

	// delete all item in history
	public void vDeleteAll() {
		/**
		 * delete if it existed
		 */
		String SELECT_QUERY = "SELECT * FROM " + TABLE_NAME_ACCOUNT;
		SQLiteDatabase db1 = getWritableDatabase();
		Cursor dbCursor =  db1.rawQuery(SELECT_QUERY, null);

		if (dbCursor.getCount() > 0){
			int noOfScorer = 0;
			if (dbCursor != null) {
				dbCursor.moveToFirst();
			}
			while ((!dbCursor.isAfterLast()) && noOfScorer < dbCursor.getCount()) {
				noOfScorer++;
				db1.delete(TABLE_NAME_ACCOUNT, ACCOUNT_ID + "=" + dbCursor.getString(0), null);
				dbCursor.moveToNext();
			}
			Log.i(TAG, "deleted all");
		} else {
			Log.i(TAG, "delete all fail");
		}

		// --
	}
	// --

	public Cursor vGet() {
		SQLiteDatabase db = getWritableDatabase();
		String SELECT_QUERY = "SELECT * FROM " + TABLE_NAME_ACCOUNT + " ORDER BY " + ACCOUNT_ID + " DESC LIMIT 1000000";
		return db.rawQuery(SELECT_QUERY, null);
	}

}