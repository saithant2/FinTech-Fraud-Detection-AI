FinTech Fraud Detection: My First Data Analysis Project
 Hello and Welcome!

Hi there! Welcome to my very first data analysis portfolio project. I'm just starting out on my data analyst journey, and for this project, I decided to dive into a massive dataset of mobile money transactions (over 6.3 million rows!) to see if I could spot patterns in fraudulent activities.

My goal was to use SQL to understand why the old rule-based fraud detection system was missing so many scams, and to learn what the scammers were actually doing. It was a huge learning curve for me, but acting like a "data detective" was incredibly fun!
 The Data

The database consists of a primary table that stores detailed tracking of money transfers, account balances, and fraud flags. Here's a quick look at the data I worked with:
Column Name	Data Type	Description
step	INT	Represents the time (1 step = 1 hour).
type	VARCHAR(20)	Transaction type (e.g., TRANSFER, CASH_OUT).
amount	DECIMAL(15,2)	The amount transferred.
nameOrig	VARCHAR(50)	The sender's account.
oldbalanceOrg / newbalanceOrig	DECIMAL	Sender's balance before and after.
nameDest	VARCHAR(50)	The receiver's account.
oldbalanceDest / newbalanceDest	DECIMAL	Receiver's balance before and after.
isFraud	INT	1 if it's an actual fraud, 0 if safe.
isFlaggedFraud	INT	1 if the old system thought it was fraud.
🔍 What I Explored (My SQL Queries)
1. Checking the Old System

First, I wanted to see how well the old detection system was doing. I was surprised to find that out of thousands of real frauds, the system only caught a tiny fraction.
SQL

SELECT 
    SUM(isFraud) AS Actual_Fraud_Count,
    SUM(isFlaggedFraud) AS System_Flagged_Count
FROM transactions;

2. Why did the old system fail?

I checked the few transactions the system did catch. It turns out, the old system was just looking for transfers over 200,000. Scammers easily figured this out and worked around it.
SQL

SELECT 
    type AS Transaction_Type, 
    amount AS Transfer_Amount, 
    oldbalanceOrg AS Balance_Before, 
    newbalanceOrig AS Balance_After
FROM transactions 
WHERE isFlaggedFraud = 1;

3. Who are the scammers targeting?

I looked at whether frauds were happening with merchants (M) or regular customers (C). The data showed that frauds were exclusively happening between regular customer accounts.
SQL

SELECT 
    LEFT(nameOrig, 1) AS Sender_Type,
    LEFT(nameDest, 1) AS Receiver_Type,
    COUNT(*) AS Fraud_Count
FROM transactions
WHERE isFraud = 1
GROUP BY Sender_Type, Receiver_Type;

4. How are they moving the money?

I found that the scammers mostly relied on TRANSFER and CASH_OUT to move the stolen funds.
SQL

SELECT 
    type,  
    COUNT(*) AS Fraud_Count
FROM transactions
WHERE isFraud = 1 
GROUP BY type;

5. The "Zero-Balance" Pattern

This was one of my favorite finds! By filtering out system transfer limits, I noticed that when scammers took money, they usually drained the victim's account completely, leaving exactly 0.00.
SQL

SELECT 
    nameOrig AS Victim_Account,
    amount AS Stolen_Amount, 
    oldbalanceOrg AS Balance_Before, 
    newbalanceOrig AS Balance_After
FROM transactions 
WHERE isFraud = 1 AND amount != 10000000
ORDER BY amount DESC
LIMIT 20;

6. What time do they strike?

I extracted the hours from the step column and saw unusual spikes in fraud at 2:00 AM (when victims are likely asleep) and 10:00 AM (possibly to blend in with busy morning hours).
SQL

SELECT 
    (step % 24) AS Hour_of_Day, 
    COUNT(*) AS Fraud_Count
FROM transactions
WHERE isFraud = 1
GROUP BY Hour_of_Day
ORDER BY Fraud_Count DESC;

7. Ghost Accounts

Sometimes the destination accounts still showed a 0.00 balance even after receiving the stolen money. This made me realize the data had tracking blindspots or the scammers were using complex routing accounts.
SQL

SELECT 
    nameDest AS Suspect_Account,
    type AS Transaction_Type,
    amount AS Stolen_Amount,
    oldbalanceDest AS Dest_Balance_Before,
    newbalanceDest AS Dest_Balance_After
FROM transactions
WHERE isFraud = 1
ORDER BY amount DESC
LIMIT 20;

🌱 My Takeaways

Working on this project was a fantastic way to practice SQL and see how data can actually tell a story. I learned that static rules aren't enough to stop financial fraud anymore, which really opened my eyes to why companies are moving towards Machine Learning and AI.

I still have a lot to learn, but I'm really proud of the insights I was able to uncover here. Thanks for reading!