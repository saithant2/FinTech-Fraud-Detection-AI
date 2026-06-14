AI-Powered FinTech Fraud Detection System

##  Project Overview
This end-to-end data analytics and software engineering project analyzes over 6.3 million financial transactions to identify fraudulent patterns and builds a real-time, AI-driven alert system to detect suspicious activities. 

**Tech Stack Used:** MySQL, Tableau, C# (.NET), Ollama (Local Llama 3 AI)

##  Data Cleaning & Preparation
Before diving into the complex SQL queries, I first needed to ensure the data was reliable:
* **Null Values:** Verified that there were no missing values across all 6.3 million rows.
* **Duplicates:** Checked for and confirmed the absence of duplicated transaction records.
* **Data Type Formatting:** Ensured that all financial columns were properly treated as decimals and standard types.

##  Phase 1: Database Analysis (SQL)
Using MySQL, I queried the `transactions` database to find loopholes in the legacy rule-based system.
* **Key Finding:** The legacy system only flagged 16 transactions as fraud out of 8,213 actual frauds.
* **Scammer Behavior:** Fraudsters predominantly use `TRANSFER` followed immediately by `CASH_OUT`. They typically drain the entire account balance (New Balance = $0).

## Phase 2: Data Visualization (Tableau)
Although its my first time using Tableau ,I tried to build a professional  interactive dashboard to visualize the insights discovered during the SQL phase, allowing stakeholders to easily identify peak hours of fraudulent activity and the methods used.
* **🔗 [Click here to view the Interactive Tableau Dashboard](https://public.tableau.com/app/profile/sai.thant8654/viz/Fraud_Detection_Dashboard_17811758094720/Fraud_Detection?publish=yes)**

##  Phase 3: AI Integration & Monitoring (C#)
To move beyond static analysis, I developed a C# Console Application that acts as a real-time monitor.
* The application connects directly to the MySQL database to scan live transactions.
* It dynamically feeds suspicious records into a local instance of **Llama 3 via Ollama**.
* The AI analyzes the transaction patterns (e.g., rapid depletion of funds) and outputs a `[FRAUD DETECTED]` or `[SAFE]` alert with a generated security reasoning.

*(Note: Database passwords in the C# source code have been removed for security purposes).*
