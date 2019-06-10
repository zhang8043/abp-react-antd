# ASP.NET Boilerplate + React + Ant Design Pro

## 新版Vue UI将在 https://github.com/zhang8043/abp-vue-antd 搭建

# 项目简介：

* abp-react-antd是一套基于ASP.NET Boilerplate + React + Ant Design Pro开发的框架，源代码完全开源。
* 采用主流框架，容易上手，简单易学，学习成本低。可完全实现二次开发、基本满足80%项目需求。
* 操作权限控制精密细致，对所有管理链接都进行权限验证，可控制到导航菜单、功能按钮。
* 提高开发效率及质量。常用类封装，日志、缓存、验证、字典、文件、邮件、,Excel。等等，目前兼容浏览器（IE8+、Chrome、Firefox、360浏览器等）

# 功能模块：

- 登录
- 注册
- 工作台
- 个人中心
- 个人设置
- 系统管理
  - 组织结构管理
  - 租户管理
  - 角色管理
  - 用户管理
  - 审计日志
  - 数据字典
  - 通知管理
  - UI设置
  - 系统设置
- 工作流
  - 流程管理
  - 表单管理
- Arcgis地图
- 企业微信

（部分功能暂未实现，玩命开发中...）

# 技术介绍：

## 什么是ASP.NET Boilerplate？

ASP.NET Boilerplate是专为新的现代Web应用程序设计的通用**应用程序框架**。它使用已经**熟悉的工具**并实现围绕它们的**最佳实践**，为您提供SOLID**开发体验**。

**常见的企业应用功能：**
* 需要身份验证和授权的应用程序的用户，角色和权限管理。
* SaaS应用程序的租户和版本管理。
* 组织单位管理。
* 语言和本地化文本管理。
* Identity Server 4集成。

<a href="https://aspnetboilerplate.com/Pages/Documents/Module-System" target="_blank">**ASP.NET Boilerplate官方文档**</a>

## 什么是 React？

React是一个用于构建用户界面的JavaScript库。

* **声明性:** React使得创建交互式UI变得轻而易举。为应用程序中的每个状态设计简单视图，当数据发生变化时，React将有效地更新和呈现正确的组件。声明性视图使您的代码更易于理解，更易于理解，并且更易于调试。
* **基于组件:** 构建管理其自身状态的封装组件，然后组合它们以制作复杂的UI。由于组件逻辑是用JavaScript而不是模板编写的，因此您可以轻松地通过应用程序传递丰富的数据，并将状态保持在DOM之外。
* **学习一次，随处写入:** 我们不会对您的技术堆栈的其余部分做出假设，因此您可以在React中开发新功能而无需重写现有代码。React还可以使用Node和使用React Native的移动应用程序在服务器上呈现。

<a href="https://react.docschina.org/docs/hello-world.html" target="_blank">**React中文文档**</a>

## 什么是 Ant Design Pro？

一套企业级的 UI 设计语言和 **React** 实现,**开箱即用**的中台前端/设计解决方案。
* **优雅美观**：基于 Ant Design 体系精心设计
* **常见设计模式**：提炼自中后台应用的典型页面和场景
* **最新技术栈**：使用 React/umi/dva/antd 等前端前沿技术开发
* **响应式**：针对不同屏幕大小设计
* **主题**：可配置的主题满足多样化的品牌诉求
* **国际化**：内建业界通用的国际化方案
* **最佳实践**：良好的工程实践助您持续产出高质量代码
* **Mock 数据**：实用的本地数据调试方案
* **UI 测试**：自动化测试保障前端产品质量

<a href="https://pro.ant.design/docs/getting-started-cn" target="_blank">**Ant Design Pro官方文档**</a>

# 开发步骤

* 项目下载完成，准备配置项目
* 根目录中的 Precise-Antd是前端React目录，Precise-Core是后台的目录

## 后端解决方案初始化配置
>* 打开项目前，请确保已经安装 **.NET CORE 2.1** 版本，下载地址：<a href="https://dotnet.microsoft.com/download/dotnet-core/2.1" target="_blank">**.NET Core**</a>

>* 还原nuget包

>* 项目数据库连接字符串和跨域的基本配置

>* 将数据库连接字符串修改为你的连接字符串 (注意:默认数据库为 SQL Server ，最低要求 2012 版本)

>* 修改项目调试启动配置信息

>* 初始化数据库

>* 将 .Web.Host 设置为启动项目

>* 打开程序包管理控制台,并设置默认项目为 .EntityFrameworkCore

>* 输入命令，迁移数据库: 
```
    update-database
```
>* 如果没有用过EFCore Code First做迁移，请先查阅资料<a href="https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/" target="_blank">官方文档</a>

>* 如果你的环境没有问题，这个时候只需要 Ctrl+F5 就能运行，运行成功后就能在浏览器看到API界面了

## 前端解决方案初始化配置
>* 进入项目中的Precise-Antd目录
```
    $ yarn 或 npm install
```

>* 运行项目
>记得先启动后台
```
    $ npm start
```
>* 启动调试，访问  <a href="http://127.0.0.1:8000" target="_blank">http://127.0.0.1:8000</a> 查看效果。

>* 构建和部署
```
    $ npm run build
```
>该命令会将所有文件编译到 `dist` 目录下

# 截图
![后台API](https://github.com/zhang8043/abp-react-antd/blob/master/Precise-Antd/src/assets/rendering/1547774802.jpg)
![登录页面](https://github.com/zhang8043/abp-react-antd/blob/master/Precise-Antd/src/assets/rendering/1547775725.jpg)
![监控页](https://github.com/zhang8043/abp-react-antd/blob/master/Precise-Antd/src/assets/rendering/1547775726.jpg)
![Arcgis地图](https://github.com/zhang8043/abp-react-antd/blob/master/Precise-Antd/src/assets/rendering/1547775803.jpg)
![G6Editor](https://github.com/zhang8043/abp-react-antd/blob/master/Precise-Antd/src/assets/rendering/1547775926.jpg)
