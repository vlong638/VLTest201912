use VLTest

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

--BusinessType
truncate table [BusinessType]
insert into BusinessType (Name) Values('产科')
insert into BusinessType (Name) Values('妇科')
insert into BusinessType (Name) Values('儿科')
select * from BusinessType

--BusinessEntity
truncate table [BusinessEntity]
insert into BusinessEntity (BusinessTypeId,Name) Values((select id from [BusinessType] where name = '产科'),'孕妇基本信息')
insert into BusinessEntity (BusinessTypeId,Name) Values((select id from [BusinessType] where name = '产科'),'检查单')
insert into BusinessEntity (BusinessTypeId,Name) Values((select id from [BusinessType] where name = '产科'),'检验项')
select * from BusinessEntity

--BusinessEntityProperty
truncate table [BusinessEntityProperty]
insert into BusinessEntityProperty (BusinessEntityId,TableName,ColumnName,DisplayName) Values((select id from BusinessEntity where Name = '孕妇基本信息'),'PregnantInfo','PersonName','姓名')
insert into BusinessEntityProperty (BusinessEntityId,TableName,ColumnName,DisplayName) Values((select id from BusinessEntity where Name = '孕妇基本信息'),'PregnantInfo','Birthday','生日')
insert into BusinessEntityProperty (BusinessEntityId,TableName,ColumnName,DisplayName) Values((select id from BusinessEntity where Name = '孕妇基本信息'),'PregnantInfo','Sex','性别')
select * from BusinessEntityProperty


			
