--User
truncate table [user]
insert into [user] (name,password) values ('admin','e10adc3949ba59abbe56e057f20f883e')
insert into [user] (name,password) values ('test01','e10adc3949ba59abbe56e057f20f883e')
insert into [user] (name,password) values ('test02','e10adc3949ba59abbe56e057f20f883e')
insert into [user] (name,password) values ('test03','e10adc3949ba59abbe56e057f20f883e')
insert into [user] (name,password) values ('test04','e10adc3949ba59abbe56e057f20f883e')
select * from [user];

--Role
truncate table [role]
insert into role (name,category) values('项目创建人',1)
insert into role (name,category) values('项目管理员',1)
insert into role (name,category) values('项目成员',1)
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

--
truncate table UserRole
truncate table RoleAuthority

INSERT INTO [Role]([db_createtime], [db_updatetime], [Name], [Category]) VALUES ('2021-01-06 14:49:04.860', NULL, N'超级管理员', 2);
declare @UserId int
set @UserId = (select Id from [User] where Name = 'admin')
declare @RoleId int
set @RoleId = (select Id from Role where Name = '超级管理员')
INSERT INTO UserRole(UserId,[RoleId]) VALUES(@UserId,@RoleId);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 101001001);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999001001);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999001002);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999001003);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999001004);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999002001);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999002002);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999002003);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999002004);
INSERT INTO RoleAuthority([RoleId], [AuthorityId]) VALUES(@RoleId, 999002005);
			
