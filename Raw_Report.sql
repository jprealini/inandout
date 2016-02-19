DELIMITER //
CREATE PROCEDURE Raw_Report_2 (IN dateFrom dateTime, IN dateTo dateTime, IN userId int)
BEGIN
	IF userId > 0 THEN SET @SQLCOMM = '
  SELECT users.UserName Usuario, fecha, hora, Actions.actionName Evento, station
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
  SELECT users.UserName Usuario, fecha, hora, Actions.actionName Evento, station
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

	
  END//
  DELIMITER ;