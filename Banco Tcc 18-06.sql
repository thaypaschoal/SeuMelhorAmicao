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
CREATE TABLE Perfil
(
	  Id INT PRIMARY KEY IDENTITY
	, Tipo VARCHAR(20) UNIQUE
);
GO
INSERT 
	INTO Perfil(Tipo)
	VALUES('Ong'),('Cliente')
GO
CREATE TABLE Usuario
(
	  Id INT PRIMARY KEY IDENTITY
	, Nome VARCHAR(70)
	, Email VARCHAR(100) UNIQUE
	, Senha VARCHAR(30)
	, Perfil	INT REFERENCES Perfil(Id)	
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
CREATE TABLE Favorito
(
	  IdUsuario INT REFERENCES Cliente(Id)
	, IdAnimal INT REFERENCES Animal(Id)
	, PRIMARY KEY(IdUsuario,IdAnimal)
)
GO
SELECT * FROM Favorito

SELECT * FROM Usuario
----------------------------------------------------------
----------------------PROCEDURE---------------------------
----------------------------------------------------------
----------
---ONG----

CREATE PROCEDURE spLogin
(
		@email		VARCHAR (100) 
	,	@senha		VARCHAR (30)
)
AS
BEGIN 

	IF(SELECT COUNT(*)  FROM Usuario WHERE Email = @email AND Senha = @senha) = 1
		BEGIN
		SELECT U.Id, U.Nome, U.Email , P.Tipo
			FROM Usuario U
			INNER JOIN Perfil P
			ON P.Id = U.Perfil
		WHERE U.Email = @email AND U.Senha = @senha
		END
		
END
GO
CREATE PROCEDURE spBuscarInformacoes
(	
	@usuarioID	INT
)
AS
BEGIN 
		DECLARE @TipoConta INT
		
	SET @TipoConta = (SELECT P.Id FROM Usuario
					 INNER JOIN Perfil P
					 ON P.Id = Usuario.Perfil WHERE Usuario.Id =@usuarioID);

	IF(@TipoConta = 1)
		BEGIN
			SELECT O.Id, U.Nome, U.Email, P.Tipo
			FROM ONG O
			INNER JOIN Usuario U
			ON O.Conta = U.Id
			INNER JOIN Perfil P
			ON P.Id = U.Perfil
			WHERE O.Conta = @usuarioID;
		END
	ELSE 
		BEGIN
			SELECT C.Id, U.Nome, U.Email, P.Tipo
				 FROM Cliente C
			INNER JOIN Usuario U
			ON C.Conta = U.Id
			INNER JOIN Perfil P
			ON P.Id = U.Perfil
			WHERE C.Conta = @usuarioID;
		END
END
GO
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
	SET NOCOUNT ON;


	---VARIAVEIS
	DECLARE		@ErrorMessage		VARCHAR(2000)
		,		@ErrorState			TINYINT
		,		@ErrorSeverity		TINYINT
	
		BEGIN TRY 
			DECLARE @idConta INT

			INSERT INTO Usuario(Nome, Email, Senha, Perfil)
			VALUES(@nome,@email,@senha, 1);


			SELECT @idConta =  @@IDENTITY;
	
			INSERT INTO Ong (Endereco, Telefone, Biografia, Foto, Conta)
			OUTPUT inserted.Id
			VALUES (@endereco, @telefone, @biografia,@foto, @idConta)

		END TRY
		BEGIN CATCH 
			SET @ErrorMessage	= ERROR_MESSAGE()
			SET @ErrorState		= ERROR_STATE()
			SET @ErrorSeverity	= ERROR_SEVERITY()
			RAISERROR(@ErrorMessage, @ErrorState, @ErrorSeverity)
		END CATCH		 
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
			O.Id 
		,	U.Nome
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
	,	@especie VARCHAR (15)
	,	@descricao VARCHAR (200)
	,	@foto VARBINARY(MAX)
	,	@OngID	INT
)
AS
BEGIN
	INSERT INTO Animal (Nome, Genero, Especie, Descricao, Foto,ONGId)
	OUTPUT inserted.Id
	VALUES (@nome, @genero, @especie, @descricao, @foto, @OngID)
END
GO
--PROCEDURE UPDATE ANIMAL
CREATE PROCEDURE spUpdateAnimal
(
		@id INT
	,	@nome VARCHAR (20)
	,	@genero CHAR (1)
	,	@especie VARCHAR (15)
	,	@descricao VARCHAR (200)
	,	@foto VARBINARY(MAX)
)
AS
BEGIN
	UPDATE Animal SET
		Nome		= @nome
	,	Genero		= @genero	
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
CREATE PROCEDURE spListaAnimalOng
(
	@OngId	INT
)
AS
BEGIN 
		SELECT Id, Nome, Genero, DataEntrada, Especie, Descricao, Foto
		FROM Animal
		WHERE ONGId = @OngId;
	
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


	INSERT INTO Usuario(Nome, Email, Senha, Perfil)
	VALUES(@nome,@email,@senha, 2);

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

	SELECT	U.Id
		,	U.Nome
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
CREATE PROCEDURE spListaFavoritos
(
	@idCliente	INT
)
AS
BEGIN 
	SELECT	
	A.Id, A.Nome, A.Genero,A.DataEntrada, A.Especie,A.Descricao,A.Foto
	 FROM Favorito F
	INNER JOIN Animal A
	ON F.IdAnimal = A.Id
	WHERE F.IdUsuario = @idCliente
END
GO