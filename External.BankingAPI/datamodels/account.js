// Import the Transaction class
const Transaction = require('./transaction');

/**
 * @typedef {object} RawAccountDataFromApi
 * @property {number} id - The unique identifier from the *banking API*.
 * @property {string} name - The name of the account.
 */

/**
 * Represents a financial account.
 * This client-side model includes the necessary properties for API communication.
 */
class Account {
    /** @type {number} - The unique identifier from the *banking API*. */
    id;

    /** @type {string} - The name of the account. */
    name;

    /** @type {Transaction[]} - The list of transactions associated with this account. */
    transactions;

    /**
     * Creates an instance of Account.
     * @param {RawAccountDataFromApi} data - A plain object containing raw account data, typically from an API response.
     */
    constructor(data) {
        if (!data || typeof data !== 'object') {
            throw new Error('Account constructor requires a data object.');
        }
        if (typeof data.id !== 'number') {
            throw new Error('Account requires a numeric id (from banking API).');
        }
        if (typeof data.name !== 'string' || data.name.trim() === '') {
            throw new Error('Account requires a non-empty name.');
        }

        this.id = data.id;
        this.name = data.name;
        this.transactions = []; // Initialize as an empty array
    }

    /**
     * Associates a list of raw transaction data with this account, automatically
     * setting the accountId for each transaction.
     * @returns {[Account, Transaction[]]} A tuple containing the Account instance itself and the list of created Transaction instances.
     */
    withTransactions(rawTransactionDataList) {
        if (!Array.isArray(rawTransactionDataList)) {
            throw new Error('withTransactions expects an array of raw transaction data.');
        }

        this.transactions = rawTransactionDataList.map(rawTxData => {
            // Ensure the accountId for the transaction is correctly set to this account's banking API ID
            const transactionDataWithAccountId = {
                ...rawTxData,
                accountId: String(this.id) // Ensure it's a string GUID if your Transaction expects that
                                          // (Even if 'id' is number, convert to string for GUID-like consistency)
            };
            return new Transaction(transactionDataWithAccountId);
        });

        // Return the account instance and its transactions, as requested
        return [this, this.transactions];
    }

    /**
     * Converts the Account instance back to a plain object suitable for sending to *your own Fin.Web API*.
     * This method correctly maps the client-side properties to your C# Account entity's expected property names.
     * @returns {object} A plain object representation of the account matching your C# model.
     */
    toPlainObject() {
        return {
            Id: this.id,
            Name: this.name,
        };
    }
}

module.exports = Account;