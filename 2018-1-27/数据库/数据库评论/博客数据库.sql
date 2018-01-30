
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
���ƣ��û���ϸ��Ϣ�� Users_Details_Details 
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
  Users_Details_Resume text --�û����� 
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
	AdminUser_Name varchar(20) not null,--�û��ǳ�
	AdminUser_RealName varchar(20) ,--�û�ʵ������
	AdminUser_Sex int default 0 ,--�û��Ա�  0  ��  1 Ů 
	AdminUser_Photo varchar(64) ,--�û�ͷ��
	AdminUser_IsSuper int,--�Ƿ��ǳ�������Ա  1��������Ա,0����Ա
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
Write_Title varchar(40),--���±���
Write_ArticleType int,--�������� 0��ʾԭ��,1��ʾת�أ�2��ʾ����
Write_Content text ,--��������?????
Backstage_Category_Id int,--���ͷ���(��̨�������)
Write_Label varchar(40),--���±�ǩ
Category_Id int,--��������ݸ���������(�Զ������)
Write_Private int,--���͵Ŀ���״̬  0 Ϊ˽�ܣ�1 Ϊ����
ULogin_Id int ,--����Id
Write_Createdate datetime,--���´��������棩ʱ��
Write_Deletedate datetime,--����ɾ��ʱ��
Write_State int ,--���͵����״̬ 0 �����  1 ���δͨ��  2 ���ͨ��    
Write_IsDel int, -- 0 ɾ��  1 ����  2���� 
Write_ReadCount int, --�����
Write_ZanCount int,  --������
Write_Collection int, --�ղش���
Blog_Recommend int, --�Ƿ��Ƽ�  1���Ƽ� 2�����Ƽ�
)
go
select * from Write_blog




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
select * from Category



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
�����ˣ�
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



/*
���ƣ����ʱ�  User_Question
ʱ�䣺18-1-17
�����ˣ�
*/
if	exists(select 1 from sys.tables where name='User_Question')

begin

	drop table User_Question
end
go
create table User_Question
(
	UQ_ID int primary key identity,	--����
	UQ_Type	int,					--��������
	UQ_Label nvarchar(64),			--�����ǩ
	UQ_Title nvarchar(64),			--��������
	UQ_Content text,				--����
	UQ_Datetime datetime,			--����ʱ��
	UQ_State int,					--(δ��� 0  �ѽ�� 1)
	UQ_ReaderCount int,				--�Ķ�����		
	UA_AnswerCount int,				--�ش����
	UQ_ReportCount int,				--���޴���
	UQ_CollectionCount int,			--�ղش���
	UQ_User_ID int					--�û�ID�����ʵ��û���
)

go


/*
���ƣ��ش��  User_Answer_Question
ʱ�䣺18-1-16
�����ˣ�
*/
if	exists(select 1 from sys.tables where name='User_Answer_Question')

begin

	drop table User_Answer_Question
end
go
create table User_Answer_Question
(
	UA_ID int primary key identity,	--�ش���
	UA_UQ_ID int,					--������
	UA_Context text,				--�ش�����
	UA_Date datetime,				--�ش�ʱ��
	UA_User_ID int,					--�ش��û����
	UA_AgreeCount int,				--�Ƿ���Ѵ𰸣�0 ����  1 �ǣ�
	UQ_ReportCount int,  			--����
	UA_CommentCount  int			--����
)

-----���۱�
if	exists(select 1 from sys.tables where name='User_Answer_Comment')
	drop table User_Answer_Comment
go
create table User_Answer_Comment
(
	UAC_ID int primary key identity,	--���۱��
	UA_UQ_ID int,						--������
	UA_ID int,							--�ش��߱��
	Comment_Time datetime ,				--���� ʱ��
	UAC_Comment nvarchar(64),			--��������
	UA_User_ID int						--�����û��߱��
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


---------������������----------

if	exists(select 1 from sys.tables where name='Blog_Comment')
	drop table Blog_Comment
go
create table Blog_Comment
(
	BC_ArticleID int primary key,           --��������id
	BC_CommentId int ,	                    --��������id(��ʽΪ��0001����ÿ��һ�����۾ͼ���)
    BC_UserID int,                          --������id(��û�м�Ϊnull)
	BC_Comment_ParentId  int,				--���۸���id(��������id,��û�м�Ϊnull) 
	BC_Comment_Time datetime ,				--����ʱ��
	BC_Comment nvarchar(64),			    --��������
)
go
