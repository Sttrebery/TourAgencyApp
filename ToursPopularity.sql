USE [TourAgencyDB]
GO
WITH ToursCount AS
(
SELECT tt.ClientToursID, Count(*) as cnt
FROM Tours AS t
JOIN TourTourists tt ON t.ID = tt.ClientToursID
GROUP BY tt.ClientToursID
)

SELECT 
	t.*
	
	--(tc.cnt / CAST(t.MaxTouristCount AS NUMERIC)) * 100 AS rating_percent
FROM Tours AS t
JOIN TourTourists tt ON t.ID = tt.ClientToursID
JOIN ToursCount tc ON tc.ClientToursID = t.ID
WHERE t.IsConducted = 0
ORDER BY DENSE_RANK() OVER (ORDER BY ( tc.cnt / CAST(t.MaxTouristCount AS NUMERIC)) ) ;
