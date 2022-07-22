CREATE TABLE tb_Status(
idStatus INT NOT NULL,
dsStatus VARCHAR(50) NOT NULL
PRIMARY KEY (idStatus)
)
GO

INSERT INTO tb_Status(idStatus, dsStatus)
VALUES (1, 'Ativo')
GO

INSERT INTO tb_Status(idStatus, dsStatus)
VALUES (2, 'Aguardando')
GO

INSERT INTO tb_Status(idStatus, dsStatus)
VALUES (3, 'Desligado')
GO

CREATE TABLE tb_Processo(
idProcesso INT NOT NULL,
nroProcesso BIGINT NOT NULL,
Autor VARCHAR(90),
DtEntrada DATE,
DtEncerramento DATE,
idStatus INT,
PRIMARY KEY(idProcesso),
FOREIGN KEY (idStatus) REFERENCES tb_Status(idStatus)
)
GO 

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (1, 1, 'Dr. Paulo', '21/07/2020', NULL, 1)
GO

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (2, 2, 'Dr. Fernando', '21/07/2021', NULL, 2)
GO

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (3, 3, 'Dr. Francisco', '21/07/2022', GETDATE(), 3)
GO

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (4, 4, 'Dr. Paulo', '21/07/2020', NULL, 1)
GO

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (5, 4, 'Dr. Paulo', '21/07/2025', NULL, 1)
GO

INSERT INTO tb_Processo(idProcesso, nroProcesso, Autor, DtEntrada, DtEncerramento, idStatus)
VALUES (6, 6, 'Dr. Silvio', '21/07/2023', '21/07/2024', 3)
GO

CREATE TABLE tb_Andamento(
idAndamento INT NOT NULL,
idProcesso INT NOT NULL,
dtAndamento DATE,
dsMovimento VARCHAR(2000),
PRIMARY KEY(idAndamento),
FOREIGN KEY (idProcesso) REFERENCES tb_Processo(idProcesso)
)
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(1, 1, '21/07/2020', 'Pedido 1')
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(2, 2, '21/07/2021', 'Pedido 2')
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(3, 3, '21/07/2022', 'Pedido 3')
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(4, 4, '21/07/2022', 'Pedido 4')
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(5, 5, '21/07/2025', 'Pedido 5')
GO
INSERT INTO tb_Andamento(idAndamento, idProcesso, dtAndamento, dsMovimento)
VALUES
(6, 6, '21/07/2023', 'Pedido 6')
GO

