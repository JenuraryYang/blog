
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


--ǰ��̨�û���ϸ��Ϣ��
/*
���ƣ�ǰ��̨�û���ϸ��Ϣ�� Users_Details
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Users_Details')
	drop table Users_Details
go
create table Users_Details
(
  Users_Details_ID int primary key identity,--������Ϣ����
  Users_Details_Name varchar(20) not null,--�û��ǳ�
  Users_Details_RealName varchar(20) ,--�û�ʵ������
  Users_Details_Jop varchar(20) ,--�û�ְλ
  Users_Details_Sex int default 0 ,--�û��Ա�  0  ��  1 Ů 
  Users_Details_Photo varchar(64) ,--�û�ͷ��
  Users_Details_Birthday varchar(64),--�û�����
  Users_Details_Industry varchar(64) ,--�û���ҵ
  Users_Details_Follow int ,--��ע����
  Users_Details_Etc int ,--��˿����
  Users_Details_LoginDate datetime,--ע��ʱ��
  Users_Details_Delete datetime,--ע��ʱ��
  Users_Details_LoginUrl varchar(10),--��½��ַ
  --Users_Details_Integral int,--���֣�>=0
  --Users_Details_Gold int,--��ң�>=0
  Users_Details_Resume varchar(600) --�û����� 
)
go


/*
���ƣ���½ע�� ULogin 
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.sysobjects where name='ULogin')
	drop table ULogin
go
create table ULogin
(
ULogin_Id int primary key identity not null,--����ID
ULogin_Phone varchar(11),--�绰����
ULogin_Email varchar(32),--�����˺�
ULogin_Password varchar(32),--����
Ulogin_BlogName varchar(32),--������
Users_Details_ID int--ǰ��̨�û���Ϣ���
)
go



/*
���ƣ���̨����Ա
ʱ�䣺2018-1-14
*/
if exists(select 1 from sys.sysobjects where name='AdminUser')
	drop table AdminUser
go
create table AdminUser
(
	AdminUser_Id int primary key identity,--���
	AdminUser_Number varchar(20),--��̨�û��˺�
	AdminUser_Pwd  varchar(20),--����
	Users_Details_ID int,--ǰ��̨�û���Ϣ���
	AdminUser_IsSuper int,--�Ƿ��ǳ�������Ա
)
go


/*
���ƣ��ֲ�ͼ��Carousel_figure
�����ˣ�
λ�ã���ҳ
ʱ�䣺18-1-12
*/
if exists (select * from sys.objects where name='Carousel_figure')
begin
		drop table Carousel_figure
end
go
create table Carousel_figure
(
Carousel_Id int primary key identity(1,1), --���
Carousel_Image varchar(100),--�ֲ�ͼƬ��ַ
Write_blog_Id varchar(100),--���ͼƬ����ת��ַ(����)
Carousel_Remark varchar(100) --��ע
)
go


/*
���ƣ����Ų����Ƽ���Blog_Recommend
�����ˣ�
λ�ã���ҳ
ʱ�䣺18-1-12
*/
--if exists (select * from sys.objects where name='Blog_Recommend')
--begin
--		drop table Blog_Recommend
--end
--go
--create table Blog_Recommend(
--id int primary key identity(1,1), --���Ų��ͱ��
--Write_blog_Id varchar(50),--���±��
--)
--go



/*
���ƣ�д����Write_blog
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Write_blog')
	drop table Write_blog
go
create table Write_blog
(
Write_blog_Id int primary key not null identity,--����id
Write_Title varchar(100),--���±���
Write_ArticleType int,--�������� 0��ʾԭ��,1��ʾת�أ�2��ʾ����
Write_Content text ,--��������?????
Backstage_Category_Id int,--���ͷ���(��̨�������)
Write_Label varchar(40),--���±�ǩ
Category_Id varchar(30),--��������ݸ���������(�Զ������)�ı��
Write_Private int,--���͵Ŀ���״̬  0 Ϊ˽�ܣ�1 Ϊ����
ULogin_Id int ,--����Id
Write_Createdate datetime,--���´��������棩ʱ��
Write_Deletedate datetime,--����ɾ��ʱ��
Write_State int ,--���͵����״̬ 0 �����  1 ���δͨ��  2 ���ͨ��    
Write_IsDel int, -- 0 ɾ��  1 ����  2���� 
Write_ReadCount int, --�����
Write_ZanCount int,  --������
Write_Collection int, --�ղش���
Blog_Recommend int, --�Ƿ��Ƽ����� 1 �Ƽ� 2 ���Ƽ�
ULogin_IdIsManager varchar(5)--�Ƿ�Ϊ����Ա  �ǣ��ǹ���Ա   �񣺲��ǹ���Ա
)
go





/*
���ƣ����۹���Comment
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.sysobjects where name='Comment')
	drop table Comment
go
create table Comment
(
Comment_Id int primary key identity not null,--����ID
Comment_User int,--�����û���� 
Comment_Time datetime ,--���� ʱ��
Write_blog_Id int,--�ظ������£��������ID
Comment_Content varchar(200),--�ظ�����
ULogin_Id int,--���������ID
Comment_deletedate datetime,--ɾ������ʱ��
Parent_Comment_Id int --�������
)
go


--select COUNT(1) from Comment where  Write_blog_Id=1 and Parent_Comment_Id is null


/*
���ƣ��������
ʱ�䣺2018-1-14
*/
if exists(select 1 from sys.sysobjects where name='Auditing_Blog')
	drop table Auditing_Blog
go
create table Auditing_Blog
(
	Auditing_Blog_Id int primary key identity,--���
	AdminUser_Id int ,--��̨�û��˺�
	Write_blog_Id int,--�������±��
	Auditing_Blog_State  int ,--���͵����״̬ 1 ���δͨ��  2 ���ͨ��    
	Auditing_Blog_Time datetime,--���ʱ��
	Auditing_Blog_Msg varchar(50)--δͨ����ʾ(�й���Ա��д)
)
go

insert into Auditing_Blog values(1,1,1,GETDATE(),'��������')

--update Write_blog set Write_State=1 where Write_blog_Id=1

--select * from write_blog

/*
���ƣ����Զ��壩������ Category
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.objects where name='Category')
	drop table Category
go
create table Category
(
Category_Id int identity primary key not null,--����ID
Category_Type varchar(20),--�������
--Category_Num int,--������µ���������
ULogin_Id int, --����ID�����
Category_Reception int--�Ƿ���ʾ��ǰ̨0��ʾ,1����ʾ
)
go




/*
���ƣ�������Ա������������ Backstage_Category
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select 1 from sys.objects where name='Backstage_Category')
	drop table Backstage_Category
go
create table Backstage_Category
(
Backstage_Category_Id int identity primary key not null,--����ID
Backstage_Category_Type varchar(20),--�������
)
go



/*
���ƣ�����ר��Special
�����ˣ�
ʱ�䣺18-1-12
*/
if exists(select * from sys.sysobjects where name='Special')
	drop table Special
go
create table Special
(
Special_Id int primary key identity not null,--����ID
Special_Name varchar(20),--ר��
Special_Introduce varchar(200),--ר������
ULogin_Id int,--���������ID
)
go


/*
���ƣ��û���ϵ��  FollowUser
ʱ�䣺18-1-13
*/
if exists(select 1 from sys.sysobjects where name='FollowUser')
	drop table FollowUser
go
create table FollowUser
(
FollowUser_ID int identity primary key,--���
FollowUser_User1 int,--��ע�˱��
FollowUser_User2 int,--����ע�˱��
FollowUser_Remark nvarchar(10),--��ע
FollowUser_FollowDate datetime,--��עʱ��
FollowUser_CancelData datetime,--ȡ����עʱ��
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
	BC_CommentId int primary key identity,	                    --��������id(��ʽΪ��0001����ÿ��һ�����۾ͼ���)
	BC_ArticleID int ,           --��������id
	BC_BlogID int,       --�������
    BC_UserID int,                          --������id(��û�м�Ϊnull)
    BC_ForUserID int,--�������ߵı��
	BC_Comment_ParentId  int,				--���۸���id(��������id,��û�м�Ϊnull) 
	BC_Comment_Time datetime ,				--����ʱ��
	BC_Comment text			    --��������
)
go


select * from Blog_Comment where BC_UserID=1




insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'��д�����34324343242343243432434545354545454354554545454545454545454545454545454545')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'лл�佱')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'��д�����������')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'����������')
insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'��д�����')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'лл�佱')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'��д�����������')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'����������')

insert into Blog_Comment values(1,1,3,1,null,GETDATE(),'��д�����f')
insert into Blog_Comment values(1,1,2,3,9,GETDATE(),'лл�佱g')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'��д�����������g')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'����������g')
insert into Blog_Comment values(1,1,2,1,null,GETDATE(),'��д�����g')
insert into Blog_Comment values(1,1,3,2,1,GETDATE(),'лл�佱g')
insert into Blog_Comment values(1,1,4,1,null,GETDATE(),'��д�����������g')
insert into Blog_Comment values(1,1,2,3,2,GETDATE(),'����������g')



insert into Blog_Comment values(2,1,2,1,null,GETDATE(),'��д�����34324343242343243432434545354545454354554545454545454545454545454545454545')
insert into Blog_Comment values(2,1,3,1,17,GETDATE(),'лл�佱')
insert into Blog_Comment values(2,1,4,1,null,GETDATE(),'��д�����������')
insert into Blog_Comment values(2,1,2,3,17,GETDATE(),'����������')
insert into Blog_Comment values(2,1,2,1,null,GETDATE(),'��д�����')
insert into Blog_Comment values(2,1,3,1,17,GETDATE(),'лл�佱')
insert into Blog_Comment values(2,1,4,1,null,GETDATE(),'��д�����������')
insert into Blog_Comment values(2,1,4,2,17,GETDATE(),'����������hty5')
insert into Blog_Comment values(2,1,1,3,18,GETDATE(),'����������r34545')


