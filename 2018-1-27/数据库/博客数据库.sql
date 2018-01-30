
use master
go

if exists (select * from sys.databases where name='Blog')
begin	
		drop database Blog		
end
go
create database Blog
go

use Blog
go


--前后台用户详细信息表
/*
名称：前后台用户详细信息表 Users_Details
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
  Users_Details_Resume varchar(600) --用户简述 
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
	Users_Details_ID int,--前后台用户信息编号
	AdminUser_IsSuper int,--是否是超级管理员
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
名称：热门博客推荐表Blog_Recommend
负责人：
位置：首页
时间：18-1-12
*/
--if exists (select * from sys.objects where name='Blog_Recommend')
--begin
--		drop table Blog_Recommend
--end
--go
--create table Blog_Recommend(
--id int primary key identity(1,1), --热门博客编号
--Write_blog_Id varchar(50),--文章编号
--)
--go



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
Write_Title varchar(100),--文章标题
Write_ArticleType int,--文章类型 0表示原创,1表示转载，2表示翻译
Write_Content text ,--博客内容?????
Backstage_Category_Id int,--博客分类(后台管理分类)
Write_Label varchar(40),--文章标签
Category_Id varchar(30),--外键，根据个人类别分类(自定义分类)的编号
Write_Private int,--博客的开放状态  0 为私密，1 为公开
ULogin_Id int ,--博主Id
Write_Createdate datetime,--文章创建（保存）时间
Write_Deletedate datetime,--文章删除时间
Write_State int ,--博客的审核状态 0 待审核  1 审核未通过  2 审核通过    
Write_IsDel int, -- 0 删除  1 保存  2发布 
Write_ReadCount int, --浏览量
Write_ZanCount int,  --点赞量
Write_Collection int, --收藏次数
Blog_Recommend int, --是否推荐播客 1 推荐 2 不推荐
ULogin_IdIsManager varchar(5)--是否为管理员  是：是管理员   否：不是管理员
)
go





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


--select COUNT(1) from Comment where  Write_blog_Id=1 and Parent_Comment_Id is null


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

insert into Auditing_Blog values(1,1,1,GETDATE(),'拉拉垃圾')

--update Write_blog set Write_State=1 where Write_blog_Id=1

--select * from write_blog

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
























if exists(select 1 from sys.sysobjects where name='KeyWord')
	drop table KeyWord
go
create table KeyWord
(
id int identity primary key,
KeyWord_Val nvarchar(10),
)
go



if	exists(select 1 from sys.tables where name='Blog_Comment')
	drop table Blog_Comment
go
create table Blog_Comment
(
	BC_CommentId int primary key identity,	                    --评论内容id(形式为‘0001’，每加一条评论就加你)
	BC_ArticleID int ,           --评论文章id
	BC_BlogID int,       --博主编号
    BC_UserID int,                          --评论者id(若没有即为null)
    BC_ForUserID int,--被评论者的编号
	BC_Comment_ParentId  int,				--评论父级id(评论内容id,若没有即为null) 
	BC_Comment_Time datetime ,				--评论时间
	BC_Comment text			    --评论内容
)
go


select * from Blog_Comment where BC_UserID=1




insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'你写得真好34324343242343243432434545354545454354554545454545454545454545454545454545')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'谢谢夸奖')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'你写得真好啦啦啦')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'哈哈哈哈哈')
insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'你写得真好')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'谢谢夸奖')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'你写得真好啦啦啦')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'哈哈哈哈哈')

insert into Blog_Comment values(1,1,3,1,null,GETDATE(),'你写得真好f')
insert into Blog_Comment values(1,1,2,3,9,GETDATE(),'谢谢夸奖g')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'你写得真好啦啦啦g')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'哈哈哈哈哈g')
insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'你写得真好g')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'谢谢夸奖g')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'你写得真好啦啦啦g')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'哈哈哈哈哈g')



insert into Blog_Comment values(2,1,2,1,null,GETDATE(),'你写得真好34324343242343243432434545354545454354554545454545454545454545454545454545')
insert into Blog_Comment values(2,1,3,1,17,GETDATE(),'谢谢夸奖')
insert into Blog_Comment values(2,1,4,1,null,GETDATE(),'你写得真好啦啦啦')
insert into Blog_Comment values(2,1,2,3,17,GETDATE(),'哈哈哈哈哈')
insert into Blog_Comment values(2,1,2,1,null,GETDATE(),'你写得真好')
insert into Blog_Comment values(2,1,3,1,17,GETDATE(),'谢谢夸奖')
insert into Blog_Comment values(2,1,4,1,null,GETDATE(),'你写得真好啦啦啦')
insert into Blog_Comment values(2,1,4,2,17,GETDATE(),'哈哈哈哈哈hty5')
insert into Blog_Comment values(2,1,1,3,18,GETDATE(),'哈哈哈哈哈r34545')


