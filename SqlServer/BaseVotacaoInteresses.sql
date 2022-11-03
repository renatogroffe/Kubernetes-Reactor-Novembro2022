CREATE DATABASE BaseVotacaoInteresses
GO

USE BaseVotacaoInteresses
GO

CREATE TABLE dbo.HistoricoVotacao(
    Id INT IDENTITY(1,1) NOT NULL,
    DataProcessamento DATETIME NOT NULL,
    EventHub VARCHAR(120) NOT NULL,
    Producer VARCHAR(120) NOT NULL,
    Consumer VARCHAR(120) NOT NULL,
    ConsumerGroup VARCHAR(120) NOT NULL,
    HorarioVoto VARCHAR(20) NOT NULL,
    IdVoto VARCHAR(50) NOT NULL,
    Interesse VARCHAR(50) NOT NULL,
    CONSTRAINT PK_HistoricoVotacao PRIMARY KEY (Id)
)
GO