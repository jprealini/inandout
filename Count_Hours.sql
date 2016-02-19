DELIMITER //
CREATE PROCEDURE Count_Hours_2 (IN dateFrom VARCHAR(10), IN dateTo VARCHAR(10), IN userId int)
BEGIN

IF userId > 0 THEN SET @SQLCOMM = 'SELECT  
	a1.userId,
    users.username,	
	a2.Fecha, 
	str_to_date(a1.hora, ''%H:%i'') as Ingreso, 
	str_to_date(a2.hora, ''%H:%i'') as Egreso, 
	Extract(hour from TIMEDIFF(a2.hora,a1.hora)) as horas,
	Extract(minute from TIMEDIFF(a2.hora,a1.hora)) as minutos,
	a2.estacion
  FROM Activity a1
	  inner join Activity a2
		on a1.id = a2.id - 1 and a1.actionId = 1
	  inner join Users users
		on a1.userId = users.id and a1.userId = a2.userId
  where 	
	STR_TO_DATE( a1.fecha, ''%d/%m/%Y'' ) >= ? and STR_TO_DATE( a1.fecha, ''%d/%m/%Y'' ) <= ? 
	and a2.userId = ?
	order by a2.id';

	PREPARE stmt1 FROM @SQLCOMM;
	set @a = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @b = STR_TO_DATE(dateTo, '%d/%m/%Y');
	set @c = userId;
	EXECUTE stmt1 USING @a, @b, @c;
	DEALLOCATE PREPARE stmt1;
	
	ELSE SET @SQLCOMM = 'SELECT  
	a1.userId,
users.username,	
	a2.Fecha, 
	str_to_date(a1.hora, ''%H:%i'') as Ingreso, 
	str_to_date(a2.hora, ''%H:%i'') as Egreso, 
	Extract(hour from TIMEDIFF(a2.hora,a1.hora)) as horas,
	Extract(minute from TIMEDIFF(a2.hora,a1.hora)) as minutos,
	a2.estacion
  FROM Activity a1
	  inner join Activity a2
		on a1.id = a2.id - 1 and a1.actionId = 1
	  inner join Users users
		on a1.userId = users.id and a1.userId = a2.userId
  where 	
	STR_TO_DATE( a1.fecha,  ''%d/%m/%Y'' ) >= ? and STR_TO_DATE( a1.fecha,  ''%d/%m/%Y'' ) <= ? 
	order by a2.userId, a2.id';

	PREPARE stmt1 FROM @SQLCOMM;
	set @a = STR_TO_DATE(dateFrom, '%d/%m/%Y');
	set @b = STR_TO_DATE(dateTo, '%d/%m/%Y');
	EXECUTE stmt1 USING @a, @b;
	DEALLOCATE PREPARE stmt1;
	
	END IF;

  END//
DELIMITER ;
