use fintech_portfolio;

-- total transactions
select count(*) as Total_Transactions
from transactions;


-- transfer type
select type, count(*) as Count_Per_Type
from transactions
group by type
order by count(*) desc;

-- Total of real fraud and flagged fraud
select 
	sum(isFraud) as Actual_Fraud_Count,
    sum(isFlaggedFraud) as System_Flagged_Count
from transactions;

-- Fruad Connection (first letter C from nameOrig stands customer and M for Merchant)
select 
	left(nameOrig, 1) as Sender_Type,
    left(nameDest, 1) as Receiver_Type,
    count(*) as Fraud_Count
from transactions
where isFraud = 1
group by Sender_Type, Receiver_Type;

-- Top 10 accounts that received fraud money the most times
select 
	nameDest as Suspect_Account,
    count(*) as Times_Received_Fraud,
    sum(amount) as Total_Stolen_Money
from transactions 
where isFraud = 1
group by nameDest
order by Times_Received_Fraud DESC
limit 10;

-- Check which transation type are used for fraud
select 
	type,  
    count(*) as Fraud_Count
from transactions
where isFraud =1 
group by type;

-- Comparing before and after balance of the top 10 largest scam transactions
select 
	nameOrig as Victim_Account,
    amount as Stolen_Amount, 
    oldbalanceOrg as Balance_Before, 
    newbalanceOrig as Balance_After
from transactions 
where isFraud = 1
order by amount desc
limit 10;

-- Comparing before and after balance of 20 scam transactions which stolen_amount is not 10000000
select 
	nameOrig as Victim_Account,
    amount as Stolen_Amount, 
    oldbalanceOrg as Balance_Before, 
    newbalanceOrig as Balance_After
from transactions 
where isFraud = 1 and amount !=10000000
order by amount desc
limit 20
;
-- Analyzing fraud occurrences by the hour of the day (0 to 23 ) 1 step means 1 hour
select 
	(step % 24) as Hour_Of_Day,
    count(*) as Fraud_Count
from transactions
where isFraud =1
group by Hour_Of_Day
order by Fraud_Count desc;

-- Checking the receiver's balance behavior in fraudulent transactions
select  
	nameDest as Suspect_Account,
    type as Transaction_Type,
    amount as Stolen_Amount ,
    oldbalanceDest AS Dest_Balance_Before,
    newbalanceDest AS Dest_Balance_After 
    from transactions
where isFraud = 1 
order by amount desc

limit 20;

-- Analyzing the criteria of isFlagged Fraud
select 
type as Transaction_Type,
amount as Transfer_Amount,
oldbalanceOrg as Balance_Before,
newbalanceOrig as Balance_After
from transactions
where isFlaggedFraud =1 ;

