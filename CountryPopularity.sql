SELECT c.ID, COUNT(c.ID) as tours_cnt
FROM Countries AS c
JOIN Tours AS t ON t.CountyID = c.ID
WHERE t.IsConducted = 0
GROUP BY c.ID;