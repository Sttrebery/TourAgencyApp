USE [TourAgencyDB]
GO

create view PopularTours
AS

WITH ToursCount AS
(
SELECT tt.ClientToursID, Count(*) as cnt
FROM Tours AS t
JOIN TourTourists tt ON t.ID = tt.ClientToursID
GROUP BY tt.ClientToursID
)

SELECT 
	ROW_NUMBER() OVER (ORDER BY ( tc.cnt / CAST(t.MaxTouristCount AS NUMERIC)) ) as rank,
	t.*
FROM Tours AS t
JOIN TourTourists tt ON t.ID = tt.ClientToursID
JOIN ToursCount tc ON tc.ClientToursID = t.ID
WHERE t.IsConducted = 0
GO


SELECT * FROM  PopularTours