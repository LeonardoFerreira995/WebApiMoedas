-- 1 Com base no modelo acima, escreva um comando SQL que liste a quantidade de processos por Status com sua descri��o.
SELECT COUNT(TB.idProcesso) qtd_processo, TS.dsStatus FROM tb_Processo TB
INNER JOIN tb_Status TS ON TS.idStatus = TB.idStatus
GROUP BY TS.dsStatus

-- 2. Com base no modelo acima, construa um comando SQL que liste a maior data de andamento por n�mero de processo, com processos encerrados no ano de 2013.
SELECT MAX(TA.dtAndamento) dtAndamento, TB.nroProcesso   FROM tb_Processo TB
INNER JOIN tb_Andamento TA ON TA.idProcesso = TB.idProcesso
GROUP BY TB.nroProcesso

-- 3. Com base no modelo acima, construa um comando SQL que liste a quantidade de Data de Encerramento agrupada por ela mesma onde a quantidade da contagem seja maior que 5.
SELECT COUNT(TB.DtEncerramento) DtEncerramento FROM tb_Processo TB
GROUP BY TB.DtEncerramento
HAVING COUNT(TB.DtEncerramento) > 5

-- 4. Possu�mos um n�mero de identifica��o do processo, onde o mesmo cont�m 12 caracteres com zero � esquerda, contudo nosso modelo e dados ele � apresentado como bigint. Como fazer para apresenta-lo com 12 caracteres considerando os zeros a esquerda?

SELECT REPLICATE('0', 12 - LEN(nroProcesso)) + CAST (nroProcesso AS VARCHAR) AS nroProcesso
FROM   tb_Processo;  