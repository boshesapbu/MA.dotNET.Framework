**For copy database with data and schema**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_Database](
	+ Databasename
	+ *Example : "MyNewDatabase", "BusinessDb", ...*
	@databaseName NVARCHAR(200), 
	+ If data is null or empty then will work all tables, else data is not null and empty then only will work typed tables
	+ *Example : "", "dbo.[my_table1], public.[my_table2]", "dbo.[my_table3]", ...*
	@tablesP NVARCHAR(MAX) = '',
	+ If data is null or empty then will work all views, else data is not null and empty then only will work typed views
	+ *Example : "", "dbo.[my_view1], public.[my_view2]", "dbo.[my_view3]", ...*
	@viewsP NVARCHAR(MAX) = '')
	
**For copy database with data**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_DatabaseData](
	+ Databasename
	+ *Example : "MyNewDatabase", "BusinessDb", ...*
	@databaseName NVARCHAR(200), 
	+ If data is null or empty then will work all tables, else data is not null and empty then only will work typed tables
	+ *Example : "", "dbo.[my_table1], public.[my_table2]", "dbo.[my_table3]", ...*
	@tablesP NVARCHAR(MAX) = '')
	
**For copy database with schema**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_DatabaseSchema](
	+ Databasename
	+ *Example : "MyNewDatabase", "BusinessDb", ...*
	@databaseName NVARCHAR(200), 
	+ If data is null or empty then will work all tables, else data is not null and empty then only will work typed tables
	+ *Example : "", "dbo.[my_table1], public.[my_table2]", "dbo.[my_table3]", ...*
	@tablesP NVARCHAR(MAX) = '',
	+ If data is null or empty then will work all views, else data is not null and empty then only will work typed views
	+ *Example : "", "dbo.[my_view1], public.[my_view2]", "dbo.[my_view3]", ...*
	@viewsP NVARCHAR(MAX) = '')
	
**For add's trigger for each backup row when insert, update, delete event's**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_CreateLogTable](
	+ If data is null or empty then will work all tables, else data is not null and empty then only will work typed tables
	+ *Example : "", "dbo.[my_table1], public.[my_table2]", "dbo.[my_table3]", ...*
	@tablesP NVARCHAR(MAX) = '')
	
**For remove this libraries function and procedures**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Delete_LogMaFunctionsAndProcedures]

**For remove this libraries table triggers**
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Delete_LogMaTableTriggers]