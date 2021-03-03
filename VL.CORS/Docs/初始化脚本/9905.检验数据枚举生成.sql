--Labs
select concat('
{"parentkey": "',lo.examname,'","parentvalue": "',lo.examcode,'","key": "',lr.itemname,'","Value": "',lr.itemid,'"},
'),lo.examname,lo.examcode,lr.itemname,lr.itemid
from LabOrder lo 
left join LabResult lr on lo.OrderId = lr.OrderId
where lo.examcode not like '%+%'
group by lo.examname,lo.examcode,lr.itemname,lr.itemid