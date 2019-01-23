# Usdk 
这是一个Unity3D移动平台sdk快速适配框架和多渠道打包平台，方便android、ios移动平台渠道sdk或者各种Native功能插件的快速接入，接入过程无需修改添加任何的C#代码。内置的打包工具，能够在不修改任何unity工程的情况下实现快速的单渠道和多渠道打包。sdk适配时间和渠道打包时间至少可以减少一半以上。

# 目录结构
[usdk目录结构与目录说明（xmind思维导图）](https://github.com/honghuachen/Usdk/blob/master/doc/Usdk%E9%A1%B9%E7%9B%AE%E7%BB%93%E6%9E%84%E8%AF%B4%E6%98%8E.xmind)
![image](https://github.com/honghuachen/Usdk/blob/master/doc/Usdk%E9%A1%B9%E7%9B%AE%E7%BB%93%E6%9E%84%E8%AF%B4%E6%98%8E.png)

# 文档
[Android开发文档](https://github.com/honghuachen/Usdk/blob/master/doc/Android%E5%BC%80%E5%8F%91%E6%96%87%E6%A1%A3.docx)  
[iOS开发文档](https://github.com/honghuachen/Usdk/blob/master/doc/iOS%E5%BC%80%E5%8F%91%E6%96%87%E6%A1%A3.docx)  
[Unity导出xcode自动配置工具文档](https://github.com/honghuachen/Usdk/blob/master/doc/Unity%E5%AF%BC%E5%87%BAxcode%E8%87%AA%E5%8A%A8%E9%85%8D%E7%BD%AE%E5%B7%A5%E5%85%B7.docx)

# 系统要求
#android：
#Java JDK1.7或1.8，android sdk8.0以上（至少升到了27）

# Unity3D版本支持
Unity全版本支持

# 多平台支持
目前支持android、ios
android平台支持eclipse、android studio工程适配
ios支持xcode工程适配

# 快速适配渠道sdk和Native插件
Usdk能够快速的适配各个渠道sdk和Native插件，适配过程无需修改和添加任何C#代码，做到C#代码的零入侵。
Usdk可以做到按需调用各种Native插件，各种Native插件可以按需打入各个渠道包，剔除无用的Native插件，从而节省包体。

# 强大的多渠道打包平台
Usdk不仅仅是一个sdk适配框架，更是一个功能强大的多渠道打包平台，通过强大的sdk适配机制能够快速的实现单渠道和多渠道的出包，并且可以大大的减少出包时间。

# Unity导出xcode自动配置工具
[文档](https://github.com/honghuachen/Usdk/blob/master/doc/Unity%E5%AF%BC%E5%87%BAxcode%E8%87%AA%E5%8A%A8%E9%85%8D%E7%BD%AE%E5%B7%A5%E5%85%B7.docx)  
在unity4.x通过xupoter插件来设置unity导出xcode自动配置，到了unity5.x unity自己实现了一套工具，并且比xupoter提供了更完善的操作可能。但是unity的工具集成在了unity编辑器里面，所以如果要想使用只能在unity工程中通过编辑器功能来操作导出xcode工程的自动配置。这样有一个缺点就是对于工程不需要变化但是又要打多个渠道的iOS包的情况下需要每次都要重新从Unity导出到xcode，大项目这是十分浪费时间的。针对上述情况，Usdk独立出了unity这套自动配置工具，完全脱离了Unity体系，能够单独作为一个外部工具随时随地的操作xcode工程下的工程配置文件和Info.plist配置文件，可以添加或者移除指定库、源码、资源等，操作非常灵活，只要你能想到的操作都能够实现。
并且工具通过unity安装目录下的mono虚拟机进行运行，所以具有跨平台的能力，不管是windows还是mac都能够使用。

# 技术支持
一群：954331139
