alter table clients add hid hierarchyid;

delete from clients where passport_number = 'ћ–100001';

insert into clients values ('‘ирма1', '‘илиал1', 'ћ–100001', hierarchyid::GetRoot()); -- корень

declare @Id hierarchyid  
select @Id = MAX(hid) from clients where hid.GetAncestor(1) = hierarchyid::GetRoot(); -- предок корень
update clients set hid = hierarchyid::GetRoot().GetDescendant(@id, null) where passport_number = 'MP000001'

declare @Id hierarchyid  
select @Id = MAX(hid) from clients where hid.GetAncestor(1) = hierarchyid::GetRoot(); -- предок корень
update clients set hid = hierarchyid::GetRoot().GetDescendant(@id, null) where passport_number = 'MP000002'

declare @Id hierarchyid  
select @Id = MAX(hid) from clients where hid.GetAncestor(1) = (select hid from clients where passport_number = 'MP000001'); -- предок MP000001
update clients set hid = (select hid from clients where passport_number = 'MP000001').GetDescendant(@Id, null) where passport_number = 'MP000003'

declare @Id hierarchyid  
select @Id = MAX(hid) from clients where hid.GetAncestor(1) = (select hid from clients where passport_number = 'MP000002'); -- предок MP000002
update clients set hid = (select hid from clients where passport_number = 'MP000002').GetDescendant(@Id, null) where passport_number = 'MP000004'

declare @Id hierarchyid  
select @Id = MAX(hid) from clients where hid.GetAncestor(1) = (select hid from clients where passport_number = 'MP000002'); -- предок MP000002
update clients set hid = (select hid from clients where passport_number = 'MP000002').GetDescendant(@Id, null) where passport_number = 'MP000005'

select * from clients;
select hid.ToString() as hid, hid.GetLevel() as level, * from clients

-- drop procedure GetChildNodes;

create procedure GetChildNodes @passport_number nvarchar(50)
as
begin
	declare @parentNodeHid hierarchyid
	select  @parentNodeHid = (select hid from clients where passport_number = @passport_number);
	select hid.ToString() as hid, hid.GetLevel() as level, * from clients where
	hid.GetAncestor(1) = @parentNodeHid;
end;

exec GetChildNodes 'MP000001';

create procedure AddChildNode(@parent_passport_number nvarchar(50), @passport_number nvarchar(50),
@surname nvarchar(50), @name nvarchar(50))   
AS   
BEGIN  
   DECLARE @parenthid hierarchyid, @hid hierarchyid  
   SELECT @parenthid = hid   
   FROM clients 
   WHERE passport_number = @parent_passport_number  
      SELECT @hid = max(hid)   
      FROM clients 
      WHERE hid.GetAncestor(1) = @parenthid ;  
      INSERT clients VALUES(@surname, @name, @passport_number, @parenthid.GetDescendant(@hid, NULL))  
END;

exec AddChildNode 'MP000001', 'MP000013', '‘амили€13', '»м€13';

exec GetChildNodes 'MP000001';

CREATE PROCEDURE MoveSubtree(@old_parent_passport_number nvarchar(50), @new_parent_passport_number nvarchar(50))
as
BEGIN
DECLARE @nold hierarchyid, @nnew hierarchyid  
SELECT @nold = hid FROM clients WHERE passport_number = @old_parent_passport_number ;  
SELECT @nnew = hid FROM clients WHERE passport_number = @new_parent_passport_number ;  
SELECT @nnew = @nnew.GetDescendant(max(hid), NULL) FROM clients WHERE hid.GetAncestor(1)=@nnew;  
UPDATE clients SET hid = hid.GetReparentedValue(@nold, @nnew) WHERE hid.IsDescendantOf(@nold) = 1;  
END; 

exec MoveSubtree 'MP000002', 'MP000001'

select hid.ToString() as hid, hid.GetLevel() as level, * from clients

--GetAncestor Ч выдает hierarchyid предка, можно указать уровень предка, например 1 выберет непосредственного предка;
--GetDescendant Ч выдает hierarchyid потомка, принимает два параметра, с помощью которых можно управл€ть тем, 
--какого именно потомка необходимо получить на выходе;
--GetLevel Ч выдает уровень hierarchyid;
--GetRoot Ч выдает уровень корн€;
--IsDescendant Ч провер€ет €вл€етс€ ли hierarchyid переданный через параметр потомком;
--Parse Ч конвертирует строковое представление hierarchyid в собственно hierarchyid;
--Reparent Ч позвол€ет изменить текущего предка;
--ToString Ч конвертирует hierarchyid в строковое представление.