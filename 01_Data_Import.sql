SET GLOBAL local_infile = 1;

USE fintech_portfolio;

LOAD DATA LOCAL INFILE 'C:/Users/LEGION/OneDrive/Documents/Portfolio/data_analyisi/fraud_detection.csv' 
INTO TABLE transactions 
FIELDS TERMINATED BY ',' 
ENCLOSED BY '"'
LINES TERMINATED BY '\n'
IGNORE 1 ROWS;