Use [TourAgencyDB]
Go
CREATE PROCEDURE st_GetProfileByUserID
	@userID int
AS
BEGIN
    SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM Employees WHERE UserID = @userID)
		Select 
			e.Name,
			e.Surname,
			e.Patronimyc,
			e.PhoneNumber,
			e.PhotoID,
			p.PhotoValue,
			u.Email
		FROM Employees AS e
		JOIN Users AS u ON u.ID = e.UserID
		LEFT JOIN Photos AS p ON p.ID = e.PhotoID
		Where UserID = @userID;
	IF EXISTS (SELECT 1 FROM Clients WHERE UserID = @userID)
		Select 
			c.Name,
			c.Surname,
			c.Patronimyc,
			c.PhoneNumber,
			c.PhotoID,
			p.PhotoValue,
			u.Email
		FROM Clients AS c
		JOIN Users AS u ON u.ID = c.UserID
		LEFT JOIN Photos AS p ON p.ID = c.PhotoID
		Where UserID = @userID;
	END
GO

