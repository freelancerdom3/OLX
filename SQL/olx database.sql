



create table Users
 (
 userId int identity primary key,
 firstName varchar(20),
 lastName varchar(20),
 userEmail varchar(50) not null,
 Password varchar(10) not null,
 MobileNo varchar(15) not null,
 Gender varchar(10),
 Address varchar(50),
 City varchar(50),
 Role varchar(20) default 'user',
 createdOn datetime default getdate(),
 updatedOn datetime default getdate()
 )

 create PROCEDURE [dbo].[deleteRecord]
@userId int


AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
delete from Users where userId=@userId
END

create procedure [dbo].[spDeleteSubcategory]
(
@userId int
)
as
begin
Delete from Users where userId=@userId
End
create PROCEDURE [dbo].[GetUser]
AS
BEGIN
Select * from Users order by userId
END

create PROCEDURE [dbo].[updateRecord]
@userId int,
@firstName varchar(20),
@lastName varchar(20),
@userEmail varchar(50),
@Password varchar(10),
@MobileNo varchar(15),
@Gender varchar(10),
@Address varchar(50),
@City varchar(50)


AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

update Users set firstName=@firstName,lastName=@lastName,userEmail=@userEmail, Password=@Password,
MobileNo=@MobileNo,Gender=@Gender,Address=@Address, City=@City where userId=@userId
END



--------Insert Store procedure--------

create table tbl_ProductCategory(productCategoryId int identity(1,1),productCategoryName varchar(30),createdOn dateTime default getdate(),updatedOn dateTime default getdate())

create table tbl_ProductSubCategory(productSubCategoryId int identity(1,1),productCategoryId int,productSubCategoryName varchar(50),createdOn dateTime default getdate(),updatedOn dateTime default getdate())

create table tbl_MyAdvertise(advertiseId int identity(1,1),productSubCategoryId int,advertiseTitle varchar(70)NOT NULL,advertiseDescription varchar(255)NOT NULL,
advertisePrice decimal(10,2) NOT NULL,areaId int,advertiseStatus bit default 0,userId int,advertiseapproved bit default 0 ,createdOn dateTime default getdate(),updatedOn dateTime default getdate())

create table tbl_AdvertiseImages (advertiseImageId int identity(1,1), advertiseId int, imageData varbinary(max) NOT NULL,createdOn dateTime default getdate(),updatedOn dateTime default getdate());

create table tbl_State(stateId int identity(1,1),stateName varchar(30))

create table tbl_City(cityId int identity(1,1),stateId int,cityName varchar(30))

create table tbl_Area(areaId int identity(1,1),cityId int,areaName varchar(30))
---------sp---------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Procedure DeleteMyAdv 
(  
   @advertiseId int
)
as  
begin  
   delete from tbl_MyAdvertise where  advertiseId=@advertiseId
End; 
Exec DeleteMyAdv 1004
---------------------------------------------------------------------------------
create procedure ps_InsertAdvertiseImage
    @advertiseId INT,
    @imageData VARBINARY(MAX)
AS
BEGIN
    INSERT INTO tbl_AdvertiseImages (advertiseId, imageData)
    VALUES (@advertiseId, @imageData);
     
END;


------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE ps_GetImages

as
begin
    select (advertiseId as advertiseId, imageData as imageData, createdOn AS createdOn, updatedOn AS updatedOn)

    from
        tbl_AdvertiseImages
end;


----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE ps_AddNewProductCategory 
(
    @productCategoryName varchar(30)
)
AS
BEGIN
    INSERT INTO tbl_ProductCategory  VALUES (@productCategoryName,getdate(),getdate())
END

-----------------------------------------------------------------------------------
Exec ps_AddNewProductCategory 'fashion';
------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE AddNewProductSubCategory 
(
   @productCategoryId int,
   @ProductSubCategoryName varchar(50)
)
AS
BEGIN
    INSERT INTO tbl_ProductSubCategory VALUES (@productCategoryId,@ProductSubCategoryName,getdate(),getdate())
END
-----------------------------------------------------------------------------
exec AddNewProductSubCategory 2,'kid';

----------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE AddNewAdvertise 
(

	
   @productSubCategoryId int,
   @advertiseTitle varchar(70),
   @advertiseDescription varchar(255),
   @advertisePrice decimal(10,2),
   @areaId int,
   @userId int
 )
AS
BEGIN
    INSERT INTO tbl_MyAdvertise (productSubCategoryId,advertiseTitle,advertiseDescription,advertisePrice,areaId,userId,createdOn,updatedOn) VALUES 
							   (@productSubCategoryId,@advertiseTitle,@advertiseDescription,@advertisePrice,@areaId,@userId,getdate(),getdate())
END
-----------------------------------------------------------------------
--exec AddNewAdvertise 2,'Xiaomi 11T ','120Hz True 10-bit AMOLED Display 120Hz True 10-bit AMOLED Display120W Xiaomi HyperCharge',12000,1,1
--------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure GetAdvertiseDetails
AS
BEGIN
    SELECT a.advertiseId, psc.productSubCategoryName AS productSubCategoryName,a.advertiseTitle,a.advertiseDescription,a.advertisePrice,area.areaName,u.userName,a.createdOn,a.updatedOn,i.imageData
    FROM
        tbl_MyAdvertise a
		left join
        tbl_AdvertiseImages i on a.advertiseId = i.advertiseId
		 left join
        tbl_Area area on a.areaId = area.areaId
		left join 
		tbl_ProductSubCategory psc on a.productSubCategoryId=psc.productSubCategoryId
		left join 
		Users u on u.userId=a.userId;
END;

create procedure spInserttbl_MyAdvertise
(
@advertiseId int ,
@productSubCategoryId int,
@advertiseTitle varchar(70) ,
@advertiseDescription varchar(MAX),
@advertisePrice decimal(10,2) ,
@areaId int,
@userId int
)
as
begin
Insert into tbl_MyAdvertise values(@productSubCategoryId,@advertiseTitle,
@advertiseDescription,
@advertisePrice,@areaId,Default,@userId,Default,getdate(),getdate())
End

create procedure spUpdatetbl_MyAdvertise
(
@advertiseId int ,
@productSubCategoryId int,
@advertiseTitle varchar(70) ,
@advertiseDescription varchar(MAX),
@advertisePrice decimal(10,2) ,
@areaId int,
@addstatus bit,
@userId int,
@advapproved bit
)
as
begin
update tbl_MyAdvertise SET
productSubCategoryId=@productSubCategoryId,
advertiseTitle=@advertiseTitle,
advertiseDescription=@advertiseDescription,
advertisePrice=@advertisePrice,
areaId=@areaId,advertiseStatus=@addstatus,userId=@userId,advertiseapproved=@advapproved where advertiseId=@advertiseId
End

create procedure spDeletetbl_MyAdvertise
(
@advertiseId int
)

as
begin
Delete from tbl_MyAdvertise where advertiseId=@advertiseId
End

create procedure spViewtbl_MyAdvertise
as
begin
Select * from tbl_MyAdvertise order by advertiseId
End

create procedure spGetAllProductSubCategory
as
Begin
select * from tbl_ProductSubCategory
order by productSubCategoryId
End
---------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE AddNewProductDetails
(


@productCategoryId int,
@productSubCategoryName varchar(70)

)
AS
BEGIN
INSERT INTO tbl_ProductSubCategory VALUES (@productCategoryId,@productSubCategoryName,getdate(),getdate())
END
------------------------------------------------------------------------------

create PROCEDURE spUpdate
@productSubCategoryId int,
@productCategoryId int,
@productSubCategoryName varchar(200)


AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

update tbl_ProductSubCategory set productCategoryId=@productCategoryId,
productSubCategoryName=@productSubCategoryName where productSubCategoryId=@productSubCategoryId
END

------------------------------------------------------------------------------------------------
create procedure SpDeleteProductDetails
(
@productSubCategoryId int
)
as
begin
Delete from tbl_ProductSubCategory where productSubCategoryId=@productSubCategoryId
End



--exec GetAdvertiseDetails

--exec deleteRecord @userId=1
