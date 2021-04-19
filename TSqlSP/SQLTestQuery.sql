USE [WMSDB]
GO
/****** Object:  StoredProcedure [dbo].[my_PageResultProc]    Script Date: 4/19/2021 11:15:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[my_PageResultProc] 
(
    @PageNum int = 1,
    @PageSize int = 25,
	@TotalRow int = 0 OUTPUT
)
AS

DECLARE @firstRecord int
DECLARE @lastRecord int
SET @firstRecord = (@PageNum - 1) * @PageSize + 1 -- 1001
SET @lastRecord = @firstRecord + @PageSize - 1   -- 1020

SET NOCOUNT ON

create Table #Temp 
(
  rownum int IDENTITY NOT NULL PRIMARY KEY CLUSTERED,
  TableKey int NOT NULL,
  Quantity int null
)
SET ROWCOUNT @lastRecord
INSERT INTO #Temp (TableKey ) SELECT ID FROM Products ORDER BY Quantity
SET ROWCOUNT 0

SELECT p.*
FROM Products p
  INNER JOIN #Temp t ON t.TableKey = p.ID
WHERE t.rownum >= @firstRecord
ORDER BY t.rownum

RETURN @@ROWCOUNT

SET ROWCOUNT 0
DROP TABLE #Temp
