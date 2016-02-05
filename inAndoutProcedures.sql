CREATE DEFINER=`grupogcp`@`icaro.servidoraweb.net` PROCEDURE `Count_Hours`(IN dateFrom DATETIME, IN dateTo DATETIME, IN userId int)
BEGIN

IF userId > 0 THEN SET @SQLCOMM = 'SELECT  
	a1.userId, 
	a2.Fecha, 
	str_to_date(a1.hora, ''%H:%i'') as Ingreso, 
	str_to_date(a2.hora, ''%H:%i'') as Egreso, 
	Extract(hour from TIMEDIFF(a2.hora,a1.hora)) as horas,
	Extract(minute from TIMEDIFF(a2.hora,a1.hora)) as minutos
  FROM Activity a1
	  inner join Activity a2
		on a1.id = a2.id - 1 and a1.actionId = 1
	  inner join Users users
		on a1.userId = users.id and a1.userId = a2.userId
  where 	
	a1.fecha >= ? and a1.fecha <= ? and Activity.userId = ?
	order by a2.id';

	PREPARE stmt1 FROM @SQLCOMM;
	set @a = date_format(dateFrom, '%d/%m/%Y');
	set @b = date_format(dateTo, '%d/%m/%Y');
	set @c = userId;
	EXECUTE stmt1 USING @a, @b, @c;
	DEALLOCATE PREPARE stmt1;
	
	ELSE SET @SQLCOMM = 'SELECT  
	a1.userId, 
	a2.Fecha, 
	str_to_date(a1.hora, ''%H:%i'') as Ingreso, 
	str_to_date(a2.hora, ''%H:%i'') as Egreso, 
	Extract(hour from TIMEDIFF(a2.hora,a1.hora)) as horas,
	Extract(minute from TIMEDIFF(a2.hora,a1.hora)) as minutos
  FROM Activity a1
	  inner join Activity a2
		on a1.id = a2.id - 1 and a1.actionId = 1
	  inner join Users users
		on a1.userId = users.id and a1.userId = a2.userId
  where 	
	a1.fecha >= ? and a1.fecha <= ?
	order by a2.userId, a2.id';

	PREPARE stmt1 FROM @SQLCOMM;
	set @a = date_format(dateFrom, '%d/%m/%Y');
	set @b = date_format(dateTo, '%d/%m/%Y');
	EXECUTE stmt1 USING @a, @b;
	DEALLOCATE PREPARE stmt1;
	
	END IF;

  END



CREATE DEFINER=`grupogcp`@`icaro.servidoraweb.net` PROCEDURE `Raw_Report`(IN dateFrom dateTime, IN dateTo dateTime, IN userId int)
BEGIN
	IF userId > 0 THEN SET @SQLCOMM = '
  SELECT users.UserName Usuario, fecha, hora, Actions.actionName Evento	
  FROM Activity	 
	inner join Users users
		on userId = users.id 
	inner join Actions
		on Activity.actionId = Actions.id
  where 	
	fecha >= date_format(?, ''%d/%m/%Y'') and fecha <= date_format(?, ''%d/%m/%Y'') and Activity.userId = ?
  Order by Activity.id';
	
	PREPARE stmt1 FROM @SQLCOMM;
	set @a = dateFrom;
	set @b = dateTo;
	set @c = userId;
	EXECUTE stmt1 USING @a, @b, @c;
	DEALLOCATE PREPARE stmt1;
	
	ELSE SET @SQLCOMM = '
  SELECT users.UserName Usuario, fecha, hora, Actions.actionName Evento	
  FROM Activity	 
	inner join Users users
		on userId = users.id 
	inner join Actions
		on Activity.actionId = Actions.id
  where 	
	fecha >= date_format(?, ''%d/%m/%Y'') and fecha <= date_format(?, ''%d/%m/%Y'')
Order by Activity.userId, Activity.id';
	
	PREPARE stmt1 FROM @SQLCOMM;
	set @a = dateFrom;
	set @b = dateTo;
	EXECUTE stmt1 USING @a, @b;
	DEALLOCATE PREPARE stmt1;
	
	END IF;

	
  END