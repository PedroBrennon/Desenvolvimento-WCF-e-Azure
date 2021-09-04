CREATE PROCEDURE [dbo].[GetAllAmigos]
AS
BEGIN
	SELECT a.Id, 
		   a.Foto,
		   a.Nome,
		   a2.Nome as Amigos
	FROM Amigos a
	LEFT JOIN Relacionamentos r
	ON r.IdAmigo1 = a.Id
	LEFT JOIN Amigos a2
	ON a2.Id = r.IdAmigo2
END