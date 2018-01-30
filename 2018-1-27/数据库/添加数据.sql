
use Blog
go



--用户详细信息表
insert into Users_Details values('我是小花花','笑哈哈','普通员工',1,'1.jpg',null,'软件工程师',0,0,GETDATE(),null,null,null)
insert into Users_Details values('我是小辉辉','校灰灰','普通员工',0,'1.jpg',null,'软件工程师',0,0,'2018-01-12 9:50',null,null,null)
insert into Users_Details values('我是小辉辉','校灰灰','普通员工',1,'1.jpg',null,'前端工程师',0,0,'2018-01-12 9:50',null,null,null)


select * from Users_Details


--用户登录表
insert into ULogin values('13271822161','1741792632@qq.com','E10ADC3949BA59ABBE56E057F20F883E','小花花',1)
insert into ULogin values('13271822356','1234567894@qq.com','E10ADC3949BA59ABBE56E057F20F883E','小灰灰',2)
insert into ULogin values('13271822568','1741792632@qq.com','E10ADC3949BA59ABBE56E057F20F883E','梅超风',3)
insert into ULogin values('12345678977','8644@163.com','E10ADC3949BA59ABBE56E057F20F883E','天马行空',1)


--update ULogin set ULogin_Password='E10ADC3949BA59ABBE56E057F20F883E' 
select * from ULogin


--编辑博客表
insert into Write_blog values('SignalR的用法',0,'创建一个项目',3,'SignalR,即时通讯','1,2',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')
insert into Write_blog values('SignalR的用法',0,'<p>一会见</p><p><br/></p><p><strong>还原u</strong></p><p><strong><br/></strong></p><p><img src="http://localhost:8000/content/utf8-net/dialogs/emotion/images/jx2/j_0001.gif"/><img src="http://localhost:8000/content/utf8-net/dialogs/emotion/images/jx2/j_0014.gif"/><img src="http://localhost:8000/content/utf8-net/dialogs/emotion/images/ldw/w_0015.gif"/></p><p><br/></p>',3,'SignalR,即时通讯','1,2',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')
insert into Write_blog values('SignalR的用法',0,'<p>一会见</p><p style="line-height: 16px;"><img src="http://localhost:8000/content/utf8-net/dialogs/attachment/fileTypeImages/icon_rar.gif"/><a style="font-size:12px; color:#0066cc;" href="/content/utf8-net/net/upload/file/20180120/6365206223174901444211146.rar" title="2018-1-19.rar">2018-1-19.rar</a></p><p><strong>还原u</strong></p><p><strong><br/></strong></p><p><strong><img src="http://img.baidu.com/hi/jx2/j_0002.gif"/></strong></p><p><br/></p><p><strong><br/></strong></p><p><img src="http://img.baidu.com/hi/jx2/j_0001.gif"/></p>',3,'SignalR,即时通讯','2,3',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')

insert into Write_blog values('ef是什么',0,'<p style="white-space: normal;">ef是.net 的框架</p><p style="white-space: normal;"><br/></p><table><tbody><tr class="firstRow"><td width="256" valign="top" style="word-break: break-all;">添加</td><td width="256" valign="top" style="word-break: break-all;">删除</td><td width="256" valign="top" style="word-break: break-all;">修改</td></tr><tr><td width="256" valign="top" style="word-break: break-all;">add</td><td width="256" valign="top" style="word-break: break-all;">deleted</td><td width="256" valign="top" style="word-break: break-all;">modified，Unchanged</td></tr><tr><td width="256" valign="top"><br/></td><td width="256" valign="top"><br/></td><td width="256" valign="top"><br/></td></tr></tbody></table><p style="white-space: normal;">Ⅶ</p><p><img src="http://localhost:8000/content/utf8-net/dialogs/emotion/images/jx2/j_0002.gif"/></p>',3,'框架','1,2',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')
insert into Write_blog values('ht6hy',0,'<pre class="brush:html;toolbar:false">&lt;script&gt; alert(123);&lt;/script&gt;</pre><p><br/></p>',3,'框架','3,1',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')
insert into Write_blog values('啦啦啦阿拉',0,'<p><img src="/content/utf8-net/net/upload/image/20180122/6365221340652319312208041.jpg" title="Koala.jpg" alt="Koala.jpg"/></p>',3,'框架','1,2',1,1,'2018-01-17 9:50',null,2,2,10,5,0,2,'否')



select * from Write_blog


--自定义分类表
insert into Category values('.NET',1,0)
insert into Category values('C++',1,0)
insert into Category values('JavaScript',1,1)
select * from Category


insert into Comment values(2,GETDATE(),2,'你得文章写的很好',1,null,null)
insert into Comment values(3,GETDATE(),2,'哇咔咔',1,null,1);
insert into Comment values(4,GETDATE(),2,'啦啦啦',1,null,null);
select * from Comment



--后台管理员分类表
insert Backstage_Category values('前端')
insert Backstage_Category values('计算机')
insert Backstage_Category values('编程语言')
insert Backstage_Category values('数据库')




