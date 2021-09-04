--Amigos
CREATE PROCEDURE [dbo].[GetAllAmigos]
AS
BEGIN
	SELECT a.Id, 
		   a.Foto,
		   a.Nome,
		   a.Sobrenome,
		   a.Email,
		   a.Aniversario,
		   a.Estado,
		   a.Pais,
		   a2.Nome as Amigos
	FROM Amigos a
	LEFT JOIN Relacionamentos r
	ON r.IdAmigo1 = a.Id
	LEFT JOIN Amigos a2
	ON a2.Id = r.IdAmigo2
END

CREATE PROCEDURE [dbo].[GetAmigoById]
	@Id INT
AS
BEGIN
	SELECT a.Id, 
		   a.Foto,
		   a.Nome,
		   a.Sobrenome,
		   a.Email,
		   a.Aniversario,
		   a.Estado,
		   a.Pais,
		   a2.Nome as Amigos
	FROM Amigos a
	LEFT JOIN Relacionamentos r
	ON r.IdAmigo1 = a.Id
	LEFT JOIN Amigos a2
	ON a2.Id = r.IdAmigo2
	WHERE a.Id = @Id
END

CREATE PROCEDURE [dbo].[GetRelacionamentoById]
	@IdAmigo INT
AS
BEGIN
	SELECT a.Foto,
		   a.Nome,
		   a.Sobrenome
	FROM Relacionamentos r
	LEFT JOIN Amigos a
	ON a.Id = r.IdAmigo2
	WHERE r.IdAmigo1 = @IdAmigo
END

CREATE PROCEDURE [dbo].[DeleteAmigo]
	@Id INT
AS
BEGIN
	DELETE r
	FROM Relacionamentos r
	WHERE r.IdAmigo1 = @Id OR r.IdAmigo2 = @Id;

	DELETE a
	FROM Amigos a
	WHERE a.Id = @Id;
END

CREATE PROCEDURE [dbo].[DeleteRelacionamento]
	@IdAmigo1 INT,
	@IdAmigo2 INT
AS
BEGIN
	DELETE r
	FROM Relacionamentos r
	WHERE r.IdAmigo1 = @IdAmigo1 AND r.IdAmigo2 = @IdAmigo2;
END

ALTER PROCEDURE [dbo].[UpdateAmigo]
	@Id int,
	@Foto nvarchar(max),
	@Nome nvarchar(50),
	@Sobrenome nvarchar(max),
	@Email nvarchar(max),
	@Telefone nvarchar(10),
	@Aniversario datetime,
	@Pais nvarchar(50),
	@Estado nvarchar(50)
AS
BEGIN
	UPDATE Amigos
	SET Amigos.Foto = @Foto,
		Amigos.Nome = @Nome,
		Amigos.Sobrenome = @Sobrenome,
		Amigos.Email = @Email,
		Amigos.Telefone = @Telefone,
		Amigos.Aniversario = @Aniversario,
		Amigos.Pais = @Pais,
		Amigos.Estado = @Estado
	FROM Amigos
	WHERE Amigos.Id = @Id
END

CREATE PROCEDURE [dbo].[InsertAmigo]
	@Foto nvarchar(max),
	@Nome nvarchar(50),
	@Sobrenome nvarchar(max),
	@Email nvarchar(max),
	@Telefone nvarchar(10),
	@Aniversario datetime,
	@Pais nvarchar(50),
	@Estado nvarchar(50)
AS
BEGIN
	INSERT INTO Amigos(Foto, Nome, Sobrenome, Email, Telefone, Aniversario, Pais, Estado)
	VALUES (@Foto, @Nome, @Sobrenome, @Email, @Telefone, @Aniversario, @Pais, @Estado)
END

CREATE PROCEDURE [dbo].[InsertRelacionamentoAmigos]
	@IdAmigo1 INT,
	@IdAmigo2 INT
AS
BEGIN
	INSERT INTO Relacionamentos(IdAmigo1, IdAmigo2)
	VALUES (@IdAmigo1, @IdAmigo2)
END



--Estados

CREATE PROCEDURE [dbo].[GetAllEstados]
AS
BEGIN
	SELECT e.Id, 
		   e.Foto,
		   e.Nome,
		   e.Pais
	FROM Estados e
END

CREATE PROCEDURE [dbo].[GetEstadoById]
	@Id INT
AS
BEGIN
	SELECT e.Id, 
		   e.Foto,
		   e.Nome,
		   e.Pais
	FROM Estados e
	WHERE @Id = e.Id
END

CREATE PROCEDURE [dbo].[DeleteEstado]
	@Id INT
AS
BEGIN
	DELETE e
	FROM Estados e
	WHERE e.Id = @Id;
END

CREATE PROCEDURE [dbo].[UpdateEstado]
	@Id int,
	@Foto nvarchar(max),
	@Nome nvarchar(50),
	@Pais int
AS
BEGIN
	UPDATE Estados
	SET Foto = @Foto,
		Nome = @Nome,
		Pais = @Pais
	FROM Estados
	WHERE Id = @Id
END

CREATE PROCEDURE [dbo].[InsertEstado]
	@Foto NVARCHAR(MAX),
	@Nome NVARCHAR(50),
	@Pais INT
AS
BEGIN
	INSERT INTO Estados(Foto, Nome, Pais)
	VALUES (@Foto, @Nome, @Pais)
END


-- Paises

CREATE PROCEDURE [dbo].[GetAllPaises]
AS
BEGIN
	SELECT p.Id, 
		   p.Foto,
		   p.Nome,
		   e.Nome as Estados
	FROM Paises p
	LEFT JOIN Estados e
	ON e.Pais = p.Id
END

CREATE PROCEDURE [dbo].[GetPaisById]
	@Id INT
AS
BEGIN
	SELECT p.Id, 
		   p.Foto,
		   p.Nome,
		   e.Nome as Estados
	FROM Paises p
	LEFT JOIN Estados e
	ON e.Pais = p.Id
	WHERE p.Id = @Id
END

CREATE PROCEDURE [dbo].[DeletePais]
	@Id INT
AS
BEGIN
	DELETE e
	FROM Estados e
	WHERE e.Pais = @Id;

	DELETE p
	FROM Paises p
	WHERE p.Id = @Id;
END

CREATE PROCEDURE [dbo].[UpdatePais]
	@Id int,
	@Foto nvarchar(max),
	@Nome nvarchar(50)
AS
BEGIN
	UPDATE Paises
	SET Paises.Foto = @Foto,
		Paises.Nome = @Nome
	FROM Paises
	WHERE Paises.Id = @Id
END

CREATE PROCEDURE [dbo].[InsertPais]
	@Foto NVARCHAR(MAX),
	@Nome NVARCHAR(50)
AS
BEGIN
	INSERT INTO Paises(Foto, Nome)
	VALUES (@Foto, @Nome)
END