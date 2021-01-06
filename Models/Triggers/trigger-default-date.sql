Use `ems`
delimiter $$;
CREATE TRIGGER `default_CreateDate` 
BEFORE INSERT ON `notice` 
FOR EACH ROW 
	if ( isnull(new.CreatedDate) ) 
		then  set new.CreatedDate=curdate(); 
	end if;
$$
delimiter ;
