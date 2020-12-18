﻿--User
truncate table [user]
insert into [user] (name,password) values ('admin','e10adc3949ba59abbe56e057f0f883e')
insert into [user] (name,password) values ('test01','e10adc3949ba59abbe56e057f0f883e')
insert into [user] (name,password) values ('test02','e10adc3949ba59abbe56e057f0f883e')
insert into [user] (name,password) values ('test03','e10adc3949ba59abbe56e057f0f883e')
insert into [user] (name,password) values ('test04','e10adc3949ba59abbe56e057f0f883e')
select * from [user];

--Role
truncate table [role]
insert into role (name) values('项目创建人')
insert into role (name) values('项目管理员')
insert into role (name) values('项目成员')
select * from  [role];

--Authority
truncate table [authority]
insert into authority (id,name) values(101001001,'新建项目')
insert into authority (id,name) values(10100100,'编辑项目')
insert into authority (id,name) values(101001003,'删除项目')
insert into authority (id,name) values(101001004,'查看项目')
select * from [authority];

--RoleAuthority
truncate table [RoleAuthority]
insert into RoleAuthority (roleid,authorityId) values ((select id from role where name = '项目管理员'),(select id from authority where name = '新建项目'))
insert into RoleAuthority (roleid,authorityId) values ((select id from role where name = '项目管理员'),(select id from authority where name = '编辑项目'))
insert into RoleAuthority (roleid,authorityId) values ((select id from role where name = '项目管理员'),(select id from authority where name = '删除项目'))
insert into RoleAuthority (roleid,authorityId) values ((select id from role where name = '项目管理员'),(select id from authority where name = '查看项目'))
insert into RoleAuthority (roleid,authorityId) values ((select id from role where name = '项目成员'),(select id from authority where name = '查看项目'))
select * from [RoleAuthority];

--Project
truncate table [Project]
insert into Project(Name,ViewAuthorizeType,CreatorId,DepartmentId) values('测试项目01',1,(select id from [user] where name = 'admin'),1)
select * from [Project];

--FavoriteProject
truncate table [FavoriteProject]
insert into FavoriteProject(UserId,ProjectId) values((select id from [user] where name = 'admin'),(select id from [Project] where name = '测试项目01'))
select * from FavoriteProject;

--ProjectMember
truncate table [ProjectMember]
insert into ProjectMember (ProjectId,UserId,RoleId) Values((select id from [Project] where name = '测试项目01')
,(select id from [user] where name = 'test01'),(select id from [role] where name = '项目管理员'))
insert into ProjectMember (ProjectId,UserId,RoleId) Values((select id from [Project] where name = '测试项目01')
,(select id from [user] where name = 'test02'),(select id from [role] where name = '项目管理员'))
insert into ProjectMember (ProjectId,UserId,RoleId) Values((select id from [Project] where name = '测试项目01')
,(select id from [user] where name = 'test03'),(select id from [role] where name = '项目成员'))
insert into ProjectMember (ProjectId,UserId,RoleId) Values((select id from [Project] where name = '测试项目01')
,(select id from [user] where name = 'test04'),(select id from [role] where name = '项目成员'))
select * from ProjectMember


select * from  ProjectMember where ProjectId = @ProjectId and RoleId = (select id from [role] where name = @RoleName)


