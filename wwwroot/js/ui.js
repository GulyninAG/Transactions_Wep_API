// Для работы с пользовательским интерфейсом (обработка событий, обновление DOM).
import { getTransaction, getTransactions, createTransaction, deleteTransaction } from './api.js';
import { generate_guid } from './utils.js';

export function initUI() {
    document.getElementById("resetBtn").addEventListener("click", reset);
    document.getElementById("oneResetBtn").addEventListener("click", resetOne);
    document.getElementById("getTransactionBtn").addEventListener("click", handleGetTransaction);
    document.getElementById("saveBtn").addEventListener("click", handleSaveTransaction);
}

export async function renderTransactions() {
    const transactions = await getTransactions();
    const rows = document.querySelector("tbody");
    rows.innerHTML = "";
    transactions.forEach(transaction => rows.append(createTransactionRow(transaction)));
}

export function createTransactionRow(transaction) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", transaction.id);

    const idTd = document.createElement("td");
    idTd.append(transaction.id);
    tr.append(idTd);

    const dateTd = document.createElement("td");
    dateTd.append(transaction.transactionDate);
    tr.append(dateTd);

    const amountTd = document.createElement("td");
    amountTd.append(transaction.amount);
    tr.append(amountTd);

    const removeLink = document.createElement("button");
    removeLink.append("Удалить транзакцию");
    removeLink.className = "btn-rmv btn btn-primary";
    removeLink.addEventListener("click", async () => {
        await deleteTransaction(transaction.id);
        tr.remove();
    });

    tr.appendChild(removeLink);

    return tr;
}

export function reset() {
    document.getElementById("transactionId").value = "";
    document.getElementById("transactionDate").value = "";
    document.getElementById("transactionAmount").value = "";
}

export function resetOne() {
    document.getElementById("oneTransactionId").value = "";
    document.getElementById("oneTransactionDate").value = "";
    document.getElementById("oneTransactionAmount").value = "";
}

async function handleGetTransaction() {
    const transactionId = document.getElementById("oneTransactionId").value;
    const transaction = await getTransaction(transactionId);
    document.getElementById("oneTransactionDate").value = transaction.transactionDate;
    document.getElementById("oneTransactionAmount").value = transaction.amount;
}

async function handleSaveTransaction() {
    const transactionDate = document.getElementById("transactionDate").value;
    const transactionAmount = document.getElementById("transactionAmount").value;

    if (transactionDate && transactionAmount) {
        const transaction = {
            Id: generate_guid(),
            TransactionDate: transactionDate,
            Amount: parseFloat(transactionAmount)
        };

        await createTransaction(transaction);
        const newTransaction = await getTransaction(transaction.Id);
        document.querySelector("tbody").append(createTransactionRow(newTransaction));
        reset();
    }
}
