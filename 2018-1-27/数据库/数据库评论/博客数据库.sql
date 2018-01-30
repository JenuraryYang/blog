
use master
go

if exists (select * from sys.databases where name='Blog')	
		drop database Blog	
go
create database Blog
go

use Blog
go


/*
名称：用户详细信息表 Users_Details_Details 
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Users_Details')
	drop table Users_Details
go
create table Users_Details
(
  Users_Details_ID int primary key identity,--个人信息表标号
  Users_Details_Name varchar(20) not null,--用户昵称
  Users_Details_RealName varchar(20) ,--用户实际名称
  Users_Details_Jop varchar(20) ,--用户职位
  Users_Details_Sex int default 0 ,--用户性别  0  男  1 女 
  Users_Details_Photo varchar(64) ,--用户头像
  Users_Details_Birthday varchar(64),--用户生日
  Users_Details_Industry varchar(64) ,--用户行业
  Users_Details_Follow int ,--关注人数
  Users_Details_Etc int ,--粉丝人数
  Users_Details_LoginDate datetime,--注册时间
  Users_Details_Delete datetime,--注销时间
  Users_Details_LoginUrl varchar(10),--登陆地址
  --Users_Details_Integral int,--积分，>=0
  --Users_Details_Gold int,--金币，>=0
  Users_Details_Resume text --用户简述 
)
go


/*
名称：登陆注册 ULogin 
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.sysobjects where name='ULogin')
	drop table ULogin
go
create table ULogin
(
ULogin_Id int primary key identity not null,--自增ID
ULogin_Phone varchar(11),--电话号码
ULogin_Email varchar(32),--邮箱账号
ULogin_Password varchar(32),--密码
Ulogin_BlogName varchar(32),--博客名
Users_Details_ID int--前后台用户信息编号
)
go


/*
名称：后台管理员
时间：2018-1-14
*/
if exists(select 1 from sys.sysobjects where name='AdminUser')
	drop table AdminUser
go
create table AdminUser
(
	AdminUser_Id int primary key identity,--编号
	AdminUser_Number varchar(20),--后台用户账号
	AdminUser_Pwd  varchar(20),--密码
	AdminUser_Name varchar(20) not null,--用户昵称
	AdminUser_RealName varchar(20) ,--用户实际名称
	AdminUser_Sex int default 0 ,--用户性别  0  男  1 女 
	AdminUser_Photo varchar(64) ,--用户头像
	AdminUser_IsSuper int,--是否是超级管理员  1超级管理员,0管理员
)
go

/*
名称：轮播图表Carousel_figure
负责人：
位置：首页
时间：18-1-12
*/
if exists (select * from sys.objects where name='Carousel_figure')
begin
		drop table Carousel_figure
end
go
create table Carousel_figure
(
Carousel_Id int primary key identity(1,1), --编号
Carousel_Image varchar(100),--轮播图片地址
Write_blog_Id varchar(100),--点击图片后跳转地址(保留)
Carousel_Remark varchar(100) --备注
)
go




/*
名称：写博客Write_blog
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Write_blog')
	drop table Write_blog
go
create table Write_blog
(
Write_blog_Id int primary key not null identity,--自增id
Write_Title varchar(40),--文章标题
Write_ArticleType int,--文章类型 0表示原创,1表示转载，2表示翻译
Write_Content text ,--博客内容?????
Backstage_Category_Id int,--博客分类(后台管理分类)
Write_Label varchar(40),--文章标签
Category_Id int,--外键，根据个人类别分类(自定义分类)
Write_Private int,--博客的开放状态  0 为私密，1 为公开
ULogin_Id int ,--博主Id
Write_Createdate datetime,--文章创建（保存）时间
Write_Deletedate datetime,--文章删除时间
Write_State int ,--博客的审核状态 0 待审核  1 审核未通过  2 审核通过    
Write_IsDel int, -- 0 删除  1 保存  2发布 
Write_ReadCount int, --浏览量
Write_ZanCount int,  --点赞量
Write_Collection int, --收藏次数
Blog_Recommend int, --是否推荐  1、推荐 2、不推荐
)
go
select * from Write_blog




/*
名称：评论管理Comment
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Comment')
	drop table Comment
go
create table Comment
(
Comment_Id int primary key identity not null,--自增ID
Comment_User int,--评论用户编号 
Comment_Time datetime ,--评论 时间
Write_blog_Id int,--回复的文章，外键文章ID
Comment_Content varchar(200),--回复内容
ULogin_Id int,--外键，博主ID
Comment_deletedate datetime,--删除评论时间
Parent_Comment_Id int --父级编号
)
go





/*
名称：文章审核
时间：2018-1-14
*/
if exists(select 1 from sys.sysobjects where name='Auditing_Blog')
	drop table Auditing_Blog
go
create table Auditing_Blog
(
	Auditing_Blog_Id int primary key identity,--编号
	AdminUser_Id int ,--后台用户账号
	Write_blog_Id int,--博客文章编号
	Auditing_Blog_State  int ,--博客的审核状态 1 审核未通过  2 审核通过    
	Auditing_Blog_Time datetime,--审核时间
	Auditing_Blog_Msg varchar(50)--未通过提示(有管理员填写)
)
go




/*
名称：（自定义）类别管理 Category
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.objects where name='Category')
	drop table Category
go
create table Category
(
Category_Id int identity primary key not null,--自增ID
Category_Type varchar(20),--类别名称
--Category_Num int,--该类别下的文章数量
ULogin_Id int, --博主ID，外键
Category_Reception int--是否显示在前台0显示,1不显示
)
go
select * from Category



/*
名称：（管理员操作）类别管理 Backstage_Category
负责人：
时间：18-1-12
*/
if exists(select 1 from sys.objects where name='Backstage_Category')
	drop table Backstage_Category
go
create table Backstage_Category
(
Backstage_Category_Id int identity primary key not null,--自增ID
Backstage_Category_Type varchar(20),--类别名称
)
go



/*
名称：博客专栏Special
负责人：
时间：18-1-12
*/
if exists(select * from sys.sysobjects where name='Special')
	drop table Special
go
create table Special
(
Special_Id int primary key identity not null,--自增ID
Special_Name varchar(20),--专栏
Special_Introduce varchar(200),--专栏介绍
ULogin_Id int,--外键，博主ID
)
go


/*
名称：用户关系表  FollowUser
时间：18-1-13
负责人：
*/
if exists(select 1 from sys.sysobjects where name='FollowUser')
	drop table FollowUser
go
create table FollowUser
(
FollowUser_ID int identity primary key,--编号
FollowUser_User1 int,--关注人编号
FollowUser_User2 int,--被关注人编号
FollowUser_Remark nvarchar(10),--备注
FollowUser_FollowDate datetime,--关注时间
FollowUser_CancelData datetime,--取消关注时间
)
go



/*
名称：提问表  User_Question
时间：18-1-17
负责人：
*/
if	exists(select 1 from sys.tables where name='User_Question')

begin

	drop table User_Question
end
go
create table User_Question
(
	UQ_ID int primary key identity,	--主键
	UQ_Type	int,					--问题类型
	UQ_Label nvarchar(64),			--问题标签
	UQ_Title nvarchar(64),			--问题名称
	UQ_Content text,				--内容
	UQ_Datetime datetime,			--提问时间
	UQ_State int,					--(未解决 0  已解决 1)
	UQ_ReaderCount int,				--阅读次数		
	UA_AnswerCount int,				--回答次数
	UQ_ReportCount int,				--点赞次数
	UQ_CollectionCount int,			--收藏次数
	UQ_User_ID int					--用户ID（提问的用户）
)

go


/*
名称：回答表  User_Answer_Question
时间：18-1-16
负责人：
*/
if	exists(select 1 from sys.tables where name='User_Answer_Question')

begin

	drop table User_Answer_Question
end
go
create table User_Answer_Question
(
	UA_ID int primary key identity,	--回答编号
	UA_UQ_ID int,					--问题编号
	UA_Context text,				--回答内容
	UA_Date datetime,				--回答时间
	UA_User_ID int,					--回答用户编号
	UA_AgreeCount int,				--是否最佳答案（0 不是  1 是）
	UQ_ReportCount int,  			--点赞
	UA_CommentCount  int			--评论
)

-----评论表
if	exists(select 1 from sys.tables where name='User_Answer_Comment')
	drop table User_Answer_Comment
go
create table User_Answer_Comment
(
	UAC_ID int primary key identity,	--评论编号
	UA_UQ_ID int,						--问题编号
	UA_ID int,							--回答者编号
	Comment_Time datetime ,				--评论 时间
	UAC_Comment nvarchar(64),			--评论内容
	UA_User_ID int						--评论用户者编号
)
go

if exists(select 1 from sys.sysobjects where name='KeyWord')
	drop table KeyWord
go
create table KeyWord
(
id int identity primary key,
KeyWord_Val nvarchar(10),
)
go


---------博客文章评论----------

if	exists(select 1 from sys.tables where name='Blog_Comment')
	drop table Blog_Comment
go
create table Blog_Comment
(
	BC_ArticleID int primary key,           --评论文章id
	BC_CommentId int ,	                    --评论内容id(形式为‘0001’，每加一条评论就加你)
    BC_UserID int,                          --评论者id(若没有即为null)
	BC_Comment_ParentId  int,				--评论父级id(评论内容id,若没有即为null) 
	BC_Comment_Time datetime ,				--评论时间
	BC_Comment nvarchar(64),			    --评论内容
)
go
