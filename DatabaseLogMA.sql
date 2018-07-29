/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_Func_SplitString]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_Func_SplitString] ( @stringToSplit NVARCHAR(MAX), @splitChar NVARCHAR(1))
RETURNS
 @returnList TABLE ([NAME] [nvarchar] (200))
AS
BEGIN
	--MUHAMMED KANDEMİR

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(@splitChar, @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(@splitChar, @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT LTRIM(RTRIM(@name))

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT LTRIM(RTRIM(@stringToSplit))

 RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_DatabaseSchema]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_DatabaseSchema](
	@tablesP NVARCHAR(MAX) = '',
	@viewsP NVARCHAR(MAX) = ''
	)
RETURNS @funcTable TABLE (
  ROW_ORDER INT IDENTITY(0, 1),
  SCRIPT NVARCHAR(MAX)
) 
AS
BEGIN
	--MUHAMMED KANDEMİR

	IF (@tablesP IS NOT NULL AND @tablesP <> '')
	BEGIN
		DECLARE @tables TABLE(
			[NAME] NVARCHAR (200)
		)

		INSERT INTO @tables
			SELECT NAME FROM dbo.DatabaseLogMA_Func_SplitString(@tablesP, ',')

		INSERT INTO @funcTable
			SELECT 
				dbo.DatabaseLogMA_GetScript_CreateTable(DEFAULT, T.[SCHEMA] + '.' + T.NAME)
			FROM dbo.DatabaseLogMA_GetList_Table() T
			INNER JOIN @tables Tv ON Tv.NAME = T.[SCHEMA] + '.' + T.NAME
	END
	ELSE
	BEGIN
		INSERT INTO @funcTable
			SELECT 
				dbo.DatabaseLogMA_GetScript_CreateTable(DEFAULT, T.[SCHEMA] + '.' + T.NAME)
			FROM dbo.DatabaseLogMA_GetList_Table() T
	END
	
	IF (@viewsP IS NOT NULL AND @viewsP <> '')
	BEGIN
		DECLARE @views TABLE(
			[NAME] NVARCHAR (200)
		)

		INSERT INTO @views
			SELECT NAME FROM dbo.DatabaseLogMA_Func_SplitString(@viewsP, ',')
			
		INSERT INTO @funcTable
			SELECT 
				dbo.DatabaseLogMA_GetScript_CreateView(DEFAULT, V.[SCHEMA] + '.' + V.NAME)
			FROM dbo.DatabaseLogMA_GetList_View() V
		INNER JOIN @views Vv ON Vv.NAME = V.[SCHEMA] + '.' + V.NAME
	END
	ELSE
	BEGIN
		INSERT INTO @funcTable
		SELECT 
			dbo.DatabaseLogMA_GetScript_CreateView(DEFAULT, V.[SCHEMA] + '.' + V.NAME)
		FROM dbo.DatabaseLogMA_GetList_View() V
	END
	
	INSERT INTO @funcTable
		SELECT 
			dbo.DatabaseLogMA_GetScript_CreateTrigger(T.TABLE_SCHEMA + '.' + T.TABLE_NAME, T.NAME)
		FROM dbo.DatabaseLogMA_GetList_Trigger_All() T

	INSERT INTO @funcTable
		SELECT 
			dbo.DatabaseLogMA_GetScript_CreateProcedure(P.[SCHEMA] + '.' + P.NAME)
		FROM dbo.DatabaseLogMA_GetList_Procedure() P

	INSERT INTO @funcTable
		SELECT 
			dbo.DatabaseLogMA_GetScript_CreateFunction(F.[SCHEMA] + '.' + F.NAME)
		FROM dbo.DatabaseLogMA_GetList_Function() F

	UPDATE @funcTable
	SET SCRIPT = LTRIM(RTRIM(SCRIPT))

	RETURN
END

GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_CreateFunction]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_CreateFunction](
	@functionNameP NVARCHAR(200))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @SQL NVARCHAR(MAX) = (
		SELECT 
			M.[definition] AS SCRIPT
		FROM sys.sql_modules M
		INNER JOIN sys.objects FUNCTION_OBJ ON FUNCTION_OBJ.object_id=M.object_id 
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = FUNCTION_OBJ.schema_id 
		WHERE 
			(FUNCTION_OBJ.type ='FN' OR FUNCTION_OBJ.type ='IF' OR FUNCTION_OBJ.type ='TF') AND
			(
				SCHEMA_OBJ.name + '.' + FUNCTION_OBJ.name = @functionNameP OR
				SCHEMA_OBJ.name + '.[' + FUNCTION_OBJ.name + ']' = @functionNameP
			)
	)

	RETURN @SQL
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_CreateProcedure]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_CreateProcedure](
	@procedureNameP NVARCHAR(200))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @SQL NVARCHAR(MAX) = (
		SELECT 
			M.[definition] AS SCRIPT
		FROM sys.sql_modules M
		INNER JOIN sys.procedures PROCEDURE_OBJ ON PROCEDURE_OBJ.object_id=M.object_id 
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = PROCEDURE_OBJ.schema_id 
		WHERE 
			PROCEDURE_OBJ.type ='P' AND
			(
				SCHEMA_OBJ.name + '.' + PROCEDURE_OBJ.name = @procedureNameP OR
				SCHEMA_OBJ.name + '.[' + PROCEDURE_OBJ.name + ']' = @procedureNameP
			)
	)

	RETURN @SQL
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_CreateTable]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_CreateTable](
	@schemaNameP NVARCHAR(200) = '',
	@tableNameP NVARCHAR(200))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @table_name SYSNAME
	SELECT @table_name = @tableNameP

	DECLARE 
		  @object_name SYSNAME
		, @object_id INT
		, @resultName NVARCHAR(200)

	SELECT 
		  @resultName = '[' + 
		  (
			CASE 
			WHEN @schemaNameP IS NOT NULL AND @schemaNameP <> '' THEN @schemaNameP
			ELSE s.name
			END
		  )
		   + '].[' + o.name + ']',
		  @object_name = '[' + s.name + '].[' + o.name + ']'
		, @object_id = o.[object_id]
	FROM sys.objects o WITH (NOWAIT)
	JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
	WHERE s.name + '.' + o.name = @table_name
		AND o.[type] = 'U'
		AND o.is_ms_shipped = 0

	DECLARE @SQL NVARCHAR(MAX) = ''

	;WITH index_column AS 
	(
		SELECT 
			  ic.[object_id]
			, ic.index_id
			, ic.is_descending_key
			, ic.is_included_column
			, c.name
		FROM sys.index_columns ic WITH (NOWAIT)
		JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id
		WHERE ic.[object_id] = @object_id
	),
	fk_columns AS 
	(
		 SELECT 
			  k.constraint_object_id
			, cname = c.name
			, rcname = rc.name
		FROM sys.foreign_key_columns k WITH (NOWAIT)
		JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id 
		JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id
		WHERE k.parent_object_id = @object_id
	)
	SELECT @SQL = 'CREATE TABLE ' + @resultName + CHAR(13) + '(' + CHAR(13) + STUFF((
		SELECT CHAR(9) + ', [' + c.name + '] ' + 
			CASE WHEN c.is_computed = 1
				THEN 'AS ' + cc.[definition] 
				ELSE UPPER(tp.name) + 
					CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary', 'text')
						   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'
						 WHEN tp.name IN ('nvarchar', 'nchar', 'ntext')
						   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'
						 WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset') 
						   THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'
						 WHEN tp.name = 'decimal' 
						   THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'
						ELSE ''
					END +
					CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +
					CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +
					CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END + 
					CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END 
			END + CHAR(13)
		FROM sys.columns c WITH (NOWAIT)
		JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id
		LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id
		LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id
		LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
		WHERE c.[object_id] = @object_id
		ORDER BY c.column_id
		FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')
		+ ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + 
						(SELECT STUFF((
							 SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END
							 FROM sys.index_columns ic WITH (NOWAIT)
							 JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
							 WHERE ic.is_included_column = 0
								 AND ic.[object_id] = k.parent_object_id 
								 AND ic.index_id = k.unique_index_id     
							 FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))
				+ ')' + CHAR(13)
				FROM sys.key_constraints k WITH (NOWAIT)
				WHERE k.parent_object_id = @object_id 
					AND k.[type] = 'PK'), '') + ')'  + CHAR(13)
		+ ISNULL((SELECT (
			SELECT CHAR(13) +
				 'ALTER TABLE ' + @object_name + ' WITH' 
				+ CASE WHEN fk.is_not_trusted = 1 
					THEN ' NOCHECK' 
					ELSE ' CHECK' 
				  END + 
				  ' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY(' 
				  + STUFF((
					SELECT ', [' + k.cname + ']'
					FROM fk_columns k
					WHERE k.constraint_object_id = fk.[object_id]
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
				   + ')' +
				  ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('
				  + STUFF((
					SELECT ', [' + k.rcname + ']'
					FROM fk_columns k
					WHERE k.constraint_object_id = fk.[object_id]
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
				   + ')'
				+ CASE 
					WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE' 
					WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'
					WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT' 
					ELSE '' 
				  END
				+ CASE 
					WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'
					WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'
					WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT'  
					ELSE '' 
				  END 
				+ CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR(13)
			FROM sys.foreign_keys fk WITH (NOWAIT)
			JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id
			WHERE fk.parent_object_id = @object_id
			FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')
		+ ISNULL(((SELECT
			 CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END 
					+ ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +
					STUFF((
					SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END
					FROM index_column c
					WHERE c.is_included_column = 0
						AND c.index_id = i.index_id
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')'  
					+ ISNULL(CHAR(13) + 'INCLUDE (' + 
						STUFF((
						SELECT ', [' + c.name + ']'
						FROM index_column c
						WHERE c.is_included_column = 1
							AND c.index_id = i.index_id
						FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR(13)
			FROM sys.indexes i WITH (NOWAIT)
			WHERE i.[object_id] = @object_id
				AND i.is_primary_key = 0
				AND i.[type] = 2
			FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')
		), '')

	RETURN @SQL
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_CreateTrigger]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_CreateTrigger](
	@tableNameP NVARCHAR(200),
	@triggerNameP NVARCHAR(200))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @SQL NVARCHAR(MAX) = (
		SELECT 
			M.[definition] AS SCRIPT
		FROM sys.sql_modules M
		INNER JOIN sys.objects TRIGGER_OBJ ON TRIGGER_OBJ.object_id=M.object_id 
		INNER JOIN sys.objects TABLE_OBJ ON TABLE_OBJ.object_id=TRIGGER_OBJ.parent_object_id 
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = TABLE_OBJ.schema_id 
		WHERE 
			TRIGGER_OBJ.type ='TR' AND 
			TRIGGER_OBJ.name = @triggerNameP AND
			(
				SCHEMA_OBJ.name + '.' + TABLE_OBJ.name = @tableNameP OR
				SCHEMA_OBJ.name + '.[' + TABLE_OBJ.name + ']' = @tableNameP
			)
	)

	RETURN @SQL
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_CreateView]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_CreateView](
	@schemaNameP NVARCHAR(200) = '',
	@viewNameP NVARCHAR(200))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @viewScript NVARCHAR(MAX)
	DECLARE @viewSchema NVARCHAR(200)
	DECLARE @viewName NVARCHAR(200)
	
	SELECT 
		@viewScript = V.VIEW_DEFINITION,
		@viewName = V.TABLE_NAME,
		@viewSchema = (
			CASE 
			WHEN @schemaNameP IS NOT NULL AND @schemaNameP <> '' THEN @schemaNameP
			ELSE V.TABLE_SCHEMA
			END
		  )
	FROM INFORMATION_SCHEMA.VIEWS V
	WHERE 
		V.TABLE_SCHEMA + '.' + V.TABLE_NAME = @viewNameP OR
		V.TABLE_SCHEMA + '.[' + V.TABLE_NAME + ']' = @viewNameP

	DECLARE @dotIndex INT = CHARINDEX('.', @viewScript, 0)
	DECLARE @viewScriptAfterDot NVARCHAR(MAX) = SUBSTRING(@viewScript, @dotIndex + 1, LEN(@viewScript) - @dotIndex - 1)
	
	DECLARE @isFirstCharBrack/* "[" */ BIT = 0
	IF (CHARINDEX('[', @viewScriptAfterDot, 0) = 1)
		SET @isFirstCharBrack = 1
		
	DECLARE @viewScriptAfterAs NVARCHAR(MAX)
	 
	IF (@isFirstCharBrack = 1)
	BEGIN
		DECLARE @endBrackIndex/* "]" */ INT = CHARINDEX(']', @viewScriptAfterDot, 0)
		SET @viewScriptAfterAs = SUBSTRING(@viewScriptAfterDot, @endBrackIndex + 1, LEN(@viewScriptAfterDot) - @endBrackIndex - 1)	
	END
	ELSE
		SET @viewScriptAfterAs = @viewScriptAfterDot
	
	DECLARE @asIndex INT = CHARINDEX('AS', @viewScriptAfterAs, 0)
	DECLARE @SQL NVARCHAR(MAX) = SUBSTRING(@viewScriptAfterAs, @asIndex + 2, LEN(@viewScriptAfterAs) - @asIndex - 1)	
	
	RETURN 'CREATE VIEW [' + @viewSchema + '].[' + @viewName + '] AS ' + @SQL
END
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetScript_DatabaseSchema]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetScript_DatabaseSchema](
	@tablesP NVARCHAR(MAX) = '',
	@viewsP NVARCHAR(MAX) = ''
	)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @SQL NVARCHAR(MAX) = ''
	
	SELECT 
		@SQL = @SQL + '
		GO
		' + SCRIPT
	FROM dbo.DatabaseLogMA_GetList_DatabaseSchema(@tablesP, @viewsP)
	
	RETURN @SQL
END


GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_Function]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_Function]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			SCHEMA_OBJ.name AS [SCHEMA],
			FUNCTION_OBJ.name AS NAME
		FROM sys.objects FUNCTION_OBJ
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = FUNCTION_OBJ.schema_id 
		WHERE 
			FUNCTION_OBJ.type ='FN' OR FUNCTION_OBJ.type ='IF' OR FUNCTION_OBJ.type ='TF'

GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_Procedure]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_Procedure]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			SCHEMA_OBJ.name AS [SCHEMA],
			PROCEDURE_OBJ.name AS NAME
		FROM sys.procedures PROCEDURE_OBJ
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = PROCEDURE_OBJ.schema_id 
		WHERE 
			PROCEDURE_OBJ.type ='P'
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_Table]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_Table]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			T.TABLE_SCHEMA AS [SCHEMA],
			T.TABLE_NAME AS [NAME]
		FROM INFORMATION_SCHEMA.TABLES T
		WHERE T.TABLE_TYPE = 'BASE TABLE'
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_TableColumns]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_TableColumns](@tableNameP NVARCHAR(200))
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			C.ORDINAL_POSITION AS POSITION,
			C.COLUMN_NAME AS NAME,
			(
				CASE
				WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN C.DATA_TYPE
				ELSE C.DATA_TYPE + '(' + 
				(
					CASE
						WHEN C.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
						ELSE CONVERT(NVARCHAR(200), C.CHARACTER_MAXIMUM_LENGTH)
					END
				) + ')'
				END
			) AS [TYPE],
			C.IS_NULLABLE,
			(
				CASE
				WHEN KCU.COLUMN_NAME IS NULL THEN 0
				ELSE 1
				END
			) AS IS_KEY,
			columnproperty(T.object_id, C.COLUMN_NAME,'IsIdentity') AS IS_IDENTITY
		FROM INFORMATION_SCHEMA.COLUMNS C
		INNER JOIN sys.schemas S ON 1 = 1
		INNER JOIN sys.tables T ON T.name = C.TABLE_NAME AND S.schema_id = T.schema_id AND C.TABLE_SCHEMA = S.name
		LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU ON 
			KCU.TABLE_SCHEMA = C.TABLE_SCHEMA AND 
			KCU.TABLE_NAME = C.TABLE_NAME AND
			KCU.COLUMN_NAME = C.COLUMN_NAME
		WHERE 
			C.TABLE_SCHEMA + '.' + C.TABLE_NAME = @tableNameP OR
			C.TABLE_SCHEMA + '.[' + C.TABLE_NAME + ']' = @tableNameP


GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_TableColumns_All]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_TableColumns_All]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			C.ORDINAL_POSITION AS POSITION,
			C.COLUMN_NAME AS NAME,
			(
				CASE
				WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN C.DATA_TYPE
				ELSE C.DATA_TYPE + '(' + 
				(
					CASE
						WHEN C.CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
						ELSE CONVERT(NVARCHAR(200), C.CHARACTER_MAXIMUM_LENGTH)
					END
				) + ')'
				END
			) AS [TYPE],
			C.IS_NULLABLE,
			(
				CASE
				WHEN KCU.COLUMN_NAME IS NULL THEN 0
				ELSE 1
				END
			) AS IS_KEY,
			columnproperty(T.object_id, C.COLUMN_NAME,'IsIdentity') AS IS_IDENTITY,
			C.TABLE_SCHEMA AS TABLE_SCHEMA,
			C.TABLE_NAME AS TABLE_NAME
		FROM INFORMATION_SCHEMA.COLUMNS C
		INNER JOIN sys.schemas S ON 1 = 1
		INNER JOIN sys.tables T ON T.name = C.TABLE_NAME AND S.schema_id = T.schema_id AND C.TABLE_SCHEMA = S.name
		LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU ON 
			KCU.TABLE_SCHEMA = C.TABLE_SCHEMA AND 
			KCU.TABLE_NAME = C.TABLE_NAME AND
			KCU.COLUMN_NAME = C.COLUMN_NAME


GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_Trigger]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_Trigger](@tableNameP NVARCHAR(200))
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			TRIGGER_SCHEMA_OBJ.name AS [SCHEMA],
			TRIGGER_OBJ.name AS NAME
		FROM sys.objects TRIGGER_OBJ 
		INNER JOIN sys.objects TABLE_OBJ ON TABLE_OBJ.object_id=TRIGGER_OBJ.parent_object_id 
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = TABLE_OBJ.schema_id 
		INNER JOIN sys.schemas TRIGGER_SCHEMA_OBJ ON TRIGGER_SCHEMA_OBJ.schema_id = TRIGGER_OBJ.schema_id 
		WHERE 
			TRIGGER_OBJ.type ='TR' AND 
			(
				SCHEMA_OBJ.name + '.' + TABLE_OBJ.name = @tableNameP OR
				SCHEMA_OBJ.name + '.[' + TABLE_OBJ.name + ']' = @tableNameP
			)
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_Trigger_All]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_Trigger_All]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			TRIGGER_SCHEMA_OBJ.name AS [SCHEMA],
			TRIGGER_OBJ.name AS NAME,
			SCHEMA_OBJ.name AS TABLE_SCHEMA,
			TABLE_OBJ.name AS TABLE_NAME
		FROM sys.objects TRIGGER_OBJ 
		INNER JOIN sys.objects TABLE_OBJ ON TABLE_OBJ.object_id=TRIGGER_OBJ.parent_object_id 
		INNER JOIN sys.schemas SCHEMA_OBJ ON SCHEMA_OBJ.schema_id = TABLE_OBJ.schema_id 
		INNER JOIN sys.schemas TRIGGER_SCHEMA_OBJ ON TRIGGER_SCHEMA_OBJ.schema_id = TRIGGER_OBJ.schema_id 
		WHERE 
			TRIGGER_OBJ.type ='TR'
GO
/****** Object:  UserDefinedFunction [dbo].[DatabaseLogMA_GetList_View]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[DatabaseLogMA_GetList_View]()
RETURNS TABLE
AS
	--MUHAMMED KANDEMİR

RETURN	SELECT 
			V.TABLE_SCHEMA AS [SCHEMA],
			V.TABLE_NAME AS [NAME]
		FROM INFORMATION_SCHEMA.VIEWS V
GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Copy_Database]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_Database](
	@databaseName NVARCHAR(200), 
	@tablesP NVARCHAR(MAX) = '',
	@viewsP NVARCHAR(MAX) = '')
AS
BEGIN
	--MUHAMMED KANDEMİR

	EXECUTE dbo.[DatabaseLogMA_Exec_Copy_DatabaseSchema] @databaseName, @tablesP, @viewsP

	EXECUTE [DatabaseLogMA_Exec_Copy_DatabaseData] @databaseName, @tablesP
END


GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Copy_DatabaseData]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_DatabaseData](
	@databaseName NVARCHAR(200), 
	@tablesP NVARCHAR(MAX) = '')
AS
BEGIN
	--MUHAMMED KANDEMİR
	DECLARE @copyTableNames TABLE(
		ROW_ORDER INT IDENTITY(0, 1),
		NAME NVARCHAR(200)
	)
	
	IF (@tablesP IS NOT NULL AND @tablesP <> '')
	BEGIN
		DECLARE @tables TABLE(
			[NAME] NVARCHAR (200)
		)

		INSERT INTO @tables
			SELECT NAME FROM dbo.DatabaseLogMA_Func_SplitString(@tablesP, ',')

		INSERT INTO @copyTableNames
			SELECT 
				T.[SCHEMA] + '.[' + T.NAME + ']'
			FROM dbo.DatabaseLogMA_GetList_Table() T
			INNER JOIN @tables Tv ON Tv.NAME = T.[SCHEMA] + '.' + T.NAME
			ORDER BY T.[SCHEMA], T.NAME
	END
	ELSE
	BEGIN
		INSERT INTO @copyTableNames
			SELECT 
				T.[SCHEMA] + '.[' + T.NAME + ']'
			FROM dbo.DatabaseLogMA_GetList_Table() T
			ORDER BY T.[SCHEMA], T.NAME
	END
	
	DECLARE @tableCount INT = (
		SELECT COUNT(*)
		FROM @copyTableNames
	)


	DECLARE @tableIndex INT = 0
	WHILE @tableIndex < @tableCount
	BEGIN
		DECLARE @tableName NVARCHAR(200) = (
				SELECT 
					NAME
				FROM @copyTableNames
				WHERE ROW_ORDER = @tableIndex
		)
		DECLARE @tableColumns NVARCHAR(MAX) = ''

		SELECT 
			@tableColumns = (
				CASE
				WHEN @tableColumns = '' THEN '[' + TC.NAME + ']'
				ELSE @tableColumns + ',' + '[' + TC.NAME + ']'
				END
			)
		FROM dbo.DatabaseLogMA_GetList_TableColumns(@tableName) TC
		WHERE TC.IS_IDENTITY = 0

		DECLARE @tableInsertScript NVARCHAR(MAX) = 'INSERT INTO ' + @databaseName + '.' + @tableName + ' SELECT ' + @tableColumns + ' FROM ' + DB_NAME() + '.' + @tableName

		PRINT '
		RUN SCRIPT : 
		' + @tableInsertScript
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableInsertScript

		SET @tableIndex = @tableIndex + 1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Copy_DatabaseSchema]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Copy_DatabaseSchema](
	@databaseName NVARCHAR(200), 
	@tablesP NVARCHAR(MAX) = '',
	@viewsP NVARCHAR(MAX) = '')
AS
BEGIN
	--MUHAMMED KANDEMİR
	DECLARE @scripts TABLE(
		ROW_ORDER INT,
		SCRIPT NVARCHAR(MAX)
	)
	INSERT INTO @scripts
		SELECT 
			ROW_ORDER,
			SCRIPT
		FROM dbo.DatabaseLogMA_GetList_DatabaseSchema(@tablesP, @viewsP)
		
	DECLARE @sqlCount INT = (
		SELECT COUNT(*)
		FROM @scripts
	)

	DECLARE @sqlIndex INT = 0
	WHILE @sqlIndex < @sqlCount
	BEGIN
		DECLARE @sql NVARCHAR(MAX) = (
			SELECT SCRIPT
			FROM @scripts
			WHERE ROW_ORDER = @sqlIndex
		)

		PRINT '
		RUN SCRIPT : 
		' + @sql
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @sql

		SET @sqlIndex = @sqlIndex + 1
	END
END


GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_CreateLogTable]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_CreateLogTable](
	@tablesP NVARCHAR(MAX) = '')
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @copyTableNames TABLE(
		ROW_ORDER INT IDENTITY(0, 1),
		NAME NVARCHAR(200)
	)
	
	IF (@tablesP IS NOT NULL AND @tablesP <> '')
	BEGIN
		DECLARE @tables TABLE(
			[NAME] NVARCHAR (200)
		)

		INSERT INTO @tables
			SELECT NAME FROM dbo.DatabaseLogMA_Func_SplitString(@tablesP, ',')

		INSERT INTO @copyTableNames
			SELECT 
				T.[SCHEMA] + '.[' + T.NAME + ']'
			FROM dbo.DatabaseLogMA_GetList_Table() T
			INNER JOIN @tables Tv ON Tv.NAME = T.[SCHEMA] + '.' + T.NAME
			ORDER BY T.[SCHEMA], T.NAME
	END
	ELSE
	BEGIN
		INSERT INTO @copyTableNames
			SELECT 
				T.[SCHEMA] + '.[' + T.NAME + ']'
			FROM dbo.DatabaseLogMA_GetList_Table() T
			ORDER BY T.[SCHEMA], T.NAME
	END

	
	DECLARE @tableCount INT = (
		SELECT COUNT(*)
		FROM @copyTableNames
	)
	
	DECLARE @databaseName NVARCHAR(200) = DB_NAME()

	DECLARE @tableIndex INT = 0
	WHILE @tableIndex < @tableCount
	BEGIN
		DECLARE @tableName NVARCHAR(200) = (
				SELECT 
					NAME
				FROM @copyTableNames
				WHERE ROW_ORDER = @tableIndex
		)
		DECLARE @tableColumns NVARCHAR(MAX) = ''

		SELECT 
			@tableColumns = @tableColumns + '[' + TC.NAME + '] ' + TC.[TYPE] + ' NULL,'
		FROM dbo.DatabaseLogMA_GetList_TableColumns(@tableName) TC
		
		DECLARE @logTableName NVARCHAR(200) = (
				SELECT T.[SCHEMA] + '.[DatabaseLogMA_' + T.NAME + ']'
				FROM dbo.DatabaseLogMA_GetList_Table() T
				WHERE 
					T.[SCHEMA] + '.' + T.NAME = @tableName OR
					T.[SCHEMA] + '.[' + T.NAME + ']' = @tableName
			)

		SET @tableColumns = @tableColumns + 'LOG_MA_PROCESS_DATE_TIME DATETIME NULL, LOG_MA_PROCESS_TYPE VARCHAR(6)'

		DECLARE @tableCreateScript NVARCHAR(MAX) = 
			'CREATE TABLE ' + @logTableName + '(' + @tableColumns + ')'
			
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableCreateScript

		DECLARE @tableTransferScript NVARCHAR(MAX) =
			'INSERT INTO ' + @logTableName + '
				SELECT *, GETDATE() AS LOG_MA_PROCESS_DATE_TIME, ''FIRST'' AS LOG_MA_PROCESS_TYPE FROM ' + @tableName
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableTransferScript

		DECLARE @triggerName NVARCHAR(200) = (
				SELECT 'DatabaseLogMA_TRIGGER_' + T.NAME
				FROM dbo.DatabaseLogMA_GetList_Table() T
				WHERE 
					T.[SCHEMA] + '.' + T.NAME = @tableName OR
					T.[SCHEMA] + '.[' + T.NAME + ']' = @tableName
			)
			
		DECLARE @tableTriggerInsertScript NVARCHAR(MAX) = 
			'CREATE TRIGGER ' + @triggerName + '_INSERT
			ON ' + @tableName + '
			AFTER INSERT
			AS
			BEGIN
				INSERT INTO ' + @logTableName + '
					SELECT *, GETDATE() AS LOG_MA_PROCESS_DATE_TIME, ''INSERT'' AS LOG_MA_PROCESS_TYPE FROM inserted
			END'
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableTriggerInsertScript
		
		DECLARE @tableTriggerUpdateScript NVARCHAR(MAX) = 
			'CREATE TRIGGER ' + @triggerName + '_UPDATE
			ON ' + @tableName + '
			AFTER UPDATE
			AS
			BEGIN
				INSERT INTO ' + @logTableName + '
					SELECT *, GETDATE() AS LOG_MA_PROCESS_DATE_TIME, ''UPDATE'' AS LOG_MA_PROCESS_TYPE FROM inserted
			END'
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableTriggerUpdateScript
		
		DECLARE @tableTriggerDeleteScript NVARCHAR(MAX) = 
			'CREATE TRIGGER ' + @triggerName + '_DELETE
			ON ' + @tableName + '
			AFTER DELETE
			AS
			BEGIN
				INSERT INTO ' + @logTableName + '
					SELECT *, GETDATE() AS LOG_MA_PROCESS_DATE_TIME, ''DELETE'' AS LOG_MA_PROCESS_TYPE FROM deleted
			END'
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @tableTriggerDeleteScript

		SET @tableIndex = @tableIndex + 1
	END

	RETURN 
END

GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Delete_LogMaFunctionsAndProcedures]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Delete_LogMaFunctionsAndProcedures]
AS
BEGIN
	--MUHAMMED KANDEMİR

	DROP FUNCTION DatabaseLogMA_GetList_DatabaseSchema
	DROP FUNCTION DatabaseLogMA_GetList_Function
	DROP FUNCTION DatabaseLogMA_GetList_Procedure
	DROP FUNCTION DatabaseLogMA_GetList_Table
	DROP FUNCTION DatabaseLogMA_GetList_TableColumns
	DROP FUNCTION DatabaseLogMA_GetList_TableColumns_All
	DROP FUNCTION DatabaseLogMA_GetList_Trigger
	DROP FUNCTION DatabaseLogMA_GetList_Trigger_All
	DROP FUNCTION DatabaseLogMA_GetList_View
	
	DROP FUNCTION DatabaseLogMA_GetScript_CreateFunction
	DROP FUNCTION DatabaseLogMA_GetScript_CreateProcedure
	DROP FUNCTION DatabaseLogMA_GetScript_CreateTable
	DROP FUNCTION DatabaseLogMA_GetScript_CreateTrigger
	DROP FUNCTION DatabaseLogMA_GetScript_CreateView
	DROP FUNCTION DatabaseLogMA_GetScript_DatabaseSchema

	DROP PROCEDURE DatabaseLogMA_Exec_Copy_Database
	DROP PROCEDURE DatabaseLogMA_Exec_Copy_DatabaseData
	DROP PROCEDURE DatabaseLogMA_Exec_Copy_DatabaseSchema
	DROP PROCEDURE DatabaseLogMA_Exec_CreateLogTable
	DROP PROCEDURE DatabaseLogMA_Exec_Sql
	DROP PROCEDURE DatabaseLogMA_Exec_Delete_LogMaFunctionsAndProcedures
	DROP PROCEDURE DatabaseLogMA_Exec_Delete_LogMaTableTriggers

	DROP FUNCTION DatabaseLogMA_Func_SplitString
END


GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Delete_LogMaTableTriggers]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Delete_LogMaTableTriggers]
AS
BEGIN
	--MUHAMMED KANDEMİR
	
	DECLARE @triggers TABLE(
		ROW_ORDER INT IDENTITY(0, 1),
		NAME NVARCHAR(200)
	)

	INSERT INTO @triggers
		SELECT T.[SCHEMA] + '.' + T.NAME
		FROM dbo.DatabaseLogMA_GetList_Trigger_All() T
		WHERE NAME LIKE 'DatabaseLogMA_%'

	DECLARE @triggerCount INT = (
		SELECT COUNT(*)
		FROM @triggers
	)
	
	DECLARE @databaseName NVARCHAR(200) = DB_NAME()

	DECLARE @triggerIndex INT = 0
	WHILE @triggerIndex < @triggerCount
	BEGIN
		DECLARE @triggerName NVARCHAR(200) = (
			SELECT NAME
			FROM @triggers
			WHERE ROW_ORDER = @triggerIndex
		)
		DECLARE @dropTriggerScript NVARCHAR(MAX) = 
			'DROP TRIGGER ' + @triggerName

		PRINT '
		RUN SCRIPT : 
		' + @dropTriggerScript
		EXEC dbo.DatabaseLogMA_Exec_Sql @databaseName, @dropTriggerScript

		SET @triggerIndex = @triggerIndex + 1
	END
END


GO
/****** Object:  StoredProcedure [dbo].[DatabaseLogMA_Exec_Sql]    Script Date: 15/10/2017 14:13:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DatabaseLogMA_Exec_Sql](@databaseName SYSNAME, @sql NVARCHAR(MAX))
AS
BEGIN
	--MUHAMMED KANDEMİR

	DECLARE @execSql NVARCHAR(MAX) = N'EXEC ' + @databaseName + '..sp_executesql N'''
           + REPLACE(@sql, '''', '''''') + '''';

	EXECUTE sp_executesql @execSql
END