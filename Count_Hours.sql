DELIMITER //
CREATE PROCEDURE Count_Hours_2 (IN dateFrom VARCHAR(10), IN dateTo VARCHAR(10), IN userId int)
BEGIN

SET @SQLCOMM = '(Select user1.userId
, Users.username
, user1.fecha as FechaIngreso
, str_to_date(user1.hora, ''%H:%i'') as Ingreso
, user2.fecha as FechaEgreso
, str_to_date(user2.hora, ''%H:%i'') as Egreso
, Extract(hour from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as horas
, Extract(minute from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as minutos
, user1.estacion as EstacionIngreso
, user2.estacion as EstacionEgreso
from
(Select (@cnt1 := @cnt1 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt1 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) >= ? AND userId = 3
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user1
inner join
(Select (@cnt2 := @cnt2 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt2 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) <= ? AND userId = 3
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user2
on user2.rowId = user1.rowId + 1 and user1.actionId = 1
inner join Users on user1.userId = Users.id)
UNION
(Select user1.userId
, Users.username
, user1.fecha as FechaIngreso
, str_to_date(user1.hora, ''%H:%i'') as Ingreso
, user2.fecha as FechaEgreso
, str_to_date(user2.hora, ''%H:%i'') as Egreso
, Extract(hour from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as horas
, Extract(minute from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as minutos
, user1.estacion as EstacionIngreso
, user2.estacion as EstacionEgreso
from
(Select (@cnt3 := @cnt3 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt3 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) >= ? AND userId = 2
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user1
inner join
(Select (@cnt4 := @cnt4 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt4 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) <= ? AND userId = 2
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user2
on user2.rowId = user1.rowId + 1 and user1.actionId = 1
inner join Users on user1.userId = Users.id)
UNION
(Select user1.userId
, Users.username
, user1.fecha as FechaIngreso
, str_to_date(user1.hora, ''%H:%i'') as Ingreso
, user2.fecha as FechaEgreso
, str_to_date(user2.hora, ''%H:%i'') as Egreso
, Extract(hour from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as horas
, Extract(minute from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as minutos
, user1.estacion as EstacionIngreso
, user2.estacion as EstacionEgreso
from
(Select (@cnt5 := @cnt5 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt5 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) >= ? AND userId = 1
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user1
inner join
(Select (@cnt6 := @cnt6 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt6 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) <= ? AND userId = 1
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user2
on user2.rowId = user1.rowId + 1 and user1.actionId = 1
inner join Users on user1.userId = Users.id)
UNION
(Select user1.userId
, Users.username
, user1.fecha as FechaIngreso
, str_to_date(user1.hora, ''%H:%i'') as Ingreso
, user2.fecha as FechaEgreso
, str_to_date(user2.hora, ''%H:%i'') as Egreso
, Extract(hour from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as horas
, Extract(minute from TIMEDIFF(STR_TO_DATE(concat(user2.fecha, '' '',user2.hora), ''%d/%m/%Y %H:%i''),STR_TO_DATE(concat(user1.fecha, '' '',user1.hora),''%d/%m/%Y %H:%i''))) as minutos
, user1.estacion as EstacionIngreso
, user2.estacion as EstacionEgreso
from
(Select (@cnt7 := @cnt7 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt7 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) >= ? AND userId = 4
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user1
inner join
(Select (@cnt8 := @cnt8 + 1) AS rowId, t.* from Activity t
CROSS JOIN (SELECT @cnt8 := 0) AS dummy
where STR_TO_DATE( fecha, ''%d/%m/%Y'' ) <= ? AND userId = 4
order by STR_TO_DATE( fecha, ''%d/%m/%Y'' ), userid, str_to_date(hora, ''%H:%i'')) as user2
on user2.rowId = user1.rowId + 1 and user1.actionId = 1
inner join Users on user1.userId = Users.id)
';

	PREPARE stmt1 FROM @SQLCOMM;
	set @a = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @b = STR_TO_DATE(dateTo, '%d/%m/%Y');
	set @c = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @d = STR_TO_DATE(dateTo, '%d/%m/%Y');
	set @e = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @f = STR_TO_DATE(dateTo, '%d/%m/%Y');
	set @h = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @i = STR_TO_DATE(dateTo, '%d/%m/%Y');
	EXECUTE stmt1 USING @a, @b, @c, @d, @e, @f, @h, @i;
	DEALLOCATE PREPARE stmt1;
	

  END//
DELIMITER ;
