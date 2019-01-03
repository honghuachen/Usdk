#!/bin/bash
export LANG="en_US.UTF-8"

RootPath=$(pwd)
publish_properties=publish.properties
version_properties=version.properties
global_properties=global.properties

buildType=debug
platform=,subPlatform=
versionName=,versionCode=
package=,appname=,cdn=,plugins=,icon=,splash=

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
		  echo "包含"
		else
		  val=${val}" "${array[0]}
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
			val=${val}" "${array[1]}
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
	_value=( $( __readINI ${publish_properties} ${platform}-${subPlatform} $1 ) )
	if [ -z "$_value" ]; then
		_value=( $( __readINI ${publish_properties} ${platform}-default $1 ) )
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

function __buildUnity(){
	package=( $( __getPublishProperties package ) )
	appname=( $( __getPublishProperties appname ) )
	cdn=( $( __getPublishProperties cdn ) )
	plugins=( $( __getPublishProperties plugins ) )
	icon=( $( __getPublishProperties icon ) )
	splash=( $( __getPublishProperties splash ) )
	
	echo $package $appname $cdn $plugins $icon $splash
	
	echo -e "\n------------Xcode export------------"
	time=$(date "+%Y%m%d%H%M%S")
	logfile=./logs/$time.log
	UNITY_PATH=( "$( __readINI ${global_properties} unity unity.exe )" )
	UNITY_PROJECT_PATH=( "$( __readINI ${global_properties} unity project.dir )" )
	XCODE_OUT_PATH=( "$( __readINI ${global_properties} unity xcode.outdir )"/$platform )
	MONO_PATH=( "$( __readINI ${global_properties} mono mono.exe )" )
	echo $XCODE_OUT_PATH
	
	#kill unity
	ps -ef | grep Unity | grep -v grep | awk '{print $2}' | xargs kill -9
	parameter="platform=$platform versionName=$versionName versionCode=$versionCode buildType=$buildType package=$package appName=$appname cdn=$cdn plugins=$plugins icon=$icon splash=$splash xcodeOut=$XCODE_OUT_PATH"
	#build unity
	#"$UNITY_PATH" -projectPath "$UNITY_PROJECT_PATH" -executeMethod BuildiOS.Build $parameter -quit -batchmode -logFile $logfile
	#"$UNITY_PATH" -projectPath "$UNITY_PROJECT_PATH" -executeMethod BuildiOS.Build $parameter -logFile $logfile
	
	echo -e "\n------------platform XcodeSetting------------"
	platformPath=$RootPath/sdk/platforms/$platform
	"$MONO_PATH" ./tools/XcodeSetting.exe "$XCODE_OUT_PATH" $platformPath
	
	echo -e "\n------------plugins XcodeSetting------------"
	array=(${plugins//,/ }) 
	for var in ${array[@]}
	do
		pluginPath=$RootPath/sdk/plugins/$var
		"$MONO_PATH" ./tools/XcodeSetting.exe "$XCODE_OUT_PATH" $pluginPath
	done
}

function __main(){
	__showVersion
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
		__buildUnity
	elif [ $select == 2 ]
	then
		buildType=release
		__inputPlatforms
		__buildUnity
	elif [ $select == 3 ]
	then
		__modifyVersion
		clear
		__main
	fi
}

__main
read -p "按回车键继续"

