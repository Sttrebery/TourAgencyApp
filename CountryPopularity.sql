USE [TourAgencyDB]
GO

create function GetTopCountry()
returns @BestCountry table (CountryName nvarchar(max), ToursCount int)
AS
BEGIN
	insert @BestCountry
	SELECT TOP 1 c.Name, COUNT(c.ID) as tours_cnt
	FROM Countries AS c
	JOIN Tours AS t ON t.CountyID = c.ID
	WHERE t.IsConducted = 0
	GROUP BY c.Name;
	return
END
GO

