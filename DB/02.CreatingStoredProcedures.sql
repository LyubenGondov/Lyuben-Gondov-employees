USE [WorkingOnProject]
GO
CREATE PROCEDURE [dbo].[uspGetTotalWorkDays]
AS
BEGIN
	
	SET NOCOUNT ON;
	
select distinct *  ,  DATEDIFF(dd, srt_date ,End_date  ) as Max_No_Days_Worked_Togather from (
	 select t1.EmployeeId as t1_EmployeeId, t1.ProjectId as t1_ProjectId, t1.DateFrom as t1_DateStart ,
	 t1.DateTo as t1_DateEnd,   t.EmployeeId as t_EmployeeID,   t.ProjectId as 
	 t_ProjectID ,t.DateFrom as t_DateStart,t.DateTo as t_DateEnd,
	 case 
	 when t1.DateFrom  = t.DateFrom  then t1.DateFrom
	 when t1.DateFrom  > t.DateFrom   then t1.DateFrom
	 when t1.DateFrom  < t.DateFrom then t.DateFrom end as srt_date,
	 case
	 when t1.DateTo  = t.DateTo then t.DateTo
	 when t1.DateTo  > t.DateTo then t.DateTo
	 when t1.DateTo  < t.DateTo then t1.DateTo end as End_date
	 from EmployeeDaysWork t1
	 left join EmployeeDaysWork t on t1.ProjectId=t.ProjectId and t1.EmployeeId<>t.EmployeeId)s
	 order by  DATEDIFF(dd, srt_date ,End_date  )  desc
    
END
GO
