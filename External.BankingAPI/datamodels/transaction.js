/**
 * @typedef {object} RawTransactionData
 * @property {string} externalId - The unique identifier from the external banking API (GUID string).
 * @property {string | null} [description] - Optional description of the transaction.
 * @property {string} accountId - The GUID string ID of the associated account.
 * @property {string} createdAt - ISO 8601 string representation of the creation date/time (UTC).
 * @property {number} amount - The transaction amount (decimal from C#).
 * @property {boolean} isDebit - True if it's a debit (money out), false if credit (money in).
 */

/**
 * Represents a financial transaction within the FinInsight application.
 * This client-side model uses 'externalId' as its primary identifier.
 */
class Transaction {
    /** @type {string} - The unique identifier for the transaction from banking API */
    id;

    /** @type {string | null} - Optional description of the transaction. */
    description;

    /** @type {string} - The GUID string ID of the associated account. */
    accountId;

    /** @type {Date} - The UTC date and time when the transaction was created. */
    createdAt;

    /** @type {number} - The transaction amount. */
    amount;

    /** @type {boolean} - Indicates if the transaction is a debit (true) or credit (false). */
    isDebit;

    /**
     * Creates an instance of Transaction.
     * @param {RawTransactionData} data - A plain object containing raw transaction data, typically from an API response.
     */
    constructor(data) {
        if (!data || typeof data !== 'object') {
            throw new Error('Transaction constructor requires a data object.');
        }
        if (typeof data.id !== 'string' || data.id.trim() === '') {
            throw new Error('Transaction requires a non-empty externalId.');
        }
        if (typeof data.accountId !== 'string' || data.accountId.trim() === '') {
            throw new Error('Transaction requires a non-empty accountId.');
        }
        if (typeof data.createdAt !== 'string' || data.createdAt.trim() === '') {
            throw new Error('Transaction requires a non-empty createdAt string.');
        }
        if (typeof data.amount !== 'number') {
            throw new Error('Transaction requires a numeric amount.');
        }
        if (typeof data.isDebit !== 'boolean') {
            throw new Error('Transaction requires a boolean isDebit value.');
        }

        this.id = data.id;
        this.description = data.description || null;
        this.accountId = data.accountId;
        this.createdAt = new Date(data.createdAt); // Convert ISO 8601 string to Date object
        this.amount = data.amount;
        this.isDebit = data.isDebit;
    }

    toPlainObject() {
        return {
            id: this.id, // Map back to 'externalId' for API communication if needed
            description: this.description,
            accountId: this.accountId,
            createdAt: this.createdAt.toISOString(), // Convert Date object back to ISO string
            amount: this.amount,
            isDebit: this.isDebit
        };
    }
}

module.exports = Transaction;