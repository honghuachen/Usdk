package vn.soha.game.sdk.model;

public class MQTTAction {
 String action,extra,roleId,roleName,userId,areaId,roleLevel,vId;
 public String getvId() {
	return vId;
}

public void setvId(String vId) {
	this.vId = vId;
}

Long time;

/*public MQTTAction(String action, String extra) {
	super();
	this.action = action;
	this.extra = extra;
}*/

public MQTTAction(String action, String extra, Long time) {
	super();
	this.action = action;
	this.extra = extra;
	this.time = time;
}

public String getRoleId() {
	return roleId;
}

public void setRoleId(String roleId) {
	this.roleId = roleId;
}

public String getRoleName() {
	return roleName;
}

public void setRoleName(String roleName) {
	this.roleName = roleName;
}

public String getUserId() {
	return userId;
}

public void setUserId(String userId) {
	this.userId = userId;
}

public String getAreaId() {
	return areaId;
}

public void setAreaId(String areaId) {
	this.areaId = areaId;
}

public String getRoleLevel() {
	return roleLevel;
}

public void setRoleLevel(String roleLevel) {
	this.roleLevel = roleLevel;
}

public Long getTime() {
	return time;
}

public void setTime(Long time) {
	this.time = time;
}

public String getAction() {
	return action;
}

public void setAction(String action) {
	this.action = action;
}

public String getExtra() {
	return extra;
}

public void setExtra(String extra) {
	this.extra = extra;
}
 
}
