CREATE DATABASE TCC
GO
USE TCC

go
----------------------------------------------------------
----------------------TABELA ONG--------------------------
----------------------------------------------------------
CREATE TABLE Ong
(
		Id INT PRIMARY KEY IDENTITY				NOT NULL
	,	Nome VARCHAR (50)						NOT NULL
	,	Endereco VARCHAR (100)					NOT NULL
	,	Telefone VARCHAR (15)					NOT NULL
	,	Email	 VARCHAR(100)
	,	Senha	VARCHAR(50)
	,	Biografia VARCHAR (200)					NOT NULL
	,	Foto  VARBINARY(MAX)
)
GO
----------------------------------------------------------
----------------------TABELA ANIMAL-----------------------
----------------------------------------------------------
CREATE TABLE Animal
(
		Id INT PRIMARY KEY IDENTITY				NOT NULL
	,	Nome VARCHAR(20)						NOT NULL
	,	Genero CHAR (1)							NOT NULL
	,	DataEntrada DATE DEFAULT GETDATE()		NOT NULL
	,	Especie VARCHAR (15)					NOT NULL	
	,	Descricao VARCHAR(200)					NOT NULL
	,	Foto VARBINARY(MAX)					
)
GO

----------------------------------------------------------
----------------------PROCEDURES ONG----------------------
----------------------------------------------------------
--PROCEDURE INSERT ONG
CREATE PROCEDURE spInsertOng
(
		@nome VARCHAR (50)					
	,	@endereco VARCHAR (100)
	,	@telefone VARCHAR (15)
	,	@biografia VARCHAR (200)
	,	@email VARCHAR (50)
	,	@senha VARCHAR (20)
	,	@foto VARBINARY(MAX)
)
AS
BEGIN 
	INSERT INTO Ong (Nome, Endereco, Telefone, Biografia, Email, Senha, Foto)
	OUTPUT inserted.Id
	VALUES (@nome, @endereco, @telefone, @biografia, @email, @senha, @foto)
END
GO
--PROCEDURE UPDATE ONG
CREATE PROCEDURE spUpdateOng
(
		@id INT
	,	@nome VARCHAR (50)					
	,	@endereco VARCHAR (100)
	,	@telefone VARCHAR (15)
	,	@biografia VARCHAR (200)
	,	@email VARCHAR (50)
	,	@senha VARCHAR (20)
	,	@foto VARBINARY(MAX)
)
AS
BEGIN
	UPDATE Ong SET
		Nome			= @nome
	,	Endereco		= @endereco
	,	Telefone		= @telefone
	,	Biografia		= @biografia
	,	Email			= @email
	,	Senha			= @senha
	,	Foto			= @foto
	WHERE Id = @id
END
GO
--PROCEDURE DELETE ONG
CREATE PROCEDURE spDeleteOng
(
	@id INT
)
AS
BEGIN
	DELETE Ong WHERE Id = @id
END
GO
--PROCEDURE BUSCAR ONG
CREATE PROCEDURE spBuscarOng
(
	@Id	INT
)
AS
BEGIN
	
	SELECT * FROM Ong
	WHERE Id = @Id

END
GO
--PROCEDURE LISTAR ONG
CREATE PROCEDURE spListaOng
(
	@pesquisa VARCHAR(50) = ''
)
AS
BEGIN 
		SELECT Id ,Nome, Endereco, Telefone, Biografia, Email, Senha, Foto
		FROM Ong
		WHERE NOME LIKE '%' + @pesquisa + '%'
END
GO
----------------------------------------------------------
----------------------PROCEDURES ANIMAL-------------------
----------------------------------------------------------
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

