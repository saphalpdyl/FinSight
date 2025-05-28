var express = require('express');
var router = express.Router();

const Account = require('../datamodels/account');
const Transaction = require('../datamodels/transaction');

router.get('/api/v1/transactions/byAccount/', function(req, res, next) {
  const { accountId } = req.query;

  const bankingApiData = {
    // Accounts from the Banking API
    accounts: [
        {
            id: 1001, // Banking API's unique ID for this account (number)
            name: "Main Checking Account",
            // Note: No internalId, transactionsCachedUntilDateTime, or userId from the banking API
            // These would be added/mapped within your FinInsight backend or application layer.
        },
        {
            id: 1002, // Banking API's unique ID for this account (number)
            name: "Savings Portfolio",
        },
        {
            id: 1003, // Banking API's unique ID for this account (number)
            name: "Credit Card",
        }
    ],

    // Transactions from the Banking API
    transactions: [
        // Transactions for Account ID: 1001 ("Main Checking Account")
        {
            id: "272f3e0c-d4a9-4b1f-8c7e-9a0b1c2d3e4f", // Banking API's unique ID for this transaction (GUID string)
            description: "Online Subscription Service",
            accountId: "1001", // Banking API's ID for the associated account (string, matching the account's numeric ID)
            createdAt: "2025-05-25T14:30:00Z", // ISO 8601 UTC string
            amount: -12.99,
            isDebit: true
        },
        {
            id: "8c7e9a0b-1c2d-3e4f-5g6h-7i8j9k0l1m2n",
            description: "Local Cafe",
            accountId: "1001",
            createdAt: "2025-05-26T09:15:00Z",
            amount: -5.75,
            isDebit: true
        },
        {
            id: "d4a94b1f-8c7e-9a0b-1c2d-3e4f5g6h7i8j",
            description: "Direct Deposit - Paycheck",
            accountId: "1001",
            createdAt: "2025-05-27T11:00:00Z",
            amount: 2500.00,
            isDebit: false
        },
        // Transactions for Account ID: 1002 ("Savings Portfolio")
        {
            id: "e5f67g8h-9i0j-1k2l-3m4n-5o6p7q8r9s0t",
            description: "Transfer to Savings",
            accountId: "1002",
            createdAt: "2025-05-27T12:30:00Z",
            amount: 500.00,
            isDebit: false
        },
        // Transactions for Account ID: 1003 ("Credit Card")
        {
            id: "f1a2b3c4-d5e6-7f8g-9h0i-1j2k3l4m5n6o",
            description: "Online Retailer Purchase",
            accountId: "1003",
            createdAt: "2025-05-26T19:00:00Z",
            amount: -89.99,
            isDebit: true
        }
    ]
  };

  // --- Example of how your JavaScript classes would consume this data ---

  // Assuming you have imported your classes:
  // import Account from './Account';
  // import Transaction from './Transaction';

  // Simulate processing the banking API data for a specific user (e.g., John Doe)
  // In a real app, your backend would handle associating these accounts/transactions
  // with a FinsightUser before sending to the frontend.
  const JOHN_DOE_USER_ID = "a1b2c3d4-e5f6-7890-1234-567890abcdef"; // This would come from your backend

  const processedAccounts = [];
  const allProcessedTransactions = [];

  // Iterate through the raw accounts from the banking API
  bankingApiData.accounts.forEach(rawAccount => {
      // For each raw account, create an Account instance
      // You might add `internalId` and `transactionsCachedUntilDateTime` here if your
      // backend sends them, or if you calculate them client-side for initial display.
      // For this example, I'm sticking to the properties your JS constructor accepts.
      const newAccount = new Account({ ...rawAccount, userId: JOHN_DOE_USER_ID }); // Add userId here if it's not from banking API

      // Filter transactions relevant to this account
      const rawAccountTransactions = bankingApiData.transactions.filter(
          tx => String(tx.accountId) === String(newAccount.id)
      );

      // Use the `withTransactions` method to associate them
      const [accountWithTx, accountTransactions] = newAccount.withTransactions(rawAccountTransactions);

      processedAccounts.push(accountWithTx);
      allProcessedTransactions.push(...accountTransactions);
  });

  console.log("--- Processed Accounts (JS objects) ---");
  processedAccounts.forEach(acc => {
      console.log(`Account ID: ${acc.id}, Name: ${acc.name}, Num Transactions: ${acc.transactions.length}`);
  });

  console.log("\n--- Processed Transactions (JS objects) ---");
  allProcessedTransactions.forEach(tx => {
      console.log(`Transaction ID: ${tx.id}, Desc: ${tx.description}, Acc ID: ${tx.accountId}, Amount: ${tx.amount}`);
  });
  
  return res.json({
    message: 'Welcome to the API',
    status: 'success'
  })
});

module.exports = router;
