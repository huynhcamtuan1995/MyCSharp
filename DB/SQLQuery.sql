USE [WMSDB]
GO
/****** Object:  StoredProcedure [dbo].[my_PageResultProc]    Script Date: 24/12/2020 12:58:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[my_PageResultProc] 
(
    @PageNum int = 1,
    @PageSize int = 25,
	@TotalRow int OUTPUT
)
AS
DECLARE @first int, @startRow int = (@PageNum - 1) * @PageSize + 1;
SET @startRow = IIF(@startRow > 0, @startRow, 0);

--SET ROWCOUNT @startRow
--SELECT @first = p.ID, @TotalRow = (COUNT(*) OVER())
--FROM Products p 
--ORDER BY p.ID
--SET ROWCOUNT @PageSize

--SELECT  t.* 
--FROM Products t
--where t.ID >= @first_id
--ORDER BY t.ID

--RETURN @@ROWCOUNT
--SET ROWCOUNT 0

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #Temp
CREATE TABLE #Temp 
(
	ID int,
	Name nvarchar(255),
	Quantity int,
	CateName nvarchar(255),
)
SET ROWCOUNT @startRow
SELECT @first= p.Quantity, @TotalRow = (COUNT(*) OVER())
FROM Products p 
inner join Categories c on c.ID = p.CategoryID
ORDER BY p.Quantity
SET ROWCOUNT @PageSize

INSERT INTO #Temp(ID, Name, Quantity, CateName)
SELECT p.ID, p.Name, p.Quantity, p.Name
FROM Products p
inner join Categories c on c.ID = p.CategoryID
where p.Quantity >= @first
ORDER BY p.Quantity

SELECT t.*
FROM #Temp t
ORDER BY t.ID

RETURN @@ROWCOUNT

SET ROWCOUNT 0
DROP TABLE #Temp
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

--DECLARE @pageNo int -- 1 based
--DECLARE @pageSize int
--SET @pageNo = 51
--SET @pageSize = 20

--DECLARE @firstRecord int
--DECLARE @lastRecord int
--SET @firstRecord = (@pageNo - 1) * @pageSize + 1 -- 1001
--SET @lastRecord = @firstRecord + @pageSize - 1   -- 1020

--DECLARE @orderedKeys TABLE (
--  rownum int IDENTITY NOT NULL PRIMARY KEY CLUSTERED,
--  TableKey int NOT NULL
--)

--SET ROWCOUNT @lastRecord
--INSERT INTO @orderedKeys (TableKey) SELECT ID FROM Orders WHERE OrderDate >= '1980-01-01' ORDER BY OrderDate

--SET ROWCOUNT 0

--SELECT t.*
--FROM Orders t
--  INNER JOIN @orderedKeys o ON o.TableKey = t.ID
--WHERE o.rownum >= @firstRecord
--ORDER BY o.rownum

--////////////////////////


--@CompanyID  int,
--@pageNumber     int,
--@pageSize   int, 
--@sort       varchar(200)

--DECLARE @sql nvarchar(4000)
--DECLARE @strPageSize nvarchar(20)
--DECLARE @strSkippedRows nvarchar(20)
--DECLARE @strFields nvarchar(4000)
--DECLARE @strFilter nvarchar(4000)
--DECLARE @sortBy nvarchar(4000)
--DECLARE @strFrom nvarchar(4000)
--DECLARE @strID nvarchar(100)

--If(@pageNumber < 0)
--  SET @pageNumber = 1
--SET @strPageSize = CAST(@pageSize AS varchar(20)) 
--SET @strSkippedRows = CAST(((@pageNumber - 1) * @pageSize) AS varchar(20))-- For    example if pageNumber is 5  pageSize is 10, then SkippedRows = 40.
--SET @strID = 'ListingDbID'
--SET @strFields = 'ListingDbID,
--ListingID,  
--[ExtraRoom]
--'
--SET @strFrom = ' vwListingSummary '

--SET @strFilter = ' WHERE
--        CompanyID = ' + CAST(@CompanyID As varchar(20)) 
--End
--SET @sortBy = ''
--if(len(ltrim(rtrim(@sort))) > 0)
--SET @sortBy = ' Order By ' + @sort

---- Total Rows Count

--SET @sql =  'SELECT Count(' + @strID + ')  FROM ' + @strFROM + @strFilter
--EXEC sp_executesql @sql

----// This technique is used in a Single Table pagination
--SET @sql = 'SELECT ' + @strFields + ' FROM ' + @strFROM +
--    ' WHERE ' + @strID +  ' IN ' + 
--   '  (SELECT TOP ' + @strPageSize + ' ' + @strID + ' FROM ' + @strFROM + @strFilter + 
--             ' AND  ' + @strID + ' NOT IN ' + '
--          (SELECT TOP ' + @strSkippedRows + ' ' + @strID + ' FROM ' + @strFROM + @strFilter + @SortBy + ') ' 
--   + @SortBy + ') ' + @SortBy
--Print @sql 
--EXEC sp_executesql @sql

