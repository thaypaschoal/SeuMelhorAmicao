USE [master]
GO	
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'dbSeuMelhorAmicao')
	BEGIN 
		CREATE DATABASE dbSeuMelhorAmicao
	END
ELSE
	BEGIN 
		DROP DATABASE dbSeuMelhorAmicao;
		CREATE DATABASE dbSeuMelhorAmicao;
	END
GO
USE dbSeuMelhorAmicao;
GO
----------------------------------------------------------
----------------------TABLE------------------------------
----------------------------------------------------------
CREATE TABLE Usuario
(
	  Id INT PRIMARY KEY IDENTITY
	, Nome VARCHAR(70)
	, Email VARCHAR(100) UNIQUE
	, Senha VARCHAR(30)
)
GO
CREATE TABLE Cliente
(
		Id INT PRIMARY KEY IDENTITY
	,	Genero CHAR(1)
	,	DataNascimneto DATE
	,	Conta INT REFERENCES Usuario(Id) 
)
GO
CREATE TABLE Ong
(
		Id INT PRIMARY KEY IDENTITY NOT NULL
	,	Endereco VARCHAR (100) NOT NULL
	,	Telefone VARCHAR (15) NOT NULL
	,	Biografia VARCHAR (200) NOT NULL
	,	Foto  VARBINARY(MAX)
	,	Conta INT REFERENCES Usuario(Id)
)
GO
CREATE TABLE Animal
(
		Id INT PRIMARY KEY IDENTITY NOT NULL
	,	Nome VARCHAR(20) NOT NULL
	,	Genero CHAR (1) NOT NULL 
	,	DataEntrada DATE DEFAULT GETDATE() NOT NULL
	,	Especie VARCHAR (15) NOT NULL 
	,	Descricao VARCHAR(200) NOT NULL
	,	Foto VARBINARY(MAX) 
	,	ONGId INT REFERENCES Ong(Id) 
)
GO
CREATE TABLE Perfil
(
	  Id INT PRIMARY KEY IDENTITY
	, Tipo VARCHAR(20)
);
GO
CREATE TABLE PerfilConta
	(
		IdConta INT REFERENCES Usuario(Id)
	,	IdPerfil INT REFERENCES Perfil(Id)
)
GO
CREATE TABLE Favorito
(
	  IdUsuario INT REFERENCES Cliente(Id)
	, IdAnimal INT REFERENCES Animal(Id)
	, PRIMARY KEY(Idusuario,IdAnimal)
)
GO
----------------------------------------------------------
----------------------PROCEDURE---------------------------
----------------------------------------------------------
----------
---ONG----
----------
--PROCEDURE INSERT ONG
CREATE PROCEDURE spInsertOng
(
		@nome		VARCHAR (50) 
	,	@endereco	VARCHAR (100)
	,	@telefone	VARCHAR (15)
	,	@biografia	VARCHAR (200)
	,	@email		VARCHAR (50)
	,	@senha		VARCHAR (20)
	,	@foto		VARBINARY(MAX)
)
AS
BEGIN 

	DECLARE @idConta INT


	INSERT INTO Usuario(Nome, Email, Senha)
	VALUES(@nome,@email,@senha);

	SELECT @idConta =  @@IDENTITY;
	
	INSERT INTO Ong (Endereco, Telefone, Biografia, Foto, Conta)
	OUTPUT inserted.Id
	VALUES (@endereco, @telefone, @biografia,@foto, @idConta)

END
GO
--PROCEDURE UPDATE ONG
CREATE PROCEDURE spUpdateOng
(
		@id			INT
	,	@nome		VARCHAR (50) 
	,	@endereco	VARCHAR (100)
	,	@telefone	VARCHAR (15)
	,	@biografia	VARCHAR (200)
	,	@email		VARCHAR (50)
	,	@senha		VARCHAR (20)
	,	@foto		VARBINARY(MAX)
)
AS
BEGIN

	DECLARE @idConta INT 

	SELECT @idConta = Conta FROM Ong	
						WHERE Id = @id;

	UPDATE Usuario
		SET Nome = @nome
		,	Senha = @senha
		,	Email = @email
	WHERE Id = @idConta;


	UPDATE Ong 
	SET		Endereco = @endereco
		,	Telefone = @telefone
		,	Biografia = @biografia
		,	Foto = @foto
	WHERE Id = @id;
END
GO
--PROCEDURE DELETE ONG
CREATE PROCEDURE spDeleteOng
(
	@id INT
)
AS
BEGIN

	DECLARE @idConta INT 

	SELECT @idConta = Conta FROM Ong	
						WHERE Id = @id;
	--ai deleta a ONG
	DELETE FROM Ong WHERE Id = @id
	--Deleta a conta
	DELETE FROM Usuario WHERE Id = @idConta;

END
GO
--PROCEDURE BUSCAR ONG
CREATE PROCEDURE spBuscarOng
(
	@Id INT
)
AS
BEGIN

	SELECT 
			U.Nome
		,	U.Email
		,	O.Endereco
		,	O.Telefone
		,	O.Biografia
		,	O.Foto
		FROM Ong O
	 INNER JOIN Usuario U
	 ON U.Id = O.Conta
	 WHERE O.Id = @id;
	 
END
GO
--PROCEDURE LISTAR ONG
CREATE PROCEDURE spListaOng
(
	@pesquisa VARCHAR(50) = ''
)
AS
BEGIN 
	SELECT O.Id ,U.Nome, O.Endereco, O.Telefone, O.Biografia, U.Email, O.Foto
	FROM Ong O
	INNER JOIN Usuario U
	ON U.Id = O.Conta
	WHERE U.NOME LIKE '%' + @pesquisa + '%'
END
GO

--------------------
---ANIMAL-----------
--------------------
CREATE PROCEDURE spInsertAnimal	
(
		@nome VARCHAR (20)
	,	@genero CHAR (1)
	,	@dataEntrada DATE
	,	@especie VARCHAR (15)
	,	@descricao VARCHAR (200)
	,	@foto VARBINARY(MAX)
)
AS
BEGIN
	INSERT INTO Animal (Nome, Genero, DataEntrada, Especie, Descricao, Foto)
	OUTPUT inserted.Id
	VALUES (@nome, @genero, @dataEntrada, @especie, @descricao, @foto)
END
GO
--PROCEDURE UPDATE ANIMAL
CREATE PROCEDURE spUpdateAnimal
(
		@id INT
	,	@nome VARCHAR (20)
	,	@genero CHAR (1)
	,	@dataEntrada DATE
	,	@especie VARCHAR (15)
	,	@descricao VARCHAR (200)
	,	@foto VARBINARY(MAX)
)
AS
BEGIN
	UPDATE Animal SET
		Nome		= @nome
	,	Genero		= @genero
	,	DataEntrada	= @dataEntrada
	,	Especie		= @especie
	,	Descricao	= @descricao
	,	Foto		= @foto
	WHERE Id = @id
END
GO
--PROCEDURE DELETE ANIMAL]
CREATE PROCEDURE spDeleteAnimal
(
	@id INT
)
AS
BEGIN
	DELETE Animal WHERE Id = @id
END
GO
--PROCEDURE BUSCAR ANIMAL
CREATE PROCEDURE spBuscarAnimal
(
	@Id	INT
)
AS
BEGIN
	
	SELECT * FROM Animal
	WHERE Id = @Id

END
GO
--PROCEDURE LISTAR ANIMAL
CREATE PROCEDURE spListaAnimal
(
	@pesquisa VARCHAR(50) = ''
)
AS
BEGIN 
		SELECT Id, Nome, Genero, DataEntrada, Especie, Descricao, Foto
		FROM Animal
		WHERE Nome LIKE '%' + @pesquisa + '%'
		OR Especie LIKE '%' + @pesquisa + '%'
	
END

GO
--PROCEDURE FAVORITAR ANIMAL
CREATE PROCEDURE spFavoritar
(
		@idUsuario	INT
	,	@IdAnimal	INT
)
AS
BEGIN 
	INSERT 
		INTO  Favorito(IdUsuario, IdAnimal)
		VALUES(@idUsuario, @IdAnimal)
END
GO
--PROCEDURE FAVORITAR ANIMAL
CREATE PROCEDURE spDesFavoritar
(
		@idUsuario	INT
	,	@IdAnimal	INT
)
AS
BEGIN 
	DELETE FROM Favorito
		WHERE IdUsuario = @idUsuario
		AND IdAnimal = @IdAnimal
END
GO

--------------------
---PERFIL-----------
--------------------
--PROCEDURE INSERT PERFIL DA CONTA
CREATE PROCEDURE spInsertPerfilConta
(
		@idConta	INT
	,	@idPerfil	INT
)
AS
BEGIN 
	INSERT 
		INTO  PerfilConta(IdConta, IdPerfil)
		VALUES(@idConta, @idPerfil)
END
GO
--PROCEDURE DELETE PERFIL DA CONTA
CREATE PROCEDURE spDeletePerfilConta
(
		@idConta	INT
	,	@idPerfil	INT
)
AS
BEGIN 
	DELETE 
		FROM PerfilConta
	WHERE IdConta = @idConta 
	AND IdPerfil = @idPerfil
END
GO
--PROCEDURE INSERT PERFIL
CREATE PROCEDURE spInsertPerfil
(
		@Tipo	VARCHAR(20)
)
AS
BEGIN 
	INSERT 
		INTO  Perfil(Tipo) VALUES(@Tipo)
END
GO
--PROCEDURE UPDATE PERFIL
CREATE PROCEDURE spUpdatePerfil
(
		@id		INT
	,	@Tipo	VARCHAR(20)
)
AS
BEGIN 
	UPDATE Perfil
		SET Tipo = @Tipo
	WHERE Id = @id;
END
GO
--PROCEDURE DELETE PERFIL
CREATE PROCEDURE spDeletePerfil
(
		@id	INT
)
AS
BEGIN 
	DELETE 
		FROM  Perfil 
	WHERE Id = @id;
END
GO
--PROCEDURE LISTAR PERFILS
CREATE PROCEDURE spListPerfil
	@pesquisa VARCHAR(10) = ''
AS
BEGIN 
	SELECT 
		* FROM Perfil
	WHERE Tipo LIKE '%'+ @pesquisa + '%';
END
GO
--PROCEDURE GET PERFIl
CREATE PROCEDURE spGetPerfil
(
	@id INT
)
AS
BEGIN 
	SELECT * FROM Perfil
		WHERE Id = @id;
	
END
GO
---------------------
---CLIENTE-----------
---------------------
CREATE PROCEDURE spInsertCliente
(
		@nome				VARCHAR (50) 
	,	@email				VARCHAR (50)
	,	@senha				VARCHAR (20)
	,	@genero				CHAR(1)
	,	@dataNascimento		DATE
)
AS
BEGIN 

	DECLARE @idConta INT


	INSERT INTO Usuario(Nome, Email, Senha)
	VALUES(@nome,@email,@senha);

	SELECT @idConta =  @@IDENTITY;
	
	INSERT INTO Cliente(Genero,DataNascimneto, Conta)
	OUTPUT inserted.Id
	VALUES (@genero,@dataNascimento, @idConta)

END
GO
CREATE PROCEDURE spUpdateCliente
(
		@id					INT
	,	@nome				VARCHAR (50) 
	,	@email				VARCHAR (50)
	,	@senha				VARCHAR (20)
	,	@genero				CHAR(1)
	,	@dataNascimento		DATE
)
AS
BEGIN

	DECLARE @idConta INT 

	SELECT @idConta = Conta FROM Ong	
						WHERE Id = @id;

	UPDATE Usuario
		SET Nome = @nome
		,	Senha = @senha
		,	Email = @email
	WHERE Id = @idConta;


	UPDATE Cliente 
	SET		Genero = @genero
		,	DataNascimneto = @dataNascimento	
	WHERE Id = @id;
END
GO
CREATE PROCEDURE spDeleteCliente
(
	@id INT
)
AS
BEGIN

	DECLARE @idConta INT 

	SELECT @idConta = Conta FROM Ong	
						WHERE Id = @id;
	
	DELETE FROM Cliente WHERE Id = @id

	DELETE FROM Usuario WHERE Id = @idConta;

END
GO
CREATE PROCEDURE spBuscarCliente
(
	@Id INT
)
AS
BEGIN

	SELECT	U.Nome
		,	U.Email
		,	U.Senha
		,	C.Genero
		,	C.DataNascimneto	
		FROM Cliente C
	 INNER JOIN Usuario U
	 ON U.Id = C.Conta
	 WHERE C.Id = @id;
	 
END
GO
CREATE PROCEDURE spListaCliente
(
	@pesquisa VARCHAR(50) = ''
)
AS
BEGIN 
	SELECT	U.Nome
		,	U.Email
		,	U.Senha
		,	C.Genero
		,	C.DataNascimneto	
		FROM Cliente C
	 INNER JOIN Usuario U
	 ON U.Id = C.Conta
	WHERE U.Nome LIKE '%' + @pesquisa + '%'
END
GO