package mobisocial.osm;

// Declare any non-default types here with import statements
//import mobisocial.osm.OsmGroup;
import mobisocial.osm.OsmHotSpot;
import mobisocial.osm.Rdl;
//import mobisocial.osm.OsmIdentity;
import android.os.Bundle;
import android.os.Messenger;

/** Example service interface */
interface IOsmService {
    void sendPicture(in Uri groupUri, in Uri imageUri);
    void sendText(in Uri groupUri, in String text);
    void sendAnimatedGif(in Uri groupUri, in Uri dataUri);
    void sendRdl(in Uri groupUri, in Rdl rdl);
    
    Uri createFeed(in String feedName, in Uri thumbnailUri, in long[] memberIds);
    Uri createFeedWithMembers(in long[] memberIds);
    Uri createControlFeed();
    void updateFeedDetails(in Uri feedUri, in String feedName, in Uri thumbnailUri);
    
    boolean isLoggedIn();
    void requestBlobs(in Uri[] blobUris, in Messenger receiver, in Bundle[] data);
    void requestBlob(in Uri blobUri, in Messenger receiver, in Bundle data);
    boolean requestSyncLimit(in Uri feedUri, in long oldest);
    long getSyncLimit(in Uri feedUri);
    
    long[] lookupIdentities(in String[] identities, in boolean requireApp);
    
    OsmHotSpot[] getHotSpots();
    
    boolean isBlobAvailable(in Uri blobUri);
    boolean[] isBlobAvailableMulti(in Uri[] blobUris);

	void sendObj(in Uri groupUri, in String type, in String jsonString);
	void sendContentFromIntent(in Uri groupUri, in Intent content);
    
    Uri createControlFeedWithMembers(in long[] memberIds);
    boolean addIdentitiesToFeed(in Uri groupUri, in long[] identityIds);
    boolean addRawIdentitiesToFeed(in Uri groupUri, in String[] rawIdentities);
    boolean removeIdentitiesFromFeed(in Uri groupUri, in long[] identityIds);
    
    String getLocalBlobPath(in Uri blobUri);
    String[] getLocalBlobPaths(in Uri[] blobUri);
}