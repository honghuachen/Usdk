#!/bin/bash
export LANG="en_US.UTF-8"

RootPath=$(pwd)
gradlebuildTemp=${RootPath}/buildTemp
gradletool_path=${RootPath}/tools/gradle

publish_properties=publish.properties
version_properties=version.properties
global_properties=global.properties

gradle_properties=${gradlebuildTemp}/gradle.properties
local_properties=${gradlebuildTemp}/local.properties
settings_gradle=${gradlebuildTemp}/settings.gradle
global_gradle=${gradlebuildTemp}/global.gradle

buildType=debug
platform=,subPlatform=
versionName=,versionCode=
build=,keystore=,package=,appname=,cdn=,plugins=,icon=,splash=
#podsType可以为Project\Workspace
podsType=Project

# __readINI [配置文件路径+名称] [节点名] [键值]
function __readINI() {
	INIFILE=$1; SECTION=$2; ITEM=$3
	#_readIni=`awk -F '=' '/\['$SECTION'\]/{a=1}a==1&&$1~/'$ITEM'/{print $2;exit}' $INIFILE`
	_readIni=`awk -F '=' "/\[${SECTION}\]/{a=1}a==1" ${INIFILE}|sed -e '1d' -e '/^$/d' -e '/^\[.*\]/,$d' -e "/^${ITEM}=.*/!d" -e "s/^${ITEM}=//"`
	echo ${_readIni}
}

function __readINI2() {
	INIFILE=$1; SECTION=$2; ITEM=$3
	_readIni=`awk -F '=' '/\['$SECTION'\]/{a=1}a==1&&$1~/'$ITEM'/{print $2;exit}' $INIFILE`
	#_readIni=`awk -F '=' "/\[${SECTION}\]/{a=1}a==1" ${INIFILE}|sed -e '1d' -e '/^$/d' -e '/^\[.*\]/,$d' -e "/^${ITEM}=.*/!d" -e "s/^${ITEM}=//"`
	echo ${_readIni}
}

# __readIniSections [配置文件路径+名称]
function __readIniSections() {
    INIFILE=$1
    val=$(awk '/\[/{printf("%s ",$1)}' ${INIFILE} | sed 's/\[//g' | sed 's/\]//g')
    echo ${val}
}

function __showPlatforms(){
	val=""
	array=($(__readIniSections ${publish_properties}))
	for var in ${array[@]}
	do
		array=(${var//-/ })  	
		if [[ ${val} =~ ${array[0]} ]]
		then
		  t="包含"
		else
		  val=${val}" ["${array[0]}]
		fi
	done
	echo $val
}

# __showSubPlatforms [platform]
function __showSubPlatforms(){
	platform=$1
	val=""
	array=($(__readIniSections ${publish_properties}))
	for var in ${array[@]}
	do
		array=(${var//-/ })  
		if [ "${array[0]}" == "${platform}" ]
		then
			val=${val}" ["${array[1]}]
		fi
	done
	echo $val
}

function __inputPlatforms(){
	__showPlatforms
	read -p "input platform:" platform
	__showSubPlatforms $platform
	read -p "input sub platform:" subPlatform
	if [ -z "$subPlatform" ];then
		subPlatform=default
	fi
}

# __getPublishProperties [键值]
function __getPublishProperties(){
	_value=( "$( __readINI ${publish_properties} ${platform}-${subPlatform} $1 )" )
	if [ -z "$_value" ]; then
		_value=( "$( __readINI ${publish_properties} ${platform}-default $1 )" )
	fi
	
	echo $_value
}

function __showVersion(){
	echo ------------Version------------
	if [ ! -f ${version_properties} ];then
		echo "1.0.0" > ${version_properties}
		echo "100" >>  ${version_properties}
	fi
	
	idx=0
	for line in `cat ${version_properties}`
	do
		let idx=$idx+1
		if [ $idx == 1 ];then
			versionName=$line
			echo "VersionName:"$versionName
		elif [ $idx == 2 ];then
			versionCode=$line
			echo "versionCode:"$versionCode
		fi
	done
	echo -e "\n"
}

function __modifyVersion(){
	read -p "VersionName:" versionName
	read -p "VersionCode:" versionCode
	echo $versionName > ${version_properties}
	echo $versionCode >>  ${version_properties}
}

function __initPlatformConfig(){
	build=( "$( __getPublishProperties build )" )
	keystore=( "$( __getPublishProperties keystore )" )
	package=( "$( __getPublishProperties package )" )
	appname=( "$( __getPublishProperties appname )" )
	cdn=( "$( __getPublishProperties cdn )" )
	plugins=( "$( __getPublishProperties plugins )" )
	icon=( "$( __getPublishProperties icon )" )
	splash=( "$( __getPublishProperties splash )" )

	echo ---------------------------------------$appname
	echo $build $keystore $package $appname $cdn $plugins $icon $splash
}

function __genGradleProperties(){
	tempDir=$RootPath/buildTemp
	if [ -d "$gradlebuildTemp" ];then
		rm -rf $gradlebuildTemp
	fi
	mkdir $gradlebuildTemp

	cp -r $RootPath/tools/gradle/wrapper/. ${gradlebuildTemp}

	storePath=$RootPath/sdk/keystore/${keystore}
	chmod +x $storePath
	dos2unix $storePath
	
	UnityProjectDir=( "$( __readINI ${global_properties} Unity project.dir )" )
	UnityProjectType=( "$( __readINI ${global_properties} Unity export.type )" )
	JavaVersion=( "$( __readINI ${global_properties} Java java.version )" )
	keystorename=( "$( __readINI ${storePath} keystore keystore )" )
	storepass=( "$( __readINI ${storePath} keystore storepass )" )
	alias=( "$( __readINI ${storePath} keystore alias )" )
	keypass=( "$( __readINI ${storePath} keystore keypass )" )
	SdkDir=( "$( __readINI ${global_properties} AndroidSdk sdk.dir )" )
	NdkDir=( "$( __readINI ${global_properties} AndroidSdk ndk.dir )" )
	echo ${UnityProjectDir} ${UnityProjectType} ${JavaVersion} ${keystorename} ${storepass} ${alias} ${keypass} ${SdkDir} ${NdkDir}

	keystoreRootPath=${RootPath}/sdk/keystore
	keystorePath=${keystoreRootPath}/${keystorename}
	cp ${keystorePath} ${UnityProjectDir}/${keystorename}
	echo VersionName=${versionName}>${gradle_properties}
	echo VersionCode=${versionCode}>>${gradle_properties}
	echo Package=${package}>>${gradle_properties}
	echo AppName=${appname}>>${gradle_properties}
	echo UnityProjectType=${UnityProjectType}>>${gradle_properties}
	echo JavaVersion=${JavaVersion}>>${gradle_properties}

	echo AppReleaseDir=./outputs/apk>>${gradle_properties}
	echo Keystore=${keystorename}>>${gradle_properties}
	echo StorePassword=${storepass}>>${gradle_properties}
	echo KeyAlias=${alias}>>${gradle_properties}
	echo KeyPassword=${keypass}>>${gradle_properties}

	echo android.enableAapt2=false>>${gradle_properties}
	echo org.gradle.parallel=true>>${gradle_properties}
	echo org.gradle.daemon=true>>${gradle_properties}
	echo org.gradle.configureondemand=true>>${gradle_properties}
	echo org.gradle.jvmargs=-Xmx4096m -Xms2048m -XX:MaxPermSize=2048m>>${gradle_properties}

	echo sdk.dir=${SdkDir}>${local_properties}
	#echo ndk.dir=${NdkDir}>>${local_properties}

	echo include \':app\'>${settings_gradle}
	echo include \':unity\'>>${settings_gradle}
	TempUnityProjectDir=$( echo ${UnityProjectDir} | sed 's:\\:\/:g' )
	echo project\(\':unity\'\).projectDir=new File\(\'${TempUnityProjectDir}\'\)>>${settings_gradle}

	unityAndroidPath=${UnityProjectDir}
	if [ ${UnityProjectType} == "eclipse" ]
	then
		appNameXmlPath=${unityAndroidPath}/res/values/strings.xml
	elif [ ${UnityProjectType} == "as" ]
	then
		appNameXmlPath=${unityAndroidPath}/src/main/res/values/strings.xml
	fi

	array=(${plugins//,/ }) 
	for var in ${array[@]}
	do
		echo include \':${var}\'>>${settings_gradle}
		echo project\(\':${var}\'\).projectDir=new File\(\'../sdk/plugins/${var}/module\'\)>>${settings_gradle}
	done
	echo include \':${platform}\'>>${settings_gradle}
	echo project\(\':${platform}\'\).projectDir=new File\(\'../sdk/platforms/${platform}/module\'\)>>${settings_gradle}
	echo include \':usdk\'>>${settings_gradle}
	echo project\(\':usdk\'\).projectDir=new File\(\'../sdk/usdk/module\'\)>>${settings_gradle}
	platformSettingGradle=./sdk/platforms/${platform}/module/settings.gradle
	if [ -f ${platformSettingGradle} ];then
		cat ${platformSettingGradle} >> ${settings_gradle}
	fi

	#extend build.gradle
	BuildGradle=${unityAndroidPath}/build.gradle
	if [ -f ${BuildGradle} ];then
		rm -rf ${BuildGradle}
	fi
	cp ${gradletool_path}/templates/mainTemplate.gradle ${BuildGradle}

	echo -e >>${BuildGradle}
	echo dependencies {>>${BuildGradle}
	echo "	compile project(':app')">>${BuildGradle}
	echo "	compile project(':usdk')">>${BuildGradle}
	echo "	compile project(':${platform}')">>${BuildGradle}
	for var in ${array[@]}
	do
		echo "	compile project(':${var}')">>${BuildGradle}
	done
	echo }>>${BuildGradle}
}

function __readySdkRes(){
	#修改appName
	java -jar ${RootPath}/tools/assetconfigtool/ModifyAppName.jar ${appNameXmlPath} 'app_name' ${appname}
	#构建临时module用于不同渠道构建差异资源
	mkdir ${gradlebuildTemp}/app
	mkdir ${gradlebuildTemp}/app/res
	mkdir ${gradlebuildTemp}/app/libs
	mkdir ${gradlebuildTemp}/app/assets
	mkdir ${gradlebuildTemp}/app/assets/bin
	mkdir ${gradlebuildTemp}/app/assets/bin/Data
	cp -r ${icon} ${gradlebuildTemp}/app/res
	cp -r ${splash} ${gradlebuildTemp}/app/assets/bin/Data
	cp ${RootPath}/tools/gradle/templates/libTemplate.gradle ${gradlebuildTemp}/app/build.gradle
	cp ${RootPath}/tools/gradle/templates/EmptyAndroidManifest.xml ${gradlebuildTemp}/app/AndroidManifest.xml
}

function __showMenu(){
	echo :Main
	echo -----------------------------------
	echo 1.Publish game to debug                  [Debug apk]
	echo 2.Publish game to release                [Release apk]
	echo 3.Modify version information             [Modify version]
	echo -----------------------------------

	read -p "select item:" select
	if [ $select == 1 ]
	then
		buildType=debug
		__inputPlatforms
	elif [ $select == 2 ]
	then
		buildType=release
		__inputPlatforms
	elif [ $select == 3 ]
	then
		__modifyVersion
		clear
		__main
	fi
}

function __main(){
	chmod +x ${RootPath}/global.properties
	chmod +x ${RootPath}/version.properties
	chmod +x ${RootPath}/publish.properties
	dos2unix ${RootPath}/global.properties
	dos2unix ${RootPath}/version.properties
	dos2unix ${RootPath}/publish.properties
	
	__showVersion
	__showMenu
	__initPlatformConfig
	__genGradleProperties
	__readySdkRes

	cd ${gradlebuildTemp}
	chmod +x gradlew
	./gradlew assembleRelease --stacktrace
}

__main
read -p "按回车键继续"